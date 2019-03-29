using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cain {
    static class Program {

        static void Main() {
            string path = @"C:\Users\Reilley\Desktop\Cain\Exemplars\EX1940316 R v. SAURON\EX1940316 Case Notes FORD.docx";
            string JSON = "test.json";


            MetricFile metricFile = new MetricFile(JSON);

            //Metric metric = new Metric(3, "Hash", "Began Hashing", "Hashing Complete");
            //metricFile.CreateMetric(metric);
            CainLibrary cain = new CainLibrary();

            cain.ConvertDocxToENTable(path);
            //metricFile.CreateMetric("Test four", "STart test four", "End test four");

            List<Metric> metrics = metricFile.GetMetrics();


            //metricFile.UpdateMetric(5, "Test Five", "Start Test Five", "End Test Five");
            //Metric metric = metricFile.GetMetric(4);
            //Console.WriteLine("GetMetric: " + metric.Name);

            //metricFile.DeleteMetric(5);


            Console.ReadKey();

        }
    }
}
