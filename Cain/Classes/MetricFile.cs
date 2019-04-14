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

        public MetricFile() { }

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
            if(this.JSON == null)
                return;
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
                new JProperty("id", metric.Id),
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
            if (this.Metrics.Count == 0) {
                this.Id = 1;
                return;
            }
            int id = this.Metrics.Last().Id;
            this.Id = id + 1;
        }

        internal int GetCurrentId() {
            SetId();
            return this.Id;
        }

        public void CreateMetric(string name, string startingValue, string endingValue, MetricType type) {
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
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                               Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);

            LoadMetrics();
        }

        public Metric GetMetric(int id) {
            LoadMetrics();
            Metric metric = new Metric();
            foreach(Metric m in this.Metrics.Where(obj => obj.Id == id)) {
                metric = m;
            }
            return metric;
        }

        public void UpdateMetric(int id, string name, string startingValue, string endingValue, MetricType type) {
            Metric metric = new Metric(name, startingValue, endingValue, type);
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            JObject oldMetric = new JObject();

            foreach (JObject m in metrics.Where(obj => obj["id"].Value<int>() == metric.Id)) {
                oldMetric = m;
            }

            oldMetric.Replace(newMetric);
            this.JSON["Metrics"] = metrics;
            string newJson = JsonConvert.SerializeObject(this.JSON,
                    Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void UpdateMetric(Metric metric) {
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            JObject oldMetric = new JObject();

            foreach (JObject m in metrics.Where(obj => obj["id"].Value<int>() == metric.Id)) {
                oldMetric = m;
            }

            oldMetric.Replace(newMetric);
            this.JSON["Metrics"] = metrics;
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void DeleteMetric(int id) {
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject oldMetric = new JObject();

            foreach (JObject m in metrics.Where(obj => obj["id"].Value<int>() == id)) {
                oldMetric = m;
            }

            metrics.Remove(oldMetric);
            this.JSON["Metrics"] = metrics;
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void DeleteMetric(Metric metric) {
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject oldMetric = new JObject();

            foreach (JObject m in metrics.Where(obj => obj["id"].Value<int>() == metric.Id)) {
                oldMetric = m;
            }

            metrics.Remove(oldMetric);
            this.JSON["Metrics"] = metrics;
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }
    }
}
