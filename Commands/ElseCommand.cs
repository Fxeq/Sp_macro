using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class ElseCommand : Directive
    {
        public static string name = "ELSE";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "ELSE", Length = 0 };

        public override LineData data => _data;
        private LineData _data;
        private  Config config = Config.getInstance();
        public ElseCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);

            if (lineData.args != null)
                if (lineData.args.get(1)?.isNotEmpty() == true || lineData.args.get(1)?.isNotEmpty() == true)
                    throw new ArgumentException("Неправильный формат записи директивы");
            if (lineData.lable?.isNotEmpty() == true)
                throw new ArgumentException("Неправильный формат записи директивы");
            
            _data = lineData;
            return true;
        }

        public override void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (config.macroMode && config.stackIf.isEmpty()) throw new ArgumentException($"Обнаружена директива {name}, но не обнаружено директивы IF");

            base.execute(tableNMacro, tableV, tableMacro, tom);
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom){
            Stack<bool> stack = config.stackIf;
            var value = stack.Pop();
            stack.Push(!value);
        }
    }
}
