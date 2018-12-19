using sp_macro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class WhileCommand : Command
    {
        public static string name = "WHILE";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "WHILE", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public WhileCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {

            if (lineData.args?.isEmpty() == true ||
                lineData.args?.Length != 3 ||
                lineData.lable?.isNotEmpty() == true ||
                !Utils.validOperation.IsMatch(lineData.args.get(1)))
            {
                throw new ArgumentException("Неправильный формат записи условия");
            }

            _data = lineData;
            return true;
        }

       public void execute(BindingList<NameMacro> tableNMacro, BindingList<Variable> tableV, BindingList<BodyMacro> tableMacro, BindingList<Instruction> tom)
        {
            Config config = Config.getInstance();
            if (config.macroMode)
            {
                tableMacro.Add(new BodyMacro()
                {
                    Number = tableMacro.Count(),
                    Body = $"{data.lable} {data.directive} {data.args[0]} {data.args.get(1)?.ToString()} {data.args.get(2)?.ToString()}",
                });
                return;
            }

            try
            {
                bool compare = Utils.Compare(getValue(data.args.get(0), tableV), getValue(data.args.get(2), tableV), data.args.get(1));
                config.stackWhile.Push(compare);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Условие невыполнимо");
            }
        }

        private int getValue(string name, IList<Variable> tableV)
        {
            string first = tableV.FirstOrDefault(i => i.Name == Utils.GetUniquePrefix(name))?.Value;
            if (first.isEmpty() == true) first = name;

            int firstValue = Utils.ConvertTo10(first);
            if (firstValue == -1) throw new ArgumentException($"Для {name} неопределено значение");

            return firstValue;
        }
    }
}
