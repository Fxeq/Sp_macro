using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class JmpCommand : Command
    {
        public static string name = "JMP";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "1", Code = "JMP", Length = 4 };

        public override LineData data => _data;
        private LineData _data;

        public JmpCommand(LineData lineData)
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
            if (data?.args.get(0).isNotEmpty() == true && !Config.getInstance().unigueLabel.ContainsKey(data.args.get(0)))
            {
                throw new ArgumentException("Обнаружена неопределенная метка " + data?.args.get(0));
            }

            tom.Add(new Instruction()
            {
                Name = Utils.GetUniqueLabel(data.lable),
                SymbolicName = data.command,
                Length = Utils.GetUniqueLabel(data.args.get(0)),
                Code = data.args.get(1),
            });
        }
    }
}
