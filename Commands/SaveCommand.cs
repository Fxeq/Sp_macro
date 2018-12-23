namespace Commands
{
    using sp_macro;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SaveCommand : ICommand
    {
        public static string name = "SAVE";

        private CommandModel commandModel = new CommandModel() { BinaryCode = "3", Code = "SAVE", Length = 3 };

        public LineData data => _data;

        private LineData _data;

        public SaveCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.lable.isNotEmpty() && Config.getInstance().macroMode)
            {
                throw new ArgumentException("Метка внутри макроса не поддерживается");
            }
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
                    data = data,
                });
                return;
            }
        }
    }
}
