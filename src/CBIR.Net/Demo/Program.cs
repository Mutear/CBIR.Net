using CBIR.Net.Feature;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ulbp = new UniformLBP();
            ulbp.Extract(new Bitmap("D:\\Documents\\Pictures\\cae0df47c844625dd4cfdb827c856b779ef96a57.jpg"));
            var ulbp2 = new UniformLBP();
            ulbp2.Extract(new Bitmap("D:\\Documents\\Pictures\\20799-1.jpg"));
            Console.WriteLine(ulbp.CalculateSimilarity(ulbp2));
            Console.ReadKey();
        }
    }
}
