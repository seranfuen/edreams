/****************************************************************************
 * eDreams: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of eDreams.

    eDreams is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    eDreams is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with eDreams.  If not, see <http://www.gnu.org/licenses/gpl-3.0.html>.]
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using eDream.program;
using System.Threading;

namespace eDream.libs {
    /// <summary>
    /// This class is used as a "wrapper" for the XMLParser and XMLWriter
    /// so it can be called from main and load/save data in their own threads,
    /// emitting a singal once the operations are completed and the result
    /// or data are ready for retrieval
    /// </summary>
    class DreamSaveLoad : IDisposable {
        /// <summary>
        /// The status after loading an XML file. Successful means that at least
        /// one entry was loaded, SuccessfulNoEntries means that no exception
        /// was triggered (the XML was correct) but it contained no entries,
        /// and Error means that an exception was triggered. No means no status
        /// yet.
        /// </summary>
        public enum enumLoadStatus {
            No, 
            Successful,
            SuccessfulNoEntries,
            Error
        }
        
        /// <summary>
        /// The status after saving an XML file. No means no status yet, 
        /// Successful that no exceptions were triggered, error an exception
        /// was triggered
        /// </summary>
        public enum enumSaveStatus {
            No,
            Successful,
            Error
        }

        /// <summary>
        /// The type of operation the thread will carry out, locking it
        /// </summary>
        private enum operationType {
            Save,
            Load
        }

        /// <summary>
        /// The path to the file that is currently being operated on. Only
        /// one file can be active at a time
        /// </summary>
        private string currentFile;

        /// <summary>
        /// Contains the DreamEntry objects loaded from a XML
        /// </summary>
        private List<DreamEntry> entriesFromXML;

        /// <summary>
        /// The DreamEntry objects that we intend to save to the XML file
        /// </summary>
        private List<DreamEntry> entriesToXML;

        /// <summary>
        /// Determines the load status  
        /// </summary>
        private enumLoadStatus loadStatus = enumLoadStatus.No;

        /// <summary>
        /// The save status
        /// </summary>
        private enumSaveStatus saveStatus = enumSaveStatus.No;

        /// <summary>
        /// Only allow one save/load operation
        /// </summary>
        private object threadLock = new object();

        /// <summary>
        /// This event fires when a thread has finished parsing an XML document.
        /// It only means it has finished, whether successfully, so 
        /// entriesFromXML is available, or with an error. Check property
        /// LoadStatus
        /// </summary>
        public EventHandler FinishedLoading;

        /// <summary>
        /// This event fires when a thread has finished saving an XML document.
        /// It only means it has finished, whether successfully or with an
        /// error so check SaveStatus
        /// </summary>
        public EventHandler FinishedSaving;

        /// <summary>
        /// The parser object
        /// </summary>
        private XMLParser XMLReader;

        /// <summary>
        /// The object that saves
        /// </summary>
        private XMLWriter XMLSaver;

        /// <summary>
        /// The status tells the result of the load operation. No means that 
        /// the operation was never carried out or hasn't finished yet
        /// </summary>
        public enumLoadStatus LoadStatus {
            get {
                return loadStatus;
            }
        }

        /// <summary>
        /// The status tells the result of the save operation. No means
        /// no save operation has been carried out or the current is not finished
        /// </summary>
        public enumSaveStatus SaveStatus {
            get {
                return saveStatus;
            }
        }
        
        /// <summary>
        /// Returns a list of dream entry objects loaded from the XML file.
        /// If no entries were found or none have been loaded, will return an
        /// empty list
        /// </summary>
        public List<DreamEntry> EntriesFromXML {
            get {
                if (entriesFromXML == null || 
                    LoadStatus == enumLoadStatus.Error) {
                        return new List<DreamEntry>();
                }
                return entriesFromXML;
            }
        }

        /// <summary>
        /// Loads a list of DreamEntry objects to be saved to a file by
        /// SaveEntries()
        /// </summary>
        public List<DreamEntry> EntriesToXML {
            set {
                entriesToXML = value;
            }
        }

        /// <summary>
        /// A string representation of the path to the file that is active 
        /// and which the loader/writer will work on
        /// </summary>
        public string CurrentFile {
            get {
                return currentFile;
            }
            set {
                currentFile = value;
            }
        }

            /// <summary>
        /// Loads entries contained in currentFile. Sends the singal 
        /// FinishedLoading when the process ends. 
        /// EntriesFromXML makes the list of DreamEntry objects parsed available.
        /// If no entries could be parsed, an empty list will be returned. Check
        /// LoadStatus to know if this was the result of an error or that a
        /// valid database didn't have any entries
        /// </summary>
        public void LoadEntries() {
            Thread loadT = new Thread(new ParameterizedThreadStart(StartOperation));
            loadT.Start(operationType.Load);
        }

        /// <summary>
        /// Save entries loaded to EntriesToXML to the currentFile path
        /// </summary>
        public void SaveEntries() {
            Thread loadT = new Thread(new ParameterizedThreadStart(StartOperation));
            loadT.Start(operationType.Save);
        }

        /// <summary>
        /// Creates a new thread and carries out the operation, through a lock
        /// to avoid two operations at the same time
        /// </summary>
        /// <param name="obj"></param>
        private void StartOperation(object obj) {
            operationType type = (operationType)obj;
            lock (threadLock) {
                if (type == operationType.Load) {
                    InternalLoadEntries();
                }
                else if (type == operationType.Save) {
                    InternalSaveEntries();
                }
            }
        }

        /// <summary>
        /// Saves entries
        /// </summary>
        private void InternalSaveEntries() {
            saveStatus = enumSaveStatus.No;
            if (XMLSaver == null) {
                XMLSaver = new XMLWriter();
            }
            if (entriesToXML == null || string.IsNullOrWhiteSpace(currentFile)) {
                SendFinishedSavingSignal(enumSaveStatus.Error);
            }
            else if (XMLSaver.ParseEntries(currentFile, entriesToXML)) {
                SendFinishedSavingSignal(enumSaveStatus.Successful);
            }
            else {
                SendFinishedSavingSignal(enumSaveStatus.Error);
            }
        }

        /// <summary>
        /// Carries out the actual loading operation. LoadEntries serves
        /// as the entry point spawning a thread
        /// </summary>
        private void InternalLoadEntries() {
            loadStatus = enumLoadStatus.No;
            if (XMLReader == null) {
                XMLReader = new XMLParser();
            }
            if (!XMLReader.IsFileValid(currentFile)) {
                SendFinishedLoadingSignal(enumLoadStatus.Error);
                return;
            }
            entriesFromXML = XMLReader.LoadFile(currentFile);
            if (entriesFromXML == null) {
                SendFinishedLoadingSignal(enumLoadStatus.Error);
                return;
            }
            if (entriesFromXML.Count > 0) {
                SendFinishedLoadingSignal(enumLoadStatus.Successful);
            }
            else {
                SendFinishedLoadingSignal(enumLoadStatus.SuccessfulNoEntries);
            }
        }

        /// <summary>
        /// Sends a signal when the loading proccess has finished and sets the
        /// status signaling the result of the process
        /// </summary>
        /// <param name="status"></param>
        private void SendFinishedLoadingSignal(enumLoadStatus status) {
            loadStatus = status;
            if (FinishedLoading != null) {
                FinishedLoading(this, new EventArgs());
            }
        }

        /// <summary>
        /// Sends a signal when the saving proccess has finished and sets the
        /// status signaling the result of the process
        /// </summary>
        /// <param name="status"></param>
        private void SendFinishedSavingSignal(enumSaveStatus status) {
            saveStatus = status;
            if (FinishedSaving != null) {
                FinishedSaving(this, new EventArgs());
            }
        }

        /// <summary>
        /// Delete object
        /// </summary>
        public void Dispose() {
            GC.SuppressFinalize(this);
        }
    }
}
