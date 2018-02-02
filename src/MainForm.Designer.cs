namespace JoyConTest
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.buttonScan = new System.Windows.Forms.Button();
			this.buttonStart = new System.Windows.Forms.Button();
			this.timerUpdate = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.picture1 = new System.Windows.Forms.PictureBox();
			this.picture2 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picture1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picture2)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonScan
			// 
			this.buttonScan.Location = new System.Drawing.Point(47, 34);
			this.buttonScan.Name = "buttonScan";
			this.buttonScan.Size = new System.Drawing.Size(75, 23);
			this.buttonScan.TabIndex = 0;
			this.buttonScan.Text = "Scan";
			this.buttonScan.UseVisualStyleBackColor = true;
			this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(151, 34);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(75, 23);
			this.buttonStart.TabIndex = 3;
			this.buttonStart.Text = "Start polling";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// timerUpdate
			// 
			this.timerUpdate.Interval = 16;
			this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(44, 73);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "press scan...";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(436, 73);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "press scan...";
			// 
			// picture1
			// 
			this.picture1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picture1.Location = new System.Drawing.Point(47, 335);
			this.picture1.Name = "picture1";
			this.picture1.Size = new System.Drawing.Size(275, 275);
			this.picture1.TabIndex = 7;
			this.picture1.TabStop = false;
			// 
			// picture2
			// 
			this.picture2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picture2.Location = new System.Drawing.Point(439, 335);
			this.picture2.Name = "picture2";
			this.picture2.Size = new System.Drawing.Size(275, 275);
			this.picture2.TabIndex = 8;
			this.picture2.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(780, 636);
			this.Controls.Add(this.picture2);
			this.Controls.Add(this.picture1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.buttonScan);
			this.Name = "MainForm";
			this.Text = "JoyCon CS";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.picture1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picture2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonScan;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Timer timerUpdate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox picture1;
		private System.Windows.Forms.PictureBox picture2;
	}
}

