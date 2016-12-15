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
            var aclh = new AnnularColorLayoutHistogram();
            aclh.Extract(new Bitmap("D:\\Documents\\Pictures\\amazarashi.jpeg"));
        }
    }
}
