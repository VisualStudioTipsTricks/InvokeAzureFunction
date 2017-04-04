using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvokeAzureFunction.Dto
{
    class Input
    {
        public string url { get; set; }
    }

    class ImageColors
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public double RedPercentage { get; set; }
        public double GreenPercentage { get; set; }
        public double BluePercentage { get; set; }
    }
}