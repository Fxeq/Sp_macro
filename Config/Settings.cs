namespace sp_macro
{
    using Commands;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Settings" />
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Defines the directives
        /// </summary>
        public static Dictionary<string, Command> directives = new Dictionary<string, Command>()
        {
            {ResbCommand.name, new ResbCommand() },
                {ReswCommand.name, new ReswCommand() },
                {ByteCommand.name, new ByteCommand() },
                {WordCommand.name, new WordCommand() },
                {EndCommand.name, new EndCommand() },
                {StartCommand.name, new StartCommand() },
                {MacroCommand.name, new MacroCommand() },
                {MendCommand.name, new MendCommand() },
                {IfCommand.name, new IfCommand() },
                {ElseCommand.name, new ElseCommand() },
                {EndifCommand.name, new EndifCommand() },
                {WhileCommand.name, new WhileCommand() },
                {EndwCommand.name, new EndwCommand() },
                {VarCommand.name, new VarCommand() },
                {SetCommand.name, new SetCommand() },
                {IncCommand.name, new IncCommand() },
                {DecCommand.name, new DecCommand() },
        };

        /// <summary>
        /// Defines the commands
        /// </summary>
        public static Dictionary<string, CommandModel> commands = new Dictionary<string, CommandModel>()
        {
               {"JMP", new CommandModel() { BinaryCode = "1", Code = "JMP", Length = 4 } },
               {"LOAD", new CommandModel() { BinaryCode = "2", Code = "LOAD", Length = 3 }},
               {"SAVE", new CommandModel() { BinaryCode = "3", Code = "SAVE", Length = 3 }},
               {"ADD", new CommandModel() { BinaryCode = "4", Code = "ADD", Length = 3 }},
               {"MUL", new CommandModel() { BinaryCode = "5", Code = "MUL", Length = 2 }},
               {"PUSH", new CommandModel() { BinaryCode = "6", Code = "PUSH", Length = 2 }},
               {"POP", new CommandModel() { BinaryCode = "7", Code = "POP", Length = 2 }},
               {"SUB", new CommandModel() { BinaryCode = "8", Code = "SUB", Length = 3 }},
               {"LD", new CommandModel() { BinaryCode = "9", Code = "LD", Length = 5 }},
               {"SUB_RA", new CommandModel() { BinaryCode = "A", Code = "SUB_RA", Length = 5 }},
               {"ADD_RA", new CommandModel() { BinaryCode = "B", Code = "ADD_RA", Length = 5 }},
        };
    }
}
