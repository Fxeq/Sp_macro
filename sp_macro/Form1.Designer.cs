namespace sp_macro
{
    partial class mainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lSourceCode = new System.Windows.Forms.Label();
            this.tm = new System.Windows.Forms.DataGridView();
            this.MNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ltm = new System.Windows.Forms.Label();
            this.lAdditionalTable = new System.Windows.Forms.Label();
            this.om = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LengthRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Arg1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Arg2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lTableSymbolicName = new System.Windows.Forms.Label();
            this.tv = new System.Windows.Forms.DataGridView();
            this.VName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lErrorFirstPass = new System.Windows.Forms.Label();
            this.bpass = new System.Windows.Forms.Button();
            this.bstep = new System.Windows.Forms.Button();
            this.R = new System.Windows.Forms.Button();
            this.err = new System.Windows.Forms.RichTextBox();
            this.ts = new System.Windows.Forms.RichTextBox();
            this.save = new System.Windows.Forms.Button();
            this.open = new System.Windows.Forms.Button();
            this.tim = new System.Windows.Forms.DataGridView();
            this.MName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MStartIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEndIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mArg1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mArg2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ltim = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.om)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tim)).BeginInit();
            this.SuspendLayout();
            // 
            // lSourceCode
            // 
            this.lSourceCode.AutoSize = true;
            this.lSourceCode.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lSourceCode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lSourceCode.Location = new System.Drawing.Point(12, 15);
            this.lSourceCode.Name = "lSourceCode";
            this.lSourceCode.Size = new System.Drawing.Size(134, 23);
            this.lSourceCode.TabIndex = 1;
            this.lSourceCode.Text = "Исходный текст";
            // 
            // tm
            // 
            this.tm.AllowUserToAddRows = false;
            this.tm.AllowUserToDeleteRows = false;
            this.tm.AllowUserToResizeColumns = false;
            this.tm.AllowUserToResizeRows = false;
            this.tm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tm.BackgroundColor = System.Drawing.SystemColors.Window;
            this.tm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MNumber,
            this.MBody});
            this.tm.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.tm.Location = new System.Drawing.Point(360, 37);
            this.tm.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tm.MultiSelect = false;
            this.tm.Name = "tm";
            this.tm.ReadOnly = true;
            this.tm.RowHeadersVisible = false;
            this.tm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            this.tm.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.tm.Size = new System.Drawing.Size(458, 474);
            this.tm.TabIndex = 2;
            // 
            // MNumber
            // 
            this.MNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MNumber.DataPropertyName = "Number";
            this.MNumber.FillWeight = 20F;
            this.MNumber.HeaderText = "Строка";
            this.MNumber.Name = "MNumber";
            this.MNumber.ReadOnly = true;
            // 
            // MBody
            // 
            this.MBody.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MBody.DataPropertyName = "Body";
            this.MBody.FillWeight = 80F;
            this.MBody.HeaderText = "Макропределение";
            this.MBody.Name = "MBody";
            this.MBody.ReadOnly = true;
            // 
            // ltm
            // 
            this.ltm.AutoSize = true;
            this.ltm.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ltm.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ltm.Location = new System.Drawing.Point(357, 15);
            this.ltm.Name = "ltm";
            this.ltm.Size = new System.Drawing.Size(235, 23);
            this.ltm.TabIndex = 3;
            this.ltm.Text = "Таблица макроопределений";
            // 
            // lAdditionalTable
            // 
            this.lAdditionalTable.AutoSize = true;
            this.lAdditionalTable.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lAdditionalTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lAdditionalTable.Location = new System.Drawing.Point(844, 15);
            this.lAdditionalTable.Name = "lAdditionalTable";
            this.lAdditionalTable.Size = new System.Drawing.Size(160, 23);
            this.lAdditionalTable.TabIndex = 4;
            this.lAdditionalTable.Text = "Ассемблерный код";
            // 
            // om
            // 
            this.om.AllowUserToAddRows = false;
            this.om.AllowUserToDeleteRows = false;
            this.om.AllowUserToResizeColumns = false;
            this.om.AllowUserToResizeRows = false;
            this.om.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.om.BackgroundColor = System.Drawing.SystemColors.Window;
            this.om.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.om.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.om.ColumnHeadersVisible = false;
            this.om.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Address,
            this.LengthRow,
            this.Code,
            this.Arg1,
            this.Arg2});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.om.DefaultCellStyle = dataGridViewCellStyle8;
            this.om.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.om.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.om.Location = new System.Drawing.Point(824, 36);
            this.om.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.om.MultiSelect = false;
            this.om.Name = "om";
            this.om.ReadOnly = true;
            this.om.RowHeadersVisible = false;
            this.om.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            this.om.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.om.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.om.Size = new System.Drawing.Size(451, 474);
            this.om.TabIndex = 5;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Type.DataPropertyName = "Name";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.DefaultCellStyle = dataGridViewCellStyle2;
            this.Type.HeaderText = "Тип";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 5;
            // 
            // Address
            // 
            this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Address.DataPropertyName = "SymbolicName";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Address.DefaultCellStyle = dataGridViewCellStyle3;
            this.Address.HeaderText = "Адрес";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Width = 5;
            // 
            // LengthRow
            // 
            this.LengthRow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LengthRow.DataPropertyName = "Length";
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LengthRow.DefaultCellStyle = dataGridViewCellStyle4;
            this.LengthRow.HeaderText = "Длина";
            this.LengthRow.Name = "LengthRow";
            this.LengthRow.ReadOnly = true;
            this.LengthRow.Width = 5;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Code.DataPropertyName = "Code";
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Code.DefaultCellStyle = dataGridViewCellStyle5;
            this.Code.HeaderText = "Код";
            this.Code.MinimumWidth = 60;
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 60;
            // 
            // Arg1
            // 
            this.Arg1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Arg1.DataPropertyName = "Arg1";
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Arg1.DefaultCellStyle = dataGridViewCellStyle6;
            this.Arg1.HeaderText = "Операнд";
            this.Arg1.MinimumWidth = 60;
            this.Arg1.Name = "Arg1";
            this.Arg1.ReadOnly = true;
            this.Arg1.Width = 60;
            // 
            // Arg2
            // 
            this.Arg2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Arg2.DataPropertyName = "Arg2";
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Arg2.DefaultCellStyle = dataGridViewCellStyle7;
            this.Arg2.HeaderText = "Операнд";
            this.Arg2.MinimumWidth = 60;
            this.Arg2.Name = "Arg2";
            this.Arg2.ReadOnly = true;
            this.Arg2.Width = 60;
            // 
            // lTableSymbolicName
            // 
            this.lTableSymbolicName.AutoSize = true;
            this.lTableSymbolicName.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lTableSymbolicName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lTableSymbolicName.Location = new System.Drawing.Point(12, 529);
            this.lTableSymbolicName.Name = "lTableSymbolicName";
            this.lTableSymbolicName.Size = new System.Drawing.Size(180, 23);
            this.lTableSymbolicName.TabIndex = 6;
            this.lTableSymbolicName.Text = "Таблица переменных";
            // 
            // tv
            // 
            this.tv.AllowUserToAddRows = false;
            this.tv.AllowUserToDeleteRows = false;
            this.tv.AllowUserToOrderColumns = true;
            this.tv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.tv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.tv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VName,
            this.VValue,
            this.VScope});
            this.tv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tv.Location = new System.Drawing.Point(15, 552);
            this.tv.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tv.MultiSelect = false;
            this.tv.Name = "tv";
            this.tv.RowHeadersVisible = false;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            this.tv.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.tv.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tv.Size = new System.Drawing.Size(319, 195);
            this.tv.TabIndex = 7;
            // 
            // VName
            // 
            this.VName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VName.DataPropertyName = "Name";
            this.VName.HeaderText = "Имя";
            this.VName.Name = "VName";
            // 
            // VValue
            // 
            this.VValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VValue.DataPropertyName = "Value";
            this.VValue.HeaderText = "Значение";
            this.VValue.Name = "VValue";
            // 
            // VScope
            // 
            this.VScope.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.VScope.DataPropertyName = "Scope";
            this.VScope.HeaderText = "Макрос";
            this.VScope.Name = "VScope";
            // 
            // lErrorFirstPass
            // 
            this.lErrorFirstPass.AutoSize = true;
            this.lErrorFirstPass.BackColor = System.Drawing.SystemColors.HighlightText;
            this.lErrorFirstPass.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lErrorFirstPass.Location = new System.Drawing.Point(844, 529);
            this.lErrorFirstPass.Name = "lErrorFirstPass";
            this.lErrorFirstPass.Size = new System.Drawing.Size(75, 23);
            this.lErrorFirstPass.TabIndex = 8;
            this.lErrorFirstPass.Text = "Ошибки";
            // 
            // bpass
            // 
            this.bpass.Location = new System.Drawing.Point(229, 758);
            this.bpass.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.bpass.Name = "bpass";
            this.bpass.Size = new System.Drawing.Size(101, 28);
            this.bpass.TabIndex = 12;
            this.bpass.Text = "Проход";
            this.bpass.UseVisualStyleBackColor = true;
            this.bpass.Click += new System.EventHandler(this.Pass);
            // 
            // bstep
            // 
            this.bstep.Location = new System.Drawing.Point(122, 758);
            this.bstep.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.bstep.Name = "bstep";
            this.bstep.Size = new System.Drawing.Size(101, 28);
            this.bstep.TabIndex = 13;
            this.bstep.Text = "Шаг";
            this.bstep.UseVisualStyleBackColor = true;
            this.bstep.Click += new System.EventHandler(this.Step);
            // 
            // R
            // 
            this.R.Location = new System.Drawing.Point(336, 758);
            this.R.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.R.Name = "R";
            this.R.Size = new System.Drawing.Size(101, 28);
            this.R.TabIndex = 20;
            this.R.Text = "Перезапуск";
            this.R.UseVisualStyleBackColor = true;
            this.R.Click += new System.EventHandler(this.Reset);
            // 
            // err
            // 
            this.err.Location = new System.Drawing.Point(847, 552);
            this.err.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.err.Name = "err";
            this.err.ReadOnly = true;
            this.err.Size = new System.Drawing.Size(451, 194);
            this.err.TabIndex = 23;
            this.err.Text = "";
            // 
            // ts
            // 
            this.ts.Location = new System.Drawing.Point(15, 37);
            this.ts.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ts.MaxLength = 8896;
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(319, 473);
            this.ts.TabIndex = 24;
            this.ts.Text = "";
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(443, 758);
            this.save.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(101, 28);
            this.save.TabIndex = 27;
            this.save.Text = "Сохранить";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.SaveAssCode);
            // 
            // open
            // 
            this.open.Location = new System.Drawing.Point(15, 758);
            this.open.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(101, 28);
            this.open.TabIndex = 29;
            this.open.Text = "Открыть";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.LoadFile);
            // 
            // tim
            // 
            this.tim.AllowUserToAddRows = false;
            this.tim.AllowUserToDeleteRows = false;
            this.tim.AllowUserToResizeColumns = false;
            this.tim.AllowUserToResizeRows = false;
            this.tim.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tim.BackgroundColor = System.Drawing.SystemColors.Window;
            this.tim.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tim.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MName,
            this.MStartIndex,
            this.MEndIndex,
            this.mArg1,
            this.mArg2});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tim.DefaultCellStyle = dataGridViewCellStyle14;
            this.tim.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.tim.Location = new System.Drawing.Point(360, 552);
            this.tim.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tim.MultiSelect = false;
            this.tim.Name = "tim";
            this.tim.ReadOnly = true;
            this.tim.RowHeadersVisible = false;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            this.tim.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.tim.Size = new System.Drawing.Size(458, 195);
            this.tim.TabIndex = 30;
            // 
            // MName
            // 
            this.MName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MName.DataPropertyName = "Name";
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MName.DefaultCellStyle = dataGridViewCellStyle11;
            this.MName.HeaderText = "Имя";
            this.MName.Name = "MName";
            this.MName.ReadOnly = true;
            // 
            // MStartIndex
            // 
            this.MStartIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MStartIndex.DataPropertyName = "StartIndex";
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MStartIndex.DefaultCellStyle = dataGridViewCellStyle12;
            this.MStartIndex.HeaderText = "Начало";
            this.MStartIndex.Name = "MStartIndex";
            this.MStartIndex.ReadOnly = true;
            // 
            // MEndIndex
            // 
            this.MEndIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MEndIndex.DataPropertyName = "EndIndex";
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MEndIndex.DefaultCellStyle = dataGridViewCellStyle13;
            this.MEndIndex.HeaderText = "Конец";
            this.MEndIndex.Name = "MEndIndex";
            this.MEndIndex.ReadOnly = true;
            // 
            // mArg1
            // 
            this.mArg1.DataPropertyName = "Arg1";
            this.mArg1.HeaderText = "Аргумент";
            this.mArg1.Name = "mArg1";
            this.mArg1.ReadOnly = true;
            this.mArg1.Width = 113;
            // 
            // mArg2
            // 
            this.mArg2.DataPropertyName = "Arg2";
            this.mArg2.HeaderText = "Аргумент";
            this.mArg2.Name = "mArg2";
            this.mArg2.ReadOnly = true;
            this.mArg2.Width = 113;
            // 
            // ltim
            // 
            this.ltim.AutoSize = true;
            this.ltim.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ltim.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ltim.Location = new System.Drawing.Point(357, 529);
            this.ltim.Name = "ltim";
            this.ltim.Size = new System.Drawing.Size(271, 23);
            this.ltim.TabIndex = 31;
            this.ltim.Text = "Таблица имен макропределений";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ClientSize = new System.Drawing.Size(1310, 796);
            this.Controls.Add(this.ltim);
            this.Controls.Add(this.tim);
            this.Controls.Add(this.open);
            this.Controls.Add(this.save);
            this.Controls.Add(this.ts);
            this.Controls.Add(this.err);
            this.Controls.Add(this.R);
            this.Controls.Add(this.bstep);
            this.Controls.Add(this.bpass);
            this.Controls.Add(this.lErrorFirstPass);
            this.Controls.Add(this.tv);
            this.Controls.Add(this.lTableSymbolicName);
            this.Controls.Add(this.om);
            this.Controls.Add(this.lAdditionalTable);
            this.Controls.Add(this.ltm);
            this.Controls.Add(this.tm);
            this.Controls.Add(this.lSourceCode);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "mainForm";
            this.ShowIcon = false;
            this.Text = "Домашняя работа";
            ((System.ComponentModel.ISupportInitialize)(this.tm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.om)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lSourceCode;
        private System.Windows.Forms.DataGridView tm;
        private System.Windows.Forms.Label ltm;
        private System.Windows.Forms.Label lAdditionalTable;
        private System.Windows.Forms.DataGridView om;
        private System.Windows.Forms.Label lTableSymbolicName;
        private System.Windows.Forms.DataGridView tv;
        private System.Windows.Forms.Label lErrorFirstPass;
        private System.Windows.Forms.Button bpass;
        private System.Windows.Forms.Button bstep;
        private System.Windows.Forms.Button R;
        private System.Windows.Forms.RichTextBox err;
        private System.Windows.Forms.RichTextBox ts;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn LengthRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Arg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Arg2;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button open;
        private System.Windows.Forms.DataGridView tim;
        private System.Windows.Forms.Label ltim;
        private System.Windows.Forms.DataGridViewTextBoxColumn VName;
        private System.Windows.Forms.DataGridViewTextBoxColumn VValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn VScope;
        private System.Windows.Forms.DataGridViewTextBoxColumn MNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn MBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn MName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MStartIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEndIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn mArg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mArg2;
    }
}

