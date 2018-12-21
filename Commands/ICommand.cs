using System;
using System.Collections.Generic;
using sp_macro;

public interface ICommand
{
    LineData data { get; }
    void execute(IList<NameMacro> tableNMacro, IList<Variable> tableV, IList<BodyMacro> tableMacro, IList<Instruction> tom);
    bool checkLineData(LineData lineData);
}
