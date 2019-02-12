using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EvilTools {
    /// <summary>
    /// A library with a utility to quickly show debug messages in the console
    /// and to a .txt file
    /// </summary>
    public class Debug {

        /// <summary>
        /// The constructor takes a debug parameter. If disabled, it will do
        /// nothing.
        /// ToConsole will only display debug information on console.
        /// ToFile will only send the debug information to the fil
        /// ToConsoleAndFil sends it to both
        /// </summary>
        public enum DebugParameters {
            Disabled,
            ToConsole,
            ToFile,
            ToConsoleAndFile
        }

        /// <summary>
        /// This delegate is used to accept debug incidences
        /// </summary>
        /// <param name="args"></param>
        public delegate void OnDebugIncidence(DebugArgs args);

        /// <summary>
        /// This class holds the details of the debug message (message thrown by 
        /// exception, stack, custom message) that will be passed when an object
        /// emits a debug-type signal.
        /// It is passed to the function AcceptDebug() to process the incidence
        /// </summary>
        public class DebugArgs {
            /// <summary>
            /// Message contained by the exception if any thrown
            /// </summary>
            private string exceptionMessage;
            /// <summary>
            /// Stack trace by the exception if any thrown
            /// </summary>
            private string stackTrace;
            /// <summary>
            /// A custom message provided by the caller
            /// </summary>
            private string message;
            /// <summary>
            /// The date and time when the debug was sent
            /// </summary>
            private DateTime date;
            /// <summary>
            /// The message that the exception returned
            /// </summary>
            public string ExceptionMessage {
                set {
                    exceptionMessage = value;
                }
                get {
                    return String.IsNullOrWhiteSpace(exceptionMessage) ? "" :
                            exceptionMessage;
                }
            }
            /// <summary>
            /// The stack trace (functions callback) by the exception
            /// </summary>
            public string StackTrace {
                set {
                    stackTrace = value;
                }
                get {
                    return String.IsNullOrWhiteSpace(stackTrace) ? "" : stackTrace;
                }
            }
            /// <summary>
            /// A custom message by the caller
            /// </summary>
            public string Message {
                set {
                    message = value;
                }
                get {
                    return String.IsNullOrWhiteSpace(message) ? "" : message;
                }
            }
            /// <summary>
            /// The date and time when the debug incidence was called
            /// </summary>
            public DateTime Date {
                get {
                    return date;
                }
            }
            public DebugArgs() {
                this.date = DateTime.Now;
            }

        }
        /// <summary>
        /// What actions it will take
        /// </summary>
        private DebugParameters operationMode;
        /// <summary>
        /// The file path where the incidences will be written if the
        /// operationMode is set to it
        /// </summary>
        private const string debugFile = "debug.txt";
        /// <summary>
        /// The details from the last incidence thrown
        /// </summary>
        private DebugArgs lastIncidence;

        /// <summary>
        /// Constructs a new Debug object in the operation mode it is set to
        /// </summary>
        /// <param name="operationMode">DebugParameters constant, determines
        /// if the messages are sent to the console, to a file, to both or to
        /// none</param>
        public Debug(DebugParameters operationMode) {
            this.operationMode = operationMode;
        }

        /// <summary>
        /// Process a debug incidence, depending on the operationMode
        /// </summary>
        /// <param name="data"></param>
        public void AcceptDebug(DebugArgs data) {
            if (operationMode == DebugParameters.Disabled) {
                return;
            }
            lastIncidence = data;
            if (operationMode == DebugParameters.ToConsole ||
                operationMode == DebugParameters.ToConsoleAndFile) {
                    SendToConsole();
            }
            if (operationMode == DebugParameters.ToFile ||
                operationMode == DebugParameters.ToConsoleAndFile) {
                SendToFile();
            }
        }
        /// <summary>
        /// Checks whether there is a last incidence (it's not null)
        /// </summary>
        /// <returns>True if there is a last incidence -object is not null-,
        /// false if it is null</returns>
        private bool LastIncidenceExists() {
            return (lastIncidence != null);
        }
        /// <summary>
        /// Sends the incidence message to the console
        /// </summary>
        private void SendToConsole() {
            if (LastIncidenceExists()) {
                Console.WriteLine(FormatDebugMessage());
            }
        }
        /// <summary>
        /// Creates a new file or 
        /// </summary>
        private void SendToFile() {
            if (!LastIncidenceExists()) {
                return;
            }
            string msg = "";
            if (File.Exists(debugFile)) {
                StreamReader reader = new StreamReader(debugFile);
                msg = reader.ReadToEnd();
                reader.Close();
            }
            msg += FormatDebugMessage();
            StreamWriter wr = new StreamWriter(debugFile);
            try {
                wr.Write(msg);
            }
            catch {
                Console.WriteLine("Debug: couldn't write to file!");
            }
            finally {
                wr.Close();
            }
        }
        /// <summary>
        /// Create the message with the debug data
        /// </summary>
        /// <returns></returns>
        private string FormatDebugMessage() {
            string msg = "\r\n=====================================\r\n";
            msg += String.Format("Debug message ({0})\r\n",
                lastIncidence.Date.ToString());
            msg += "=====================================\r\n";
            msg += String.Format("MESSAGE:\r\n{0}\r\n", lastIncidence.Message);
            msg += String.Format("EXCEPTION MESSAGE:\r\n{0}\r\n",
                lastIncidence.ExceptionMessage);
            msg += String.Format("STACK TRACE:\r\n{0}\r\n",
                lastIncidence.StackTrace);
            return msg;
        }
    }
}
