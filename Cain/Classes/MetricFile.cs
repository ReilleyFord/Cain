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
 
        private List<Metric> Metrics { get; set; }
        private string       Path    { get; set; }
        private string       File    { get; set; }
        private JObject      JSON    { get; set; }
        private int          Id      { get; set; }

        public MetricFile(string path) {
            this.Path    = path;
            this.Metrics = new List<Metric>();

            if (System.IO.File.Exists(path))
                LoadMetricsFile(path);
            else
                CreateMetricsFile(path);

            this.Id = GetCurrentId();
            LoadMetrics();
        }

        private void CreateMetricsFile(string path) {
            this.Id = 0;

            JArray  metrics = new JArray();
            JObject obj     = new JObject();
            obj["Metrics"] = metrics;

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
            foreach(JToken metric in this.JSON.GetValue("Metrics")) {
                Metric newMetric = new Metric();
                newMetric.Id = (int)metric["id"];
                newMetric.Name = (string) metric["Name"];
                newMetric.StartingValue = (string) metric["Start Value"];
                newMetric.EndingValue = (string) metric["End Value"];

                this.Metrics.Add(newMetric);
            }
            this.Id = GetCurrentId();
        }

        private JObject BuildJObject(Metric metric) {
            JObject jObj = new JObject(
                new JProperty("id", this.Id),
                new JProperty("Name", metric.Name),
                new JProperty("Start Value", metric.StartingValue),
                new JProperty("End Value", metric.EndingValue)
                );
            return jObj;
        }

        public int GetCurrentId() {
            if (this.Id == 0)
                return this.Id;
            else {
                JArray metrics = (JArray)this.JSON["Metrics"];
                JToken lastMetric = metrics.Last;

                int id = (int)lastMetric["id"];

                return id;
            }
        }

        public void CreateMetric(string name, string startingValue, string endingValue) {
            this.Id++;
            Metric metric = new Metric(name, startingValue, endingValue);
            metric.Id = this.Id;

            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                               Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void CreateMetric(Metric metric) {
            this.Id++;
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject newMetric = BuildJObject(metric);
            metrics.Add(newMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                               Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);

            LoadMetrics();
        }

        public Metric GetMetric(int id) {
            Metric metric = new Metric();

            try {
                foreach (Metric m in this.Metrics) {
                    if (m.Id == id)
                        metric = m;
                    else
                        throw new System.ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch (System.ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            return metric;
        }

        public void UpdateMetric(int id, string name, string startingValue, string endingValue) {
            JArray metrics = (JArray)this.JSON["Metrics"];
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
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }

        public void DeleteMetric(int id) {
            JArray metrics = (JArray)this.JSON["Metrics"];
            JObject oldMetric = new JObject();

            try {
                foreach (JObject m in metrics) {
                    if ((int)m["id"] == id)
                        oldMetric = m;
                    else
                        throw new System.ArgumentOutOfRangeException("Error: That ID was not found");
                }
            } catch (System.ArgumentOutOfRangeException e) { Console.WriteLine(e.Message); }

            metrics.Remove(oldMetric);
            string newJson = JsonConvert.SerializeObject(this.JSON,
                   Formatting.Indented);
            System.IO.File.WriteAllText(this.Path, newJson);
            LoadMetrics();
        }
    }
}
