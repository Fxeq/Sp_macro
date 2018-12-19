using System;
using System.ComponentModel;
using sp_macro;

public interface Command
{
    LineData data { get; }
    void execute(BindingList<NameMacro> tableNMacro, BindingList<Variable> tableV, BindingList<BodyMacro> tableMacro, BindingList<Instruction> tom);
    bool checkLineData(LineData lineData);
}
