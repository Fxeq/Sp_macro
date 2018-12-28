using Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sp_macro
{
    public class LineParser
    {
        public LineData parse(string line)
        {
            //if (line.Trim().Length == 0) throw new ArgumentException();
            int index = 0;
            LineData data = new LineData();
            string[] arguments = line.Trim().Split();
            
            if (CommandDefiner.isExistDirective(arguments[index])) data.directive = arguments[index++];
            else if (CommandDefiner.isExistCommand(arguments[index])) data.command = arguments[index++];
            
            if (arguments.Length > 1)
            {
                if (data.directive.isEmpty() && data.command.isEmpty())
                {
                    data.lable = arguments[index++];
                    if (CommandDefiner.isExistDirective(arguments[index]))
                    {
                        data.directive = arguments[index++];
                    }
                    else if (CommandDefiner.isExistCommand(arguments[index]))
                    {
                        {
                            data.command = arguments[index++];
                        }
                    }
                } 
            }

            if (data.command.isEmpty() && data.directive.isEmpty() && data.lable.isEmpty()) data.lable = arguments[index++];
            if (arguments.Length > index) data.args = arguments.subArray(index, arguments.Length);
            return data;
        }

    }
}
