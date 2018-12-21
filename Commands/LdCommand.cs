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

        public override LineData data => _data;
        private LineData _data;

        public LdCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);
            _data = lineData;
            return true;
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
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
