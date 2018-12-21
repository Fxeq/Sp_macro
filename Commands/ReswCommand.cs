using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class ReswCommand : Directive
    {
        public static string name = "RESW";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "RESW", Length = 2 };

        public override LineData data => _data;
        private LineData _data;

        public ReswCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);
            if (lineData.args.Length > 1) throw new ArgumentException("Неправильный формат объявления директивы");
            _data = lineData;
            return true;
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            string varValue = null;
            if (tableV.Any(i => i.Name == Utils.GetUniquePrefix(data.args.get(0))))
            {
                string arg = data.args.get(0);
                varValue = int.Parse(tableV.First(i => i.Name == Utils.GetUniquePrefix(arg)).Value).ToString();
            }
            if (Utils.ConvertTo16(varValue) == -1)
            {
                throw new ArgumentException("Неправильно указан размер");
            }

            tom.Add(new Instruction()
            {
                Name = data.lable,
                SymbolicName = data.directive,
                Length = data.args?.get(0),
                Code = data.args?.get(1),
            });
        }
    }
}
