namespace sp_macro
{
    using Commands;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Models;
    using System.ComponentModel;

    public partial class mainForm : Form
    {
        public static readonly string CurrentDirectory = new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.FullName;

        public string inFile = CurrentDirectory + "\\input.txt";

        public string outFile = CurrentDirectory + "\\result.txt";

        private Executor executor;
        

        BindingSource tableVBind = new BindingSource();

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

            executor.Res();

            //tm.DataSource = executor.tableMacro;
            //tv.DataSource = executor.tableV;
            //om.DataSource = executor.tom;
            //tim.DataSource = executor.tableNMacro;


            ClearSelectLine();
            SelectLine();
        }

        public void Init()
        {
            // init GUI
            err.ResetText();
            bpass.Enabled = true;
            bstep.Enabled = true;
            save.Enabled = false;

            executor = new Executor(inFile);
            ts.Text = executor.ts.ToString();
            executor.CodeSource = () => ts.Text.ToString();

            tm.DataSource = executor.tableMacro;
            tv.DataSource = executor.tableV;
            om.DataSource = executor.tom;
            tim.DataSource = executor.tableNMacro;

            SelectLine();
        }


        public void Preparation()
        {
            ts.ReadOnly = true;
            open.Enabled = false;
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

            try
            {
                executor.Pass(sender, e);
            }
            catch (Exception ex)
            {
                err.AppendText(ex.Message);
                bstep.Enabled = false;
                bpass.Enabled = false;
                save.Enabled = true;
                open.Enabled = true;
            }
        }

        public void SaveAssCode(object sender, EventArgs e)
        {
            executor.SaveAssCode(sender, e);
        }

        private void LoadFile(object sender, EventArgs e)
        {
            executor.LoadFile(sender, e);
        }

        public void SelectLine()
        {
            ClearSelectLine();
            if (executor.codeReader.currentLine + 1 >= ts.Lines.Count()) return;

            int startFromIndex = ts.GetFirstCharIndexFromLine(executor.codeReader.currentLine + 1);
            //Получаем длину строки
            int lineLength = ts.Lines[executor.codeReader.currentLine + 1].Length + 1;
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
            try
            {
                executor.Step(sender, e);
                SelectLine();
                Renew();

                if (executor.end)
                {
                    bstep.Enabled = false;
                    bpass.Enabled = false;
                    save.Enabled = true;
                    open.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                err.AppendText(ex.Message);
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
