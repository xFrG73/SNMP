namespace SNMP
{
    partial class SNMPTool
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SNMPTool));
            panelMain = new Panel();
            buttonStart = new Button();
            comboBoxOIDs = new ComboBox();
            labelOID = new Label();
            textBoxOID = new TextBox();
            buttonScan = new Button();
            labelIP = new Label();
            textBoxIP = new TextBox();
            listViewResults = new ListView();
            columnHeaderID = new ColumnHeader();
            columnHeaderOID = new ColumnHeader();
            columnHeaderResult = new ColumnHeader();
            label1 = new Label();
            label2 = new Label();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelMain
            // 
            panelMain.Controls.Add(buttonStart);
            panelMain.Controls.Add(comboBoxOIDs);
            panelMain.Controls.Add(labelOID);
            panelMain.Controls.Add(textBoxOID);
            panelMain.Controls.Add(buttonScan);
            panelMain.Controls.Add(labelIP);
            panelMain.Controls.Add(textBoxIP);
            panelMain.Location = new Point(19, 12);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(404, 174);
            panelMain.TabIndex = 4;
            // 
            // buttonStart
            // 
            buttonStart.BackColor = SystemColors.HighlightText;
            buttonStart.Location = new Point(257, 137);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(111, 23);
            buttonStart.TabIndex = 5;
            buttonStart.Text = "Démarrer";
            buttonStart.UseVisualStyleBackColor = false;
            buttonStart.Click += buttonStart_Click;
            // 
            // comboBoxOIDs
            // 
            comboBoxOIDs.FormattingEnabled = true;
            comboBoxOIDs.Location = new Point(77, 97);
            comboBoxOIDs.Name = "comboBoxOIDs";
            comboBoxOIDs.Size = new Size(291, 23);
            comboBoxOIDs.TabIndex = 4;
            comboBoxOIDs.Text = "Selection automatique de l'OID";
            // 
            // labelOID
            // 
            labelOID.AutoSize = true;
            labelOID.Location = new Point(31, 68);
            labelOID.Name = "labelOID";
            labelOID.Size = new Size(27, 15);
            labelOID.TabIndex = 3;
            labelOID.Text = "OID";
            // 
            // textBoxOID
            // 
            textBoxOID.Location = new Point(77, 68);
            textBoxOID.Name = "textBoxOID";
            textBoxOID.Size = new Size(291, 23);
            textBoxOID.TabIndex = 2;
            // 
            // buttonScan
            // 
            buttonScan.Location = new Point(338, 30);
            buttonScan.Name = "buttonScan";
            buttonScan.Size = new Size(30, 23);
            buttonScan.TabIndex = 6;
            buttonScan.Text = "🔍";
            buttonScan.UseVisualStyleBackColor = true;
            buttonScan.Click += buttonScan_Click_1;
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Location = new Point(41, 33);
            labelIP.Name = "labelIP";
            labelIP.Size = new Size(17, 15);
            labelIP.TabIndex = 1;
            labelIP.Text = "IP";
            // 
            // textBoxIP
            // 
            textBoxIP.Location = new Point(77, 30);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.Size = new Size(255, 23);
            textBoxIP.TabIndex = 0;
            // 
            // listViewResults
            // 
            listViewResults.Columns.AddRange(new ColumnHeader[] { columnHeaderID, columnHeaderOID, columnHeaderResult });
            listViewResults.Location = new Point(19, 203);
            listViewResults.Name = "listViewResults";
            listViewResults.Size = new Size(404, 177);
            listViewResults.TabIndex = 5;
            listViewResults.UseCompatibleStateImageBehavior = false;
            listViewResults.View = View.Details;
            // 
            // columnHeaderID
            // 
            columnHeaderID.Text = "ID";
            columnHeaderID.Width = 50;
            // 
            // columnHeaderOID
            // 
            columnHeaderOID.Text = "OID";
            columnHeaderOID.Width = 150;
            // 
            // columnHeaderResult
            // 
            columnHeaderResult.Text = "RESULT";
            columnHeaderResult.Width = 200;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 390);
            label1.Name = "label1";
            label1.Size = new Size(141, 15);
            label1.TabIndex = 6;
            label1.Text = "Créer par Pierre Bouillaud";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(377, 390);
            label2.Name = "label2";
            label2.Size = new Size(46, 15);
            label2.TabIndex = 7;
            label2.Text = "v1.0.0.0";
            // 
            // SNMPTool
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.HighlightText;
            ClientSize = new Size(438, 414);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listViewResults);
            Controls.Add(panelMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "SNMPTool";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SNMP TOOL";
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panelMain;
        private Label labelOID;
        private TextBox textBoxOID;
        private Label labelIP;
        private TextBox textBoxIP;
        private Button buttonScan;
        private Button buttonStart;
        private ComboBox comboBoxOIDs;
        private ListView listViewResults;
        private ColumnHeader columnHeaderID;
        private ColumnHeader columnHeaderOID;
        private ColumnHeader columnHeaderResult;
        private Label label1;
        private Label label2;
    }
}
