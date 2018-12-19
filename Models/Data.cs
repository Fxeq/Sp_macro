using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_macro
{
    public class LineData
    {
        public string lable { get; set; }
        public string directive { get; set; }
        public string command { get; set; }
        public string address { get; set; }
        public string[] args { get; set; }
    }
}
