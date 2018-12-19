namespace Commands
{
    using sp_macro;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ByteCommand : Command
    {
        public static string name = "BYTE";

        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "BYTE", Length = 1 };

        public LineData data => _data;

        private LineData _data;

        public ByteCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }

        public bool checkLineData(LineData lineData)
        {
            if (lineData.args.isEmpty() || lineData.args.Length > 1) throw new ArgumentException("Неверный формат сроки");
            if (!Regex.IsMatch(lineData.args.get(0), @"[cC](['""])(.+)\1")) throw new ArgumentException("Неверный формат выражения");

            //var byteC = ;

            //if (byteC.Count == 1)
            //{
            //    if (tempLine.Length == (byteC[0].Index + byteC[0].Length))
            //    {
            //        tempArg1 = tempLine.Substring(byteC[0].Index, byteC[0].Length);
            //        tempLine = tempLine.Substring(0, tempLine.Length - byteC[0].Length);
            //    }
            //    else
            //    {
            //        err.AppendText(string.Format("Неверный формат строки {0}\n", lineCount + 1));
            //        return;
            //    }
            //}
            //else if (byteC.Count > 1)
            //{
            //    err.AppendText(string.Format("Неверный формат строки {0}\n", lineCount + 1));
            //    return;
            //}
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
                    Body = $"{data.lable?.ToString()} {data.directive.ToString()} {(data.args != null ? data.args.get(0)?.ToString() : "")} {(data.args != null ? data.args.get(1)?.ToString() : "")}",
                });
                return;
            }
            
            tom.Add(new Instruction()
            {
                Name = Utils.GetUniqueLabel(data.lable),
                SymbolicName = data.directive,
                Length = data.args.get(0),
                Code = data.args.get(1),
            });
        }
    }
}
