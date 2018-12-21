using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace sp_macro
{
    class ConsoleArgsParser
    {
        public static readonly string CurrentDirectory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;
        public string inFile = CurrentDirectory + "\\input.txt";
        public string outFile = CurrentDirectory + "\\result.txt";

        Executor exe;
        public List<string> code;
        public List<string> ass;
        public List<string> macros;
        public List<string> nameMacros;

        public bool IsStep = false;
        public bool IsEnd = false;
        public bool fileNot = false;
        public bool IsM = false;
        public bool IsO = false;
        public bool IsI = false;
        public bool IsH = false;

        public ConsoleArgsParser(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    if (args.get(0).ToLower() == "-h")
                    {
                        IsH = true;
                    }
                    else
                    {
                        throw new Exception("Неправильный аргумент");
                    }
                    break;
                case 2:
                    if (args.get(0).ToLower() == "-i")
                    {
                        inFile = string.Format("{0}\\{1}", CurrentDirectory, args.get(1));
                        IsI = true;
                    }
                    else if (args.get(0).ToLower() == "-o")
                    {
                        outFile = string.Format("{0}\\{1}", CurrentDirectory, args.get(1));
                        IsO = true;
                        IsI = true;
                    }
                    else
                    {
                        throw new Exception("Неправильный аргумент");
                    }
                    break;
                case 3:
                    if (args.get(0).ToLower() == "-i" && args[2].ToLower() == "-d")
                    {
                        inFile = string.Format("{0}\\{1}", CurrentDirectory, args.get(1));
                        IsM = true;
                        IsI = true;
                    }
                    else if (args.get(0).ToLower() == "-o" && args[2].ToLower() == "-d")
                    {
                        outFile = string.Format("{0}\\{1}", CurrentDirectory, args.get(1));
                        IsM = true;
                        IsI = true;
                        IsO = true;
                    }
                    else
                    {
                        throw new Exception("Неправильный аргумент");
                    }
                    break;
                case 4:
                    if (args.get(0).ToLower() == "-i")
                    {
                        inFile = string.Format("{0}\\{1}", CurrentDirectory, args.get(1));
                        IsI = true;
                        if (args[2].ToLower() == "-o")
                        {
                            outFile = string.Format("{0}\\{1}", CurrentDirectory, args[3]); ;
                            IsO = true;
                        }
                        else
                        {
                            throw new Exception("Недопустимый ключ! Должен быть -o");
                        }
                    }
                    else
                    {
                        throw new Exception("Неправильный первый аргумент");
                    }
                    break;
                case 5:
                    {
                        if (args.get(0).ToLower() == "-i")
                        {
                            inFile = string.Format("{0}\\{1}", CurrentDirectory, args.get(1));
                            IsI = true;
                            if (args[2].ToLower() == "-d" && args[3].ToLower() == "-o")
                            {
                                IsM = true;
                                IsO = true;
                                outFile = string.Format("{0}\\{1}", CurrentDirectory, args[4]); ;
                            }
                            else
                            {
                                throw new Exception("Недопустимый ключ! Должен быть -o");
                            }
                        }
                        else
                        {
                            throw new Exception("Неправильный первый аргумент");
                        }
                        break;
                    }
                default:
                    throw new Exception("Неверное количество аргументов");
            }


            if (Path.GetFullPath(inFile).isNotEmpty())
            {
                code = new List<string>();
                ass = new List<string>();
                macros = new List<string>();
                nameMacros = new List<string>();
                exe = new Executor(inFile);
            }
            else
            {
                fileNot = true;
            }
        }

        public void Update()
        {
            GetAssCode();
            GetCode();
            GetMacros();
            GetNameMacros();
        }

        public List<string> GetCode()
        {
            var temp = new List<string>();
            for (int i = 0; i < exe.ts.ToString().Count(); ++i)
            {
                temp.Add(string.Format("{0}", exe.ts[i]));
            }
            code = temp;
            return code;
        }

        public List<string> GetAssCode()
        {
            var temp = new List<string>();
            foreach (var line in exe.tom)
            {
                temp.Add(string.Format("{0} {1} {2} {3} {4} {5}", line.Name, line.SymbolicName, line.Length, line.Code, line.Arg1, line.Arg2));
            }
            ass = temp;
            return ass;
        }

        public List<string> GetMacros()
        {
            var temp = new List<string>();
            foreach (var line in exe.tableMacro)
            {
                temp.Add(string.Format("{0} {1}", line.Number, line.Body));
            }
            macros = temp;
            return macros;
        }

        public List<string> GetNameMacros()
        {
            var temp = new List<string>();
            foreach (var line in exe.tableNMacro)
            {
                temp.Add(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t", line.Name, line.StartIndex, line.EndIndex, line.Arg1, line.Arg2));
            }
            nameMacros = temp;
            return nameMacros;
        }

        public Message Pass()
        {
            return exe.Pass(null, null);
        }

        public static string GetHelp()
        {
            return "Справка.\r\n" +
                        "Доступные ключи: -h -i -d -o\r\n" +
                        "-h\tВызов справки; \r\n" +
                        "-i\tКлюч для указания пути к файлу с исходным текстом; \r\n" +
                        "-d\tКлюч для вывода промежуточных данных.\r\n" +
                        "-o\tКлюч для указания пути сохранения результата\r\n";
        }
    }
}
