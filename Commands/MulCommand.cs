using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class MulCommand : Command
    {
        public static string name = "MUL";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "5", Code = "MUL", Length = 2 };

        public override LineData data => _data;
        private LineData _data;

        public MulCommand(LineData lineData)
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
        }
    }
}
