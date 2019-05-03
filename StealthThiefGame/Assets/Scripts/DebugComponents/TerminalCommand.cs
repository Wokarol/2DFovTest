using CommandTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CommandTerminal
{
    public class TerminalCommand : TerminalCommandGeneric
    {
        [SerializeField] UnityEvent onCommandEvent = new UnityEvent();
        protected override void Execute(CommandArg[] args)
        {
            onCommandEvent.Invoke();
        }
    } 
}
