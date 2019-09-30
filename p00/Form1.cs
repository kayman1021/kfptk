using System;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace p00
{
    public partial class Form1 : Form
    {
        BP_Data rrwwtt = new BP_Data();

        public Form1()
        {
            InitializeComponent();
            comboBox_Import_DNG_Select.Items.Add(DngFileType.MLVApp14bit);
            comboBox_Import_DNG_Select.Items.Add(DngFileType.Xiaomi16bit);
            comboBox_Import_DNG_Select.SelectedIndex = 0;
        }

        private void button_Import_DNG_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "DNG file|*.dng";
                if (open.ShowDialog() == DialogResult.OK) { textBox_Import_DNG_Text.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_Import_DNG_Import_Click(object sender, EventArgs e)
        {
            if ((DngFileType)comboBox_Import_DNG_Select.SelectedItem == DngFileType.MLVApp14bit)
            {
                rrwwtt.Left = rrwwtt.ImportRawData14bitUncompressed(textBox_Import_DNG_Text.Text);
                //rrwwtt.ImportRawData14bitUncompressed2(textBox_Import_DNG_Text.Text);
            }
            if ((DngFileType)comboBox_Import_DNG_Select.SelectedItem == DngFileType.Xiaomi16bit)
            {
                rrwwtt.Left = rrwwtt.ImportRawDataXiaomi(textBox_Import_DNG_Text.Text);
            }
        }
        private void button_Import_FPM_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "FPM file|*.fpm";
                if (open.ShowDialog() == DialogResult.OK) { textBox_Import_FPM_Text.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_Import_FPM_Import_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.ImportPixelMapFromFPM(textBox_Import_FPM_Text.Text, rrwwtt.Left);
        }
        private void button_Import_TIFF_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "DNG file|*.dng";
                if (open.ShowDialog() == DialogResult.OK) { textBox_Import_TIFF_Text.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_Import_TIFF_Import_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.ImportRawDataTiff(textBox_Export_TIFF_Text.Text);
        }
        private void button_Import_Mapped_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif; *.tiff;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif; *.tiff";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    textBox_Import_Mapped_Text.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading file");
            }
        }
        private void button_Import_Mapped_Import_Click(object sender, EventArgs e)
        {
            //rrwwtt.Right = rrwwtt.ImportPixelMapFromPicture(textBox_Import_Mapped_Text.Text);
            rrwwtt.Right = rrwwtt.OpenAsPixelmap(new Bitmap(@textBox_Import_Mapped_Text.Text));
        }
        private void button_Tools_Deinterlace_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, false);
        }
        private void button_Tools_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, true);
        }
        private void button_Tools_Transpose_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.TransposeArray(rrwwtt.Left);
        }
        private void button_Tools_Interlace_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.InterlaceUniversal(rrwwtt.Left, false);
        }
        private void button_Tools_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.InterlaceUniversal(rrwwtt.Left, true);
        }
        private void button_Tools_SwapSides_Click(object sender, EventArgs e)
        {
            Matrix<double> temp = Matrix<double>.Build.Dense(rrwwtt.Left.RowCount, rrwwtt.Left.ColumnCount);
            temp = rrwwtt.Left;
            rrwwtt.Left = rrwwtt.Right;
            rrwwtt.Right = temp;
        }
        private void button_Export_TIFF_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "tiff file|*.tiff";
                save.ShowDialog();
                if (save.FileName != "") { textBox_Export_TIFF_Text.Text = save.FileName; }
            }
            catch (Exception) { throw new ApplicationException(""); }
        }
        private void button_Export_TIFF_Export_Click(object sender, EventArgs e)
        {
            rrwwtt.ExportRawDataTiff(@textBox_Export_TIFF_Text.Text, rrwwtt.Left);
        }
        private void button_Export_FPM_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "fpm file|*.fpm";
                save.ShowDialog();
                if (save.FileName != "") { textBox_Export_FPM_Text.Text = save.FileName; }
            }
            catch (Exception) { throw new ApplicationException(""); }
        }
        private void button_Export_FPM_Export_Click(object sender, EventArgs e)
        {
            rrwwtt.ExportFPM(@textBox_Export_FPM_Text.Text, rrwwtt.Right);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, false);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            rrwwtt.ExportRawDataXiaomi(rrwwtt.Left, @textBox_Import_DNG_Text.Text);
        }

        private void button_EXP_EOS_Click(object sender, EventArgs e)
        {
            rrwwtt.ExportRawData14bitUncompressed(rrwwtt.Left, @textBox_Import_DNG_Text.Text);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            System.IO.Stream myStream;
            OpenFileDialog thisDialog = new OpenFileDialog();

            thisDialog.Filter = "All files (*.*)|*.*";
            thisDialog.FilterIndex = 2;
            thisDialog.RestoreDirectory = true;
            thisDialog.Multiselect = true;
            thisDialog.Title = "Please Select Source File(s) for Conversion";

            if (thisDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in thisDialog.FileNames)
                {
                    try
                    {
                        if ((myStream = thisDialog.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                listBox_MASS_DUAL_ISO.Items.Add(file);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int radius = 3;
            int rounds = 2;
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 0)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 1, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 1, 1), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 1)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 2, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 2), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 2)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 3, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 1, 3), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 3)));

            for (int i = 0; i < rounds; i++)
            {
                rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 0), radius));
                rrwwtt.ModifyBlock(rrwwtt.Left, 1, 1, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 1, 1), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 1), radius));
                rrwwtt.ModifyBlock(rrwwtt.Left, 0, 2, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 2), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 2), radius));
                rrwwtt.ModifyBlock(rrwwtt.Left, 1, 3, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 1, 3), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 3), radius));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int radius = 3;
            int rounds = 2;
            BP_Data mass = new BP_Data();
            foreach (string path in listBox_MASS_DUAL_ISO.Items)
            {
                mass.Left = mass.ImportRawData14bitUncompressed(@path);
                mass.Left = mass.DeinterlaceUniversal(mass.Left, true);

                mass.ModifyBlock(mass.Left, 0, 0, mass.Prefit(mass.SliceBlock(mass.Left, 2, 4, 0, 0), mass.SliceBlock(rrwwtt.Right, 2, 4, 0, 0)));
                mass.ModifyBlock(mass.Left, 1, 1, mass.Prefit(mass.SliceBlock(mass.Left, 2, 4, 1, 1), mass.SliceBlock(rrwwtt.Right, 2, 4, 1, 1)));
                mass.ModifyBlock(mass.Left, 0, 2, mass.Prefit(mass.SliceBlock(mass.Left, 2, 4, 0, 2), mass.SliceBlock(rrwwtt.Right, 2, 4, 0, 2)));
                mass.ModifyBlock(mass.Left, 1, 3, mass.Prefit(mass.SliceBlock(mass.Left, 2, 4, 1, 3), mass.SliceBlock(rrwwtt.Right, 2, 4, 1, 3)));

                for (int i = 0; i < rounds; i++)
                {
                    mass.ModifyBlock(mass.Left, 0, 0, mass.Collector(mass.SliceBlock(mass.Left, 2, 4, 0, 0), mass.SliceBlock(rrwwtt.Right, 2, 4, 0, 0), radius));
                    mass.ModifyBlock(mass.Left, 1, 1, mass.Collector(mass.SliceBlock(mass.Left, 2, 4, 1, 1), mass.SliceBlock(rrwwtt.Right, 2, 4, 1, 1), radius));
                    mass.ModifyBlock(mass.Left, 0, 2, mass.Collector(mass.SliceBlock(mass.Left, 2, 4, 0, 2), mass.SliceBlock(rrwwtt.Right, 2, 4, 0, 2), radius));
                    mass.ModifyBlock(mass.Left, 1, 3, mass.Collector(mass.SliceBlock(mass.Left, 2, 4, 1, 3), mass.SliceBlock(rrwwtt.Right, 2, 4, 1, 3), radius));
                }

                mass.Left = mass.InterlaceUniversal(mass.Left, true);
                mass.ExportRawData14bitUncompressed(mass.Left, @path);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //rrwwtt.Left= rrwwtt.Reflow(rrwwtt.Left, ((3/1856)+32)+3);
            rrwwtt.Left = rrwwtt.ImportAdobeConverted(textBox_Import_DNG_Text.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int radius = 3;
            int sliceX = 2;
            int sliceY = 2;
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Prefit2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 0)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 1, rrwwtt.Prefit2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 1), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 1)));
            //rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Collector2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 0), radius));
           //rrwwtt.ModifyBlock(rrwwtt.Left, 0, 1, rrwwtt.Collector2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 1), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 1), radius));
            Console.WriteLine();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            rrwwtt.ExportAdobeConverted(rrwwtt.Left, textBox_Import_DNG_Text.Text);
        }
    }
}