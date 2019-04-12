using System;
using System.Threading;

namespace Cain
{
    public enum MetricType { Case, Property }

    public class Metric
    {
        public int        Id            { get; set; }
        public string     Name          { get; set; }
        public string     StartingValue { get; set; }
        public string     EndingValue   { get; set; }
        public MetricType Type          { get; set; }

        public Metric() { }

        public Metric(string name, string start, string end, MetricType type) {
            this.Name          = name;
            this.StartingValue = start;
            this.EndingValue   = end;
            this.Type          = type;
        }

    }
}
