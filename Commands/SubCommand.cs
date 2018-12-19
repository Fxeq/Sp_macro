using sp_macro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class SubCommand : Command
    {
        public static string name = "SUB";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "8", Code = "SUB", Length = 3 };

        public LineData data => _data;
        private LineData _data;

        public SubCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            _data = lineData;
            return true;
        }

       public void execute(BindingList<NameMacro> tableNMacro, BindingList<Variable> tableV, BindingList<BodyMacro> tableMacro, BindingList<Instruction> tom)
        {
            if (Config.getInstance().macroMode){ 
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.directive.ToString()} {(data.args!=null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;

            }

            tom.Add(new Instruction()
            {
                Name = Utils.GetUniqueLabel(data.lable),
                SymbolicName = data.command,
                Length = data.args.get(0),
                Code = data.args.get(1),
            });
        }
    }
}
