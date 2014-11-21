using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    class ExpectCommand : ParsedCommand
    {
        public ExpectCommand(ExpectCommandCode expectCommand, uint waitTime)
            : base(CommandType.EXPECT)
        {
            this.expectCommand = expectCommand;
            this.waitTime = waitTime;
        }

        public ExpectCommandCode getExpectCommand()
        {
            return expectCommand;
        }

        public uint getWaitTime()
        {
            return waitTime;
        }

        public override string ToString()
        {
            return base.ToString() + ", Expect Command: " + expectCommand.ToString() + ", Wait Time: " + waitTime.ToString();
        }

        private ExpectCommandCode expectCommand;
        private uint waitTime;
    }
}
