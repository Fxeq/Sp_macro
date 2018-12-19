namespace Commands
{
    using sp_macro;
    using System;
    using System.Collections.Generic;

    public class CommandDefiner
    {
        private static LineData _lineData = null;

        public static Dictionary<string, Func<Command>> directives = new Dictionary<string, Func<Command>>()
        {
            {ResbCommand.name, () => new ResbCommand(_lineData) },
                {ReswCommand.name,  () =>new ReswCommand(_lineData) },
                {ByteCommand.name, () => new ByteCommand(_lineData) },
                {WordCommand.name, () => new WordCommand(_lineData) },
                {EndCommand.name, () => new EndCommand(_lineData) },
                {StartCommand.name, () => new StartCommand(_lineData) },
                {MacroCommand.name, () => new MacroCommand(_lineData) },
                {MendCommand.name, () => new MendCommand(_lineData) },
                {IfCommand.name, () => new IfCommand(_lineData) },
                {ElseCommand.name, () => new ElseCommand(_lineData) },
                {EndifCommand.name, () => new EndifCommand(_lineData) },
                {WhileCommand.name, () => new WhileCommand(_lineData) },
                {EndwCommand.name, () => new EndwCommand(_lineData) },
                {VarCommand.name, () => new VarCommand(_lineData) },
                {SetCommand.name, () => new SetCommand(_lineData) },
                {IncCommand.name, () => new IncCommand(_lineData) },
                {DecCommand.name, () => new DecCommand(_lineData) },
        };

        public static Dictionary<string, Func<Command>> commands = new Dictionary<string, Func<Command>>()
        {
               {JmpCommand.name,  () => new JmpCommand(_lineData)},
               {LoadCommand.name,  () => new LoadCommand(_lineData)},
               {SaveCommand.name,  () => new SaveCommand(_lineData)},
               {AddCommand.name,  () => new AddCommand(_lineData)},
               {MulCommand.name,  () => new MulCommand(_lineData)},
               {PushCommand.name,  () => new PushCommand(_lineData)},
               {PopCommand.name,  () => new PopCommand(_lineData)},
               {SubCommand.name,  () => new SubCommand(_lineData)},
               {LdCommand.name,  () => new LdCommand(_lineData)},
               {SubRaCommand.name,  () => new SubRaCommand(_lineData)},
               {AddRaCommand.name,  () => new AddRaCommand(_lineData)},
        };

        public static Dictionary<string, int> regs = new Dictionary<string, int>()
            {
                { "R0", 00}, { "R1", 01}, { "R2", 02}, { "R3", 03}, { "R4", 04}, { "R5", 05}, { "R6", 06}, { "R7", 07},
                { "R8", 08}, { "R9", 09}, { "R10", 10}, { "R11", 11}, { "R12", 12}, { "R13", 13}, { "R14", 14}, { "R15", 15},
            };

        public Command define(LineData lineData)
        {
            Command command = null;
            //_lineData = lineData;
            if (isExistDirective(lineData.directive)) command = directives[lineData.directive]();
            else if (isExistCommand(lineData.command)) command = commands[lineData.command]();
            else if (lineData.lable.isNotEmpty() && Config.getInstance().macros.ContainsKey(lineData.lable)) command = new CallMacroCommand(lineData);

            command?.checkLineData(lineData);
            return command;
        }

        public static bool isExistDirective(string directive)
        {
            return directive != null && directives.ContainsKey(directive);
        }

        public static bool isExistCommand(string command)
        {
            return command != null && commands.ContainsKey(command);
        }
    }
}
