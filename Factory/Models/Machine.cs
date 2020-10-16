using System.Collections.Generic;

namespace Factory.Models
{
    public class Machine
    {
        public Machine()
        {
            this.Engineers = new HashSet<EngineerMachine>();
        }

        public int MachineId { get; set; }
        public string MachineName { get; set; }

        public ICollection<EngineerMachine> Engineers { get;}
    }
}