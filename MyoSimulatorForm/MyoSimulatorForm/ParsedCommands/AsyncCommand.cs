using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    class AsyncCommand : ParsedCommand
    {
        public AsyncCommand(AsyncCommandCode asyncCommand)
            : base(CommandType.ASYNC)
        {
            this.asyncCommand = asyncCommand;
        }

        public AsyncCommandCode getAsyncCommand()
        {
            return asyncCommand;
        }

        public override string ToString()
        {
            return base.ToString() + ", Async Command: " + asyncCommand.ToString();
        }

        private AsyncCommandCode asyncCommand;
    }
}
