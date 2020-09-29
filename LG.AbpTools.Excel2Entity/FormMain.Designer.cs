namespace LG.AbpTools.Excel2Entity
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textExcel = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAutoIntToEnum = new System.Windows.Forms.CheckBox();
            this.chkAutoIntToBool = new System.Windows.Forms.CheckBox();
            this.chkRemarkAsAnnotation = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textClassEN = new System.Windows.Forms.TextBox();
            this.textClassZH = new System.Windows.Forms.TextBox();
            this.textModule = new System.Windows.Forms.TextBox();
            this.textNamespace = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textSQL = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textExcel
            // 
            this.textExcel.Location = new System.Drawing.Point(15, 70);
            this.textExcel.Margin = new System.Windows.Forms.Padding(4);
            this.textExcel.Multiline = true;
            this.textExcel.Name = "textExcel";
            this.textExcel.Size = new System.Drawing.Size(1488, 380);
            this.textExcel.TabIndex = 0;
            this.textExcel.TextChanged += new System.EventHandler(this.textExcel_TextChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 27);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(201, 93);
            this.button4.TabIndex = 1;
            this.button4.Text = "生成sql";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAutoIntToEnum);
            this.panel1.Controls.Add(this.chkAutoIntToBool);
            this.panel1.Controls.Add(this.chkRemarkAsAnnotation);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textClassEN);
            this.panel1.Controls.Add(this.textClassZH);
            this.panel1.Controls.Add(this.textModule);
            this.panel1.Controls.Add(this.textNamespace);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 852);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1510, 138);
            this.panel1.TabIndex = 2;
            // 
            // chkAutoIntToEnum
            // 
            this.chkAutoIntToEnum.AutoSize = true;
            this.chkAutoIntToEnum.Enabled = false;
            this.chkAutoIntToEnum.Location = new System.Drawing.Point(1036, 98);
            this.chkAutoIntToEnum.Name = "chkAutoIntToEnum";
            this.chkAutoIntToEnum.Size = new System.Drawing.Size(187, 22);
            this.chkAutoIntToEnum.TabIndex = 5;
            this.chkAutoIntToEnum.Text = "识别Enum的int字段";
            this.chkAutoIntToEnum.UseVisualStyleBackColor = true;
            // 
            // chkAutoIntToBool
            // 
            this.chkAutoIntToBool.AutoSize = true;
            this.chkAutoIntToBool.Checked = true;
            this.chkAutoIntToBool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoIntToBool.Location = new System.Drawing.Point(1036, 63);
            this.chkAutoIntToBool.Name = "chkAutoIntToBool";
            this.chkAutoIntToBool.Size = new System.Drawing.Size(187, 22);
            this.chkAutoIntToBool.TabIndex = 5;
            this.chkAutoIntToBool.Text = "识别bool的int字段";
            this.chkAutoIntToBool.UseVisualStyleBackColor = true;
            // 
            // chkRemarkAsAnnotation
            // 
            this.chkRemarkAsAnnotation.AutoSize = true;
            this.chkRemarkAsAnnotation.Checked = true;
            this.chkRemarkAsAnnotation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemarkAsAnnotation.Location = new System.Drawing.Point(1036, 27);
            this.chkRemarkAsAnnotation.Name = "chkRemarkAsAnnotation";
            this.chkRemarkAsAnnotation.Size = new System.Drawing.Size(178, 22);
            this.chkRemarkAsAnnotation.TabIndex = 4;
            this.chkRemarkAsAnnotation.Text = "字段说明作为注释";
            this.chkRemarkAsAnnotation.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(226, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "类名EN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 94);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "类名ZH";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(639, 94);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 18);
            this.label6.TabIndex = 3;
            this.label6.Text = "模块";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(639, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "命名空间";
            // 
            // textClassEN
            // 
            this.textClassEN.Location = new System.Drawing.Point(320, 34);
            this.textClassEN.Margin = new System.Windows.Forms.Padding(4);
            this.textClassEN.Name = "textClassEN";
            this.textClassEN.Size = new System.Drawing.Size(290, 28);
            this.textClassEN.TabIndex = 2;
            // 
            // textClassZH
            // 
            this.textClassZH.Location = new System.Drawing.Point(320, 88);
            this.textClassZH.Margin = new System.Windows.Forms.Padding(4);
            this.textClassZH.Name = "textClassZH";
            this.textClassZH.Size = new System.Drawing.Size(290, 28);
            this.textClassZH.TabIndex = 2;
            // 
            // textModule
            // 
            this.textModule.Location = new System.Drawing.Point(727, 90);
            this.textModule.Margin = new System.Windows.Forms.Padding(4);
            this.textModule.Name = "textModule";
            this.textModule.Size = new System.Drawing.Size(274, 28);
            this.textModule.TabIndex = 2;
            // 
            // textNamespace
            // 
            this.textNamespace.Location = new System.Drawing.Point(727, 42);
            this.textNamespace.Margin = new System.Windows.Forms.Padding(4);
            this.textNamespace.Name = "textNamespace";
            this.textNamespace.Size = new System.Drawing.Size(274, 28);
            this.textNamespace.TabIndex = 2;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1304, 27);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(201, 93);
            this.button5.TabIndex = 1;
            this.button5.Text = "生成文件";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.textSQL);
            this.panel2.Controls.Add(this.textExcel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1510, 852);
            this.panel2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(18, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 33);
            this.label5.TabIndex = 2;
            this.label5.Text = "Excel";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(18, 458);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 33);
            this.label4.TabIndex = 1;
            this.label4.Text = "SQL";
            // 
            // textSQL
            // 
            this.textSQL.Location = new System.Drawing.Point(15, 494);
            this.textSQL.Margin = new System.Windows.Forms.Padding(4);
            this.textSQL.Multiline = true;
            this.textSQL.Name = "textSQL";
            this.textSQL.Size = new System.Drawing.Size(1488, 380);
            this.textSQL.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1510, 990);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1532, 1046);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1274, 670);
            this.Name = "FormMain";
            this.Text = "代码生成器";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textExcel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textClassZH;
        private System.Windows.Forms.TextBox textNamespace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textSQL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textClassEN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textModule;
        private System.Windows.Forms.CheckBox chkRemarkAsAnnotation;
        private System.Windows.Forms.CheckBox chkAutoIntToEnum;
        private System.Windows.Forms.CheckBox chkAutoIntToBool;
    }
}

