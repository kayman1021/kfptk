namespace p00
{
    partial class Form1
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
            this.textBox_ImportRaw = new System.Windows.Forms.TextBox();
            this.textBox_ExportTiff = new System.Windows.Forms.TextBox();
            this.button_ImportRawXiaomi = new System.Windows.Forms.Button();
            this.button_ExportTiff = new System.Windows.Forms.Button();
            this.button_DeinterlaceVertical = new System.Windows.Forms.Button();
            this.button_InterlaceVertical = new System.Windows.Forms.Button();
            this.button_DeinterlaceHorizontal = new System.Windows.Forms.Button();
            this.button_InterlaceHorizontal = new System.Windows.Forms.Button();
            this.button_ExportIntArray = new System.Windows.Forms.Button();
            this.button_ImportRaw14bit = new System.Windows.Forms.Button();
            this.textBox_ExportIntArray = new System.Windows.Forms.TextBox();
            this.button_ImportRawBrowse = new System.Windows.Forms.Button();
            this.button_ExportTiffBrowse = new System.Windows.Forms.Button();
            this.button_ExportIntArrayBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_ImportRaw
            // 
            this.textBox_ImportRaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBox_ImportRaw.Location = new System.Drawing.Point(12, 12);
            this.textBox_ImportRaw.Multiline = true;
            this.textBox_ImportRaw.Name = "textBox_ImportRaw";
            this.textBox_ImportRaw.Size = new System.Drawing.Size(500, 24);
            this.textBox_ImportRaw.TabIndex = 0;
            // 
            // textBox_ExportTiff
            // 
            this.textBox_ExportTiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBox_ExportTiff.Location = new System.Drawing.Point(12, 150);
            this.textBox_ExportTiff.Multiline = true;
            this.textBox_ExportTiff.Name = "textBox_ExportTiff";
            this.textBox_ExportTiff.Size = new System.Drawing.Size(500, 24);
            this.textBox_ExportTiff.TabIndex = 2;
            // 
            // button_ImportRawXiaomi
            // 
            this.button_ImportRawXiaomi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ImportRawXiaomi.Location = new System.Drawing.Point(518, 42);
            this.button_ImportRawXiaomi.Name = "button_ImportRawXiaomi";
            this.button_ImportRawXiaomi.Size = new System.Drawing.Size(150, 24);
            this.button_ImportRawXiaomi.TabIndex = 3;
            this.button_ImportRawXiaomi.Text = "Import Raw Xiaomi";
            this.button_ImportRawXiaomi.UseVisualStyleBackColor = true;
            this.button_ImportRawXiaomi.Click += new System.EventHandler(this.button_ImportRawXiaomi_Click);
            // 
            // button_ExportTiff
            // 
            this.button_ExportTiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ExportTiff.Location = new System.Drawing.Point(518, 185);
            this.button_ExportTiff.Name = "button_ExportTiff";
            this.button_ExportTiff.Size = new System.Drawing.Size(150, 24);
            this.button_ExportTiff.TabIndex = 4;
            this.button_ExportTiff.Text = "Export Tiff";
            this.button_ExportTiff.UseVisualStyleBackColor = true;
            this.button_ExportTiff.Click += new System.EventHandler(this.button_ExportTiff_Click);
            // 
            // button_DeinterlaceVertical
            // 
            this.button_DeinterlaceVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_DeinterlaceVertical.Location = new System.Drawing.Point(12, 42);
            this.button_DeinterlaceVertical.Name = "button_DeinterlaceVertical";
            this.button_DeinterlaceVertical.Size = new System.Drawing.Size(160, 24);
            this.button_DeinterlaceVertical.TabIndex = 5;
            this.button_DeinterlaceVertical.Text = "Deinterlace Vertical";
            this.button_DeinterlaceVertical.UseVisualStyleBackColor = true;
            this.button_DeinterlaceVertical.Click += new System.EventHandler(this.button_DeinterlaceVertical_Click);
            // 
            // button_InterlaceVertical
            // 
            this.button_InterlaceVertical.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_InterlaceVertical.Location = new System.Drawing.Point(178, 42);
            this.button_InterlaceVertical.Name = "button_InterlaceVertical";
            this.button_InterlaceVertical.Size = new System.Drawing.Size(160, 24);
            this.button_InterlaceVertical.TabIndex = 6;
            this.button_InterlaceVertical.Text = "Interlace Vertical";
            this.button_InterlaceVertical.UseVisualStyleBackColor = true;
            this.button_InterlaceVertical.Click += new System.EventHandler(this.button_InterlaceVertical_Click);
            // 
            // button_DeinterlaceHorizontal
            // 
            this.button_DeinterlaceHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_DeinterlaceHorizontal.Location = new System.Drawing.Point(12, 72);
            this.button_DeinterlaceHorizontal.Name = "button_DeinterlaceHorizontal";
            this.button_DeinterlaceHorizontal.Size = new System.Drawing.Size(160, 24);
            this.button_DeinterlaceHorizontal.TabIndex = 7;
            this.button_DeinterlaceHorizontal.Text = "Deinterlace Horizontal";
            this.button_DeinterlaceHorizontal.UseVisualStyleBackColor = true;
            this.button_DeinterlaceHorizontal.Click += new System.EventHandler(this.button_DeinterlaceHorizontal_Click);
            // 
            // button_InterlaceHorizontal
            // 
            this.button_InterlaceHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_InterlaceHorizontal.Location = new System.Drawing.Point(178, 72);
            this.button_InterlaceHorizontal.Name = "button_InterlaceHorizontal";
            this.button_InterlaceHorizontal.Size = new System.Drawing.Size(160, 24);
            this.button_InterlaceHorizontal.TabIndex = 8;
            this.button_InterlaceHorizontal.Text = "Interlace Horizontal";
            this.button_InterlaceHorizontal.UseVisualStyleBackColor = true;
            this.button_InterlaceHorizontal.Click += new System.EventHandler(this.button_InterlaceHorizontal_Click);
            // 
            // button_ExportIntArray
            // 
            this.button_ExportIntArray.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ExportIntArray.Location = new System.Drawing.Point(518, 330);
            this.button_ExportIntArray.Name = "button_ExportIntArray";
            this.button_ExportIntArray.Size = new System.Drawing.Size(150, 24);
            this.button_ExportIntArray.TabIndex = 9;
            this.button_ExportIntArray.Text = "Export Int Array";
            this.button_ExportIntArray.UseVisualStyleBackColor = true;
            this.button_ExportIntArray.Click += new System.EventHandler(this.button_ExportIntArray_Click);
            // 
            // button_ImportRaw14bit
            // 
            this.button_ImportRaw14bit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ImportRaw14bit.Location = new System.Drawing.Point(674, 42);
            this.button_ImportRaw14bit.Name = "button_ImportRaw14bit";
            this.button_ImportRaw14bit.Size = new System.Drawing.Size(150, 24);
            this.button_ImportRaw14bit.TabIndex = 10;
            this.button_ImportRaw14bit.Text = "Import Raw 14bit";
            this.button_ImportRaw14bit.UseVisualStyleBackColor = true;
            this.button_ImportRaw14bit.Click += new System.EventHandler(this.button_ImportRaw14bit_Click);
            // 
            // textBox_ExportIntArray
            // 
            this.textBox_ExportIntArray.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBox_ExportIntArray.Location = new System.Drawing.Point(12, 300);
            this.textBox_ExportIntArray.Multiline = true;
            this.textBox_ExportIntArray.Name = "textBox_ExportIntArray";
            this.textBox_ExportIntArray.Size = new System.Drawing.Size(500, 24);
            this.textBox_ExportIntArray.TabIndex = 11;
            // 
            // button_ImportRawBrowse
            // 
            this.button_ImportRawBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ImportRawBrowse.Location = new System.Drawing.Point(518, 12);
            this.button_ImportRawBrowse.Name = "button_ImportRawBrowse";
            this.button_ImportRawBrowse.Size = new System.Drawing.Size(306, 24);
            this.button_ImportRawBrowse.TabIndex = 12;
            this.button_ImportRawBrowse.Text = "Browse for DNG";
            this.button_ImportRawBrowse.UseVisualStyleBackColor = true;
            this.button_ImportRawBrowse.Click += new System.EventHandler(this.button_ImportRawBrowse_Click);
            // 
            // button_ExportTiffBrowse
            // 
            this.button_ExportTiffBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ExportTiffBrowse.Location = new System.Drawing.Point(518, 150);
            this.button_ExportTiffBrowse.Name = "button_ExportTiffBrowse";
            this.button_ExportTiffBrowse.Size = new System.Drawing.Size(306, 24);
            this.button_ExportTiffBrowse.TabIndex = 13;
            this.button_ExportTiffBrowse.Text = "Browse for TIFF";
            this.button_ExportTiffBrowse.UseVisualStyleBackColor = true;
            this.button_ExportTiffBrowse.Click += new System.EventHandler(this.button_ExportTiffBrowse_Click);
            // 
            // button_ExportIntArrayBrowse
            // 
            this.button_ExportIntArrayBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ExportIntArrayBrowse.Location = new System.Drawing.Point(518, 300);
            this.button_ExportIntArrayBrowse.Name = "button_ExportIntArrayBrowse";
            this.button_ExportIntArrayBrowse.Size = new System.Drawing.Size(306, 24);
            this.button_ExportIntArrayBrowse.TabIndex = 14;
            this.button_ExportIntArrayBrowse.Text = "Browse for TXT";
            this.button_ExportIntArrayBrowse.UseVisualStyleBackColor = true;
            this.button_ExportIntArrayBrowse.Click += new System.EventHandler(this.button_ExportIntArrayBrowse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 369);
            this.Controls.Add(this.button_ExportIntArrayBrowse);
            this.Controls.Add(this.button_ExportTiffBrowse);
            this.Controls.Add(this.button_ImportRawBrowse);
            this.Controls.Add(this.textBox_ExportIntArray);
            this.Controls.Add(this.button_ImportRaw14bit);
            this.Controls.Add(this.button_ExportIntArray);
            this.Controls.Add(this.button_InterlaceHorizontal);
            this.Controls.Add(this.button_DeinterlaceHorizontal);
            this.Controls.Add(this.button_InterlaceVertical);
            this.Controls.Add(this.button_DeinterlaceVertical);
            this.Controls.Add(this.button_ExportTiff);
            this.Controls.Add(this.button_ImportRawXiaomi);
            this.Controls.Add(this.textBox_ExportTiff);
            this.Controls.Add(this.textBox_ImportRaw);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_ImportRaw;
        private System.Windows.Forms.TextBox textBox_ExportTiff;
        private System.Windows.Forms.Button button_ImportRawXiaomi;
        private System.Windows.Forms.Button button_ExportTiff;
        private System.Windows.Forms.Button button_DeinterlaceVertical;
        private System.Windows.Forms.Button button_InterlaceVertical;
        private System.Windows.Forms.Button button_DeinterlaceHorizontal;
        private System.Windows.Forms.Button button_InterlaceHorizontal;
        private System.Windows.Forms.Button button_ExportIntArray;
        private System.Windows.Forms.Button button_ImportRaw14bit;
        private System.Windows.Forms.TextBox textBox_ExportIntArray;
        private System.Windows.Forms.Button button_ImportRawBrowse;
        private System.Windows.Forms.Button button_ExportTiffBrowse;
        private System.Windows.Forms.Button button_ExportIntArrayBrowse;
    }
}

