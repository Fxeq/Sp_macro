using sp_macro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class SetCommand : Command
    {
        public static string name = "SET";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "SET", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public SetCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.args.get(0)?.isEmpty() == true || lineData.args.get(1)?.isEmpty() == true || lineData.lable?.isNotEmpty() == true)
            {
                throw new ArgumentException("Неправильный формат записи");
            }
            _data = lineData;
            return true;
        }

       public void execute(BindingList<NameMacro> tableNMacro, BindingList<Variable> tableV, BindingList<BodyMacro> tableMacro, BindingList<Instruction> tom)
        {
            if (Config.getInstance().macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.directive.ToString()} {(data.args != null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }

                Variable val = tableV.FirstOrDefault(i => i.Name == Utils.GetUniquePrefix(data.args?.get(0)));
            if (val == null) throw new ArgumentException($"Переменная {data.args?.get(0)} неопределена");

            Variable argValue = tableV.FirstOrDefault(i => i.Name == Utils.GetUniquePrefix(data.args?.get(1)));

            
            int value;
            if (argValue != null)
            {
                value = Utils.ConvertTo10(val.Value);
            } else
            {
                value = Utils.ConvertTo10(data.args?.get(1));
            }

            if (value == -1) throw new ArgumentException($"Для переменной {data.args?.get(0)} определено неправильное значение");
            val.Value = val.ToString();
        }
    }
}
