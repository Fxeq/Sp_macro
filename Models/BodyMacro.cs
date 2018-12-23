using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_macro
{
    public class BodyMacro
    {
        public int Number { get; set; }
        public string Body { get { return $"{data.lable?.ToString()} {(data.directive.isNotEmpty() ? data.directive?.ToString() : data.command?.ToString())} {(data.args.isNotEmpty() ? string.Join(" ", data.args) : "")}"; } }
        public LineData data { get; set; }
    }
}
