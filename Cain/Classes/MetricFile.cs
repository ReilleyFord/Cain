using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Cain
{
    public class MetricFile
    {
        private int          Id      { get; set; }
        private List<Metric> Metrics { get; set; }
        private string       Path    { get; set; }
        private string       File    { get; set; }
        private JObject      JSON    { get; set; }

        public MetricFile(string path) {
            this.Path    = path;
            this.Metrics = new List<Metric>();
            if (System.IO.File.Exists(path))
                LoadMetricsFile(path);
            else
                CreateMetricsFile(path);

            LoadMetrics();
        }

        private void CreateMetricsFile(string path) {
            this.Id = 0;

            JArray  metrics = new JArray();
            JObject obj     = new JObject();
            obj["Metrics"]  = metrics;

            System.IO.File.WriteAllText(this.Path, JsonConvert.SerializeObject(obj));
            LoadMetricsFile(path);
        }

        private void LoadMetricsFile(string path) {
            try {
                this.File = System.IO.File.ReadAllText(path);
                this.JSON = JObject.Parse(this.File);
            } catch (IOException) { throw; }

        }

        private void LoadMetrics() {
            List<Metric> metrics = new List<Metric>();
            foreach(JToken metric in this.JSON.GetValue("Metrics")) {
                Metric newMetric = new Metric();
                newMetric.Id = (int)metric["id"];
                newMetric.Name = (string) metric["Name"];
                newMetric.StartingValue = (string) metric["Start Value"];
                newMetric.EndingValue = (string) metric["End Value"];
                if ((string)metric["Type"] == MetricType.Case.ToString())
                    newMetric.Type = MetricType.Case;
                else if ((string)metric["Type"] == MetricType.Property.ToString())
                    newMetric.Type = MetricType.Property;

                metrics.Add(newMetric);
            }
            this.Metrics = metrics;
        }

        private JObject BuildJObject(Metric metric) {
            JObject jObj = new JObject(
                new JProperty("id", this.Id),
                new JProperty("Name", metric.Name),
                new JProperty("Start Value", metric.StartingValue),
                new JProperty("End Value", metric.EndingValue),
                new JProperty("Type", metric.Type)
                );
            return jObj;
        }

        public List<Metric> GetMetrics() {
            LoadMetrics();
            return this.Metrics;
        }

        public void SetId() {
            LoadMetrics();
            if (this.Metrics.Count == 0)
                return;
            int id = this.Metrics.Last().Id;
            this.Id = id + 1;
        }

        public void CreateMetric(string name, string startingValue, string endingValue, MetricType type) {
            SetId();
            Metric metric = new Metric(name, startingValue, endingValue, type);

            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                               Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void CreateMetric(Metric metric) {
            SetId();
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                               Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);

            LoadMetrics();
        }

        public Metric GetMetric(int id) {
            throw new NotImplementedException();
            //return metric;
        }

        public void UpdateMetric(int id, string name, string startingValue, string endingValue, MetricType type) {
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject oldMetric = new JObject();
            Metric metric = new Metric(name, startingValue, endingValue, type);

            JObject newMetric = BuildJObject(metric);
            try {
                foreach (JObject m in metrics) {
                    if ((int)m["id"] == id)
                        oldMetric = m;
                    else
                        throw new ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch(ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            oldMetric.Replace(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void UpdateMetric(Metric metric) {
            JArray metrics = new JArray(this.Metrics);
            JObject oldMetric = new JObject();

            JObject newMetric = BuildJObject(metric);
            foreach (JObject m in metrics) {
                if ((int)m["id"] == metric.Id)
                    oldMetric = m;
            }

            oldMetric.Replace(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void DeleteMetric(int id) {
            JArray metrics = new JArray(this.Metrics); //) metrics.ToObject<List<Metric>>();
            JObject oldMetric = new JObject();

            try {
                foreach (JObject m in metrics) {
                    if ((int)m["id"] == id)
                        oldMetric = m;
                    else
                        throw new ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch (ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            metrics.Remove(oldMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void DeleteMetric(Metric metric) {
            JArray metrics = new JArray(this.Metrics); //) metrics.ToObject<List<Metric>>();
            JObject oldMetric = new JObject();

            try {
                foreach (JObject m in metrics) {
                    if ((int)m["id"] == metric.Id)
                        oldMetric = m;
                    else
                        throw new ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch (ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            metrics.Remove(oldMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }
    }
}
