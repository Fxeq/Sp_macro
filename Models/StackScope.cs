using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class StackScope
    {
        private static StackScope INSTANCE = null;
        public Stack<string> stack { get; }

        public static StackScope getInstance()
        {
            if (INSTANCE == null) INSTANCE = new StackScope();
            return INSTANCE;
        }

        private StackScope()
        {
            stack = new Stack<string>();
        }
    }
}
