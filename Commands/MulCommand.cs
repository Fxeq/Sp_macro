﻿using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class MulCommand : ICommand
    {
        public static string name = "MUL";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "5", Code = "MUL", Length = 2 };

        public LineData data => _data;
        private LineData _data;

        public MulCommand(LineData lineData)
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
            if (Config.getInstance().macroMode){
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable?.ToString()} {data.command.ToString()} {(data.args!=null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }
        }
    }
}
