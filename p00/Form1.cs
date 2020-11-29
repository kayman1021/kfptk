using System;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using System.Windows;
using System.IO;

namespace p00
{
    public partial class Form1 : Form
    {
        //MLV_DATA fff = new MLV_DATA(@"F:\\ooo.MLV");
        //MLV_DATA fff = new MLV_DATA(@"E:\\MLV\M06-0752.MLV");
        //StringFinder sf = new StringFinder(@"E:\\MLV\M26-2108.MLV");
        //StringFinder sf = new StringFinder(@"F:\\ooo.MLV");

        Size GroupboxSize = new Size(308, 500);
        BP_Data rrwwtt = new BP_Data();
        PPP_Groupbox ppp;
        PPP_Groupbox qqq;
        PPP_Groupbox rrr;

        public Form1()
        {
            ppp = new PPP_Groupbox("Raw", GroupboxSize.Width, GroupboxSize.Height, 1);
            qqq = new PPP_Groupbox("Pixel Map", GroupboxSize.Width,GroupboxSize.Height, 2);
            rrr = new PPP_Groupbox("Bad Pixel", GroupboxSize.Width, GroupboxSize.Height, 3);
            this.Controls.Add(ppp);
            this.Controls.Add(qqq);
            this.Controls.Add(rrr);
            InitializeComponent();
            //comboBox_Import_DNG_Select.Items.Add(DngFileType.MLVApp14bit);
            //comboBox_Import_DNG_Select.Items.Add(DngFileType.Xiaomi16bit);
            //comboBox_Import_DNG_Select.SelectedIndex = 0;
        }
        /*
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
                rrwwtt.LLL = rrwwtt._ImportRawData14bitUncompressed(textBox_Import_DNG_Text.Text);
            }
            if ((DngFileType)comboBox_Import_DNG_Select.SelectedItem == DngFileType.Xiaomi16bit)
            {
                rrwwtt.LLL = rrwwtt._ImportRawDataXiaomi(textBox_Import_DNG_Text.Text);
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
            rrwwtt.RRR = rrwwtt._ImportPixelMapFromFPM(rrwwtt.LLL.Width(), rrwwtt.LLL.Height(), textBox_Import_FPM_Text.Text);
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
            rrwwtt.LLL = rrwwtt._ImportRawDataTiff(textBox_Export_TIFF_Text.Text);
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
            rrwwtt.RRR = rrwwtt._OpenAsPixelmap(new Bitmap(@textBox_Import_Mapped_Text.Text));
        }
        private void button_Tools_Deinterlace_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(2, 2));
        }
        private void button_Tools_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(2, 4));
        }
        private void button_Tools_Transpose_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.Transpose());
        }
        private void button_Tools_Interlace_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.InterlaceUniversal(2, 2));
        }
        private void button_Tools_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.InterlaceUniversal(2, 4));
        }
        private void button_Tools_SwapSides_Click(object sender, EventArgs e)
        {
            rrwwtt._SwapSides();
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
            rrwwtt._ExportRawDataTiff(rrwwtt.LLL, @textBox_Export_TIFF_Text.Text);
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
            rrwwtt._ExportFPM(rrwwtt.LLL, @textBox_Export_FPM_Text.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(2, 4));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(2, 2));
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            rrwwtt._ExportRawDataXiaomi(rrwwtt.LLL.Data(), @textBox_Import_DNG_Text.Text);
        }

        private void button_EXP_EOS_Click(object sender, EventArgs e)
        {
            rrwwtt._ExportRawData14bitUncompressed(rrwwtt.LLL.Data(), @textBox_Import_DNG_Text.Text);
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

        private void button10_Click(object sender, EventArgs e)//DUAL CANON
        {
            int radius = 3;
            int rounds = 2;
            //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(2, 4, 0, 0), rrwwtt.RRR.SliceBlock(2, 4, 0, 0)));
            //rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(2, 4, 1, 1), rrwwtt.RRR.SliceBlock(2, 4, 1, 1)));
            //rrwwtt.LLL.ModifyBlock(0, 2, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(2, 4, 0, 2), rrwwtt.RRR.SliceBlock(2, 4, 0, 2)));
            //rrwwtt.LLL.ModifyBlock(1, 3, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(2, 4, 1, 3), rrwwtt.RRR.SliceBlock(2, 4, 1, 3)));



            int sliceX = 2;
            int sliceY = 4;
            //Console.WriteLine();
            //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0)));
            //rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 1)));
            //rrwwtt.LLL.ModifyBlock(0, 2, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 2), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 2)));
            //rrwwtt.LLL.ModifyBlock(1, 3, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 3), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 3)));

            for (int i = 0; i < rounds; i++)
            {
                rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0), radius));
                rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 1), radius));
                rrwwtt.LLL.ModifyBlock(0, 2, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 2), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 2), radius));
                rrwwtt.LLL.ModifyBlock(1, 3, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 3), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 3), radius));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            //BP_Data mass = new BP_Data();
            foreach (string path in listBox_MASS_DUAL_ISO.Items)
            {
                rrwwtt.LLL = rrwwtt._ImportRawData14bitUncompressed(@path);
                rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(2, 2));
                rrwwtt._ExportRawDataTiff(rrwwtt.LLL, path.Substring(0, (@path).Length - 4) + ".tiff");
            }
        }

        private void button1_Click(object sender, EventArgs e)//ADOBE
        {
            //rrwwtt.Left= rrwwtt.Reflow(rrwwtt.Left, ((3/1856)+32)+3);
            rrwwtt.LLL.UpdateData(rrwwtt._ImportAdobeConverted(textBox_Import_DNG_Text.Text));
        }

        private void button2_Click(object sender, EventArgs e)//ADOBE
        {
            int radius = 3;
            int sliceX = 2;
            int sliceY = 2;
            //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Prefit2(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0)));
            //rrwwtt.LLL.ModifyBlock(0, 1, rrwwtt._Prefit2(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 1)));
            //rrwwtt.ModifyBlock(rrwwtt.Left, 0, 1, rrwwtt.Prefit2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 1), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 1)));
            //rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Collector2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 0), radius));
            //rrwwtt.ModifyBlock(rrwwtt.Left, 0, 1, rrwwtt.Collector2(rrwwtt.SliceBlock(rrwwtt.Left, sliceX, sliceY, 0, 1), rrwwtt.SliceBlock(rrwwtt.Right, sliceX, sliceY, 0, 1), radius));
            Console.WriteLine();
        }

        private void button7_Click(object sender, EventArgs e)//ADOBE
        {
            Console.WriteLine();
            rrwwtt._ExportAdobeConverted(rrwwtt.LLL, textBox_Import_DNG_Text.Text);
        }

        private void button8_Click(object sender, EventArgs e)//SINGLE XIAOMI
        {
            int radius = 4;
            int rounds = 8;

            int sliceX = 2;
            int sliceY = 2;
            Console.WriteLine();
            //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0)));

            for (int i = 0; i < rounds; i++)
            {
                rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0), radius));
            }
        }

        private void button_DEINT_MAN_Click(object sender, EventArgs e)
        {
            rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal((int)numericUpDown1.Value, (int)numericUpDown2.Value));
        }

        private void Repair(DngFileType dngFileType, bool isDualIso)
        {
            int radius = 5;
            int rounds = 3;

            int sliceX;
            int sliceY;
            switch (dngFileType)
            {
                case DngFileType.Xiaomi16bit:
                    sliceX = 2;
                    sliceY = 2;
                    //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Prefit(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0)));
                    for (int i = 0; i < rounds; i++)
                    {
                        //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0), radius));
                        //rrwwtt.LLL.ModifyBlock(0, 1, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 1), radius));
                        rrwwtt.LLL.ModifyBlock(1, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 0), radius));
                        //rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 1), radius));
                    }
                    break;
                case DngFileType.MLVApp14bit:
                    if (isDualIso)
                    {
                        sliceX = 2;
                        sliceY = 4;
                        for (int i = 0; i < rounds; i++)
                        {
                            rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0), radius));
                            rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 1), radius));
                            rrwwtt.LLL.ModifyBlock(0, 2, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 2), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 2), radius));
                            rrwwtt.LLL.ModifyBlock(1, 3, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 3), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 3), radius));
                        }
                    }
                    else
                    {
                        sliceX = 2;
                        sliceY = 2;
                        for (int i = 0; i < rounds; i++)
                        {
                            //rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0), radius));
                            //rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 1), radius));
                            rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector2(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 0), radius));
                            rrwwtt.LLL.ModifyBlock(0, 1, rrwwtt._Collector2(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 0, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 0, 1), radius));
                            rrwwtt.LLL.ModifyBlock(1, 1, rrwwtt._Collector2(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 1), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 1), radius));
                            rrwwtt.LLL.ModifyBlock(1, 0, rrwwtt._Collector2(rrwwtt.LLL.SliceBlock(sliceX, sliceY, 1, 0), rrwwtt.RRR.SliceBlock(sliceX, sliceY, 1, 0), radius));
                        }
                    }
                    break;
                case DngFileType.AdobeExported16bit:
                    break;
                default:
                    break;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Repair(DngFileType.MLVApp14bit, false);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int lll = textBox_Import_DNG_Text.Text.Length;
            string orig = textBox_Import_DNG_Text.Text.Substring(0, lll - 4);
            for (int i = 2; i < rrwwtt.LLL.Width() / 2; i++)
            {
                rrwwtt.LLL = rrwwtt._ImportRawData14bitUncompressed(textBox_Import_DNG_Text.Text);
                rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(i, 2));
                string sss = orig + "__" + i + ".tiff";
                rrwwtt._ExportRawDataTiff(rrwwtt.LLL, @sss);
            }
        }

        private void button_XIAOMI16rep_Click(object sender, EventArgs e)
        {
            Repair(DngFileType.Xiaomi16bit, false);
        }

        private void button_XIAOMI16rep_filelist_Click(object sender, EventArgs e)
        {
            //string basedir = "E:\\SHARE\\OpenCamera\\stack5";
            string basedir = "D:\\IMAGES";
            string[] filelist = File.ReadAllLines(basedir + "\\filelist.txt");
            rrwwtt.RRR = rrwwtt._OpenAsPixelmap(new Bitmap("E:\\SHARE\\OpenCamera\\xiaomi4X.tiff"));

            for (int i = 0; i < filelist.Length; i++)
            {
                rrwwtt.LLL = rrwwtt._ImportRawDataXiaomi(basedir+"\\"+filelist[i]);
                rrwwtt.LLL.UpdateData(rrwwtt.LLL.DeinterlaceUniversal(2, 2));

                for (int j = 0; j < 2; j++)
                {
                    rrwwtt.LLL.ModifyBlock(0, 0, rrwwtt._Collector(rrwwtt.LLL.SliceBlock(2, 2, 0, 0), rrwwtt.RRR.SliceBlock(2, 2, 0, 0), 5));
                }
                rrwwtt.LLL.UpdateData(rrwwtt.LLL.InterlaceUniversal(2, 2));
                rrwwtt._ExportRawDataXiaomi(rrwwtt.LLL.Data(), basedir + "\\" + filelist[i]);
            }
        }*/
    }
}