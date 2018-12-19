using sp_macro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class EndCommand : Command
    {
        public static string name = "END";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "END", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public EndCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.args.isNotEmpty() && (!Utils.validAddress.IsMatch(lineData.args.get(0)) || Utils.ConvertTo16(lineData.args.get(0)) == -1)) throw new ArgumentException("Адрес точки входа должен быть равен 0");
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
                Name = data.directive,
                SymbolicName = "",
                Length = data?.args?.get(0),
                Code = data?.args?.get(1),
            });
        }
    }
}
