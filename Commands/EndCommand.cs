using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class EndCommand : Directive
    {
        public static string name = "END";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "END", Length = 0 };

        public override LineData data => _data;
        private LineData _data;

        public EndCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);
            if (lineData.args.isNotEmpty() && (!Utils.validAddress.IsMatch(lineData.args.get(0)) || Utils.ConvertTo16(lineData.args.get(0)) == -1)) throw new ArgumentException("Адрес точки входа должен быть равен 0");
            
            _data = lineData;
            return true;
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            tom.Add(new Instruction()
            {
                Name = data.directive,
                SymbolicName = "",
                Length = data?.args?.get(0),
                Code = data?.args?.get(1),
            });
        }
    }
}
