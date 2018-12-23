using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public abstract class Command : ICommand
    {
        public abstract LineData data { get; }
        private Config config = Config.getInstance();

        public virtual bool checkLineData(LineData lineData)
        {
            if (lineData == null) return false;


            var label = lineData.lable;
            if (label.isNotEmpty() && !Utils.validMacroLable.IsMatch(label) && Config.getInstance().macroMode)
            {
                throw new ArgumentException("Метка внутри макроса не поддерживается");
            }


            if (config.stackWhile.Count != 0 && label.isNotEmpty())
            {
                throw new ArgumentException("Внутри цикла обнаружена ассемблерная метка");
            }

            return false;
        }

        public virtual void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (config.macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    data = data,
                });
                return;
            }
            make(tableNMacro, tableV, tableMacro, tom);
        }

        internal abstract void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom);
    }
}
