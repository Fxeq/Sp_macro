﻿using sp_macro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class MacroCommand : ICommand
    {
        public static string name = "MACRO";
        private CommandModel commandModel = new CommandModel() { BinaryCode = "0", Code = "MACRO", Length = 0 };

        public LineData data => _data;
        private LineData _data;

        public MacroCommand(LineData lineData)
        {
            if (lineData != null)
                checkLineData(lineData);
        }
        public bool checkLineData(LineData lineData)
        {
            if (lineData.lable.isEmpty()) throw new ArgumentException("Отсутсвует обязательное имя макроса");
            if (lineData.args.isNotEmpty())
            {
                throw new ArgumentException("Макрос не поддерживает параметры");
                if (!Utils.validArg.IsMatch(lineData.args.get(0))) throw new ArgumentException("Неправильный формат объявления аргументов");
                if (lineData.args.Length > 1 && !Utils.validArg.IsMatch(lineData.args.get(1))) throw new ArgumentException("Неправильный формат объявления аргументов");
            }
            Config.getInstance().macroMode = true;
            _data = lineData;
            return true;
        }

       public void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom)
        {

            //string tempArg1Name = Utils.validArgKey.IsMatch(data?.args?.get(0)) ? data?.args?.get(0).Remove(4, data.args.get(0).Length - 4) : data?.args?.get(0);
            //string tempArg2Name = null;
            //if (data.args.Length > 1) tempArg2Name = Utils.validArgKey.IsMatch(data.args.get(1)) ? data.args.get(1).Remove(4, data.args.get(1).Length - 4) : data.args.get(1);

            //if (tempArg1Name == tempArg2Name) throw new ArgumentException ($"Параметры не могут иметь одинаковых имен");

            if (tableNMacro.Any(i => i.Name == data.lable))
            {
                throw new ArgumentException($"Имя макроса {data.lable} уже содердится в талбице макроопределений\n");
            }
            else
            {
                string arg2 = null;
                //if (data.args.Length == 2) arg2 = data.args.get(1);
                NameMacro nameMacro = new NameMacro() { Name = data.lable, StartIndex = tableMacro.Count(), EndIndex = -1};
                tableNMacro.Add(nameMacro);
                
                Config.getInstance().openMacro(this);
            }
        }
    }
}
