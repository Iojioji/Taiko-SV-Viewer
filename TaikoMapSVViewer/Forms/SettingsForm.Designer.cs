namespace TaikoMapSVViewer
{
    partial class SettingsForm
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
            this.GraphLineColorSample = new System.Windows.Forms.PictureBox();
            this.GraphLineColorKiaiSample = new System.Windows.Forms.PictureBox();
            this.SaveColorsButton = new System.Windows.Forms.Button();
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.DefaultButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GraphLineColorSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphLineColorKiaiSample)).BeginInit();
            this.SuspendLayout();
            // 
            // GraphLineColorSample
            // 
            this.GraphLineColorSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.GraphLineColorSample.Location = new System.Drawing.Point(12, 105);
            this.GraphLineColorSample.Name = "GraphLineColorSample";
            this.GraphLineColorSample.Size = new System.Drawing.Size(100, 50);
            this.GraphLineColorSample.TabIndex = 0;
            this.GraphLineColorSample.TabStop = false;
            this.GraphLineColorSample.Click += new System.EventHandler(this.GraphLineColorSample_Click);
            // 
            // GraphLineColorKiaiSample
            // 
            this.GraphLineColorKiaiSample.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.GraphLineColorKiaiSample.Location = new System.Drawing.Point(186, 105);
            this.GraphLineColorKiaiSample.Name = "GraphLineColorKiaiSample";
            this.GraphLineColorKiaiSample.Size = new System.Drawing.Size(100, 50);
            this.GraphLineColorKiaiSample.TabIndex = 0;
            this.GraphLineColorKiaiSample.TabStop = false;
            this.GraphLineColorKiaiSample.Click += new System.EventHandler(this.GraphLineColorKiaiSample_Click);
            // 
            // SaveColorsButton
            // 
            this.SaveColorsButton.Location = new System.Drawing.Point(12, 341);
            this.SaveColorsButton.Name = "SaveColorsButton";
            this.SaveColorsButton.Size = new System.Drawing.Size(75, 23);
            this.SaveColorsButton.TabIndex = 1;
            this.SaveColorsButton.Text = "Save";
            this.SaveColorsButton.UseVisualStyleBackColor = true;
            this.SaveColorsButton.Click += new System.EventHandler(this.SaveColorsButton_Click);
            // 
            // DefaultButton
            // 
            this.DefaultButton.Location = new System.Drawing.Point(12, 312);
            this.DefaultButton.Name = "DefaultButton";
            this.DefaultButton.Size = new System.Drawing.Size(75, 23);
            this.DefaultButton.TabIndex = 1;
            this.DefaultButton.Text = "Default";
            this.DefaultButton.UseVisualStyleBackColor = true;
            this.DefaultButton.Click += new System.EventHandler(this.DefaultButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 376);
            this.Controls.Add(this.DefaultButton);
            this.Controls.Add(this.SaveColorsButton);
            this.Controls.Add(this.GraphLineColorKiaiSample);
            this.Controls.Add(this.GraphLineColorSample);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.GraphLineColorSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GraphLineColorKiaiSample)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox GraphLineColorSample;
        private System.Windows.Forms.PictureBox GraphLineColorKiaiSample;
        private System.Windows.Forms.Button SaveColorsButton;
        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.Button DefaultButton;
    }
}