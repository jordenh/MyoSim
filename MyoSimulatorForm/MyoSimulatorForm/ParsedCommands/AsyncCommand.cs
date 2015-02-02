using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    class AsyncCommand : ParsedCommand
    {
        public AsyncCommand(AsyncCommandCode asyncCommand,
            HubCommunicator.Arm arm = HubCommunicator.Arm.RIGHT,
            HubCommunicator.XDirection xDirection = HubCommunicator.XDirection.FACING_ELBOW)
            : base(CommandType.ASYNC)
        {
            this.asyncCommand = asyncCommand;
            if (asyncCommand == ParsedCommand.AsyncCommandCode.ARM_RECOGNIZED)
            {
                this.armDir = new AsyncCommand.armDirection(arm, xDirection);
            }
        }

        public AsyncCommandCode getAsyncCommand()
        {
            return asyncCommand;
        }

        public override string ToString()
        {
            return base.ToString() + ", Async Command: " + asyncCommand.ToString();
        }

        public armDirection getArmDirection()
        {
            return armDir;
        }

        public struct armDirection
        {
            public armDirection(HubCommunicator.Arm arm,
                HubCommunicator.XDirection xDirection)
            {
                this.arm = arm;
                this.xDirection = xDirection;
            }

            public String toString()
            {
                return String.Format("arm: {0} xDirection: {1}", arm,
                    xDirection);
            }

            public HubCommunicator.Arm arm;
            public HubCommunicator.XDirection xDirection;
        }

        private AsyncCommandCode asyncCommand;
        private armDirection armDir;
    }
}
