using sp_macro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class VarCommand : Command
    {
        public static string name = "VAR";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "VAR", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public VarCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.args.get(0)?.isEmpty() == true )
            {
                throw new ArgumentException("Отсутсвует обязательное имя переменной");
            }
            if ( lineData.args.get(1)?.isEmpty() == true || lineData.lable?.isNotEmpty() == true)
            {
                throw new ArgumentException("Неправильный формат записи директивы");
            }

            if(CommandDefiner.isExistCommand(lineData.args.get(0)) || CommandDefiner.isExistDirective(lineData.args.get(0)))
                throw new ArgumentException("Некорректное имя переменной ");

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
            if (tableNMacro.Any(i => i.Name == data.args?.get(0)?.ToString()))
                throw new ArgumentException("Некорректное имя переменной ");

            if (tableV.Any(i => i.Name == Utils.GetUniquePrefix(data.args?.get(0)?.ToString()))) throw new ArgumentException($"Переменная {data.args?.get(0)?.ToString()} уже определена в данном макроопределении {Config.getInstance().stack.Peek()}");

            string prefixName = Utils.GetUniquePrefix(data.args?.get(1));
            string val = tableV.FirstOrDefault(i => i.Name == prefixName)?.Value;

            if (val == null) val = data.args?.get(1);

            int value = Utils.ConvertTo10(val);
            if (value == -1)
            {
                throw new ArgumentException($"Для переменной {data.args?.get(1)} определенно неправильное значение\n");
            }

            tableV.Add(new Variable() { Name = Utils.GetUniquePrefix(data.args?.get(0)), Value = value.ToString(), Scope = Config.getInstance().stack.Peek(), });
        }
    }
}
