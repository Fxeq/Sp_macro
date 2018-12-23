using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class AgoCommand : Directive
    {
        public static string name = "AGO";
        private LineData _data = null;
        public override LineData data => _data;

        public Func<int, int> changeLineIndex { get; set; }

        public AgoCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }

        public override bool checkLineData(LineData lineData)
        {
            base.checkLineData(lineData);

            if (lineData.args.isEmpty()) throw new ArgumentException("Неправильный формат записи директивы " + name);
            if (!Utils.validMacroLable.IsMatch(lineData.args.get(0))) throw new ArgumentException("Неправильный формат записи макро метки ");

            _data = lineData;
            return true;
        }
        internal override void make(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {
            Config config = Config.getInstance();

            var label = data.args.get(0);
            var macro = tableMacro.FirstOrDefault(item => item.data.lable?.Equals(label) == true);

            if (macro == null) throw new ArgumentException("Не найдено определение ассемблерной метки " + label);

            changeLineIndex(macro.Number);


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
