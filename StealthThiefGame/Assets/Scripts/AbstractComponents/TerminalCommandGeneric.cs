using CommandTerminal;
using UnityEngine;

namespace CommandTerminal
{
    public abstract class TerminalCommandGeneric : MonoBehaviour
    {
        [Tooltip("{name} is replaced by object name, {parent_name} is replaced by name of the parent")]
        [SerializeField] string commandName = "Noname_Command";
        [SerializeField] string help = "There is no help!";

        virtual protected int ArgumentCount { get => 0; }
        bool canRegisterCommand = false;

        private void Start()
        {
            canRegisterCommand = true;
            OnEnable();
        }

        private void OnEnable()
        {
            if (canRegisterCommand) {
                Debug.Log($"Registered {commandName}");
                var formattedCommand = commandName
                       .Replace("{name}", name)
                       .Replace("{parent_name}", (transform.parent != null) ? transform.parent.name : name);

                TerminalUtils.SafeAddCommand(formattedCommand, Execute, ArgumentCount, ArgumentCount, help); 
            }
        }

        private void OnDisable()
        {
            var formattedCommand = commandName
                .Replace("{name}", name)
                .Replace("{parent_name}", (transform.parent != null) ? transform.parent.name : name);

            TerminalUtils.SafeAddCommand(formattedCommand, Execute, ArgumentCount, ArgumentCount, help);
        }

        protected abstract void Execute(CommandArg[] args);
    }
}