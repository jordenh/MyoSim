using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSimGUI.ParsedCommands
{
    class DelayCommand : ParsedCommand
    {
        public DelayCommand(uint delay)
            : base(CommandType.DELAY)
        {
            this.delay = delay;
        }

        public uint getDelay()
        {
            return delay;
        }

        public override string ToString()
        {
            return base.ToString() + ", Delay: " + delay.ToString();
        }

        private uint delay;
    }
}
