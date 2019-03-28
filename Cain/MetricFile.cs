using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Cain
{
    public class MetricFile
    {
 
        private List<Metric> _metrics { get; set; }
        private string _path { get; set; }
        private string _file { get; set; }
        private JObject _JSON { get; set; }
        private int _id { get; set; }

        public MetricFile(string path) {
            this._path = path;
            this._metrics = new List<Metric>();

            if (File.Exists(path)) {
                LoadMetricsFile(path);
            }
            else {
                CreateMetricsFile(path);
            }

            this._JSON = JObject.Parse(this._file);
            this._id = GetCurrentId();
            loadMetrics();

        }

        private void CreateMetricsFile(string path) {
            if (!File.Exists(path))
                File.Create(path);
        }

        private void LoadMetricsFile(string path) {
            try {
                this._file = File.ReadAllText(path);
            } catch(IOException) {
                throw;
            }

        }

        private void loadMetrics() { 
            foreach(JToken metric in this._JSON
                .GetValue("Metrics")) {
                Metric newMetric = new Metric();
                newMetric.Id = (int)metric["id"];
                newMetric.Name = (string) metric["Name"];
                newMetric.StartingValue = (string) metric["Start Value"];
                newMetric.EndingValue = (string) metric["End Value"];

                this._metrics.Add(newMetric);
            }
            this._id = GetCurrentId();
        }

        public List<Metric> GetMetrics() {
            return this._metrics;
        }

        public int GetCurrentId() {
            JArray metrics = (JArray)this._JSON["Metrics"];
            JToken lastMetric = metrics.Last;

            int id = (int)lastMetric["id"];

            return id;
        }

        public void CreateMetric(string name, string startingValue, string endingValue) {
            Metric metric = new Metric(this._id++, name, startingValue, endingValue);

            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject jObj = new JObject(
                new JProperty("id", this._id++),
                new JProperty("Name", metric.Name),
                new JProperty("Start Value", metric.StartingValue),
                new JProperty("End Value", metric.EndingValue)
                );
            metrics.Add(jObj);
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(this._JSON,
                               Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(this._path, newJsonResult);
            loadMetrics();
        }

        public void CreateMetric(Metric metric) {
            JArray metrics = (JArray) this._JSON["Metrics"];
            JObject jObj = new JObject(
                new JProperty("id", this._id++),
                new JProperty("Name", metric.Name),
                new JProperty("Start Value", metric.StartingValue),
                new JProperty("End Value", metric.EndingValue)
                );
            metrics.Add(jObj);
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(this._JSON,
                               Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(this._path, newJsonResult);

            loadMetrics();
        }

        public Metric GetMetric(int id) {
            Metric metric = new Metric();

            foreach(Metric m in this._metrics) {
                if (m.Id == id)
                    metric = m;
            }

            return metric;
        }

        public void UpdateMetric(int id, string name, string startingValue, string endingValue) {
            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject oldMetric = new JObject();

            JObject newMetric = new JObject(
                new JProperty("id", this._id++),
                new JProperty("Name", name),
                new JProperty("Start Value", startingValue),
                new JProperty("End Value", endingValue)
               );
            foreach (JObject m in metrics) {
                if ((int)m["id"] == id)
                    oldMetric = m;
            }

            oldMetric.Replace(newMetric);
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(this._JSON,
                   Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(this._path, newJsonResult);
            loadMetrics();
        }

        public void DeleteMetric(int id) {
            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject oldMetric = new JObject();

            foreach (JObject m in metrics) {
                if ((int)m["id"] == id)
                    oldMetric = m;
            }

            metrics.Remove(oldMetric);
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(this._JSON,
                   Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(this._path, newJsonResult);
            loadMetrics();
        }
    }
}
