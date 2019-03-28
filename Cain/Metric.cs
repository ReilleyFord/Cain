using System;
using System.Collections.Generic;
using System.Text;

namespace Cain
{
    public class Metric
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartingValue { get; set; }
        public string EndingValue { get; set; }

        public Metric() {

        }

        public Metric(int id, string name, string start, string end) {
            this.Id = id;
            this.Name = name;
            this.StartingValue = start;
            this.EndingValue = end;
        }
    }
}
