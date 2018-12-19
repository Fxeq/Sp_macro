using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class Config
    {
        private static Config INSTANCE = null;
        public Stack<string> stack { get; }
        public Stack<bool> stackIf { get; }
        public Stack<bool> stackWhile { get; }
        public int whileIndex { get; set; } = -1;
        public bool macroMode { get; set; } = false;
        public Dictionary<string, MacroCommand> macros = new Dictionary<string, MacroCommand>();
        private MacroCommand macroCommand = null;
        public Dictionary<string, string> unigueLabel { get; set; } = new Dictionary<string, string>();

        public static Config getInstance()
        {
            if (INSTANCE == null) INSTANCE = new Config();
            return INSTANCE;
        }

        private Config()
        {
            stack = new Stack<string>();
            stackIf = new Stack<bool>();
            stackWhile = new Stack<bool>();
            macroMode = false;
        }

        public void openMacro(MacroCommand macro)
        {
            macroCommand = macro;
        }

        public void closeLastMacro()
        {
            if (macroCommand != null)
            {
                macros[macroCommand.data.lable] = macroCommand;
                macroCommand = null;
            }
        }
    }
}
