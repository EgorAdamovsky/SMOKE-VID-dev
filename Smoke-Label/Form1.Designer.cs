namespace Smoke_Label
{
    partial class MyForm
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
            this._openfile = new System.Windows.Forms.OpenFileDialog();
            this._choosefile = new System.Windows.Forms.Button();
            this._pathfile = new System.Windows.Forms.TextBox();
            this._screen = new System.Windows.Forms.PictureBox();
            this._slider = new System.Windows.Forms.TrackBar();
            this._counter = new System.Windows.Forms.TextBox();
            this._progress = new System.Windows.Forms.ProgressBar();
            this._list = new System.Windows.Forms.ListBox();
            this._x = new System.Windows.Forms.TextBox();
            this._y = new System.Windows.Forms.TextBox();
            this._w = new System.Windows.Forms.TextBox();
            this._h = new System.Windows.Forms.TextBox();
            this._add = new System.Windows.Forms.Button();
            this._del = new System.Windows.Forms.Button();
            this._seq = new System.Windows.Forms.CheckBox();
            this._compile = new System.Windows.Forms.Button();
            this._compiledlist = new System.Windows.Forms.ListBox();
            this._len = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._clear = new System.Windows.Forms.Button();
            this._manager = new System.Windows.Forms.ComboBox();
            this._addobj = new System.Windows.Forms.Button();
            this._delobj = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._create = new System.Windows.Forms.Button();
            this._trainorval = new System.Windows.Forms.ComboBox();
            this._status = new System.Windows.Forms.StatusStrip();
            this._ww = new System.Windows.Forms.ToolStripStatusLabel();
            this._hh = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._screen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._slider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this._status.SuspendLayout();
            this.SuspendLayout();
            // 
            // _openfile
            // 
            this._openfile.Filter = "Видео|*.*|AVI|*.avi|MP4|*.mp4|MOV|*.mov|MPG|*.mpg";
            this._openfile.Title = "Видеофайл с дымом";
            // 
            // _choosefile
            // 
            this._choosefile.Location = new System.Drawing.Point(5, 390);
            this._choosefile.Name = "_choosefile";
            this._choosefile.Size = new System.Drawing.Size(100, 25);
            this._choosefile.TabIndex = 1;
            this._choosefile.Text = "Выбрать файл";
            this._choosefile.UseVisualStyleBackColor = true;
            this._choosefile.Click += new System.EventHandler(this._choosefile_Click);
            // 
            // _pathfile
            // 
            this._pathfile.Location = new System.Drawing.Point(5, 365);
            this._pathfile.Name = "_pathfile";
            this._pathfile.ReadOnly = true;
            this._pathfile.Size = new System.Drawing.Size(390, 20);
            this._pathfile.TabIndex = 2;
            // 
            // _screen
            // 
            this._screen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._screen.Location = new System.Drawing.Point(5, 5);
            this._screen.Name = "_screen";
            this._screen.Size = new System.Drawing.Size(480, 320);
            this._screen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._screen.TabIndex = 4;
            this._screen.TabStop = false;
            this._screen.Paint += new System.Windows.Forms.PaintEventHandler(this._screen_Paint);
            this._screen.MouseDown += new System.Windows.Forms.MouseEventHandler(this._screen_MouseDown);
            this._screen.MouseMove += new System.Windows.Forms.MouseEventHandler(this._screen_MouseMove);
            this._screen.MouseUp += new System.Windows.Forms.MouseEventHandler(this._screen_MouseUp);
            // 
            // _slider
            // 
            this._slider.LargeChange = 10;
            this._slider.Location = new System.Drawing.Point(5, 330);
            this._slider.Name = "_slider";
            this._slider.Size = new System.Drawing.Size(390, 45);
            this._slider.TabIndex = 5;
            this._slider.Scroll += new System.EventHandler(this._slider_Scroll);
            // 
            // _counter
            // 
            this._counter.Location = new System.Drawing.Point(400, 335);
            this._counter.Name = "_counter";
            this._counter.Size = new System.Drawing.Size(40, 20);
            this._counter.TabIndex = 6;
            this._counter.Text = "0";
            this._counter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._counter.TextChanged += new System.EventHandler(this._counter_TextChanged);
            // 
            // _progress
            // 
            this._progress.Location = new System.Drawing.Point(110, 390);
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(375, 25);
            this._progress.TabIndex = 7;
            // 
            // _list
            // 
            this._list.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._list.FormattingEnabled = true;
            this._list.ItemHeight = 12;
            this._list.Location = new System.Drawing.Point(490, 29);
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(270, 268);
            this._list.TabIndex = 8;
            this._list.DoubleClick += new System.EventHandler(this._list_DoubleClick);
            // 
            // _x
            // 
            this._x.Location = new System.Drawing.Point(30, 45);
            this._x.Name = "_x";
            this._x.ReadOnly = true;
            this._x.Size = new System.Drawing.Size(40, 20);
            this._x.TabIndex = 9;
            this._x.Text = "0";
            this._x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _y
            // 
            this._y.Location = new System.Drawing.Point(105, 45);
            this._y.Name = "_y";
            this._y.ReadOnly = true;
            this._y.Size = new System.Drawing.Size(40, 20);
            this._y.TabIndex = 10;
            this._y.Text = "0";
            this._y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _w
            // 
            this._w.Location = new System.Drawing.Point(180, 45);
            this._w.Name = "_w";
            this._w.ReadOnly = true;
            this._w.Size = new System.Drawing.Size(40, 20);
            this._w.TabIndex = 11;
            this._w.Text = "0";
            this._w.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _h
            // 
            this._h.Location = new System.Drawing.Point(255, 45);
            this._h.Name = "_h";
            this._h.ReadOnly = true;
            this._h.Size = new System.Drawing.Size(40, 20);
            this._h.TabIndex = 12;
            this._h.Text = "0";
            this._h.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _add
            // 
            this._add.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
            this._add.Location = new System.Drawing.Point(10, 75);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(60, 25);
            this._add.TabIndex = 13;
            this._add.Text = "Добавить";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _del
            // 
            this._del.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
            this._del.Location = new System.Drawing.Point(85, 75);
            this._del.Name = "_del";
            this._del.Size = new System.Drawing.Size(60, 25);
            this._del.TabIndex = 14;
            this._del.Text = "Удалить";
            this._del.UseVisualStyleBackColor = true;
            this._del.Click += new System.EventHandler(this._del_Click);
            // 
            // _seq
            // 
            this._seq.AutoSize = true;
            this._seq.Checked = true;
            this._seq.CheckState = System.Windows.Forms.CheckState.Checked;
            this._seq.Location = new System.Drawing.Point(10, 20);
            this._seq.Name = "_seq";
            this._seq.Size = new System.Drawing.Size(275, 17);
            this._seq.TabIndex = 15;
            this._seq.Text = "Интерполировать до следующей ключевой точки";
            this._seq.UseVisualStyleBackColor = true;
            // 
            // _compile
            // 
            this._compile.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
            this._compile.Location = new System.Drawing.Point(235, 75);
            this._compile.Name = "_compile";
            this._compile.Size = new System.Drawing.Size(60, 25);
            this._compile.TabIndex = 16;
            this._compile.Text = "Собрать";
            this._compile.UseVisualStyleBackColor = true;
            this._compile.Click += new System.EventHandler(this._compile_Click);
            // 
            // _compiledlist
            // 
            this._compiledlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._compiledlist.FormattingEnabled = true;
            this._compiledlist.ItemHeight = 12;
            this._compiledlist.Location = new System.Drawing.Point(765, 29);
            this._compiledlist.Name = "_compiledlist";
            this._compiledlist.Size = new System.Drawing.Size(270, 268);
            this._compiledlist.TabIndex = 17;
            // 
            // _len
            // 
            this._len.Location = new System.Drawing.Point(445, 335);
            this._len.Name = "_len";
            this._len.ReadOnly = true;
            this._len.Size = new System.Drawing.Size(40, 20);
            this._len.TabIndex = 18;
            this._len.Text = "0";
            this._len.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this._clear);
            this.groupBox1.Controls.Add(this._seq);
            this.groupBox1.Controls.Add(this._x);
            this.groupBox1.Controls.Add(this._y);
            this.groupBox1.Controls.Add(this._w);
            this.groupBox1.Controls.Add(this._compile);
            this.groupBox1.Controls.Add(this._h);
            this.groupBox1.Controls.Add(this._del);
            this.groupBox1.Controls.Add(this._add);
            this.groupBox1.Location = new System.Drawing.Point(490, 305);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 110);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры рамки";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(235, 48);
            this.label7.MaximumSize = new System.Drawing.Size(20, 15);
            this.label7.MinimumSize = new System.Drawing.Size(20, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 15);
            this.label7.TabIndex = 32;
            this.label7.Text = "H:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(155, 48);
            this.label6.MaximumSize = new System.Drawing.Size(25, 15);
            this.label6.MinimumSize = new System.Drawing.Size(25, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 15);
            this.label6.TabIndex = 31;
            this.label6.Text = "W:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(85, 48);
            this.label5.MaximumSize = new System.Drawing.Size(20, 15);
            this.label5.MinimumSize = new System.Drawing.Size(20, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 15);
            this.label5.TabIndex = 30;
            this.label5.Text = "Y:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 48);
            this.label4.MaximumSize = new System.Drawing.Size(20, 15);
            this.label4.MinimumSize = new System.Drawing.Size(20, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 15);
            this.label4.TabIndex = 29;
            this.label4.Text = "X:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _clear
            // 
            this._clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F);
            this._clear.Location = new System.Drawing.Point(160, 75);
            this._clear.Name = "_clear";
            this._clear.Size = new System.Drawing.Size(60, 25);
            this._clear.TabIndex = 17;
            this._clear.Text = "Очистить";
            this._clear.UseVisualStyleBackColor = true;
            this._clear.Click += new System.EventHandler(this._clear_Click);
            // 
            // _manager
            // 
            this._manager.FormattingEnabled = true;
            this._manager.Location = new System.Drawing.Point(10, 19);
            this._manager.Name = "_manager";
            this._manager.Size = new System.Drawing.Size(210, 21);
            this._manager.TabIndex = 20;
            this._manager.SelectedIndexChanged += new System.EventHandler(this._manager_SelectedIndexChanged);
            // 
            // _addobj
            // 
            this._addobj.Location = new System.Drawing.Point(10, 45);
            this._addobj.Name = "_addobj";
            this._addobj.Size = new System.Drawing.Size(85, 25);
            this._addobj.TabIndex = 21;
            this._addobj.Text = "Добавить";
            this._addobj.UseVisualStyleBackColor = true;
            this._addobj.Click += new System.EventHandler(this._addobj_Click);
            // 
            // _delobj
            // 
            this._delobj.Location = new System.Drawing.Point(100, 45);
            this._delobj.Name = "_delobj";
            this._delobj.Size = new System.Drawing.Size(85, 25);
            this._delobj.TabIndex = 22;
            this._delobj.Text = "Удалить";
            this._delobj.UseVisualStyleBackColor = true;
            this._delobj.Click += new System.EventHandler(this._delobj_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._manager);
            this.groupBox2.Controls.Add(this._delobj);
            this.groupBox2.Controls.Add(this._addobj);
            this.groupBox2.Location = new System.Drawing.Point(800, 305);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 80);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Объекты";
            // 
            // _create
            // 
            this._create.Location = new System.Drawing.Point(800, 390);
            this._create.Name = "_create";
            this._create.Size = new System.Drawing.Size(100, 25);
            this._create.TabIndex = 24;
            this._create.Text = "Создать датасет";
            this._create.UseVisualStyleBackColor = true;
            this._create.Click += new System.EventHandler(this._create_Click);
            // 
            // _trainorval
            // 
            this._trainorval.FormattingEnabled = true;
            this._trainorval.Items.AddRange(new object[] {
            "train",
            "val"});
            this._trainorval.Location = new System.Drawing.Point(905, 391);
            this._trainorval.Name = "_trainorval";
            this._trainorval.Size = new System.Drawing.Size(129, 21);
            this._trainorval.TabIndex = 23;
            this._trainorval.Text = "train";
            // 
            // _status
            // 
            this._status.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this._status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._ww,
            this._hh});
            this._status.Location = new System.Drawing.Point(0, 419);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(1044, 22);
            this._status.TabIndex = 25;
            // 
            // _ww
            // 
            this._ww.Name = "_ww";
            this._ww.Size = new System.Drawing.Size(0, 17);
            // 
            // _hh
            // 
            this._hh.Name = "_hh";
            this._hh.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(400, 365);
            this.label1.MaximumSize = new System.Drawing.Size(85, 15);
            this.label1.MinimumSize = new System.Drawing.Size(85, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 26;
            this.label1.Text = " Кадр      Всего";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(490, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Исходные метки";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(765, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Скомпилированные метки";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 441);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._status);
            this.Controls.Add(this._trainorval);
            this.Controls.Add(this._create);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._len);
            this.Controls.Add(this._compiledlist);
            this.Controls.Add(this._list);
            this.Controls.Add(this._progress);
            this.Controls.Add(this._pathfile);
            this.Controls.Add(this._counter);
            this.Controls.Add(this._slider);
            this.Controls.Add(this._screen);
            this.Controls.Add(this._choosefile);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MyForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smoke-Label";
            ((System.ComponentModel.ISupportInitialize)(this._screen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._slider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this._status.ResumeLayout(false);
            this._status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog _openfile;
        private System.Windows.Forms.Button _choosefile;
        private System.Windows.Forms.TextBox _pathfile;
        private System.Windows.Forms.PictureBox _screen;
        private System.Windows.Forms.TrackBar _slider;
        private System.Windows.Forms.TextBox _counter;
        private System.Windows.Forms.ProgressBar _progress;
        private System.Windows.Forms.ListBox _list;
        private System.Windows.Forms.TextBox _x;
        private System.Windows.Forms.TextBox _y;
        private System.Windows.Forms.TextBox _w;
        private System.Windows.Forms.TextBox _h;
        private System.Windows.Forms.Button _add;
        private System.Windows.Forms.Button _del;
        private System.Windows.Forms.CheckBox _seq;
        private System.Windows.Forms.Button _compile;
        private System.Windows.Forms.ListBox _compiledlist;
        private System.Windows.Forms.TextBox _len;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox _manager;
        private System.Windows.Forms.Button _addobj;
        private System.Windows.Forms.Button _delobj;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button _create;
        private System.Windows.Forms.ComboBox _trainorval;
        private System.Windows.Forms.StatusStrip _status;
        private System.Windows.Forms.ToolStripStatusLabel _ww;
        private System.Windows.Forms.ToolStripStatusLabel _hh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _clear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}

