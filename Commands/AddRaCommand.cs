using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class AddRaCommand : Command
    {
        public static string name = "ADD_RA";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "B", Code = "ADD_RA", Length = 5 };

        public override LineData data => _data;
        private LineData _data;

        public AddRaCommand(LineData lineData)
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
