using CommandTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CommandTerminal
{
    public class TerminalBoolCommand : TerminalCommandGeneric
    {
        protected override int ArgumentCount => 1;

        [SerializeField] CommandEvent onCommandEvent = new CommandEvent();
        protected override void Execute(CommandArg[] args)
        {
            onCommandEvent.Invoke(args[0].Bool);
        }
        [System.Serializable]
        private class CommandEvent : UnityEvent<bool>
        {
        }
    } 
}
