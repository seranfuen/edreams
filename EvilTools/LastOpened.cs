using System;
using System.Collections.Generic;
using System.Text;

namespace EvilTools {
    
    /// <summary>
    /// This class is used to keep a number of file paths stored in a plain text
    /// file and to retrieve them in a Last In First Out order. Their paths
    /// can also be truncated by setting MaxLength to the max number of
    /// characters, although it may not be always possible to shorten it.
    /// This class is used to keep track of the last files that have been
    /// loaded
    /// </summary>
    public class LastOpened {
        /**
         * path separator, normally \ in windows
         */
        private const string PathSeparator = "\\";
        /**
         * name of file where we store the entries in plain text
         */ 
        private const string FileName = "last_opened.ini";
        /**
         * maximum number of last opened databases
         */
        private const int LastOpenedNumber = 5;
        /**
         * the dream database extension, it should be xml
         */
        private const string FileExtension = "xml";
        /**
         * the approximate max length in characters of the path\filename to 
         * display. The algorithm may make it longer if, for example,
         * there are no more paths to shorten
         */
        private const int MaxLength = 80;
        /**
         * The string that will be used to shorten 
         */
        private const string Shortener = "...";
        /**
         * Line break control characters
         */
        private const string LineBreak = "\r\n";
        /**
         * a string list with the paths and filenames of databases last opened
         */
        private string[] lastOpen = new string[LastOpenedNumber];
        /**
         * Whether any paths were successfully loaded
         */
        private bool loaded = false;

        /**
         * Adds a new file path as a string to the internal list
         * If it's already present, will move all entries before it to the right
         * one place and then add it at the beginning
         */
        public void AddPath(string path) {
            if (String.IsNullOrEmpty(path)) {
                return;
            }
            // Remove line break
            if (path.LastIndexOf('\r') != -1) {
                path = path.Substring(0, path.LastIndexOf('\r'));
            }
            for (int i = 0; i < lastOpen.Length; i++) {
                if (String.IsNullOrEmpty(lastOpen[i])) {
                    break;
                }
                if (lastOpen[i].ToLower() == path.ToLower()) {
                    for (int j = i; j > 0; j--) {
                        lastOpen[j] = lastOpen[j - 1];
                    }
                    lastOpen[0] = path;
                    return;
                }
            }
            // Move array items to the right and add new one
            for (int i = lastOpen.Length-1; i > 0; i--) {
                lastOpen[i] = lastOpen[i - 1];
            }
            lastOpen[0] = path;
        }

        public bool Loaded {
            get {
                return loaded;
            }
        }

        /**
         * Saves the current paths stored (from last to first up to 
         * lastOpenNumber) to a text file, each path per line
         */
        public void SavePaths() {
            if (lastOpen.Length == 0) {
                return;
            }
            try {
                int EndI = lastOpen.Length - LastOpenedNumber >= 0 ?
                   lastOpen.Length - LastOpenedNumber : 0;
                string text = String.Empty;
                for (int i = lastOpen.Length - 1; i >= EndI; i--) {
                    if (String.IsNullOrEmpty(lastOpen[i])) {
                        continue;
                    }
                    text += lastOpen[i] + LineBreak;
                }
                System.IO.File.WriteAllText(FileName, text, Encoding.UTF8);
            }
            catch (Exception e) {
                Console.WriteLine("<<Debug>> Could not write recent files\n" +
                    e.Message);
            }
        }

        /**
         * Loads, if possible, the file with the recent files
         */
        public void LoadPaths() {
            try {
                String text = System.IO.File.ReadAllText(FileName);
                String[] theLines = text.Split(LineBreak[1]);
                for (int i = 0; i < theLines.Length; i++) {
                    if (theLines[i].Trim() != "") {
                        AddPath(theLines[i]);
                    }
                }
                if (lastOpen.Length == 0) {
                    loaded = false;
                }
                else {
                    loaded = true;
                }
            }
            catch (Exception e) {
                Console.WriteLine("<<Debug>> Could not read recent files\n" +
                    e.Message);
                loaded = false;
            }
        }

        public string[] GetPaths() {
            return lastOpen;
        }

        /**
         * This algorithm takes a well-formatted path longer than MaxLength,
         * such as C:\dir1\dir2\dir3\file.xml
         * and, leaving the drive letter and filename as they are, starts
         * changing each dir with ..., starting from the first dir (closest to
         * the drive letter) and advancing one at a time until the filename 
         * is shorter than MaxLength or until there are no more dirs to shorten
         * 
         * Therefore, a more user-friendly graphical representation can be
         * shown in the GUI for long paths.
         * 
         * If shortening is not possible or necessary, will return the original
         * string
         */ 
        public string ShortenPath(String path) {
            if (String.IsNullOrEmpty(path)) {
                return String.Empty;
            }
            if (path.Length <= MaxLength || path.Substring(1,2) != ":" + 
                PathSeparator || path.Substring(path.Length-3).ToLower() != 
                FileExtension.ToLower()) {
                return path;
            }
            String[] thePaths = path.Substring(3,
                path.LastIndexOf(PathSeparator[0])-3).Split(PathSeparator[0]);
            /** 
             * Will be false if no more dirs to shorten
             */
            int ShortenedI = 0;
            string tempPath = path;
            while (tempPath.Length > MaxLength && ShortenedI != -1) {
                ShortenedI = GetLastShortened(thePaths);
                thePaths[ShortenedI] = Shortener;
                tempPath = path.Substring(0, 3) + String.Join(PathSeparator, 
                    thePaths)+ path.Substring(path.LastIndexOf(PathSeparator[0]));
            }
            return tempPath;
        }

        /**
         * gets the index to the first path in the array that has not been 
         * shortened or returns -1 if all shortened or no finds
         */ 
      private int GetLastShortened(String[] paths) {
            for (int i = 0; i < paths.Length; i++) {
                if (paths[i] != Shortener) {
                    return i;
                }
            }
            return -1;
        }
    }
}
