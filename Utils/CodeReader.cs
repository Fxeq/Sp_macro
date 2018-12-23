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

        private bool _isCodeReady = false;
        public bool isCodeReady { get { return _isCodeReady; } }

        private int _currentLine = -1;
        public int currentLine { get { return _currentLine; } set { _currentLine = value; } }

        private Dictionary<int, int> doneLines = new Dictionary<int, int>();

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
            {
                if (doneLines.ContainsKey(_currentLine - 1) && doneLines[_currentLine - 1] != -1)
                    _currentLine = doneLines[_currentLine - 1];

                return linesArray[_currentLine];
            }
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
            fixedLine = _currentLine - 1;
        }

        public void returnLine()
        {
            _currentLine = fixedLine;
        }

        public void saveStartIndexLine()
        {
            doneLines.Add(_currentLine - 1, -1);
        }

        public void saveLastIndexLine()
        {
           doneLines[doneLines.Last().Key] = _currentLine + 1;
        }
    }
}
