using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.ImmediateWindow.Services
{
    [Serializable]
    internal class History
    {
        // Disabled until it can be made to work properly
        //private static int MaxHistory = 50;
        
        private static History instance = new History();
        public static History Instance { get { return instance; } }

        protected List<Command> Commands
        {
            get { return State.Instance.Commands; }
        }
        
        public History()
        {
        }

        public Command AddCommand(string code)
        {
            Command result = null;
            if (!string.IsNullOrEmpty(code))
            {
                var previousIsSame = Commands.Any() && Commands.Last().code == code;
                if (!previousIsSame)
                {
                    result = new Command {code = code, index = Commands.Count};
                    Commands.Add(result);

                    // TODO: Cannot crop the list since it makes all indexes change. Index is probably awful way of doing it anyway..
                    // Commands = TakeLast(Commands, MaxHistory).ToList();
                }
            }

            return result;
        }

        // Return the previous command relative to given command
        public Command PreviousCommand(Command command = null)
        {
            if (command == null)
                return Commands.LastOrDefault();

            var index = Math.Min(command.index, Commands.Count - 1); // Make sure index doesn't go out of range
            if (index != 0)
                return Commands.ElementAt(index - 1);

            return command;
        }
       
        // Return the next command relative to given command
        public Command NextCommand(Command command = null)
        {
            if (command == null)
                return null;

            if (command.index < (Commands.Count - 1))
                return Commands.ElementAt(command.index + 1);

            return null;
        }

        public void Clear()
        {
            Commands.Clear();
        }

        public static IEnumerable<Command> TakeLast(IEnumerable<Command> coll, int N)
        {
            return coll.Reverse().Take(N).Reverse();
        }
    }
}