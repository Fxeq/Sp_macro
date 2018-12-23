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

        public virtual bool checkLineData(LineData lineData)
        {
            if (lineData == null) return false;

            if (lineData.lable.isNotEmpty() && Config.getInstance().macroMode)
            {
                throw new ArgumentException("Метка внутри макроса не поддерживается");
            }
            return false;
        }

        public virtual void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (Config.getInstance().macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.directive?.ToString()} {(data.args.isNotEmpty() ? string.Join(" ", data.args) : "")}",
                });
                return;
            }

            make(tableNMacro, tableV, tableMacro, tom);
        }

        internal abstract void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom);
    }
}
