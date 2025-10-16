namespace SNMP
{
    partial class NetworkScannerForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelScan = new Panel();
            labelRange = new Label();
            textBoxRange = new TextBox();
            buttonStartScan = new Button();
            progressBarScan = new ProgressBar();
            labelStatus = new Label();
            listViewDevices = new ListView();
            columnHeaderIP = new ColumnHeader();
            columnHeaderHostname = new ColumnHeader();
            columnHeaderDescription = new ColumnHeader();
            buttonSelect = new Button();
            buttonCancel = new Button();
            panelScan.SuspendLayout();
            SuspendLayout();
            // 
            // panelScan
            // 
            panelScan.Controls.Add(labelRange);
            panelScan.Controls.Add(textBoxRange);
            panelScan.Controls.Add(buttonStartScan);
            panelScan.Controls.Add(progressBarScan);
            panelScan.Controls.Add(labelStatus);
            panelScan.Location = new Point(12, 12);
            panelScan.Name = "panelScan";
            panelScan.Size = new Size(560, 120);
            panelScan.TabIndex = 0;
            // 
            // labelRange
            // 
            labelRange.AutoSize = true;
            labelRange.Location = new Point(15, 20);
            labelRange.Name = "labelRange";
            labelRange.Size = new Size(93, 15);
            labelRange.TabIndex = 0;
            labelRange.Text = "Plage d'adresses";
            // 
            // textBoxRange
            // 
            textBoxRange.Location = new Point(110, 17);
            textBoxRange.Name = "textBoxRange";
            textBoxRange.Size = new Size(200, 23);
            textBoxRange.TabIndex = 1;
            textBoxRange.Text = "192.168.1.1-192.168.1.254";
            // 
            // buttonStartScan
            // 
            buttonStartScan.Location = new Point(320, 16);
            buttonStartScan.Name = "buttonStartScan";
            buttonStartScan.Size = new Size(100, 25);
            buttonStartScan.TabIndex = 2;
            buttonStartScan.Text = "Scanner";
            buttonStartScan.UseVisualStyleBackColor = true;
            buttonStartScan.Click += buttonStartScan_Click_1;
            // 
            // progressBarScan
            // 
            progressBarScan.Location = new Point(15, 55);
            progressBarScan.Name = "progressBarScan";
            progressBarScan.Size = new Size(530, 23);
            progressBarScan.TabIndex = 3;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(15, 85);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(81, 15);
            labelStatus.TabIndex = 4;
            labelStatus.Text = "Prêt à scanner";
            // 
            // listViewDevices
            // 
            listViewDevices.Columns.AddRange(new ColumnHeader[] { columnHeaderIP, columnHeaderHostname, columnHeaderDescription });
            listViewDevices.FullRowSelect = true;
            listViewDevices.GridLines = true;
            listViewDevices.Location = new Point(12, 150);
            listViewDevices.Name = "listViewDevices";
            listViewDevices.Size = new Size(560, 300);
            listViewDevices.TabIndex = 1;
            listViewDevices.UseCompatibleStateImageBehavior = false;
            listViewDevices.View = View.Details;
            // 
            // columnHeaderIP
            // 
            columnHeaderIP.Text = "Adresse IP";
            columnHeaderIP.Width = 120;
            // 
            // columnHeaderHostname
            // 
            columnHeaderHostname.Text = "Nom d'hôte";
            columnHeaderHostname.Width = 150;
            // 
            // columnHeaderDescription
            // 
            columnHeaderDescription.Text = "Description SNMP";
            columnHeaderDescription.Width = 280;
            // 
            // buttonSelect
            // 
            buttonSelect.Enabled = false;
            buttonSelect.Location = new Point(388, 465);
            buttonSelect.Name = "buttonSelect";
            buttonSelect.Size = new Size(103, 30);
            buttonSelect.TabIndex = 2;
            buttonSelect.Text = "Sélectionner";
            buttonSelect.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(497, 465);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 30);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Annuler";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // NetworkScannerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 511);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSelect);
            Controls.Add(listViewDevices);
            Controls.Add(panelScan);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NetworkScannerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Scanner réseau SNMP";
            panelScan.ResumeLayout(false);
            panelScan.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelScan;
        private Label labelRange;
        private TextBox textBoxRange;
        private Button buttonStartScan;
        private ProgressBar progressBarScan;
        private Label labelStatus;
        private ListView listViewDevices;
        private ColumnHeader columnHeaderIP;
        private ColumnHeader columnHeaderHostname;
        private ColumnHeader columnHeaderDescription;
        private Button buttonSelect;
        private Button buttonCancel;
    }
}