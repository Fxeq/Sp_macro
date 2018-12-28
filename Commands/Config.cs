using sp_macro;
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
        private MacroCommand _macroCommand = null;
        public MacroCommand macroCommand { get { return _macroCommand; } }
        public Dictionary<string,List<Variable>> variables = new Dictionary<string, List<Variable>>();

        public Dictionary<string, string> unigueLabel { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, List<int>> lateInitMacros;
        public bool inProgress { get; set; } = false;

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
            lateInitMacros = new Dictionary<string, List<int>>();
            macroMode = false;
        }

        public void openMacro(MacroCommand macro)
        {
            _macroCommand = macro;
        }

        public void closeLastMacro()
        {
            if (_macroCommand != null)
            {
                macros[_macroCommand.data.lable] = _macroCommand;
                variables.Remove(_macroCommand.data.lable);
                //if (lateInitMacros.ContainsKey(macroCommand.data.lable))
                //{
                //    foreach (int item in lateInitMacros[macroCommand.data.lable])
                //        changeCursorListener(item);
                //}
                _macroCommand = null;
            }
        }

        public void clear()
        {
            stack.Clear();
            stackIf.Clear();
            stackWhile.Clear();
            whileIndex = -1;
            macroMode = false;
            inProgress = false;
            macros.Clear();
            _macroCommand = null;
            unigueLabel.Clear();
        }
    }
}
