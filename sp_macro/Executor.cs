using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Mathos.Parser;
using System.Reflection;
using System.Diagnostics;
using Models;
using Commands;
using System.ComponentModel;

namespace sp_macro
{
    public class Executor
    {
        public static readonly string CurrentDirectory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;

        public string inFile = CurrentDirectory + "\\input.txt";

        public string outFile = CurrentDirectory + "\\result.txt";

        internal int maxCountLooping = 30;

        internal bool end = false;


        private Config config = Config.getInstance();

        internal BindingList<NameMacro> tableNMacro { get; set; }

        internal BindingList<Variable> tableV { get; set; }

        internal BindingList<BodyMacro> tableMacro { get; set; }

        public BindingList<Instruction> tom { get; set; }

        internal Dictionary<string, string> header { get; set; }

        internal Dictionary<string, string> ender { get; set; }

        internal Dictionary<string, int> regs { get; set; }

        internal Dictionary<string, string> unigueLabel { get; set; }

        internal CodeReader codeReader;

        internal LineParser lineParser = new LineParser();

        internal CommandDefiner commandDefiner = new CommandDefiner();

        public StringBuilder ts = new StringBuilder();

        public Func<string> CodeSource;

        public string Code
        {
            set
            {
                if (codeReader != null)
                    codeReader.code = value;
            }
        }

        public Executor(string path)
        {
            inFile = path;
            Init();
        }

        public void Res()
        {

            tom.Clear();
            tableV.Clear();
            tableMacro.Clear();
            tableNMacro.Clear();

            ts.Clear();
            end = false;

            codeReader.clear();
            config.clear();
        }

        public void Init()
        {
            tom = new BindingList<Instruction>();
            tableV = new BindingList<Variable>();
            tableMacro = new BindingList<BodyMacro>();
            tableNMacro = new BindingList<NameMacro>();

            unigueLabel = new Dictionary<string, string>();
            regs = new Dictionary<string, int>()
            {
                { "R0", 00}, { "R1", 01}, { "R2", 02}, { "R3", 03}, { "R4", 04}, { "R5", 05}, { "R6", 06}, { "R7", 07},
                { "R8", 08}, { "R9", 09}, { "R10", 10}, { "R11", 11}, { "R12", 12}, { "R13", 13}, { "R14", 14}, { "R15", 15},
            };
            header = new Dictionary<string, string>()
            {
                {"name", "" }, {"address", ""}, {"length", ""}
            };
            ender = new Dictionary<string, string>()
            {
                {"address", ""}
            };
            ;
            end = false;

            codeReader = new CodeReader() { };

            codeReader.clear();
            config.clear();

            ReadFile();
        }


        public void Preparation()
        {
            codeReader = new CodeReader();
        }

        public void ReadFile()
        {
            StreamReader reader = new StreamReader(inFile);
            bool end = false;

            while (true)
            {
                if (reader.EndOfStream)
                {
                    throw new ArgumentException("Встречен конец файла\n");
                }

                string[] line = reader.ReadLine().Trim().Split();

                for (int i = 0; i < line.Length; ++i)
                {
                    var directive = lineParser.parse(string.Join("", line))?.directive;
                    if (directive.isEmpty() == true || directive?.Equals(EndCommand.name) == false)
                    {
                        if (i == line.Length - 1)
                        {
                            ts.Append(string.Format("{0}\n", line[i]));
                        }
                        else
                        {
                            ts.Append(string.Format("{0} ", line[i]));
                        }
                    }
                    else
                    {
                        if (i + 1 == line.Length)
                        {
                            ts.Append(string.Format("{0}\n", line[i]));
                        }
                        else
                        {
                            ts.Append(string.Format("{0} {1}\n", line[i], line[i + 1]));
                        }
                        end = true;
                        break;
                    }
                }

                if (end)
                {
                    break;
                }
            }

            if (!end)
            {
                throw new ArgumentException("Отсутствует директива END");
            }

            reader.Close();
        }

        public void SaveAssCode(object sender, EventArgs e)
        {
            if (end)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.InitialDirectory = CurrentDirectory;
                open.Filter = "(*.txt)|*.txt";
                open.ShowDialog();
                if (open.FileName.isNotEmpty())
                {
                    outFile = open.FileName;
                }
                try
                {
                    StreamWriter sw = new StreamWriter(outFile);
                    foreach (var line in tom)
                    {
                        sw.WriteLine(string.Format("{0} {1} {2} {3} {4} {5}", line.Name, line.SymbolicName, line.Length, line.Code, line.Arg1, line.Arg2));
                    }
                    sw.Close();
                    Console.WriteLine("\nЗапись успешна.\n");
                    Process.Start("notepad.exe", outFile);
                }
                catch
                {
                    Console.WriteLine("\nЗапись невозможна, так как не задан или не найден файл\n");
                }
            }
        }

        public void LoadFile(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = CurrentDirectory;
            open.Filter = "(*.txt)|*.txt";
            open.ShowDialog();
            if (open.FileName.isNotEmpty())
            {
                inFile = open.FileName;
                ts = new StringBuilder();
                ReadFile();
            }
        }



        public Message Pass(object sender, EventArgs e)
        {
            while (!end)
            {
                Step(sender, e);
            }

            return new Message(true, null, true);
        }


        public void Step(object sender, EventArgs e)
        {
            if (!codeReader.isCodeReady)
            {
                Preparation();
                ts.Clear();
                ts.Append(CodeSource());
                codeReader.code = ts.ToString();
            }
            try
            {
                string lineCode = "";
                lineCode = codeReader.readNext();

                if (lineCode.isEmpty()) return;

                LineData lineData = lineParser.parse(lineCode);
                executeLine(codeReader, lineData, false);
            }
            catch (EofException ex)
            {
                end = true;
                return;
            }
            catch (Exception ex)
            {
                var err = new StringBuilder();
                err.Append(ex.Message);
                err.Append(" Строка №" + (codeReader.currentLine + 1));
                err.Append("\n" + ex.StackTrace);
                throw new ArgumentException(err.ToString());

            }


        }

        private void executeLine(CodeReader codeReader, LineData lineData, bool isMacro)
        {
            ICommand command = commandDefiner.define(lineData);
            if (codeReader.currentLine == 0 && !isMacro)
            {
                if (command == null || !(command is StartCommand)) throw new ArgumentException("Не обнаружена директива START");
                else config.stack.Push("main");
            }
            if (command == null)
            {
                if (lineData.args.get(1)?.isNotEmpty() == true) throw new ArgumentException("Неправильный формат объявления директивы");
                if (lineData.lable?.isNotEmpty() == true && Config.getInstance().macroMode) throw new ArgumentException("Макропроцессор не поддерживает макровызовы внутри макроопределений");
                else if (lineData.lable?.isNotEmpty() == true && !Config.getInstance().macroMode)
                {

                }
                else throw new ArgumentException($"Макропроцессор не поддерживает директиву.");
            }

            if (config.stackIf.isNotEmpty() && !(command is ElseCommand || command is EndifCommand))
            {
                if (config.stackIf.isNotEmpty() && !config.stackIf.Peek())
                {
                    return;
                }
            }

            if (command is WhileCommand)
                config.whileIndex = codeReader.currentLine - 1;

            if (config.stackWhile.isNotEmpty() && config.stackWhile.Peek())
            {
                if (config.stackWhile.Count > 50) throw new ArgumentException("Обнаружен бесконечный цикл");
                if (command is EndwCommand)
                {
                    codeReader.currentLine = config.whileIndex;
                    return;
                }
            }
            else if (config.stackWhile.isNotEmpty() && !config.stackWhile.Peek() && !(command is EndwCommand))
            {
                return;
            }
            command.execute(tableNMacro, tableV, tableMacro, tom);
            if (command is CallMacroCommand && !config.macroMode)
            {
                CodeReader macroCodeReader = new CodeReader();
                macroCodeReader.codeLinesList = tableMacro.startFrom((command as CallMacroCommand).startMacro).map(item => item.Body);
                while (macroCodeReader.hasNext())
                {
                    LineData macroLineData = lineParser.parse(macroCodeReader.readNext());
                    executeLine(macroCodeReader, macroLineData, true);
                }
            }

            if (command is EndCommand)
            {
                end = true;
                // Res();
            }
        }

    }
}