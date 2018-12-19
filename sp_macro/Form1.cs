namespace sp_macro
{
    using Commands;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Models;

    public partial class mainForm : Form
    {
        public static readonly string CurrentDirectory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;

        public string inFile = CurrentDirectory + "\\input.txt";

        public string outFile = CurrentDirectory + "\\result.txt";

        public static string[] signs = { "==", ">=", "<=", "!=", ">", "<" };

        internal Regex validAddress = new Regex(@"^([0-9a-f]+)$");

        internal Regex validByteC = new Regex(@"^[cC](['""])(.+)\1$");

        internal Regex validByteX = new Regex(@"^[xX](['""])([a-fA-F0-9]+)\1$");

        internal Regex validCommandR = new Regex(@"^([A-Z]+)(_RA)$");

        internal Regex validCommand = new Regex(@"^([A-Z]+)$");

        internal Regex validName = new Regex(@"^([a-zA-Z]*)([0-9_]*)$");

        internal Regex validOperation = new Regex(@"^(([a-zA-Z0-9_]+)|([-0-9]+))((>)|(<)|(>=)|(<=)|(!=)|(==))(([a-zA-Z0-9_]+)|([-0-9]+))$");

        internal Regex validArg = new Regex(@"^((ARG)([0-9]+))(=)([0-9]+)$");

        internal Regex validArgKey = new Regex(@"^((ARG)([0-9]+))(=)([a-zA-Z0-9]+)$");

        internal int lineCount = 0, countWhile = 0, uniqueIndex = 0, maxCountLooping = 30;

        internal bool end = false, macro = false, modeWhile = false, isIf = false, modeIf = false;

        internal string tempCode = "";

        internal Stack<WhileStack> stackWhile;


        private Config config = Config.getInstance();
        internal Stack<bool> stackIf ;

        internal Stack<string> stackScope;

        internal BindingList<NameMacro> tableNMacro;

        internal BindingList<Variable> tableV;

        internal BindingList<BodyMacro> tableMacro;

        internal BindingList<Instruction> tom;

        internal Dictionary<string, string> header;

        internal Dictionary<string, string> ender;

        internal Dictionary<string, int> regs;

        internal Dictionary<string, string> unigueLabel;

        internal List<CommandModel> tableDirective;

        internal List<CommandModel> tableCommand;

        internal CodeReader codeReader;

        internal LineParser lineParser = new LineParser();

        internal CommandDefiner commandDefiner = new CommandDefiner();

        private StartCommand startCommand;

        public mainForm()
        {
            InitializeComponent();
            Init();
        }

        public void Res()
        {
            // init GUI
            err.ResetText();
            ts.ReadOnly = false;
            bpass.Enabled = true;
            bstep.Enabled = true;
            save.Enabled = false;
            open.Enabled = true;

            tom = new BindingList<Instruction>();
            tableV = new BindingList<Variable>();
            tableMacro = new BindingList<BodyMacro>();
            tableNMacro = new BindingList<NameMacro>();

            tm.DataSource = tableMacro;
            tv.DataSource = tableV;
            om.DataSource = tom;
            tim.DataSource = tableNMacro;

            end = false; macro = false; modeWhile = false; modeIf = false;
            lineCount = 0; countWhile = 0; uniqueIndex = 0;
            stackWhile = new Stack<WhileStack>();
            stackIf = new Stack<bool>();
            unigueLabel = new Dictionary<string, string>();
            stackScope = Config.getInstance().stack;

            codeReader.clear();
            //if (tempCode.isNotEmpty())
            //{
            //    ts.Text = tempCode;
            //    ClearSelectLine();
            //    SelectLine();
            //}
            //else
            //{
            //    err.AppendText("Введите исходный текст программы");
            //}
        }

        public void Init()
        {
            // init GUI
            err.ResetText();
            bpass.Enabled = true;
            bstep.Enabled = true;
            save.Enabled = false;

            // init data

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


            lineCount = 0; countWhile = 0; uniqueIndex = 0;
            end = false; macro = false; modeWhile = false; isIf = false; modeIf = false;
            stackWhile = new Stack<WhileStack>();
            stackIf = new Stack<bool>();
            stackScope = Config.getInstance().stack;
            tempCode = "";

            tm.DataSource = tableMacro;
            tim.DataSource = tableNMacro;
            tv.DataSource = tableV;
            om.DataSource = tom;
            codeReader = new CodeReader() { };

            ReadFile();
            SelectLine();
        }
        

        public void Preparation()
        {
            ts.ReadOnly = true;
            open.Enabled = false;
            codeReader = new CodeReader();
        }

        private void Reset(object sender, EventArgs e)
        {
            Res();
        }

        private void Pass(object sender, EventArgs e)
        {
            bpass.Enabled = false;
            bstep.Enabled = false;
            save.Enabled = false;
            open.Enabled = false;
            while (err.Text.isEmpty() && !end)
            {
                Step(sender, e);
            }
        }

        public void ReadFile()
        {
            StreamReader reader = new StreamReader(inFile);
            bool end = false;

            while (true)
            {
                if (reader.EndOfStream)
                {
                    err.AppendText("Встречен конец файла\n");
                    break;
                }

                string[] line = reader.ReadLine().Trim().Split();

                for (int i = 0; i < line.Length; ++i)
                {
                    var directive = lineParser.parse(string.Join("", line))?.directive;
                    if (directive.isEmpty() == true || directive?.Equals(EndCommand.name) == false)
                    {
                        if (i == line.Length - 1)
                        {
                            ts.AppendText(string.Format("{0}\n", line[i]));
                        }
                        else
                        {
                            ts.AppendText(string.Format("{0} ", line[i]));
                        }
                    }
                    else
                    {
                        if (i + 1 == line.Length)
                        {
                            ts.AppendText(string.Format("{0}\n", line[i]));
                        }
                        else
                        {
                            ts.AppendText(string.Format("{0} {1}\n", line[i], line[i + 1]));
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
                err.AppendText("Отсутствует директива END");
                bpass.Enabled = false;
                bstep.Enabled = false;
            }

            reader.Close();
        }

        private void SaveAssCode(object sender, EventArgs e)
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

        private void LoadFile(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = CurrentDirectory;
            open.Filter = "(*.txt)|*.txt";
            open.ShowDialog();
            if (open.FileName.isNotEmpty())
            {
                inFile = open.FileName;
                ts.ResetText();
                ReadFile();
            }
        }

        public void SelectLine()
        {
            ClearSelectLine();
            if (lineCount >= ts.Lines.Count()) return;

            int startFromIndex = ts.GetFirstCharIndexFromLine(lineCount);
            //Получаем длину строки
            int lineLength = ts.Lines[lineCount].Length + 1;
            //Выделяем текст с первого символа строки до конца строки
            ts.Select(startFromIndex, lineLength);
            //Устанавливаем выделенному тексту фон
            ts.SelectionBackColor = Color.DodgerBlue;
            ts.SelectionColor = Color.White;
        }

        public void ClearSelectLine()
        {
            //Снимаем выделение
            ts.Select(0, 8896);
            ts.SelectionBackColor = Color.White;
            ts.SelectionColor = Color.Black;
            ts.DeselectAll();
        }

        public void Renew()
        {
            om.Refresh();
            tv.Refresh();
            tim.Refresh();
            tm.Refresh();
        }
        
        private void Step(object sender, EventArgs e)
        {
            if (!codeReader.isCodeReady)
            {
                Preparation();
                codeReader.code = ts.Text;
                tempCode = ts.Text;
            }
           
            string tempName = "", tempCommand = "", tempArg1 = "", tempArg2 = "";

            //DoLineMain(ref tempName, ref tempCommand, ref tempArg1, ref tempArg2);


            string lineCode = "";
            try
            {
                lineCode = codeReader.readNext();
            } catch(EofException ex)
            {
                end = true;
                return;
            }

            if (lineCode.isEmpty()) return;

            LineData lineData = lineParser.parse(lineCode);


            try
            {
                Command command = commandDefiner.define(lineData);
                if (codeReader.currentLine == 0) {
                    if (command == null || !(command is StartCommand)) throw new ArgumentException("Не обнаружена директива START");
                    else stackScope.Push("main");
                }
                if (command == null)
                {
                    if (lineData.args.get(1)?.isNotEmpty() == true) throw new ArgumentException("Неправильный формат объявления директивы");
                    if (lineData.lable?.isNotEmpty() == true && Config.getInstance().macroMode) throw new ArgumentException("Макропроцессор не поддерживает макровызовы внутри макроопределений");
                    else if (lineData.lable?.isNotEmpty() == true && !Config.getInstance().macroMode) {

                    }
                    else throw new ArgumentException($"Макропроцессор не поддерживает директиву.");
                }

                command.execute(tableNMacro, tableV, tableMacro, tom);
                if (command is CallMacroCommand)
                {
                    CodeReader macroCodeReader = new CodeReader();
                    macroCodeReader.codeLinesList = tableMacro.startFrom((command as CallMacroCommand).startMacro).map(item => item.Body);
                    while (macroCodeReader.hasNext())
                    {
                        LineData macroLineData = lineParser.parse(macroCodeReader.readNext());
                        Command macroCommand = commandDefiner.define(macroLineData);

                        if (config.stackIf.isNotEmpty() && !(macroCommand is ElseCommand || macroCommand is EndifCommand)) {
                            if (config.stackIf.isNotEmpty() && !config.stackIf.Peek())
                            {
                                continue;
                            }
                        }

                        if (macroCommand is WhileCommand)
                            config.whileIndex = macroCodeReader.currentLine - 1;

                        if (config.stackWhile.isNotEmpty() && config.stackWhile.Peek() )
                        {
                            if (macroCommand is EndwCommand)
                            {
                                macroCodeReader.currentLine = config.whileIndex;
                                continue;
                            }
                        } else if (config.stackWhile.isNotEmpty() && !config.stackWhile.Peek() && !(macroCommand is EndwCommand))
                        {
                            continue;
                        }

                        macroCommand.execute(tableNMacro, tableV, tableMacro, tom);
                    }
                }

                if (CheckError()) return;
                lineCount++;
                SelectLine();
                Renew();
                return;
               
            }
            catch (Exception ex)
            {
                err.AppendText(ex.Message);
                err.AppendText(" Строка №" + (codeReader.currentLine + 1));
                err.AppendText("\n" + ex.StackTrace);
                bstep.Enabled = false;
                bpass.Enabled = false;
                save.Enabled = true;
                open.Enabled = true;
            }
        }

        public bool CheckError()
        {
            if (err.Text.isNotEmpty())
            {
                bstep.Enabled = false;
                bpass.Enabled = false;
                return true;
            }
            return false;
        }
    }
}
