using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using EvilTools;

namespace EvilTools {
    /// <summary>
    /// This class can be used as a single settings manager: it will load the
    /// settings contained in a plain text file and return a list of objects
    /// that contain a pair of strings called key and value, so that it is 
    /// possible to search for a particular key and obtain its value if it
    /// was present.
    /// 
    /// The format the plain text file (preferibably .ini but anything will do)
    /// is key=value
    /// One key and one value per line
    /// </summary>
    public class Settings {
        // What separates the key & value pairs on each line
        private const string keyValueSeparator = "=";
        // The control characters used to indicate a new line
        private const string lineBreakCharacters = "\r\n";
        // String representation of the file path
        private string settingsFile;
        // Stream reader and writer objects
        private StreamReader reader;
        private StreamWriter writer;
        // The list of settings pairs parsed
        List<SettingsPair> loadedSettings = new List<SettingsPair>();
        // A list of default settings in case they are not found in file
        List<SettingsPair> defaultSettings = new List<SettingsPair>();
        // Debug
        Debug.OnDebugIncidence DebugIncidence;
        

        /// <summary>
        /// Represents a key-value pair. Keys are always unique and 
        /// non case sensitive so a duplicated key cannot be added
        /// </summary>
        public class SettingsPair {

            private bool toDelete = false;
            /// <summary>
            /// The key and values
            /// </summary>
            private string key;
            private string value;
            /// <summary>
            /// Whether the object has been set to deletion so it will not be
            /// added to the file when writing and will not be returned when
            /// accessed. A new settingsPair object with the same key can be
            /// added then. It is false by default
            /// </summary>
            public bool ToDelete {
                get {
                    return toDelete;
                }
                set {
                    toDelete = value;
                }
            }
            /// <summary>
            /// A key is unique in all the file and represents a particular 
            /// setting
            /// </summary>
            public string Key {
                get {
                    return key;
                }
                set {
                    key = value.ToLower();
                }
            }
            /// <summary>
            /// The value represents the state of the key. The same value can
            /// apply to any number of keys
            /// </summary>
            public string Value {
                get {
                    return value;
                }
                set {
                    this.value = value.ToLower();
                }
            }
        }

        /// <summary>
        /// Returns the value the key has or String.Empty is the key is not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key] {
            get {
                return GetValue(key);
            }
        }

        /// <summary>
        /// Create a settings object based around the file
        /// </summary>
        /// <param name="file"></param>
        public Settings(string file) {
            this.settingsFile = file;
            ReadFile();
        }
        /// <summary>
        /// Create a settings object based around the file and with a list
        /// of default settings: if a particular key is not found in the
        /// list loaded from the file but it is found in the default settings,
        /// its value will be returned.
        /// An empty file name can be passed, this way the object will not
        /// try to read or write any files but will use the default settigns
        /// list to retrieve settings
        /// </summary>
        /// <param name="file"></param>
        /// <param name="defaultSettings"></param>
        public Settings(string file, List<SettingsPair> defaultSettings) {
            this.settingsFile = file;
            this.defaultSettings = defaultSettings;
            ReadFile();
        }

        /// <summary>
        /// Returns true if a key of the same value (non case sensitive) exists
        /// in the list, false if it was not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool KeyAlreadyExists(string key) {
            for (int i = 0; i < loadedSettings.Count; i++) {
                if (String.Compare(key, loadedSettings[i].Key, true) == 0 && 
                    ! loadedSettings[i].ToDelete) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the value of the key given or String.Empty if not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key) {
            // Try parsed settings
            for (int i = 0; i < loadedSettings.Count; i++) {
                if (String.Compare(key, loadedSettings[i].Key, true) == 0) {
                    if (!loadedSettings[i].ToDelete) {
                        return loadedSettings[i].Value;    
                    }
                }
            }
            // Try default settings
            for (int i = 0; i < defaultSettings.Count; i++) {
                if (String.Compare(key, defaultSettings[i].Key, true) == 0) {
                    if (!defaultSettings[i].ToDelete) {
                        return defaultSettings[i].Value;
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Ads a new SettingsPair object to the list if the key doesn't exist
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddKeyValuePair(string key, string value) {
            if (KeyAlreadyExists(key)) {
                return;
            }
            loadedSettings.Add(new SettingsPair() {
                Key = key.Trim(),
                Value = value.Trim()
            });
        }

        /// <summary>
        /// Sets a particular key to delete
        /// </summary>
        /// <param name="key"></param>
        public void DeleteKeyValuePair(string key) {
            foreach (SettingsPair i in loadedSettings) {
                if (String.Compare(key, i.Key, true) == 0 &&
                    !i.ToDelete) {
                    i.ToDelete = true;
                }
            }
        }
        /// <summary>
        /// Changes the value of a key to the new one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ChangeValue(string key, string value) {
            foreach (SettingsPair i in loadedSettings) {
                if (String.Compare(key, i.Key, true) == 0 &&
                    !i.ToDelete) {
                    i.Value = value;
                    return;
                }
            }
            // If we didn't find a pre-existing key, create it with the value given
            AddKeyValuePair(key, value);
        }

        /// <summary>
        /// Reads the settings file and loads the settings to a list of
        /// SettingsPair objects
        /// </summary>
        private void ReadFile() {
            if (String.IsNullOrWhiteSpace(settingsFile) || 
                !File.Exists(settingsFile)) {
                return;
            }
            try {
                string key, value;
                reader = new StreamReader(settingsFile);
                string text = reader.ReadToEnd();
                string[] lines = Regex.Split(text, lineBreakCharacters);
                foreach (string i in lines) {
                    int index = i.IndexOf(keyValueSeparator);
                    if (index < 0) {
                        continue;
                    }
                    key = i.Substring(0, index);
                    if (String.IsNullOrWhiteSpace(key)) {
                        continue;
                    }
                    value = i.Substring(index + 1);
                    AddKeyValuePair(key, value);
                }
            }
            catch (Exception e) {
                ThrowDebugIncidence(e);
            }
            finally {
                reader.Close();
            }
        }

        /// <summary>
        /// Write the currently loaded settings to file
        /// </summary>
        public void SaveFile() {
            if (String.IsNullOrWhiteSpace(settingsFile)) {
                return;
            }
            string str = "";
            foreach (SettingsPair i in loadedSettings) {
                if (i.ToDelete) {
                    continue;
                }
                str += i.Key + keyValueSeparator + i.Value + lineBreakCharacters;
            }
            try {
                writer = new StreamWriter(settingsFile);
                writer.Write(str);
            }
            catch (Exception e) {
                ThrowDebugIncidence(e);
            }
            finally {
                writer.Close();
            }
        }

        /// <summary>
        /// Throw a debut incidence via DebugIncidence listener
        /// </summary>
        /// <param name="e"></param>
        private void ThrowDebugIncidence(Exception e) {
            if (DebugIncidence != null) {
                DebugIncidence(new Debug.DebugArgs() {
                    ExceptionMessage = e.Message,
                    StackTrace = e.StackTrace
                });
            }
        }
    }
}
