using System;
using System.Collections.Generic;

namespace UnityEditor.ImmediateWindow.Services
{
    [Serializable]
    internal class State
    {
        private static State instance = new State();
        public static State Instance { get { return instance; } }

        public List<Command> Commands = new List<Command>();
        public string ScriptCode;
        public bool ShowPrivate = true;
        public bool MultiLineMode;
        public bool ShowContext = true;
        public List<string> Namespaces = new List<string>();

        public static void Init(ref State value)
        {
            if (value == null)  // UI window opened
            {
                value = instance;
            }
            else // Domain reload
            {
                instance = value;
            }
        }
    }
}