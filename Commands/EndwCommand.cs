using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class EndwCommand : Command
    {
        public static string name = "ENDW";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "ENDW", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public EndwCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }

        public bool checkLineData(LineData lineData)
        {
            if (lineData.args != null || lineData.lable?.isNotEmpty() == true)
            {
                throw new ArgumentException("Неправильный формат записи директивы");
            }
            _data = lineData;
            return true;
        }

       public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            Config config = Config.getInstance();
            if (config.macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.directive.ToString()} {(data.args!=null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }
            config.stackWhile.Pop();

        }
    }
}
