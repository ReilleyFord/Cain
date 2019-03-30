using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
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

            if (File.Exists(path))
                LoadMetricsFile(path);
            else
                CreateMetricsFile(path);

            this._id = GetCurrentId();
            LoadMetrics();
        }

        private void CreateMetricsFile(string path) {
            this._id = 0;

            JArray metrics = new JArray();
            JObject obj = new JObject();
            obj["Metrics"] = metrics;

            File.WriteAllText(this._path, JsonConvert.SerializeObject(obj));
            LoadMetricsFile(path);
        }

        private void LoadMetricsFile(string path) {
            try {
                this._file = File.ReadAllText(path);
                this._JSON = JObject.Parse(this._file);
            } catch (IOException) { throw; }

        }

        private void LoadMetrics() { 
            
            foreach(JToken metric in this._JSON.GetValue("Metrics")) {
                Metric newMetric = new Metric();
                newMetric.Id = (int)metric["id"];
                newMetric.Name = (string) metric["Name"];
                newMetric.StartingValue = (string) metric["Start Value"];
                newMetric.EndingValue = (string) metric["End Value"];

                this._metrics.Add(newMetric);
            }
            this._id = GetCurrentId();
        }

        private JObject BuildJObject(Metric metric) {
            JObject jObj = new JObject(
                new JProperty("id", this._id),
                new JProperty("Name", metric.Name),
                new JProperty("Start Value", metric.StartingValue),
                new JProperty("End Value", metric.EndingValue)
                );
            return jObj;
        }

        public int GetCurrentId() {
            if (this._id == 0)
                return this._id;
            else {
                JArray metrics = (JArray)this._JSON["Metrics"];
                JToken lastMetric = metrics.Last;

                int id = (int)lastMetric["id"];

                return id;
            }
        }

        public void CreateMetric(string name, string startingValue, string endingValue) {
            this._id++;
            Metric metric = new Metric(name, startingValue, endingValue);
            metric.Id = this._id;

            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this._JSON,
                               Formatting.Indented);
            File.WriteAllText(this._path, newJson);
            LoadMetrics();
        }

        public void CreateMetric(Metric metric) {
            this._id++;
            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this._JSON,
                               Formatting.Indented);
            File.WriteAllText(this._path, newJson);

            LoadMetrics();
        }

        public Metric GetMetric(int id) {
            Metric metric = new Metric();

            try {
                foreach (Metric m in this._metrics) {
                    if (m.Id == id)
                        metric = m;
                    else
                        throw new System.ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch (System.ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            return metric;
        }

        public void UpdateMetric(int id, string name, string startingValue, string endingValue) {
            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject oldMetric = new JObject();
            Metric metric = new Metric(name, startingValue, endingValue);

            JObject newMetric = BuildJObject(metric);
            try {
                foreach (JObject m in metrics) {
                    if ((int)m["id"] == id)
                        oldMetric = m;
                    else
                        throw new System.ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch(System.ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            oldMetric.Replace(newMetric);
            string newJson = JsonConvert.SerializeObject(this._JSON,
                   Formatting.Indented);
            File.WriteAllText(this._path, newJson);
            LoadMetrics();
        }

        public void DeleteMetric(int id) {
            JArray metrics = (JArray)this._JSON["Metrics"];
            JObject oldMetric = new JObject();

            try {
                foreach (JObject m in metrics) {
                    if ((int)m["id"] == id)
                        oldMetric = m;
                                        else
                        throw new System.ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch (System.ArgumentOutOfRangeException e) {
                Console.WriteLine(e.Message);
            }

            metrics.Remove(oldMetric);
            string newJson = JsonConvert.SerializeObject(this._JSON,
                   Formatting.Indented);
            File.WriteAllText(this._path, newJson);
            LoadMetrics();
        }
    }
}
