using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class MendCommand : ICommand
    {
        public static string name = "MEND";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "MEND", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public MendCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.lable.isNotEmpty() || lineData.args.isNotEmpty()) throw new ArgumentException("Неправильный формат объявления директивы " + lineData.directive);
            _data = lineData;
            return true;
        }

        public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            Config config = Config.getInstance();
            if (config.stackIf.Count != 0)
                throw new ArgumentException("Не все ветви IF имеют ENDIF ");
            if (config.stackWhile.Count != 0)
                throw new ArgumentException("Не все WHILE имеют ENDW ");

            if (tableNMacro.ToList().Last().StartIndex != tableMacro.Count())
            {
                tableNMacro.ToList().Last().EndIndex = tableMacro.Count() - 1;
            }
            else
            {
                tableNMacro.ToList().Last().StartIndex = -1;
                tableNMacro.ToList().Last().EndIndex = -1;
            }
            config.macroMode = false;

            config.closeLastMacro();
        }
    }
}
