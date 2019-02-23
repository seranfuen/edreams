/****************************************************************************
 * FrmMain: a dream diary application
 * Author: Sergio Ángel Verbo
 * Copyright © 2012-2019, Sergio Ángel Verbo
 ****************************************************************************/
/****************************************************************************
    This file is part of FrmMain.

    FrmMain is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FrmMain is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FrmMain.  If not, see <http://www.gnu.org/licenses/gpl-3.0.html>.]
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using eDream.program;

namespace eDream.libs
{
    internal class DreamSaveLoad : IDisposable
    {
        public enum LoadingResult
        {
            No,
            Successful,
            SuccessfulNoEntries,
            Error
        }

        public enum SavingResult
        {
            No,
            Successful,
            Error
        }

        private readonly object _threadLock = new object();


        private List<DreamEntry> _entriesFromXml;

        private List<DreamEntry> _entriesToXml;


        private XMLParser _xmlReader;

        private XMLWriter _xmlSaver;


        public EventHandler FinishedLoading;


        public EventHandler FinishedSaving;

        public LoadingResult LoadStatus { get; private set; } = LoadingResult.No;

        public SavingResult SaveStatus { get; private set; } = SavingResult.No;

        public List<DreamEntry> EntriesFromXml
        {
            get
            {
                if (_entriesFromXml == null ||
                    LoadStatus == LoadingResult.Error)
                    return new List<DreamEntry>();
                return _entriesFromXml;
            }
        }

        public List<DreamEntry> EntriesToXml
        {
            set => _entriesToXml = value;
        }


        public string CurrentFile { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public void LoadEntries()
        {
            var loadT = new Thread(StartOperation);
            loadT.Start(OperationType.Load);
        }

        public void SaveEntries()
        {
            var loadT = new Thread(StartOperation);
            loadT.Start(OperationType.Save);
        }

        private void StartOperation(object obj)
        {
            var type = (OperationType) obj;
            lock (_threadLock)
            {
                if (type == OperationType.Load)
                    InternalLoadEntries();
                else if (type == OperationType.Save) InternalSaveEntries();
            }
        }


        private void InternalSaveEntries()
        {
            SaveStatus = SavingResult.No;
            if (_xmlSaver == null) _xmlSaver = new XMLWriter();
            if (_entriesToXml == null || string.IsNullOrWhiteSpace(CurrentFile))
                SendFinishedSavingSignal(SavingResult.Error);
            else if (_xmlSaver.ParseEntries(CurrentFile, _entriesToXml))
                SendFinishedSavingSignal(SavingResult.Successful);
            else
                SendFinishedSavingSignal(SavingResult.Error);
        }


        private void InternalLoadEntries()
        {
            LoadStatus = LoadingResult.No;
            if (_xmlReader == null) _xmlReader = new XMLParser();
            if (!_xmlReader.IsFileValid(CurrentFile))
            {
                SendFinishedLoadingSignal(LoadingResult.Error);
                return;
            }

            _entriesFromXml = _xmlReader.LoadFile(CurrentFile);
            if (_entriesFromXml == null)
            {
                SendFinishedLoadingSignal(LoadingResult.Error);
                return;
            }

            if (_entriesFromXml.Count > 0)
                SendFinishedLoadingSignal(LoadingResult.Successful);
            else
                SendFinishedLoadingSignal(LoadingResult.SuccessfulNoEntries);
        }


        private void SendFinishedLoadingSignal(LoadingResult status)
        {
            LoadStatus = status;
            if (FinishedLoading != null) FinishedLoading(this, new EventArgs());
        }


        private void SendFinishedSavingSignal(SavingResult status)
        {
            SaveStatus = status;
            if (FinishedSaving != null) FinishedSaving(this, new EventArgs());
        }

        private enum OperationType
        {
            Save,
            Load
        }
    }
}