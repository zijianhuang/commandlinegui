using System;

namespace Fonlow.CommandLineGui
{
    /// <summary>
    /// Send the subscriber a text message
    /// </summary>
    public class TextMessageEventArgs : EventArgs
    {
        public TextMessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { private set; get; }
    }

    /// <summary>
    /// Used when a cursor changes position in a line of command text, 
    /// the subscriber or its delegate should try to give a parameter.
    /// Basically this is a data object passing between parties.
    /// </summary>
    public class ParameterHighlightedEventArgs : EventArgs
    {
        public ParameterHighlightedEventArgs(string commandText, int cursorPosition)
        {
            CommandText = commandText;
            CursorPosition = cursorPosition;
        }

        public string CommandText { get; private set; }

        public int CursorPosition { get; private set; }

        /// <summary>
        /// The subscriber or its delegate should analyze then return the parameter at the cursor. If not found, return null
        /// </summary>
        public string Parameter { get; set; }
    }



}
