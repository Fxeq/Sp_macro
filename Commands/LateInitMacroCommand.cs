using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sp_macro;

namespace Commands
{
    public class LateInitMacroCommand : ICommand
    {

        private LineData _data = null;
        public LineData data => _data;

        public LateInitMacroCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData?.lable?.Equals(Config.getInstance().macroCommand?.data?.lable) == true) throw new ArgumentException("Обнаружено зацикливание");
            _data = lineData;
            return true;
        }

        public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            if (Config.getInstance().macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()}",
                });
                return;
            }
        }
    }
}
