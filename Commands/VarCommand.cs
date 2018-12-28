using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class VarCommand : Directive
    {
        public static string name = "VAR";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "VAR", Length = 0 };

        public override LineData data => _data;
        private LineData _data;

        public VarCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);
            if (lineData.args.Length != 2)
                throw new ArgumentException("Неправильный формат записи директивы");
            if (lineData.args.get(0)?.isEmpty() == true )
            {
                throw new ArgumentException("Отсутсвует обязательное имя переменной");
            }
            if ( lineData.args.get(1)?.isEmpty() == true )
            {
                throw new ArgumentException("Неправильный формат записи директивы");
            }
            
            if (CommandDefiner.isExistCommand(lineData.args.get(0)) || CommandDefiner.isExistDirective(lineData.args.get(0)) || lineData.args.get(0).Length > 6)
                throw new ArgumentException("Некорректное имя переменной ");

            _data = lineData;
            return true;
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (tableNMacro.Any(i => i.Name == data.args?.get(0)?.ToString()))
                throw new ArgumentException("Некорректное имя переменной ");

            if (tableV.Any(i => i.Name == Utils.GetUniquePrefix(data.args?.get(0)?.ToString()))) throw new ArgumentException($"Переменная {data.args?.get(0)?.ToString()} уже определена в данном макроопределении {Config.getInstance().stack.Peek()}");

            string prefixName = Utils.GetUniquePrefix(data.args?.get(1));
            string val = tableV.FirstOrDefault(i => i.Name == prefixName)?.Value;

            if (val == null) val = data.args?.get(1);

            int value = Utils.ConvertTo10(val);
            if (value == -1)
            {
                throw new ArgumentException($"Для переменной {data.args?.get(0)} определенно неправильное значение\n");
            }

            var config = Config.getInstance();
            var variable = new Variable() { Name = Utils.GetUniquePrefix(data.args?.get(0)), Value = value.ToString(), Scope = config.stack.Peek(), };
            tableV.Add(variable);
            var variables = config.variables;
            if (!variables.ContainsKey(variable.Scope))
                variables.Add(variable.Scope, new List<Variable>() { variable });
            else
                variables[variable.Scope].Add(variable);
        }
    }
}
