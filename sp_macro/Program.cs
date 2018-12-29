using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sp_macro
{
    static class Program
    {

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();
            bool IsEnd = false;
            bool IsErr = false;

            if (args.Length == 0)
            {
                ShowWindow(handle, SW_HIDE);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new mainForm());
            } else
            {
                ShowWindow(handle, SW_SHOW);
                try
                {
                    ConsoleArgsParser run = new ConsoleArgsParser(args);
                    if (run.fileNot)
                    {
                        Console.Write("Файл не найден!");
                    }
                    else
                    {
                        if (run.IsH)
                        {
                            Console.Write(ConsoleArgsParser.GetHelp());
                        }
                        else if (run.IsI)
                        {
                            try
                            {
                                Message temp;
                                temp = run.Pass();
                                run.Update();
                                IsErr = temp.result;
                                IsEnd = temp.IsEnd;
                                if (IsEnd)
                                {
                                    if (run.IsO)
                                    {
                                        try
                                        {
                                            StreamWriter sw = new StreamWriter(run.outFile);
                                            if (run.IsM)
                                            {
                                                sw.WriteLine("\nПромежуточные данные\n");
                                                sw.WriteLine("\nТаблица имен макроопределений\n");
                                                sw.WriteLine("Имя\tНачало\tКонец\tОбласть\tАргумент\tАргумент\n");
                                                foreach (string line in run.nameMacros)
                                                {
                                                    sw.WriteLine(line.ToString());
                                                }
                                                sw.WriteLine("\n_______________________________\n");
                                                sw.WriteLine("\nТаблица макроопределений\n");
                                                foreach (string line in run.macros)
                                                {
                                                    sw.WriteLine(line.ToString());
                                                }
                                                sw.WriteLine("\n________________________\n");
                                            }
                                            sw.WriteLine("\nАссемблерный код\n");
                                            foreach (string line in run.ass)
                                            {
                                                sw.WriteLine(line.ToString());
                                            }
                                            sw.WriteLine("\n________________\n");
                                            sw.Close();
                                            Console.WriteLine("\nЗапись успешна.\n");
                                            Process.Start("notepad.exe", run.outFile);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("\nЗапись невозможна. Файл не задан или не найден\n");
                                        }
                                    }
                                    else
                                    {
                                        if (run.IsM)
                                        {
                                            Console.WriteLine("\nПромежуточные данные\n");
                                            Console.WriteLine("\nТаблица имен макроопределений\n");
                                            Console.WriteLine("Имя\tНачало\tКонец\tОбласть\tАргумент\tАргумент\n");
                                            foreach (string line in run.nameMacros)
                                            {
                                                Console.WriteLine(line);
                                            }
                                            Console.WriteLine("\n_____________________________\n");
                                            Console.WriteLine("\nТаблица макроопределений\n");
                                            foreach (string line in run.macros)
                                            {
                                                Console.WriteLine(line);
                                            }
                                            Console.WriteLine("\n________________________\n");
                                        }
                                        Console.WriteLine("\nАссемблерный код\n");
                                        foreach (string line in run.ass)
                                        {
                                            Console.WriteLine(line);
                                        }
                                        Console.WriteLine("\n________________\n");
                                        Console.WriteLine("\nПрограмма завершена.\n");
                                    }
                                }
                            } catch (Exception ex)
                            {
                                Console.WriteLine("\nБыли обнаружены следующие ошибки:\n");
                                Console.WriteLine(ex.Message);
                                //Console.WriteLine(ex.StackTrace);
                                Console.WriteLine("\nИсправьте все ошибки в исходном тексте и перезапустите программу.\n");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\nОшибка. " + ex.Message + "\n\n");
                    //Console.WriteLine( ex.StackTrace);
                }
            }
        }
    }
}
