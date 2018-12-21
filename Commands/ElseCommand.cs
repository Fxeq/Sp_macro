using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class ElseCommand : Command
    {
        public static string name = "ELSE";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "ELSE", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public ElseCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.args != null)
                if (lineData.args.get(1)?.isNotEmpty() == true || lineData.args.get(1)?.isNotEmpty() == true)
                    throw new ArgumentException("Неправильный формат записи директивы");
            if (lineData.lable?.isNotEmpty() == true)
                throw new ArgumentException("Неправильный формат записи директивы");

            _data = lineData;
            return true;
        }

        public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            var config = Config.getInstance();
            Stack<bool> stack = Config.getInstance().stackIf;
            if (config.macroMode){
                stack.Push(false);
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.directive.ToString()} {(data.args!=null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }

            var value = stack.Pop();
            stack.Push(!value);
        }
    }
}
