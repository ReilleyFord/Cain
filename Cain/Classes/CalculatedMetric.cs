using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cain {
    public class CalculatedMetric {
        public DateTime StartValue;
        public DateTime EndValue;
        public int      CalculatedValue;

        public CalculatedMetric() { }

        public CalculatedMetric(string start, string end, int value) {
            this.StartValue      = start;
            this.EndValue        = end;
            this.CalculatedValue = value;
        }

    }
}
