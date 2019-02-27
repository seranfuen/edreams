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
using System.Linq;
using System.Threading;
using eDream.Annotations;
using eDream.program;

namespace eDream.libs
{
    public class DreamSaveLoadService : IDisposable, IDreamDiaryPersistenceService
    {
        private readonly object _threadLock = new object();


        private List<DreamEntry> _entriesFromXml;

        private List<DreamEntry> _entriesToPersist;


        private XMLParser _xmlReader;

        private XMLWriter _xmlSaver;

        private string DiaryFilePath { get; set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public event EventHandler<FinishedPersistingEventArgs> FinishedPersisting;

        public void PersistEntries([NotNull] IEnumerable<DreamEntry> entriesToPersist, [NotNull] string path)
        {
            if (entriesToPersist == null) throw new ArgumentNullException(nameof(entriesToPersist));
            DiaryFilePath = path ?? throw new ArgumentNullException(nameof(path));
            _entriesToPersist = entriesToPersist.ToList();
            var loadThread = new Thread(StartOperation);
            loadThread.Start(OperationType.Save);
        }


        public event EventHandler<FinishedLoadingEventArgs> FinishedLoading;

        public void LoadDiary([NotNull] string path)
        {
            DiaryFilePath = path ?? throw new ArgumentNullException(nameof(path));
            var loadT = new Thread(StartOperation);
            loadT.Start(OperationType.Load);
        }


        private void InternalLoadEntries()
        {
            _entriesFromXml = null;
            if (_xmlReader == null) _xmlReader = new XMLParser();
            if (!_xmlReader.IsFileValid(DiaryFilePath))
            {
                SendFinishedLoadingSignal(LoadingResult.Error);
                return;
            }

            _entriesFromXml = _xmlReader.LoadFile(DiaryFilePath);
            if (_entriesFromXml == null)
            {
                SendFinishedLoadingSignal(LoadingResult.Error);
                return;
            }

            SendFinishedLoadingSignal(LoadingResult.Successful);
        }

        private void InternalSaveEntries()
        {
            if (_xmlSaver == null) _xmlSaver = new XMLWriter();
            if (_entriesToPersist == null || string.IsNullOrWhiteSpace(DiaryFilePath))
                SendFinishedSavingSignal(PersistenceOperationResult.Error);
            else if (_xmlSaver.ParseEntries(DiaryFilePath, _entriesToPersist))
                SendFinishedSavingSignal(PersistenceOperationResult.Successful);
            else
                SendFinishedSavingSignal(PersistenceOperationResult.Error);
        }


        private void SendFinishedLoadingSignal(LoadingResult status)
        {
            FinishedLoading?.Invoke(this, new FinishedLoadingEventArgs(status, _entriesFromXml));
        }


        private void SendFinishedSavingSignal(PersistenceOperationResult status)
        {
            FinishedPersisting?.Invoke(this, new FinishedPersistingEventArgs(status));
        }

        private void StartOperation(object obj)
        {
            var type = (OperationType) obj;
            lock (_threadLock)
            {
                switch (type)
                {
                    case OperationType.Load:
                        InternalLoadEntries();
                        break;
                    case OperationType.Save:
                        InternalSaveEntries();
                        break;
                }
            }
        }

        private enum OperationType
        {
            Save,
            Load
        }
    }

    public enum LoadingResult
    {
        Successful,
        Error
    }

    public enum PersistenceOperationResult
    {
        Successful,
        Error
    }
}