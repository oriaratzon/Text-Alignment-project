using System.Drawing;
using System.Windows.Forms;
namespace text_alignment
{
    partial class textBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(textBox));
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.manuscriptPanel = new System.Windows.Forms.Panel();
            this.manuPicBox = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rightDirButton = new System.Windows.Forms.Button();
            this.leftDirButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.findLineButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.LoadManuButton = new System.Windows.Forms.Button();
            this.loadTextButton = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadManuscriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setParametersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hightLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.manuscriptPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.manuPicBox)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.searchTextBox.Location = new System.Drawing.Point(319, 4);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(198, 29);
            this.searchTextBox.TabIndex = 11;
            this.searchTextBox.Text = "Search...";
            this.searchTextBox.Click += new System.EventHandler(this.searchTextBox_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(673, 640);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.manuscriptPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-3, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1361, 688);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // manuscriptPanel
            // 
            this.manuscriptPanel.AutoScroll = true;
            this.manuscriptPanel.Controls.Add(this.manuPicBox);
            this.manuscriptPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manuscriptPanel.Location = new System.Drawing.Point(3, 43);
            this.manuscriptPanel.Name = "manuscriptPanel";
            this.manuscriptPanel.Size = new System.Drawing.Size(674, 642);
            this.manuscriptPanel.TabIndex = 17;
            // 
            // manuPicBox
            // 
            this.manuPicBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.manuPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.manuPicBox.InitialImage = null;
            this.manuPicBox.Location = new System.Drawing.Point(0, 0);
            this.manuPicBox.Name = "manuPicBox";
            this.manuPicBox.Size = new System.Drawing.Size(671, 639);
            this.manuPicBox.TabIndex = 13;
            this.manuPicBox.TabStop = false;
            this.manuPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.manuPicBox_MouseDown);
            this.manuPicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.manuPicBox_MouseMove);
            this.manuPicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.manuPicBox_MouseUp);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.richTextBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(683, 43);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(675, 642);
            this.panel3.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rightDirButton);
            this.panel2.Controls.Add(this.leftDirButton);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.loadButton);
            this.panel2.Controls.Add(this.searchTextBox);
            this.panel2.Controls.Add(this.playButton);
            this.panel2.Controls.Add(this.findLineButton);
            this.panel2.Controls.Add(this.saveButton);
            this.panel2.Controls.Add(this.LoadManuButton);
            this.panel2.Controls.Add(this.loadTextButton);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(580, 33);
            this.panel2.TabIndex = 20;
            // 
            // rightDirButton
            // 
            this.rightDirButton.BackgroundImage = global::text_alignment.Properties.Resources.back_158491_640___Copy;
            this.rightDirButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rightDirButton.Location = new System.Drawing.Point(270, -1);
            this.rightDirButton.Name = "rightDirButton";
            this.rightDirButton.Size = new System.Drawing.Size(34, 34);
            this.rightDirButton.TabIndex = 16;
            this.rightDirButton.UseVisualStyleBackColor = true;
            this.rightDirButton.Click += new System.EventHandler(this.rightDirButton_Click);
            // 
            // leftDirButton
            // 
            this.leftDirButton.BackColor = System.Drawing.Color.Transparent;
            this.leftDirButton.BackgroundImage = global::text_alignment.Properties.Resources.back_158491_640;
            this.leftDirButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.leftDirButton.Location = new System.Drawing.Point(230, -1);
            this.leftDirButton.Name = "leftDirButton";
            this.leftDirButton.Size = new System.Drawing.Size(34, 34);
            this.leftDirButton.TabIndex = 15;
            this.leftDirButton.UseVisualStyleBackColor = false;
            this.leftDirButton.Click += new System.EventHandler(this.leftDirButton_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::text_alignment.Properties.Resources.BlueOrbSearch2___Copy;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(522, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 34);
            this.button1.TabIndex = 14;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // loadButton
            // 
            this.loadButton.BackgroundImage = global::text_alignment.Properties.Resources._1217861754507811278mightyman_Button_Icons_3_svg_med;
            this.loadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loadButton.Location = new System.Drawing.Point(115, -1);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(34, 34);
            this.loadButton.TabIndex = 13;
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // playButton
            // 
            this.playButton.BackgroundImage = global::text_alignment.Properties.Resources.play;
            this.playButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.playButton.Location = new System.Drawing.Point(191, -1);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(33, 34);
            this.playButton.TabIndex = 6;
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // findLineButton
            // 
            this.findLineButton.BackgroundImage = global::text_alignment.Properties.Resources.kuba_arrow_button_right1;
            this.findLineButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.findLineButton.Location = new System.Drawing.Point(154, -1);
            this.findLineButton.Name = "findLineButton";
            this.findLineButton.Size = new System.Drawing.Size(31, 35);
            this.findLineButton.TabIndex = 3;
            this.findLineButton.UseVisualStyleBackColor = true;
            this.findLineButton.Click += new System.EventHandler(this.findNextLine_Click);
            // 
            // saveButton
            // 
            this.saveButton.BackgroundImage = global::text_alignment.Properties.Resources.Floppy_Small_icon;
            this.saveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.saveButton.Location = new System.Drawing.Point(75, 0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(34, 34);
            this.saveButton.TabIndex = 2;
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // LoadManuButton
            // 
            this.LoadManuButton.BackgroundImage = global::text_alignment.Properties.Resources.NewFile1;
            this.LoadManuButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadManuButton.Location = new System.Drawing.Point(3, 0);
            this.LoadManuButton.Name = "LoadManuButton";
            this.LoadManuButton.Size = new System.Drawing.Size(31, 34);
            this.LoadManuButton.TabIndex = 0;
            this.LoadManuButton.TabStop = false;
            this.LoadManuButton.UseVisualStyleBackColor = true;
            this.LoadManuButton.Click += new System.EventHandler(this.onLoadManuscriptClick);
            // 
            // loadTextButton
            // 
            this.loadTextButton.BackgroundImage = global::text_alignment.Properties.Resources.NewFile___Copy1;
            this.loadTextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.loadTextButton.Location = new System.Drawing.Point(38, 0);
            this.loadTextButton.Name = "loadTextButton";
            this.loadTextButton.Size = new System.Drawing.Size(31, 34);
            this.loadTextButton.TabIndex = 1;
            this.loadTextButton.UseVisualStyleBackColor = true;
            this.loadTextButton.Click += new System.EventHandler(this.onLoadTextClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.widthLabel);
            this.panel4.Controls.Add(this.hightLabel);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Location = new System.Drawing.Point(683, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(579, 33);
            this.panel4.TabIndex = 21;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.BackgroundImage = global::text_alignment.Properties.Resources.menu_backgroud;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1354, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadManuscriptToolStripMenuItem,
            this.loadTextToolStripMenuItem,
            this.loadProjectToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadManuscriptToolStripMenuItem
            // 
            this.loadManuscriptToolStripMenuItem.Name = "loadManuscriptToolStripMenuItem";
            this.loadManuscriptToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.loadManuscriptToolStripMenuItem.Text = "Load Manuscript...";
            this.loadManuscriptToolStripMenuItem.Click += new System.EventHandler(this.loadManuscriptToolStripMenuItem_Click);
            // 
            // loadTextToolStripMenuItem
            // 
            this.loadTextToolStripMenuItem.Name = "loadTextToolStripMenuItem";
            this.loadTextToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.loadTextToolStripMenuItem.Text = "Load Transcript...";
            this.loadTextToolStripMenuItem.Click += new System.EventHandler(this.loadTextToolStripMenuItem_Click);
            // 
            // loadProjectToolStripMenuItem
            // 
            this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
            this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.loadProjectToolStripMenuItem.Text = "Load Project...";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.saveAsToolStripMenuItem.Text = "Save as...";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem});
            this.editToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setParametersMenuItem});
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // setParametersMenuItem
            // 
            this.setParametersMenuItem.Name = "setParametersMenuItem";
            this.setParametersMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setParametersMenuItem.Text = "Set Parameters";
            this.setParametersMenuItem.Click += new System.EventHandler(this.transcriptionFontToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hight: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Width:";
            // 
            // hightLabel
            // 
            this.hightLabel.AutoSize = true;
            this.hightLabel.Location = new System.Drawing.Point(71, 10);
            this.hightLabel.Name = "hightLabel";
            this.hightLabel.Size = new System.Drawing.Size(35, 13);
            this.hightLabel.TabIndex = 2;
            this.hightLabel.Text = "label3";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(178, 10);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(35, 13);
            this.widthLabel.TabIndex = 3;
            this.widthLabel.Text = "label3";
            // 
            // textBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::text_alignment.Properties.Resources.Untitled;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1354, 709);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "textBox";
            this.Text = "Paleography text alignment tool";
            this.Load += new System.EventHandler(this.textBox_Load_1);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.manuscriptPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.manuPicBox)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadManuButton;
        private System.Windows.Forms.Button loadTextButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Panel manuscriptPanel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem loadManuscriptToolStripMenuItem;
        private ToolStripMenuItem loadTextToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem setParametersMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Panel panel3;
        private PictureBox manuPicBox;
        private Panel panel2;
        private Button saveButton;
        private Button findLineButton;
        private Button playButton;
        private Panel panel4;
        private Button loadButton;
        private ToolStripMenuItem loadProjectToolStripMenuItem;
        private Button button1;
        private Button leftDirButton;
        private Button rightDirButton;
        private Label widthLabel;
        private Label hightLabel;
        private Label label2;
        private Label label1;
    }
}

