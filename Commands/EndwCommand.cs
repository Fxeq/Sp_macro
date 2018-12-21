using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class EndwCommand : Directive
    {
        public static string name = "ENDW";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "ENDW", Length = 0 };

        public override LineData data => _data;
        private LineData _data;

        public EndwCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }

        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);
            if (lineData.args != null || lineData.lable?.isNotEmpty() == true)
            {
                throw new ArgumentException("Неправильный формат записи директивы");
            }
            _data = lineData;
            return true;
        }

        public override void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            var config = Config.getInstance();

            if (config.macroMode && config.stackWhile.isEmpty()) throw new ArgumentException($"Обнаружена директива {name}, но не обнаружено директивы WHILE");
            config.stackWhile.Pop();
            base.execute(tableNMacro, tableV, tableMacro, tom);
        }

        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            Config.getInstance().stackWhile.Pop();
        }
    }
}
