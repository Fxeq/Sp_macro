﻿namespace Commands
{
    using sp_macro;
    using System.ComponentModel;
    using System.Linq;

    public class SaveCommand : Command
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
            _data = lineData;
            return true;
        }

        public void execute(BindingList<NameMacro> tableNMacro, BindingList<Variable> tableV, BindingList<BodyMacro> tableMacro, BindingList<Instruction> tom)
        {
            if (Config.getInstance().macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.command.ToString()} {(data.args != null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }
        }
    }
}
