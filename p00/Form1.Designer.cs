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
            this.button_Deinterlace = new System.Windows.Forms.Button();
            this.button_Interlace = new System.Windows.Forms.Button();
            this.button_ExportIntArray = new System.Windows.Forms.Button();
            this.button_ImportRaw14bit = new System.Windows.Forms.Button();
            this.textBox_ExportIntArray = new System.Windows.Forms.TextBox();
            this.button_ImportRawBrowse = new System.Windows.Forms.Button();
            this.button_ExportTiffBrowse = new System.Windows.Forms.Button();
            this.button_ExportIntArrayBrowse = new System.Windows.Forms.Button();
            this.button_SeperateDualISO = new System.Windows.Forms.Button();
            this.button_Transpose = new System.Windows.Forms.Button();
            this.button_DeinterlaceDualISO = new System.Windows.Forms.Button();
            this.button_InterlaceDualISO = new System.Windows.Forms.Button();
            this.button_KeepTopHalf = new System.Windows.Forms.Button();
            this.button_KeepBottomHalf = new System.Windows.Forms.Button();
            this.button_AlternateDualISO = new System.Windows.Forms.Button();
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
            this.textBox_ExportTiff.Location = new System.Drawing.Point(12, 500);
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
            this.button_ExportTiff.Location = new System.Drawing.Point(518, 530);
            this.button_ExportTiff.Name = "button_ExportTiff";
            this.button_ExportTiff.Size = new System.Drawing.Size(150, 24);
            this.button_ExportTiff.TabIndex = 4;
            this.button_ExportTiff.Text = "Export Tiff";
            this.button_ExportTiff.UseVisualStyleBackColor = true;
            this.button_ExportTiff.Click += new System.EventHandler(this.button_ExportTiff_Click);
            // 
            // button_Deinterlace
            // 
            this.button_Deinterlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_Deinterlace.Location = new System.Drawing.Point(12, 100);
            this.button_Deinterlace.Name = "button_Deinterlace";
            this.button_Deinterlace.Size = new System.Drawing.Size(160, 24);
            this.button_Deinterlace.TabIndex = 5;
            this.button_Deinterlace.Text = "Deinterlace";
            this.button_Deinterlace.UseVisualStyleBackColor = true;
            this.button_Deinterlace.Click += new System.EventHandler(this.button_Deinterlace_Click);
            // 
            // button_Interlace
            // 
            this.button_Interlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_Interlace.Location = new System.Drawing.Point(178, 100);
            this.button_Interlace.Name = "button_Interlace";
            this.button_Interlace.Size = new System.Drawing.Size(160, 24);
            this.button_Interlace.TabIndex = 6;
            this.button_Interlace.Text = "Interlace";
            this.button_Interlace.UseVisualStyleBackColor = true;
            this.button_Interlace.Click += new System.EventHandler(this.button_Interlace_Click);
            // 
            // button_ExportIntArray
            // 
            this.button_ExportIntArray.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_ExportIntArray.Location = new System.Drawing.Point(518, 630);
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
            this.textBox_ExportIntArray.Location = new System.Drawing.Point(12, 600);
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
            this.button_ExportTiffBrowse.Location = new System.Drawing.Point(518, 500);
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
            this.button_ExportIntArrayBrowse.Location = new System.Drawing.Point(518, 600);
            this.button_ExportIntArrayBrowse.Name = "button_ExportIntArrayBrowse";
            this.button_ExportIntArrayBrowse.Size = new System.Drawing.Size(306, 24);
            this.button_ExportIntArrayBrowse.TabIndex = 14;
            this.button_ExportIntArrayBrowse.Text = "Browse for TXT";
            this.button_ExportIntArrayBrowse.UseVisualStyleBackColor = true;
            this.button_ExportIntArrayBrowse.Click += new System.EventHandler(this.button_ExportIntArrayBrowse_Click);
            // 
            // button_SeperateDualISO
            // 
            this.button_SeperateDualISO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_SeperateDualISO.Location = new System.Drawing.Point(12, 300);
            this.button_SeperateDualISO.Name = "button_SeperateDualISO";
            this.button_SeperateDualISO.Size = new System.Drawing.Size(160, 24);
            this.button_SeperateDualISO.TabIndex = 15;
            this.button_SeperateDualISO.Text = "Seperate Dual ISO";
            this.button_SeperateDualISO.UseVisualStyleBackColor = true;
            this.button_SeperateDualISO.Click += new System.EventHandler(this.button_SeperateDualISO_Click);
            // 
            // button_Transpose
            // 
            this.button_Transpose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_Transpose.Location = new System.Drawing.Point(12, 400);
            this.button_Transpose.Name = "button_Transpose";
            this.button_Transpose.Size = new System.Drawing.Size(160, 24);
            this.button_Transpose.TabIndex = 16;
            this.button_Transpose.Text = "Transpose Image";
            this.button_Transpose.UseVisualStyleBackColor = true;
            this.button_Transpose.Click += new System.EventHandler(this.button_Transpose_Click);
            // 
            // button_DeinterlaceDualISO
            // 
            this.button_DeinterlaceDualISO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_DeinterlaceDualISO.Location = new System.Drawing.Point(12, 330);
            this.button_DeinterlaceDualISO.Name = "button_DeinterlaceDualISO";
            this.button_DeinterlaceDualISO.Size = new System.Drawing.Size(160, 24);
            this.button_DeinterlaceDualISO.TabIndex = 17;
            this.button_DeinterlaceDualISO.Text = "Deinterlace Dual ISO";
            this.button_DeinterlaceDualISO.UseVisualStyleBackColor = true;
            this.button_DeinterlaceDualISO.Click += new System.EventHandler(this.button_DeinterlaceDualISO_Click);
            // 
            // button_InterlaceDualISO
            // 
            this.button_InterlaceDualISO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_InterlaceDualISO.Location = new System.Drawing.Point(178, 330);
            this.button_InterlaceDualISO.Name = "button_InterlaceDualISO";
            this.button_InterlaceDualISO.Size = new System.Drawing.Size(160, 24);
            this.button_InterlaceDualISO.TabIndex = 18;
            this.button_InterlaceDualISO.Text = "Interlace Dual ISO";
            this.button_InterlaceDualISO.UseVisualStyleBackColor = true;
            this.button_InterlaceDualISO.Click += new System.EventHandler(this.button_InterlaceDualISO_Click);
            // 
            // button_KeepTopHalf
            // 
            this.button_KeepTopHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_KeepTopHalf.Location = new System.Drawing.Point(12, 200);
            this.button_KeepTopHalf.Name = "button_KeepTopHalf";
            this.button_KeepTopHalf.Size = new System.Drawing.Size(160, 24);
            this.button_KeepTopHalf.TabIndex = 19;
            this.button_KeepTopHalf.Text = "Keep Top Half";
            this.button_KeepTopHalf.UseVisualStyleBackColor = true;
            this.button_KeepTopHalf.Click += new System.EventHandler(this.button_KeepTopHalf_Click);
            // 
            // button_KeepBottomHalf
            // 
            this.button_KeepBottomHalf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_KeepBottomHalf.Location = new System.Drawing.Point(178, 200);
            this.button_KeepBottomHalf.Name = "button_KeepBottomHalf";
            this.button_KeepBottomHalf.Size = new System.Drawing.Size(160, 24);
            this.button_KeepBottomHalf.TabIndex = 20;
            this.button_KeepBottomHalf.Text = "Keep Bottom Half";
            this.button_KeepBottomHalf.UseVisualStyleBackColor = true;
            this.button_KeepBottomHalf.Click += new System.EventHandler(this.button_KeepBottomHalf_Click);
            // 
            // button_AlternateDualISO
            // 
            this.button_AlternateDualISO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button_AlternateDualISO.Location = new System.Drawing.Point(178, 300);
            this.button_AlternateDualISO.Name = "button_AlternateDualISO";
            this.button_AlternateDualISO.Size = new System.Drawing.Size(160, 24);
            this.button_AlternateDualISO.TabIndex = 21;
            this.button_AlternateDualISO.Text = "Alternate Dual ISO";
            this.button_AlternateDualISO.UseVisualStyleBackColor = true;
            this.button_AlternateDualISO.Click += new System.EventHandler(this.button_AlternateDualISO_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 689);
            this.Controls.Add(this.button_AlternateDualISO);
            this.Controls.Add(this.button_KeepBottomHalf);
            this.Controls.Add(this.button_KeepTopHalf);
            this.Controls.Add(this.button_InterlaceDualISO);
            this.Controls.Add(this.button_DeinterlaceDualISO);
            this.Controls.Add(this.button_Transpose);
            this.Controls.Add(this.button_SeperateDualISO);
            this.Controls.Add(this.button_ExportIntArrayBrowse);
            this.Controls.Add(this.button_ExportTiffBrowse);
            this.Controls.Add(this.button_ImportRawBrowse);
            this.Controls.Add(this.textBox_ExportIntArray);
            this.Controls.Add(this.button_ImportRaw14bit);
            this.Controls.Add(this.button_ExportIntArray);
            this.Controls.Add(this.button_Interlace);
            this.Controls.Add(this.button_Deinterlace);
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
        private System.Windows.Forms.Button button_Deinterlace;
        private System.Windows.Forms.Button button_Interlace;
        private System.Windows.Forms.Button button_ExportIntArray;
        private System.Windows.Forms.Button button_ImportRaw14bit;
        private System.Windows.Forms.TextBox textBox_ExportIntArray;
        private System.Windows.Forms.Button button_ImportRawBrowse;
        private System.Windows.Forms.Button button_ExportTiffBrowse;
        private System.Windows.Forms.Button button_ExportIntArrayBrowse;
        private System.Windows.Forms.Button button_SeperateDualISO;
        private System.Windows.Forms.Button button_Transpose;
        private System.Windows.Forms.Button button_DeinterlaceDualISO;
        private System.Windows.Forms.Button button_InterlaceDualISO;
        private System.Windows.Forms.Button button_KeepTopHalf;
        private System.Windows.Forms.Button button_KeepBottomHalf;
        private System.Windows.Forms.Button button_AlternateDualISO;
    }
}

