using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class LoadCommand : Command
    {
        public static string name = "LOAD";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "2", Code = "LOAD", Length = 3 };

        public override LineData data => _data;
        private LineData _data;

        public LoadCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);
            foreach (string arg in lineData.args)
            {
                if (!Utils.isReg(arg)) throw new ArgumentException("Команда поддерживает только регистры");
            }
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
