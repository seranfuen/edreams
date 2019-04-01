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
using System.ComponentModel;
using System.Linq;
using eDream.Annotations;
using eDream.GUI;
using eDream.program;

namespace eDream.libs
{
    public class DreamDiaryPersistenceService : IDreamDiaryPersistenceService
    {
        public event EventHandler<FinishedPersistingEventArgs> FinishedPersisting;

        public void PersistEntries([NotNull] IEnumerable<DreamEntry> entriesToPersist, [NotNull] string path)
        {
            if (entriesToPersist == null) throw new ArgumentNullException(nameof(entriesToPersist));
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(GuiStrings.ValueCannotBeNullOrWhitespace, nameof(path));

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += PersistBackgroundWorkerOnDoWork;
            backgroundWorker.RunWorkerCompleted += PersistBackgroundWorkerOnRunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(new PersistDreamEntriesArgs(path, entriesToPersist));
        }

        public event EventHandler<FinishedLoadingEventArgs> FinishedLoading;

        public void LoadDiary([NotNull] string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += LoadBackgroundWorkerOnDoWork;
            backgroundWorker.RunWorkerCompleted += LoadBackgroundWorkerOnRunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(path);
        }

        private static void LoadBackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            if (!(e.Argument is string path)) return;

            var xmlReader = new DiaryReader();
            e.Result = !xmlReader.IsFileValid(path) ? null : xmlReader.LoadFile(path);
        }

        private void LoadBackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!(e.Result is IEnumerable<DreamEntry> entries))
                SendFinishedLoadingSignal(LoadingResult.Error, null);
            else
                SendFinishedLoadingSignal(LoadingResult.Successful, entries);
        }

        private static void PersistBackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            var xmlSaver = new DiaryXmlWriter();
            var arg = (PersistDreamEntriesArgs) e.Argument;

            var result = xmlSaver.WriteEntriesToFile(arg.Path, arg.Entries.Where(entry => !entry.ToDelete));
            e.Result = result ? PersistenceOperationResult.Successful : PersistenceOperationResult.Error;
        }

        private void PersistBackgroundWorkerOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SendFinishedSavingSignal((PersistenceOperationResult) e.Result);
        }

        private void SendFinishedLoadingSignal(LoadingResult status, IEnumerable<DreamEntry> entries)
        {
            FinishedLoading?.Invoke(this, new FinishedLoadingEventArgs(status, entries));
        }

        private void SendFinishedSavingSignal(PersistenceOperationResult status)
        {
            FinishedPersisting?.Invoke(this, new FinishedPersistingEventArgs(status));
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