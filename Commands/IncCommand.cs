using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class IncCommand : Directive
    {
        public static string name = "INC";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "INC", Length = 0 };

        public override LineData data => _data;
        private LineData _data;

        public IncCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }

        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);

            if (lineData.args.get(0)?.isEmpty() == true || lineData.args.get(1)?.isNotEmpty() == true || lineData.lable?.isNotEmpty() == true)
            {
                throw new ArgumentException("Неправильный формат записи директивы");
            }
            if (CommandDefiner.isExistCommand(lineData.args.get(0)) || CommandDefiner.isExistDirective(lineData.args.get(0)) || !Utils.validName.IsMatch(lineData.args.get(0)))
                throw new ArgumentException("Неправильное имя переменной " + lineData.args.get(0));
            
            _data = lineData;
            return true;
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            
            Variable variable = tableV.FirstOrDefault(i => i.Name == Utils.GetUniquePrefix(data.args.get(0)?.ToString()));
            if (variable == null)
                throw new ArgumentException("Переменная неопределена");
            try
            {
                int val = int.Parse(variable.Value) + 1;
                tableV.First(i => i.Name == Utils.GetUniquePrefix(data.args.get(0)?.ToString())).Value = val.ToString();
            }
            catch (Exception) { }

        }
    }
}
