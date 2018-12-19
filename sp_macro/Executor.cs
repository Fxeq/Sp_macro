using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace sp_macro
{
    public class Executor
    {
        // beginning of the definition of variables
        // 
        public static readonly string CurrentDirectory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;
        public string inFile = CurrentDirectory + "\\input.txt";
        public string outFile = CurrentDirectory + "\\result.txt";
        public static string[] signs = { "==", ">=", "<=", "!=", ">", "<" };
        Regex validAddress = new Regex(@"^([0-9a-f]+)$");
        Regex validByteC = new Regex(@"^[cC](['""])(.+)\1$");
        Regex validByteX = new Regex(@"^[xX](['""])([a-fA-F0-9]+)\1$");
        Regex validCommandR = new Regex(@"^([A-Z]+)(_RA)$");
        Regex validCommand = new Regex(@"^([A-Z]+)$");
        Regex validName = new Regex(@"^([a-zA-Z]*)([0-9_]*)$");
        Regex validOperation = new Regex(@"^(([a-zA-Z0-9_]+)|([-0-9]+))((>)|(<)|(>=)|(<=)|(!=)|(==))(([a-zA-Z0-9_]+)|([-0-9]+))$");
        Regex validArg = new Regex(@"^((ARG)([0-9]+))(=)([0-9]+)$");
        Regex validArgKey = new Regex(@"^((ARG)([0-9]+))(=)([a-zA-Z0-9]+)$");
        //
        int lineCount = 0, countWhile = 0, maxCountLooping = 30, uniqueIndex = 0;
        bool end = false, macro = false, modeWhile = false, isIf = false, modeIf = false;
        public bool fileOk;

        Stack<WhileStack> stackWhile;
        Stack<string> stackScope;
        Stack<bool> stackIf;
        //
        public List<NameMacro> tableNMacro;
        public List<Variable> tableV;
        public List<BodyMacro> tableMacro;
        public List<Instruction> tom;

        List<string> err;
        public List<string> ts;

        Dictionary<string, string> header;
        Dictionary<string, string> ender;
        Dictionary<string, int> regs;
        Dictionary<string, string> unigueLabel;
        List<CommandModel> tableDirective;
        List<CommandModel> tableCommand;
        //
        // end

        public Executor(string arg)
        {
            Init(arg);
        }

        public void Init(string arg)
        {
            // init data
            tableCommand = new List<CommandModel>()
            {
                new CommandModel() { BinaryCode = "1", Code = "JMP", Length = 4 },
                new CommandModel() { BinaryCode = "2", Code = "LOAD", Length = 3 },
                new CommandModel() { BinaryCode = "3", Code = "SAVE", Length = 3 },
                new CommandModel() { BinaryCode = "4", Code = "ADD", Length = 3 },
                new CommandModel() { BinaryCode = "5", Code = "MUL", Length = 2 },
                new CommandModel() { BinaryCode = "6", Code = "PUSH", Length = 2 },
                new CommandModel() { BinaryCode = "7", Code = "POP", Length = 2 },
                new CommandModel() { BinaryCode = "8", Code = "SUB", Length = 3 },
                new CommandModel() { BinaryCode = "9", Code = "LD", Length = 5 },
                new CommandModel() { BinaryCode = "A", Code = "SUB_RA", Length = 5 },
                new CommandModel() { BinaryCode = "B", Code = "ADD_RA", Length = 5 },
            };

            tom = new List<Instruction>();
            tableV = new List<Variable>();
            tableMacro = new List<BodyMacro>();
            tableNMacro = new List<NameMacro>();
            err = new List<string>();
            ts = new List<string>();

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
            tableDirective = new List<CommandModel>()
            {
                new CommandModel() { BinaryCode = "0", Code = "RESB", Length = 1 },
                new CommandModel() { BinaryCode = "0", Code = "RESW", Length = 2 },
                new CommandModel() { BinaryCode = "0", Code = "BYTE", Length = 1 },
                new CommandModel() { BinaryCode = "0", Code = "WORD", Length = 2 },
                new CommandModel() { BinaryCode = "0", Code = "END", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "START", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "MACRO", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "MEND", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "IF", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "ELSE", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "ENDIF", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "WHILE", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "ENDW", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "VAR", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "SET", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "INC", Length = 0 },
                new CommandModel() { BinaryCode = "0", Code = "DEC", Length = 0 },
            };


            lineCount = 0; countWhile = 0; maxCountLooping = 30; uniqueIndex = 0;
            end = false; macro = false; modeWhile = false; isIf = false; modeIf = false;
            stackWhile = new Stack<WhileStack>();
            stackIf = new Stack<bool>();
            stackScope = new Stack<string>();

            ReadFile(arg);
            if (IsError())
            {
                fileOk = false;
            }
            else
            {
                fileOk = true;
            }
        }

        public void ParseCondition(string str, out int first, out int second, out string sign)
        {
            string[] arr;
            first = 0;
            second = 0;
            sign = "";
            int temp;
            foreach (string sgn in signs)
            {
                if ((arr = str.Split(new string[] { sgn }, StringSplitOptions.None)).Length > 1)
                {
                    if (IsVarContains(arr[0]))
                    {
                        int val = GetVarValue(arr[0]);
                        if (val == -9999999)
                        {
                            err.Add(string.Format("В строке {0} неправильно значение переменной {1}\n", lineCount, arr[0]));
                        }
                        else
                        {
                            first = val;
                        }
                    }
                    else if (Int32.TryParse(arr[0], out temp) == false)
                    {
                        err.Add(string.Format("В строке {0} неправильно значение {1}\n", lineCount, arr[0]));
                    }
                    else
                    {
                        first = Int32.Parse(arr[0]);
                    }

                    if (IsVarContains(arr[1]))
                    {
                        int val = GetVarValue(arr[1]);
                        if (val == -9999999)
                        {
                            err.Add(string.Format("В строке {0} неправильно значение переменной {1}\n", lineCount, arr[1]));
                        }
                        else
                        {
                            second = val;
                        }
                    }
                    else if (Int32.TryParse(arr[1], out temp) == false)
                    {
                        err.Add(string.Format("В строке {0} неправильно значение {1}\n", lineCount, arr[1]));
                    }
                    else
                    {
                        second = Int32.Parse(arr[1]);
                    }

                    sign = sgn;
                    return;
                }
            }
            err.Add(string.Format("В строке {0} неправильно запись условия\n", lineCount));
        }

        public bool Compare(string str)
        {
            int first;
            int second;
            string sign;
            ParseCondition(str, out first, out second, out sign);
            switch (sign)
            {
                case ">":
                    return first > second;
                case "<":
                    return first < second;
                case ">=":
                    return first >= second;
                case "<=":
                    return first <= second;
                case "==":
                    return first == second;
                case "!=":
                    return first != second;
                default:
                    return false;
            }
        }

        public void ReadFile(string arg)
        {
            StreamReader reader = new StreamReader(arg);
            bool end = false;

            while (true)
            {
                if (reader.EndOfStream)
                {
                    err.Add("Встречен конец файла\n");
                    break;
                }

                string[] line = reader.ReadLine().Trim().Split();
                string newLine = "";

                for (int i = 0; i < line.Length; ++i)
                {
                    if (!IsEnd(line[i]))
                    {
                        if (i == line.Length - 1)
                        {
                            newLine += string.Format("{0}", line[i]);
                        }
                        else
                        {
                            newLine += string.Format("{0} ", line[i]);
                        }
                    }
                    else
                    {
                        if (i + 1 == line.Length)
                        {
                            newLine += string.Format("{0}", line[i]);
                        }
                        else
                        {
                            newLine += string.Format("{0} {1}", line[i], line[i + 1]);
                        }
                        end = true;
                        break;
                    }
                }

                ts.Add(newLine);

                if (end)
                {
                    break;
                }
            }

            if (!end)
            {
                err.Add("Отсутствует директива END");
            }

            reader.Close();
        }

        public bool IsName(string arg)
        {
            if (arg.Length <= 6 && (validName.IsMatch(arg) || validByteC.IsMatch(arg) || validByteX.IsMatch(arg)))
            {
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsArg(string arg)
        {
            if (arg.Length <= 10 && (validArg.IsMatch(arg)) && !IsReg(arg))
            {
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsArgKey(string arg)
        {
            if (arg.Length <= 10 && (validArgKey.IsMatch(arg)))
            {
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsCondition(string arg)
        {
            if (arg.Length <= 10 && validOperation.IsMatch(arg))
            {
                arg = arg.ToUpper();
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsNameValid(string arg)
        {
            if (arg.Length <= 6)
            {
                arg = arg.ToUpper();
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)))
                {
                    return true;
                }
            }
            return false;
        }

        public int ConvertTo16(string arg)
        {
            try
            {
                int res = Convert.ToInt32(arg, 16);
                return res;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int ConvertTo(string arg)
        {
            try
            {
                int res = Convert.ToInt32(arg, 10);
                return res;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool IsWordValid(int arg) => arg >= 0 && arg <= 65535;

        public string StringFormatAddress(int arg) => string.Format("{0:X}", arg).PadLeft(6, '0');

        public string StringFormatCommand(int arg) => string.Format("{0:X}", arg).PadLeft(2, '0');

        public bool IsByteC(string arg) => validByteC.IsMatch(arg);

        public bool IsByteX(string arg) => validByteX.IsMatch(arg);

        public bool IsReg(string arg) => IsFilled(arg) ? regs.Any(k => k.Key == arg) : false;

        public bool IsDirective(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg)) : false;

        public bool IsSet(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "SET") : false;

        public bool IsIf(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "IF") : false;

        public bool IsElse(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "ELSE") : false;

        public bool IsEndIf(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "ENDIF") : false;

        public bool IsWhile(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "WHILE") : false;

        public bool IsEndW(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "ENDW") : false;

        public bool IsEnd(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "END") : false;

        public bool IsMacroContains(string arg) => IsFilled(arg) ? tableNMacro.Any(i => i.Name == arg) : false;

        public NameMacro GetNameMacro(string arg)
        {
            var el = tableNMacro.FirstOrDefault(i => i.Name == arg);
            if (el?.StartIndex == -1)
            {
                return null;
            }
            return el;
        }

        public void SetEndMacroAddress()
        {
            if (tableNMacro.ToList().Last().StartIndex != tableMacro.Count())
            {
                tableNMacro.ToList().Last().EndIndex = tableMacro.Count() - 1;
            }
            else
            {
                tableNMacro.ToList().Last().StartIndex = -1;
                tableNMacro.ToList().Last().EndIndex = -1;
            }
        }

        public bool IsStart(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "START") : false;

        public bool IsCommand(string arg) => IsFilled(arg) ? tableCommand.Any(i => i.EqualsMnemonic(arg)) : false;

        public bool IsFilled(string arg) => arg?.Trim() != "";

        public bool IsError() => err.Count() != 0;

        public void ShowHeader()
        {
            tom.Add(new Instruction()
            {
                Name = header["name"],
                SymbolicName = "START",
                Length = header["address"],
            });
        }

        public void ShowEnder()
        {
            tom.Add(new Instruction()
            {
                Name = "END",
            });
        }

        public bool IsLabel(string arg)
        {
            if (arg.Length <= 6 && validName.IsMatch(arg))
            {
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)) && !IsReg(arg))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAddressValid(string arg)
        {
            arg = arg.ToUpper();
            if (validAddress.IsMatch(arg))
            {
                if (!tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ReadStart()
        {
            bool start = false, name = false, address = false;

            if (ts.Count() == 0)
            {
                err.Add("Введите исходный текст программы\n");
                return false;
            }

            for (; lineCount < ts.Count(); ++lineCount)
            {
                string tempLine = ts[lineCount].Trim();

                if (string.IsNullOrEmpty(tempLine))
                {
                    continue;
                }
                break;
            }

            string[] line = ts[lineCount].Trim().Split();
            line = line.ToList().FindAll(p => IsFilled(p)).Select(k => k.Trim(',')).ToArray();

            foreach (var el in line)
            {
                if (IsEnd(el))
                {
                    err.Add("Неправильный формат первой строки. Встречена директива End\n");
                    return false;
                }
                else if (IsLabel(el) && !name)
                {
                    header["name"] = el;
                    name = true;
                }
                else if (IsLabel(el) && !start)
                {
                    err.Add("Неправильный формат первой строки. После имени модуля должна следовать директива START\n");
                    return false;
                }
                else if (IsStart(el) && !name)
                {
                    err.Add("Неправильный формат первой строки. Отсутствует корректное имя модуля\n");
                    return false;
                }
                else if (IsStart(el) && !start)
                {
                    start = true;
                }
                else if (!start && IsAddressValid(el))
                {
                    err.Add("Неправильный формат первой строки. Отсутствует директива START\n");
                    return false;
                }
                else if (IsAddressValid(el) && !address)
                {
                    int load = ConvertTo16(el);
                    if (load == -1)
                    {
                        err.Add("Неверный формат адреса загрузки\n");
                        return false;
                    }
                    else
                    {
                        header["address"] = StringFormatAddress(load);
                        address = true;
                    }
                }
                else if (name && start && !address)
                {
                    err.Add("Неверный формат первой строки. Отсутствует корректный адрес загрузки\n");
                    return false;
                }
                else
                {
                    break;
                }
            }

            if (name && start && address)
            {
                lineCount++;
                ShowHeader();
                return true;
            }
            else if (!start)
            {
                err.Add("Неверный формат первой строки. Не была найдена директика START\n");
                return false;
            }
            else if (!address)
            {
                err.Add("Неверный формат первой строки. Отсутствует корректный адрес загрузки\n");
                return false;
            }
            else
            {
                return false;
            }
        }

        public Message Pass()
        {
            while (err.Count() == 0 && !end)
            {
                Step();
            }
            return IsError() ? new Message(false, err) : new Message(true, null, true);
        }

        public bool IsEnoughMemory(int arg)
            => tom.Count() == 0 || (tom.Count() > 0 && arg >= ConvertTo16(tom.First().SymbolicName) && arg <= ConvertTo16(tom.Last().SymbolicName));

        public bool IsMacro(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "MACRO") : false;

        public bool IsMend(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "MEND") : false;

        public bool IsVar(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "VAR") : false;

        public bool IsInc(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "INC") : false;

        public bool IsDec(string arg) => IsFilled(arg) ? tableDirective.Any(i => i.EqualsMnemonic(arg) && i.Code == "DEC") : false;

        public bool IsVarContains(string arg) => IsFilled(arg) ? tableV.Any(i => i.Name == GetUniquePrefix(arg)) : false;

        public bool IsVarValid(string arg)
            => arg.Length <= 6 && !tableDirective.Any(i => i.EqualsMnemonic(arg)) && !tableCommand.Any(i => i.EqualsMnemonic(arg)) && !tableNMacro.Any(i => i.Name == arg);

        public int GetVarValue(string arg)
        {
            if (IsFilled(tableV.First(i => i.Name == GetUniquePrefix(arg)).Value))
            {
                try
                {
                    int val = Int32.Parse(tableV.First(i => i.Name == GetUniquePrefix(arg)).Value);
                    return val;
                }
                catch (Exception) { }
            }
            return -9999999;
        }

        public void IncValue(string arg)
        {
            if (IsFilled(tableV.First(i => i.Name == GetUniquePrefix(arg)).Value))
            {
                try
                {
                    int val = Int32.Parse(tableV.First(i => i.Name == GetUniquePrefix(arg)).Value) + 1;
                    SetVar(arg, val.ToString());
                }
                catch (Exception) { }
            }
        }

        public void DecValue(string arg)
        {
            if (IsFilled(tableV.First(i => i.Name == GetUniquePrefix(arg)).Value))
            {
                try
                {
                    int val = Int32.Parse(tableV.First(i => i.Name == GetUniquePrefix(arg)).Value) - 1;
                    SetVar(arg, val.ToString());
                }
                catch (Exception) { }
            }
        }

        public void SetVar(string arg, string val)
        {
            if (IsVarContains(arg))
            {
                tableV.First(i => i.Name == GetUniquePrefix(arg)).Value = val;
            }
            else
            {
                err.Add($"Переменная {arg} не определена в данной области {stackScope.Peek()}");
            }
        }

        private string GetUniquePrefix(string arg = "") => $"{stackScope.Peek()}_{stackScope.Count()}_{arg}";

        public string GetUniqueLabel(string arg) => unigueLabel.ContainsKey(GetUniquePrefix(arg)) ? unigueLabel.First(i => i.Key == GetUniquePrefix(arg)).Value : "";

        private string GenerateUniqueLabel(string arg) => IsFilled(arg) && IsLabel(arg) ? $"{GetUniquePrefix(arg)}.{uniqueIndex++}" : "";

        private bool SetUniqueLabel(string arg)
        {
            if (!IsFilled(arg)) return true;
            else
            {
                string keyArg = GetUniquePrefix(arg);
                if (!unigueLabel.Any(i => i.Key == keyArg))
                {
                    unigueLabel.Add(keyArg, GenerateUniqueLabel(arg));
                    return true;
                }
            }
            return false;
        }

        private void SaveAssCode(object sender, EventArgs e)
        {
            if (end)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.InitialDirectory = CurrentDirectory;
                open.Filter = "(*.txt)|*.txt";
                open.ShowDialog();
                if (IsFilled(open.FileName))
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

        private void DoMacro(string tempName, string tempCommand, string tempArg1, string tempArg2, ref int index, int inc = 0)
        {
            if (IsMend(tempCommand))
            {
                if (IsFilled(tempName) || IsFilled(tempArg1) || IsFilled(tempArg2))
                {
                    err.Add($"Неправильный формат объявления директивы в строке {index + inc}\n");
                }
                else
                {
                    macro = false;
                    SetEndMacroAddress();
                }
            }
            else if (IsMacro(tempCommand))
            {
                if (!IsFilled(tempName))
                {
                    err.Add($"Отсутсвует обязательное имя макроса в строке {index + inc}\n");
                }
                else if ((IsFilled(tempArg2) && !IsArg(tempArg2)) || (IsFilled(tempArg1) && !IsArg(tempArg1)))
                {
                    err.Add($"Неправильный формат объявления аргументов в строке {index + inc}\n");
                }
                else
                {
                    string tempArg1Name = IsArgKey(tempArg1) ? tempArg1.Remove(4, tempArg1.Length - 4) : tempArg1;
                    string tempArg2Name = IsArgKey(tempArg2) ? tempArg2.Remove(4, tempArg2.Length - 4) : tempArg2;
                    if (tempArg1Name == tempArg2Name)
                    {
                        err.Add($"Параметры не могут иметь одинаковых имен в строке {index + inc}\n");
                        return;
                    }
                    if (IsMacroContains(tempName))
                    {
                        err.Add($"Имя макроса в строке {index + inc} уже содердится в талбице макроопределений\n");
                    }
                    else
                    {
                        tableNMacro.Add(new NameMacro() { Name = tempName, StartIndex = tableMacro.Count(), EndIndex = -1, Arg1 = tempArg1, Arg2 = tempArg2, });
                    }
                }
            }
            else
            {
                if (IsCommand(tempCommand))
                {
                    tableMacro.Add(new BodyMacro()
                    {
                        Number = tableMacro.Count(),
                        Body = $"{tempName} {tempCommand} {tempArg1} {tempArg2}",
                    });
                }
                else if (IsDirective(tempCommand))
                {
                    if (IsVar(tempCommand) || IsSet(tempCommand))
                    {
                        if (!IsFilled(tempArg1) || !IsFilled(tempArg2) || IsFilled(tempName))
                        {
                            err.Add($"Неправильный формат записи директивы в строке {index + inc}\n");
                        }
                    }
                    else if (IsIf(tempCommand) || IsWhile(tempCommand) || IsInc(tempCommand) || IsDec(tempCommand))
                    {
                        if (!IsFilled(tempArg1) || IsFilled(tempArg2) || IsFilled(tempName))
                        {
                            err.Add($"Неправильный формат записи директивы в строке {index + inc}\n");
                        }
                    }
                    else if (IsElse(tempCommand) || IsEndIf(tempCommand) || IsEndW(tempCommand))
                    {
                        if (IsFilled(tempArg1) || IsFilled(tempArg2) || IsFilled(tempName))
                        {
                            err.Add($"Неправильный формат записи директивы в строке {index + inc}\n");
                        }
                    }
                    else if (IsFilled(tempArg2))
                    {
                        err.Add($"Неправильный формат объявления директивы в строке {index + inc}\n");
                    }

                    if (IsError()) return;

                    tableMacro.Add(new BodyMacro()
                    {
                        Number = tableMacro.Count(),
                        Body = $"{tempName} {tempCommand} {tempArg1} {tempArg2}"
                    });
                }
                else
                {
                    if (IsFilled(tempName))
                    {
                        err.Add($"Макропроцессор не поддерживает макровызовы внутри макроопределений. Ошибка в строке {index + inc}\n");
                    }
                    else
                    {
                        err.Add($"Макропроцессор не поддерживает директиву в строке {index + inc}");
                    }
                }
            }
        }

        private void DoDirective(string tempCommand, ref string tempArg1, int index)
        {
            var directive = tableDirective.Find(k => k.EqualsMnemonic(tempCommand));
            if (directive.EqualsMnemonic("RESB") || directive.EqualsMnemonic("RESW"))
            {
                if (IsVarContains(tempArg1))
                {
                    tempArg1 = GetVarValue(tempArg1).ToString();
                }
                if (ConvertTo16(tempArg1) == -1)
                {
                    err.Add(string.Format("В строке {0} неправильно указан размер\n", index));
                }
            }
            else if (directive.EqualsMnemonic("BYTE"))
            {
                if (!IsByteC(tempArg1) && !IsByteX(tempArg1))
                {
                    err.Add(string.Format("Неправильный формат операнда в строке {0}\n", index));
                }
            }
            else if (directive.EqualsMnemonic("WORD"))
            {
                if (IsVarContains(tempArg1))
                {
                    tempArg1 = GetVarValue(tempArg1).ToString();
                }
                int memory = ConvertTo16(tempArg1);
                if (memory != -1)
                {
                    if (!IsWordValid(memory))
                    {
                        err.Add(string.Format("Размер аргумента в строке {0} не может быть WORD\n", index));
                    }
                }
                else
                {
                    err.Add(string.Format("В строке {0} неправильно указан размер\n", index));
                }
            }
            else
            {
                err.Add(string.Format("В строке {0} обнаружена недопустимая директива\n", index));
            }
        }

        private void DoCommand(string tempCommand, ref string tempArg1, ref string tempArg2, int index, bool isMacro = false)
        {
            if (IsVarContains(tempArg1))
            {
                tempArg1 = GetVarValue(tempArg1).ToString();
            }
            if (IsVarContains(tempArg2))
            {
                tempArg2 = GetVarValue(tempArg2).ToString();
            }
            var command = tableCommand.ToList().Find(k => k.EqualsMnemonic(tempCommand));
            int size = command.Length - 1, type = 0;
            switch (size)
            {
                case 1:
                    {
                        if (!IsFilled(tempArg1))
                        {
                            err.Add($"В строке отсутствует {index} обязательный операнд регистр\n");
                        }
                        else if (!IsReg(tempArg1))
                        {
                            err.Add($"Команда в строке {index} поддерживает в качестве операнда только регистр\n");
                        }
                        else if (IsFilled(tempArg2))
                        {
                            err.Add($"Неправильный формат в строке {index}\n");
                        }
                        else
                        {
                            type = 0;
                        }
                        break;
                    }
                case 2:
                    {
                        if (!IsFilled(tempArg1) || !IsFilled(tempArg2))
                        {
                            err.Add($"В строке отсутствует {index} обязательный операнд\n");
                        }
                        else if (!IsReg(tempArg1) || !IsReg(tempArg2))
                        {
                            err.Add($"Команда в строке {index} поддерживает в качестве операнда только регистр\n");
                        }
                        else
                        {
                            type = 0;
                        }
                        break;
                    }
                case 3:
                    {
                        if (!IsFilled(tempArg1))
                        {
                            err.Add($"В строке отсутствует {index} обязательный операнд\n");
                        }
                        else if (!IsName(tempArg1) || IsReg(tempArg1))
                        {
                            err.Add($"Команда в строке {index} поддерживает в качестве операнда только символическое имя\n");
                        }
                        else if (IsFilled(tempArg2))
                        {
                            err.Add($"Неправильный формат в строке {index}\n");
                        }
                        else
                        {
                            type = 1;
                            if (isMacro)
                            {
                                if (!IsFilled(GetUniqueLabel(tempArg1)))
                                {
                                    err.Add(string.Format("В строке {0} обнаружена неопределенная метка {1}", index, tempArg1));
                                    return;
                                }
                                tempArg1 = GetUniqueLabel(tempArg1);
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        if (!IsFilled(tempArg1) || !IsFilled(tempArg2))
                        {
                            err.Add($"В строке отсутствует {index} обязательный операнд\n");
                        }
                        else if (!IsName(tempArg2) || IsReg(tempArg2))
                        {
                            err.Add($"Команда в строке {index} поддерживает в качестве второго операнда только символическое имя\n");
                        }
                        else if (!IsReg(tempArg1))
                        {
                            err.Add($"Команда в строке {index} поддерживает в качестве первого операнда только регистр\n");
                        }
                        else
                        {
                            type = 1;
                            if (isMacro)
                            {
                                if (!IsFilled(GetUniqueLabel(tempArg2)))
                                {
                                    err.Add($"В строке {index} обнаружена неопределенная метка {tempArg2}");
                                    return;
                                }
                                tempArg2 = GetUniqueLabel(tempArg2);
                            }
                        }
                        break;
                    }
                default:
                    {
                        err.Add($"Команда в строке {index} не поддерживается в данной реализации ассемблирующей программы\n");
                        break;
                    }
            }

            if (validCommandR.IsMatch(tempCommand))
            {
                if (type == 0)
                {
                    err.Add($"Выявлена ошибка в строке {index}. Относительная адресация невозможна при использовании регистров\n");
                }
                else
                {
                    type = 2;
                }
            }
        }

        private void DoLineMain(ref string tempName, ref string tempCommand, ref string tempArg1, ref string tempArg2)
        {
            bool haveLabel = false, haveCommand = false;

            if (string.IsNullOrEmpty(ts[lineCount].Trim())) err.Add($"Макропроцессор не поддерживает пустые строки в исходном тексте. Ошибка в строке {lineCount} обнаружена пустая строка\n");

            if (IsError()) return;

            if (lineCount >= ts.Count() && !end) err.Add("Отсутствует директива END\n");

            if (IsError()) return;

            string tempLine = ts[lineCount].Trim();
            var byteC = Regex.Matches(tempLine, @"[cC](['""])(.+)\1");
            if (byteC.Count == 1)
            {
                if (tempLine.Length == (byteC[0].Index + byteC[0].Length))
                {
                    tempArg1 = tempLine.Substring(byteC[0].Index, byteC[0].Length);
                    tempLine = tempLine.Substring(0, tempLine.Length - byteC[0].Length);
                }
                else
                {
                    err.Add(string.Format("Неверный формат строки {0}\n", lineCount + 1));
                    return;
                }
            }
            else if (byteC.Count > 1)
            {
                err.Add(string.Format("Неверный формат строки {0}\n", lineCount + 1));
                return;
            }

            string[] line = tempLine.Trim().Split();
            line = line.ToList().FindAll(p => IsFilled(p)).Select(k => k.Trim(',')).ToArray();

            for (int l = 0; l < line.Length; ++l)
            {
                if (IsEnd(line[l]))
                {
                    end = true;
                    if (l + 1 < line.Length)
                    {
                        if (IsAddressValid(line[l + 1]))
                        {
                            int load = ConvertTo16(line[l + 1]);
                            if (!IsEnoughMemory(load))
                            {
                                err.Add("Адрес точки входа за пределами модуля\n");
                            }
                            else
                            {
                                ender["address"] = StringFormatAddress(load);
                            }
                        }
                        else
                        {
                            err.Add("Адрес точки входа должен быть равен 0\n");
                        }
                    }
                    else
                    {
                        if (IsFilled(header["address"]))
                        {
                            ender["address"] = header["address"];
                        }
                        else
                        {
                            err.Add("Адрес точки входа не определен\n");
                        }
                    }
                    break;
                }
                else if ((IsLabel(line[l])) && !haveLabel)
                {
                    haveLabel = true;
                    tempName = line[l];
                }
                else if ((IsArg(line[l])) && haveLabel)
                {
                    if (IsFilled(tempArg1) && IsFilled(tempArg2))
                    {
                        err.Add(string.Format("Неправильный формат записи в строке {0}\n", lineCount + 1));
                        break;
                    }
                    haveCommand = true;
                    if (!IsFilled(tempArg1))
                    {
                        tempArg1 = line[l];
                    }
                    else if (!IsFilled(tempArg2))
                    {
                        tempArg2 = line[l];
                    }
                }
                else if ((IsLabel(line[l])) && !haveCommand)
                {
                    err.Add(string.Format("Неправильный формат записи в строке {0}\n", lineCount + 1));
                    break;
                }
                else if (IsDirective(line[l]) && !haveCommand)
                {
                    if (IsEnd(line[l]) || IsStart(line[l]))
                    {
                        err.Add($"Недопустимая директива {line[l]} в строке {lineCount + 1}");
                        break;
                    }
                    else
                    {
                        if (IsMacro(line[l]) && !macro)
                        {
                            macro = true;
                        }
                        else if (IsMacro(line[l]))
                        {
                            err.Add($"Недопустимая директива {line[l]} в строке {lineCount + 1}");
                            break;
                        }
                        haveCommand = true;
                        haveLabel = true;
                        tempCommand = line[l];

                    }
                }
                else if (IsCommand(line[l]) && !haveCommand)
                {
                    haveCommand = true;
                    haveLabel = true;
                    tempCommand = line[l];
                }
                else if (IsCondition(line[l]))
                {
                    if (!IsFilled(tempArg1))
                    {
                        tempArg1 = line[l];
                    }
                    else
                    {
                        err.Add($"Неправильный формат записи в строке {lineCount}\n");
                    }
                }
                else if ((IsName(line[l]) || IsArg(line[l])) && !IsFilled(tempArg1))
                {
                    tempArg1 = line[l];
                }
                else if ((IsName(line[l]) || IsArg(line[l])) && !IsFilled(tempArg2))
                {
                    tempArg2 = line[l];
                }
                else
                {
                    err.Add($"В строке {lineCount + 1} выявлен неправильный формат\n");
                }
            }
        }

        private void DoLine(ref string tempName, ref string tempCommand, ref string tempArg1, ref string tempArg2, ref int index)
        {
            bool haveLabel = false, haveCommand = false;
            string tempLine = tableMacro[index].Body.Trim();
            var byteC = Regex.Matches(tempLine, @"[cC](['""])(.+)\1");
            if (byteC.Count == 1)
            {
                if (tempLine.Length == (byteC[0].Index + byteC[0].Length))
                {
                    tempArg1 = tempLine.Substring(byteC[0].Index, byteC[0].Length);
                    tempLine = tempLine.Substring(0, tempLine.Length - byteC[0].Length);
                }
                else
                {
                    err.Add($"Неверный формат строки {index}\n");
                    return;
                }
            }
            else if (byteC.Count > 1)
            {
                err.Add($"Неверный формат строки {index}\n");
                return;
            }

            string[] line = tempLine.Trim().Split();
            line = line.ToList().FindAll(p => IsFilled(p)).Select(k => k.Trim(',')).ToArray();

            for (int l = 0; l < line.Length; ++l)
            {
                if ((IsLabel(line[l])) && !haveLabel)
                {
                    haveLabel = true;
                    tempName = line[l];
                }
                else if ((IsLabel(line[l])) && !haveCommand)
                {
                    err.Add($"Неправильный формат записи в строке {index}\n");
                    break;
                }
                else if (IsDirective(line[l]) && !haveCommand)
                {
                    if (IsEnd(line[l]) || IsStart(line[l]) || IsMacro(line[l]))
                    {
                        err.Add($"Недопустимая директива {line[l]} в строке {index}");
                        break;
                    }
                    else
                    {
                        haveCommand = true;
                        haveLabel = true;
                        tempCommand = line[l];
                    }
                }
                else if (IsCommand(line[l]) && !haveCommand)
                {
                    haveCommand = true;
                    haveLabel = true;
                    tempCommand = line[l];
                }
                else if (IsCondition(line[l]))
                {
                    if (!IsFilled(tempArg1))
                    {
                        tempArg1 = line[l];
                    }
                    else
                    {
                        err.Add($"Неправильный формат записи в строке {index}\n");
                    }
                }
                else if ((IsName(line[l])) && !IsFilled(tempArg1))
                {
                    tempArg1 = line[l];
                }
                else if ((IsName(line[l])) && !IsFilled(tempArg2))
                {
                    tempArg2 = line[l];
                }
                else
                {
                    err.Add($"В строке {index} выявлен неправильный формат\n");
                }
            }
        }

        private void CallMacro(string name, string arg1, string arg2)
        {
            var element = GetNameMacro(name);
            stackScope.Push(name);
            int startMacro = element.StartIndex;
            int callMacroCount = element.EndIndex;
            string mArg1 = element.Arg1;
            string mArg2 = element.Arg2;
            if ((!IsFilled(mArg1) && IsFilled(arg1))
                || (!IsFilled(mArg2) && IsFilled(arg2))
                || (IsArgKey(mArg1) && !IsArgKey(arg1) && IsFilled(arg1))
                || (IsArgKey(mArg2) && !IsArgKey(arg2) && IsFilled(arg2)))
            {
                err.Add($"Неправильные параметры были переданы в макрос в строке {startMacro}");
            }
            else
            {
                string varName = element.Arg1;
                if (IsFilled(arg1))
                {
                    if (IsArgKey(arg1))
                    {
                        arg1 = arg1.Remove(0, 5);
                        varName = varName.Remove(4, varName.Length - 4);
                    }
                }
                else if (IsArgKey(mArg1))
                {
                    arg1 = varName.Remove(0, 5);
                    varName = varName.Remove(4, varName.Length - 4);
                }

                if (IsFilled(arg1) || IsArgKey(mArg1))
                {
                    int val = ConvertTo(arg1);
                    if (val == -1)
                    {
                        err.Add($"Для переменной в строке {startMacro} определенно неправильное значение\n");
                        return;
                    }
                    tableV.Add(new Variable()
                    {
                        Name = GetUniquePrefix(varName),
                        Value = arg1,
                        Scope = stackScope.Peek(),
                    });
                }

                varName = element.Arg2;
                if (IsFilled(arg2))
                {
                    if (IsArgKey(arg2))
                    {
                        arg2 = arg2.Remove(0, 5);
                        varName = varName.Remove(4, varName.Length - 4);
                    }
                }
                else if (IsArgKey(mArg2))
                {
                    arg2 = varName.Remove(0, 5);
                    varName = varName.Remove(4, varName.Length - 4);
                }

                if (IsFilled(arg2) || IsArgKey(mArg2))
                {
                    int val = ConvertTo(arg2);
                    if (val == -1)
                    {
                        err.Add($"Для переменной в строке {startMacro} определенно неправильное значение\n");
                        return;
                    }
                    tableV.Add(new Variable()
                    {
                        Name = GetUniquePrefix(varName),
                        Value = arg2,
                        Scope = stackScope.Peek(),
                    });
                }
            }

            for (int index = startMacro; index <= callMacroCount; ++index)
            {
                string tempName = "", tempCommand = "", tempArg1 = "", tempArg2 = "";

                DoLine(ref tempName, ref tempCommand, ref tempArg1, ref tempArg2, ref index);

                if (IsError()) return;

                if (macro)
                {
                    DoMacro(tempName, tempCommand, tempArg1, tempArg2, ref index);
                    if (IsError()) return;
                    continue;
                }
                else
                {
                    if (IsInc(tempCommand) || IsDec(tempCommand))
                    {
                        if (!IsFilled(tempArg1) && IsFilled(tempArg2))
                        {
                            err.Add($"Неправильная запись в строке {index} \n");
                        }
                        else if (!IsName(tempArg1))
                        {
                            err.Add($"Неправильное имя переменной {index} в строке {tempArg1} \n");
                        }
                        else if (!IsVarContains(tempArg1))
                        {
                            err.Add($"Переменная в строке {index} неопределена\n");
                        }
                        else
                        {
                            if (IsInc(tempCommand))
                            {
                                IncValue(tempArg1);
                            }
                            else
                            {
                                DecValue(tempArg1);
                            }
                        }
                    }
                    else if (IsVar(tempCommand))
                    {
                        if (IsFilled(tempArg1))
                        {
                            if (!IsVarValid(tempArg1))
                            {
                                err.Add($"Некорректное имя переменной в строке {index}\n");
                            }
                            else if (!IsVarContains(tempArg1))
                            {
                                if (IsFilled(tempArg2))
                                {
                                    if (IsVarContains(tempArg2))
                                    {
                                        tempArg2 = GetVarValue(tempArg2).ToString();
                                    }
                                    int val = ConvertTo(tempArg2);
                                    if (val == -1)
                                    {
                                        err.Add($"Для переменной в строке {index} определенно неправильное значение\n");
                                        return;
                                    }
                                }
                                tableV.Add(new Variable() { Name = GetUniquePrefix(tempArg1), Value = tempArg2, Scope = stackScope.Peek(), });
                            }
                            else
                            {
                                err.Add($"Переменная в строке {index} уже определена в данном макроопределении {stackScope.Peek()}\n");
                            }
                        }
                        else
                        {
                            err.Add($"Отсутсвует обязательное имя переменной в строке {index}\n");
                        }
                    }
                    else if (IsSet(tempCommand))
                    {
                        if (IsFilled(tempArg1) && IsFilled(tempArg2))
                        {
                            if (IsVarContains(tempArg1))
                            {
                                int val = ConvertTo(tempArg2);
                                if (val == -1)
                                {
                                    err.Add($"Для переменной в строке {index} определенно неправильное значение\n");
                                }
                                SetVar(tempArg1, tempArg2);
                            }
                            else
                            {
                                err.Add(string.Format("Переменная в строке {0} неопределена\n", index));
                            }
                        }
                        else
                        {
                            err.Add(string.Format("Неправильный формат записи в строке {0}\n", index));
                        }
                    }
                    else if (IsIf(tempCommand))
                    {
                        if (IsFilled(tempArg1) && !IsFilled(tempArg2))
                        {
                            if (!validOperation.IsMatch(tempArg1))
                            {
                                err.Add(string.Format("Неправильный формат записи условия в строке {0}\n", index));
                            }
                            else
                            {
                                isIf = Compare(tempArg1);
                                stackIf.Push(isIf);
                                modeIf = true;
                                if (!isIf)
                                {
                                    for (; index <= callMacroCount; ++index)
                                    {
                                        string tempL = tableMacro[index].Body.Trim();
                                        if (!tempL.Contains("ELSE") && !tempL.Contains("ENDIF"))
                                        {
                                            if (index == callMacroCount)
                                            {
                                                err.Add($"Отсутствует директива ELSE/ENDIF для IF в строке {index}\n");
                                                break;
                                            }
                                            continue;
                                        }
                                        index--;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            err.Add($"Неправильный формат записи в строке {index}\n");
                        }
                    }
                    else if (IsElse(tempCommand))
                    {
                        if (!IsFilled(tempArg1) && !IsFilled(tempArg2))
                        {
                            if (!modeIf)
                            {
                                err.Add(string.Format("Встречена директива {0} в строке {1} прежде директивы IF\n", tempCommand, index));
                            }
                            else
                            {
                                if (stackIf.Peek())
                                {
                                    for (++index; index <= callMacroCount; ++index)
                                    {
                                        string tempL = tableMacro[index].Body.Trim();
                                        if (!tempL.Contains("ENDIF"))
                                        {
                                            if (index == callMacroCount)
                                            {
                                                err.Add(string.Format("Отсутствует директива ENDIF для IF в строке {0}\n", index));
                                                break;
                                            }
                                            continue;
                                        }
                                        index--;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            err.Add(string.Format("Неправильный формат записи в строке {0}\n", index));
                        }
                    }
                    else if (IsEndIf(tempCommand))
                    {
                        if (!IsFilled(tempArg1) && !IsFilled(tempArg2))
                        {
                            if (!modeIf)
                            {
                                err.Add(string.Format("Встречена директива {0} в строке {1} прежде директивы IF\n", tempCommand, index));
                            }
                            else
                            {
                                if (stackIf.Count() > 1)
                                {
                                    stackIf.Pop();
                                    isIf = stackIf.Peek();
                                }
                                else if (stackIf.Count() == 1)
                                {
                                    isIf = stackIf.Pop();
                                    isIf = false;
                                    modeIf = false;
                                }
                            }
                        }
                        else
                        {
                            err.Add($"Неправильный формат записи в строке {index}\n");
                        }
                    }
                    else if (IsWhile(tempCommand))
                    {
                        if (IsFilled(tempArg1) && !IsFilled(tempArg2))
                        {
                            if (validOperation.IsMatch(tempArg1))
                            {
                                if (countWhile == maxCountLooping)
                                {
                                    err.Add(string.Format("Обнаружен бесконечный цикл в строке {0}\n", index));
                                    return;
                                }
                                isIf = Compare(tempArg1);
                                stackWhile.Push(new WhileStack() { start = index, isTrue = isIf });
                                modeWhile = true;
                                countWhile++;
                                if (!isIf)
                                {
                                    for (int countAll = 0; index <= callMacroCount; ++index)
                                    {
                                        string tempL = tableMacro[index].Body.Trim();
                                        if (tempL.Contains("WHILE"))
                                        {
                                            countAll++;
                                        }
                                        else if (tempL.Contains("ENDW"))
                                        {
                                            countAll--;
                                            if (countAll == 0)
                                            {
                                                index--;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (index == callMacroCount)
                                            {
                                                err.Add(string.Format("Отсутствует директива ENDW для WHILE в строке {0}\n", index));
                                                break;
                                            }
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                err.Add(string.Format("Неправильный формат записи условия в строке {0}\n", index));
                            }
                        }
                        else
                        {
                            err.Add($"Неправильный формат записи в строке {index}\n");
                        }
                    }
                    else if (IsEndW(tempCommand))
                    {
                        if (!IsFilled(tempArg1) && !IsFilled(tempArg2))
                        {
                            if (modeWhile)
                            {
                                if (stackWhile.Count() > 1)
                                {
                                    if (!stackWhile.Peek().isTrue)
                                    {
                                        stackWhile.Pop();
                                        isIf = stackWhile.Peek().isTrue;
                                    }
                                    else
                                    {
                                        var el = stackWhile.Pop();
                                        isIf = el.isTrue;
                                        index = el.start - 1;
                                    }
                                }
                                else if (!stackWhile.Peek().isTrue)
                                {
                                    isIf = stackWhile.Pop().isTrue;
                                    modeWhile = false;
                                }
                                else
                                {
                                    var el = stackWhile.Pop();
                                    isIf = el.isTrue;
                                    index = el.start - 1;
                                }
                            }
                            else
                            {
                                err.Add($"Встречена директива {tempCommand} в строке {index} прежде директивы WHILE\n");
                            }
                        }
                        else
                        {
                            err.Add($"Неправильный формат записи в строке {index}\n");
                        }
                    }
                    else if (IsDirective(tempCommand))
                    {
                        DoDirective(tempCommand, ref tempArg1, index);
                        if (IsError()) return;

                        if (!SetUniqueLabel(tempName)) err.Add($"В строке {index} обнаружена неуникальная метка {tempName}");
                        if (IsError()) return;

                        tom.Add(new Instruction()
                        {
                            Name = GetUniqueLabel(tempName),
                            SymbolicName = tempCommand,
                            Length = tempArg1,
                            Code = tempArg2,
                        });
                    }
                    else if (IsCommand(tempCommand))
                    {
                        DoCommand(tempCommand, ref tempArg1, ref tempArg2, index, true);
                        if (IsError()) return;

                        if (!SetUniqueLabel(tempName)) err.Add($"В строке {index} обнаружена неуникальная метка {tempName}");
                        if (IsError()) return;

                        tom.Add(new Instruction()
                        {
                            Name = GetUniqueLabel(tempName),
                            SymbolicName = tempCommand,
                            Length = tempArg1,
                            Code = tempArg2,
                        });
                    }
                    else
                    {
                        if (IsFilled(tempName))
                        {
                            if ((IsFilled(tempArg1) && (!IsArgKey(tempArg1))) || (IsFilled(tempArg2) && (!IsArgKey(tempArg2))))
                            {
                                err.Add($"Неправильный параметр в строке {index}\n");
                            }
                            else if (!IsMacroContains(tempName))
                            {
                                err.Add($"Макроопределение {tempName} неопределенно");
                            }
                            else
                            {
                                var el = GetNameMacro(tempName);
                                if (el != null)
                                {
                                    if (IsArgKey(tempArg1))
                                    {
                                        if (IsVarContains(tempArg1.Remove(0, 5)))
                                        {
                                            tempArg1 = tempArg1.Remove(5, tempArg1.Length - 5) + GetVarValue(tempArg1.Remove(0, 5));
                                        }
                                    }

                                    if (IsArgKey(tempArg2))
                                    {
                                        if (IsVarContains(tempArg2.Remove(0, 5)))
                                        {
                                            tempArg2 = tempArg2.Remove(5, tempArg2.Length - 5) + GetVarValue(tempArg2.Remove(0, 5));
                                        }
                                    }

                                    CallMacro(el.Name, tempArg1, tempArg2);
                                }
                            }
                        }
                        else
                        {
                            err.Add($"В строке {index} обнаружен неправильный формат\n");
                        }
                    }

                    if (IsError()) return;
                }
            }

            if (stackWhile.Count() != 0) err.Add(string.Format("Не вce WHILE имеют ENDW"));
            if (stackIf.Count() != 0) err.Add(string.Format("Не все ветви IF имеют ENDIF"));
            if (IsError()) return;

            var vars = tableV.Where(i => i.Name.StartsWith(GetUniquePrefix())).ToList();
            var uniqueLabels = unigueLabel.Where(i => i.Key.StartsWith(GetUniquePrefix())).ToList();
            foreach (var item in vars) tableV.Remove(item);
            foreach (var item in uniqueLabels) unigueLabel.Remove(item.Key);
            stackScope.Pop();
        }

        private void Step()
        {
            if (lineCount == 0)
            {
                ReadStart();
                if (!IsError())
                {
                    stackScope.Push("main");
                }
                return;
            }

            string tempName = "", tempCommand = "", tempArg1 = "", tempArg2 = "";

            DoLineMain(ref tempName, ref tempCommand, ref tempArg1, ref tempArg2);

            if (IsError()) return;

            if (macro)
            {
                DoMacro(tempName, tempCommand, tempArg1, tempArg2, ref lineCount, 1);
                if (IsError()) return;
                lineCount++;
                return;
            }
            else if (!end)
            {
                if (IsFilled(tempCommand))
                {
                    if (IsDirective(tempCommand))
                    {
                        if (IsFilled(tempArg2)) err.Add($"Неправильный формат объявления директивы в строке {lineCount + 1}\n");
                        if (IsError()) return;

                        DoDirective(tempCommand, ref tempArg1, lineCount + 1);
                        if (IsError()) return;

                        tom.Add(new Instruction()
                        {
                            Name = tempName,
                            SymbolicName = tempCommand,
                            Length = tempArg1,
                            Code = tempArg2,
                        });
                    }
                    else if (IsCommand(tempCommand))
                    {
                        DoCommand(tempCommand, ref tempArg1, ref tempArg2, lineCount + 1);
                        if (IsError()) return;

                        tom.Add(new Instruction()
                        {
                            Name = tempName,
                            SymbolicName = tempCommand,
                            Length = tempArg1,
                            Code = tempArg2,
                        });
                    }
                }
                else
                {
                    if (IsFilled(tempName))
                    {
                        if ((IsFilled(tempArg1) && !IsArgKey(tempArg1)) || (IsFilled(tempArg2) && !IsArgKey(tempArg2)))
                        {
                            err.Add($"Неправильный параметр в строке {lineCount + 1}\n");
                        }
                        else if (!IsMacroContains(tempName))
                        {
                            err.Add($"Макроопределение {tempName} неопределенно");
                        }
                        else
                        {
                            var el = GetNameMacro(tempName);
                            if (el != null)
                            {
                                if (IsArgKey(tempArg1))
                                {
                                    if (IsVarContains(tempArg1.Remove(0, 5)))
                                    {
                                        tempArg1 = tempArg1.Remove(5, tempArg1.Length - 5) + GetVarValue(tempArg1.Remove(0, 5));
                                    }
                                }

                                if (IsArgKey(tempArg2))
                                {
                                    if (IsVarContains(tempArg2.Remove(0, 5)))
                                    {
                                        tempArg2 = tempArg2.Remove(5, tempArg2.Length - 5) + GetVarValue(tempArg2.Remove(0, 5));
                                    }
                                }

                                CallMacro(el.Name, tempArg1, tempArg2);
                            }
                        }
                    }
                    else
                    {
                        err.Add($"В строке {lineCount + 1} обнаружен неправильный формат\n");
                    }
                }
                if (IsError()) return;
                lineCount++;
                return;
            }
            else if (end)
            {
                if (stackWhile.Count() != 0) err.Add("Не вce WHILE имеют ENDW");
                header["length"] = StringFormatAddress(ConvertTo16(header["address"]));
                ShowEnder();
                return;
            }
        }
    }
}