using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_macro
{
    public class CodeReader
    {
        private string[] linesArray;
        public int currentLine { get { return _currentLine;  } set { _currentLine = value; } }
        public bool isCodeReady { get { return _isCodeReady; } }
        private bool _isCodeReady = false;
        private int _currentLine = -1;
        private int fixedLine = -1;
        public string code {
            set {
                if (value.Trim().Length == 0) throw new ArgumentException("Необнаружено исходного текста");
                linesArray = value.Split('\n');
                _isCodeReady = true;
            }
        }
        public IList<string> codeLinesList {
            set {
                if (value.Count == 0) throw new ArgumentException("Необнаружено исходного текста");
                linesArray = value.ToArray();
                _isCodeReady = true;
            }
        }

        public string readNext() {
            if (++_currentLine < linesArray.Length)
                return linesArray[_currentLine];
            else
                throw new EofException();
        }

        public bool hasNext()
        {
            return currentLine + 1 < linesArray.Length;
        }
        public string readLine(int line) {
            if (line >= linesArray.Length) throw new ArgumentException("Недостижимая строка №" + line);
            return linesArray[line];
        }

        public void clear()
        {
            _isCodeReady = false;
            _currentLine = -1;
            linesArray = new string[] { };

        }

        public void fixIndexLine()
        {
            fixedLine = _currentLine;
        }
    }
}
