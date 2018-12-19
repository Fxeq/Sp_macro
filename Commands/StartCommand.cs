using sp_macro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class StartCommand : Command
    {
        public static string name = "START";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "START", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public StartCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.lable.isEmpty()) throw new ArgumentException("Неправильный формат первой строки. Отсутствует корректное имя модуля");
            if (lineData.directive.isEmpty() || !lineData.directive.Trim().Equals(StartCommand.name)) throw new ArgumentException("Неправильный формат первой строки. Отсутствует директива START");
            if (lineData.args.isEmpty() || !Utils.validAddress.IsMatch(lineData.args.get(0)) || Utils.ConvertTo16(lineData.args.get(0)) == -1) throw new ArgumentException("Неверный формат первой строки. Отсутствует корректный адрес загрузки\n");
            _data = lineData;
            return true;
        }

       public void execute(BindingList<NameMacro> tableNMacro, BindingList<Variable> tableV, BindingList<BodyMacro> tableMacro, BindingList<Instruction> tom)
        {

            tom.Add(new Instruction()
            {
                Name = data.lable,
                SymbolicName = "START",
                Length = Utils.StringFormatAddress(Convert.ToInt16(data.args.get(0)))
            });
            if (Config.getInstance().macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.directive.ToString()} {(data.args != null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }
        }
    }
}
