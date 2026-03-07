namespace KooliProjekt.WinFormsApp;

partial class Form1
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
        this.dataGridView1 = new System.Windows.Forms.DataGridView();
        this.idTextBox = new System.Windows.Forms.TextBox();
        this.nameTextBox = new System.Windows.Forms.TextBox();
        this.newButton = new System.Windows.Forms.Button();
        this.saveButton = new System.Windows.Forms.Button();
        this.deleteButton = new System.Windows.Forms.Button();
        this.refreshButton = new System.Windows.Forms.Button();
        this.statusLabel = new System.Windows.Forms.Label();
        this.labelId = new System.Windows.Forms.Label();
        this.labelName = new System.Windows.Forms.Label();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        this.groupBox1.SuspendLayout();
        this.SuspendLayout();
        // 
        // dataGridView1
        // 
        this.dataGridView1.AllowUserToAddRows = false;
        this.dataGridView1.AllowUserToDeleteRows = false;
        this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridView1.Location = new System.Drawing.Point(12, 65);
        this.dataGridView1.Name = "dataGridView1";
        this.dataGridView1.ReadOnly = true;
        this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dataGridView1.Size = new System.Drawing.Size(560, 350);
        this.dataGridView1.TabIndex = 0;
        this.dataGridView1.SelectionChanged += new System.EventHandler(this.DataGridView1_SelectionChanged);
        // 
        // idTextBox
        // 
        this.idTextBox.Location = new System.Drawing.Point(70, 30);
        this.idTextBox.Name = "idTextBox";
        this.idTextBox.ReadOnly = true;
        this.idTextBox.Size = new System.Drawing.Size(200, 27);
        this.idTextBox.TabIndex = 1;
        this.idTextBox.BackColor = System.Drawing.SystemColors.Control;
        // 
        // nameTextBox
        // 
        this.nameTextBox.Location = new System.Drawing.Point(70, 70);
        this.nameTextBox.Name = "nameTextBox";
        this.nameTextBox.Size = new System.Drawing.Size(200, 27);
        this.nameTextBox.TabIndex = 2;
        // 
        // newButton
        // 
        this.newButton.BackColor = System.Drawing.Color.LightBlue;
        this.newButton.Location = new System.Drawing.Point(20, 120);
        this.newButton.Name = "newButton";
        this.newButton.Size = new System.Drawing.Size(120, 40);
        this.newButton.TabIndex = 3;
        this.newButton.Text = "New";
        this.newButton.UseVisualStyleBackColor = false;
        this.newButton.Click += new System.EventHandler(this.NewButton_Click);
        // 
        // saveButton
        // 
        this.saveButton.BackColor = System.Drawing.Color.LightGreen;
        this.saveButton.Location = new System.Drawing.Point(150, 120);
        this.saveButton.Name = "saveButton";
        this.saveButton.Size = new System.Drawing.Size(120, 40);
        this.saveButton.TabIndex = 4;
        this.saveButton.Text = "Save";
        this.saveButton.UseVisualStyleBackColor = false;
        this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
        // 
        // deleteButton
        // 
        this.deleteButton.BackColor = System.Drawing.Color.LightCoral;
        this.deleteButton.Location = new System.Drawing.Point(20, 170);
        this.deleteButton.Name = "deleteButton";
        this.deleteButton.Size = new System.Drawing.Size(120, 40);
        this.deleteButton.TabIndex = 5;
        this.deleteButton.Text = "Delete";
        this.deleteButton.UseVisualStyleBackColor = false;
        this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
        // 
        // refreshButton
        // 
        this.refreshButton.Location = new System.Drawing.Point(12, 12);
        this.refreshButton.Name = "refreshButton";
        this.refreshButton.Size = new System.Drawing.Size(120, 40);
        this.refreshButton.TabIndex = 6;
        this.refreshButton.Text = "Refresh";
        this.refreshButton.UseVisualStyleBackColor = true;
        this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
        // 
        // statusLabel
        // 
        this.statusLabel.AutoSize = true;
        this.statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.statusLabel.Location = new System.Drawing.Point(0, 428);
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Padding = new System.Windows.Forms.Padding(5);
        this.statusLabel.Size = new System.Drawing.Size(59, 30);
        this.statusLabel.TabIndex = 7;
        this.statusLabel.Text = "Ready";
        // 
        // labelId
        // 
        this.labelId.AutoSize = true;
        this.labelId.Location = new System.Drawing.Point(20, 33);
        this.labelId.Name = "labelId";
        this.labelId.Size = new System.Drawing.Size(27, 20);
        this.labelId.TabIndex = 8;
        this.labelId.Text = "ID:";
        // 
        // labelName
        // 
        this.labelName.AutoSize = true;
        this.labelName.Location = new System.Drawing.Point(20, 73);
        this.labelName.Name = "labelName";
        this.labelName.Size = new System.Drawing.Size(52, 20);
        this.labelName.TabIndex = 9;
        this.labelName.Text = "Name:";
        // 
        // groupBox1
        // 
        this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.groupBox1.Controls.Add(this.labelId);
        this.groupBox1.Controls.Add(this.idTextBox);
        this.groupBox1.Controls.Add(this.labelName);
        this.groupBox1.Controls.Add(this.nameTextBox);
        this.groupBox1.Controls.Add(this.newButton);
        this.groupBox1.Controls.Add(this.saveButton);
        this.groupBox1.Controls.Add(this.deleteButton);
        this.groupBox1.Location = new System.Drawing.Point(590, 65);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(290, 230);
        this.groupBox1.TabIndex = 10;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Team Details";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(892, 458);
        this.Controls.Add(this.statusLabel);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.refreshButton);
        this.Controls.Add(this.dataGridView1);
        this.Name = "Form1";
        this.Text = "Teams Management - Windows Forms";
        this.Load += new System.EventHandler(this.Form1_Load);
        ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.TextBox idTextBox;
    private System.Windows.Forms.TextBox nameTextBox;
    private System.Windows.Forms.Button newButton;
    private System.Windows.Forms.Button saveButton;
    private System.Windows.Forms.Button deleteButton;
    private System.Windows.Forms.Button refreshButton;
    private System.Windows.Forms.Label statusLabel;
    private System.Windows.Forms.Label labelId;
    private System.Windows.Forms.Label labelName;
    private System.Windows.Forms.GroupBox groupBox1;
}
