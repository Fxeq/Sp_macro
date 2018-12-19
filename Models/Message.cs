using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_macro
{
    public class Message
    {
        public bool result;
        public bool IsEnd;
        public List<string> message;

        public Message(bool result, List<string> message)
        {
            this.result = result;
            this.message = message;
            IsEnd = false;
        }

        public Message(bool result, List<string> message, bool end)
        {
            this.result = result;
            this.message = message;
            IsEnd = end;
        }
    }
}
