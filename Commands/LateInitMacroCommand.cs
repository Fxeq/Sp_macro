using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sp_macro;

namespace Commands
{
    public class LateInitMacroCommand : Directive
    {

        private LineData _data = null;
        public override LineData data => _data;

        public LateInitMacroCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            return base.checkLineData(lineData);
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
        }
    }
}
