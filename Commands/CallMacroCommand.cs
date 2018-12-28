using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sp_macro;

namespace Commands
{
    public class CallMacroCommand : Command
    {
        public override LineData data => _data;
        private LineData _data;

        private int _startMacro = 0;
        public int startMacro { get { return _startMacro; } }

        private int _endMacro = 0;
        public int endMacro { get { return _endMacro; } }

        public CallMacroCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }

        public override bool checkLineData(LineData lineData)
        {
            if (lineData.lable.isEmpty()) throw new ArgumentException("Обнаружен неправильный формат");
            if (lineData.args?.get(0)?.isNotEmpty() == true && !Utils.validArgKey.IsMatch(lineData.args.get(0))
                || lineData.args?.get(1)?.isNotEmpty() == true && !Utils.validArgKey.IsMatch(lineData.args.get(1))) throw new ArgumentException("Неправильный параметр");

            _data = lineData;
            return true;
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            Config config = Config.getInstance();

            if (!config.inProgress)
                tableV.Clear();

            var macro = tableNMacro.FirstOrDefault(i => i.Name == data.lable);
            if (macro == null)
            {
                throw new ArgumentException($"Макроопределение {data.lable} неопределенно");
            }

            config.stack.Push(macro.Name);

            _startMacro = macro.StartIndex;
            _endMacro = macro.EndIndex;
            int callMacroCount = macro.EndIndex;

            string mArg1 = macro.Arg1;
            string mArg2 = macro.Arg2;

            if (mArg1.isEmpty() && data.args?.get(0)?.isNotEmpty() == true
                || mArg2.isEmpty() && data.args?.get(1)?.isNotEmpty() == true
                || data.args?.get(0)?.isNotEmpty() == true && Utils.validArgKey.IsMatch(mArg1) && !Utils.validArgKey.IsMatch(data.args?.get(0))
                || data.args?.get(1)?.isNotEmpty() == true && Utils.validArgKey.IsMatch(mArg2) && !Utils.validArgKey.IsMatch(data.args?.get(1))
                )
            {
                throw new ArgumentException("Неправильные параметры были переданы в макрос");
            }

            //tableV.Add(buildVariable(mArg1, data.args?.get(0)));
            //var secondVar = buildVariable(mArg2, data.args?.get(1));
            //if (secondVar != null)
            //    tableV.Add(secondVar);

        }

        private Variable buildVariable(string varName, string arg)
        {
            if (arg.isEmpty() && varName.isEmpty()) return null;

            if (arg.isNotEmpty())
            {
                if (Utils.validArgKey.IsMatch(arg))
                {
                    arg = arg.Remove(0, 5);
                    varName = varName.Remove(4, varName.Length - 4);
                }
            }
            else if (varName.isNotEmpty() && Utils.validArgKey.IsMatch(varName))
            {
                arg = varName.Remove(0, 5);
                varName = varName.Remove(4, varName.Length - 4);
            }

            if (arg.isNotEmpty() || varName.isNotEmpty() && Utils.validArgKey.IsMatch(varName))
            {
                int val = Utils.ConvertTo10(arg);
                if (val == -1)
                {
                    throw new ArgumentException($"Для переменной в строке {startMacro} определенно неправильное значение\n");
                }

            }

            return new Variable()
            {
                Name = Utils.GetUniquePrefix(varName),
                Value = arg,
                Scope = Config.getInstance().stack.Peek(),
            };
        }
    }
}
