using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class LdCommand : Command
    {
        public static string name = "LD";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "9", Code = "LD", Length = 5 };

        public LineData data => _data;
        private LineData _data;

        public LdCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            _data = lineData;
            return true;
        }

       public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (Config.getInstance().macroMode){
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.command.ToString()} {(data.args!=null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
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
