using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class WordCommand : Command
    {
        public static string name = "WORD";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "WORD", Length = 2 };

        public LineData data => _data;
        private LineData _data;

        public WordCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.args.Length > 1) throw new ArgumentException("Неправильный формат объявления директивы");
            _data = lineData;
            return true;
        }

        public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
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

            string lable = Utils.GetUniqueLabel(data.lable);
            tom.Add(new Instruction()
            {
                Name = lable,
                SymbolicName = data.directive,
                Length = data.args.get(0),
                Code = data.args.get(1),
            });
        }
    }
}
