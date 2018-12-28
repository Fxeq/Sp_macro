using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sp_macro;

namespace Commands
{
    public abstract class Directive : ICommand
    {
        public abstract LineData data { get; }

        private Config config = Config.getInstance();

        public virtual bool checkLineData(LineData lineData)
        {
            if (lineData == null) return false;

            var label = lineData.lable;
            if (label.isNotEmpty())
            {
                if (!Utils.validMacroLable.IsMatch(label) && Config.getInstance().macroMode)
                {
                    throw new ArgumentException("Метка внутри макроса не поддерживается");
                }

                if (label.Length > 6) throw new ArgumentException("Неправильный формат ассемблерной метки");
            }


            //if (config.stackWhile.Count != 0 && label.isNotEmpty() && config.macroCommand?.data?.lable?.Equals(config.stack.Peek()) == true)
            //{
            //    throw new ArgumentException("Внутри цикла обнаружена ассемблерная метка");
            //}
            return false;
        }

        public virtual void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (config.macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    data = data
                });
                return;
            }

            make(tableNMacro, tableV, tableMacro, tom);
        }

        internal abstract void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom);
    }
}
