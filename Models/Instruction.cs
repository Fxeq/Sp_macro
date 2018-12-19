using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_macro
{
    public class Instruction
    {
        public string Name { get; set; }
        public string SymbolicName { get; set; }
        public string Length { get; set; }
        public string Code { get; set; }
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
    }
}
