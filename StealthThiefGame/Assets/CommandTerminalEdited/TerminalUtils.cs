using System;
using UnityEngine;

namespace CommandTerminal
{
    public static class TerminalUtils
    {
        public static void SafeAddCommand(string name, CommandInfo info)
        {
            Terminal.Shell.AddCommand(name, info);
            if (!Terminal.Autocomplete.Contains(name)) {
                Terminal.Autocomplete.Register(name);
            }
        }

        public static void SafeAddCommand(string name, Action<CommandArg[]> proc, int min_args = 0, int max_args = -1, string help = "", string hint = null)
        {
            var info = new CommandInfo() {
                proc = proc,
                min_arg_count = min_args,
                max_arg_count = max_args,
                help = help,
                hint = hint
            };
            SafeAddCommand(name, info);
        }

        public static void SafeRemoveCommand(string name)
        {
            Terminal.Shell.RemoveCommand(name);
            if (Terminal.Autocomplete.Contains(name)) {
                Terminal.Autocomplete.Remove(name);
            }
        }
    }
}
