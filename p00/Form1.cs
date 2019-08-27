using System;
using System.Windows.Forms;

namespace p00
{
    public partial class Form1 : Form
    {
        
        RawData rwt = new RawData();


        public Form1()
        {
            InitializeComponent();
            comboBox_Import_DNG_Select.Items.Add(dngFileType.MLVApp14bit);
            comboBox_Import_DNG_Select.Items.Add(dngFileType.Xiaomi16bit);
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
            if ((dngFileType)comboBox_Import_DNG_Select.SelectedItem == dngFileType.MLVApp14bit)
            {
                rwt.rawData = rwt.ImportRawData14bitUncompressed(textBox_Import_DNG_Text.Text);
            }
            if ((dngFileType)comboBox_Import_DNG_Select.SelectedItem == dngFileType.Xiaomi16bit)
            {
                rwt.rawData = rwt.ImportRawDataXiaomi(textBox_Import_DNG_Text.Text);
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
            rwt.mapData = rwt.ImportPixelMapFromFPM(textBox_Import_FPM_Text.Text, rwt.rawData);
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
            rwt.rawData = rwt.ImportRawDataTiff(textBox_Import_TIFF_Text.Text);
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
            rwt.mapData = rwt.ImportPixelMapFromPicture(textBox_Import_Mapped_Text.Text);
        }
        private void button_Tools_Deinterlace_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, false);
        }
        private void button_Tools_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, true);
        }
        private void button_Tools_Transpose_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.TransposeArray(rwt.rawData);
        }
        private void button_Tools_Interlace_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.InterlaceUniversal(rwt.rawData, false);
        }
        private void button_Tools_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.InterlaceUniversal(rwt.rawData, true);
        }
        private void button_Tools_SwapSides_Click(object sender, EventArgs e)
        {
            int[,] temp;
            temp = rwt.rawData;
            rwt.rawData = rwt.mapData;
            rwt.mapData = temp;
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
            rwt.ExportRawDataTiff(@textBox_Export_TIFF_Text.Text, rwt.rawData);
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
            rwt.ExportFPM(@textBox_Export_FPM_Text.Text, rwt.mapData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rwt.ModifyBlock(rwt.rawData, 0, 0, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 2, 0, 0));
            rwt.ModifyBlock(rwt.rawData, 1, 1, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 2, 1, 1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rwt.ModifyBlock(rwt.rawData, 0, 0, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 0, 0));
            rwt.ModifyBlock(rwt.rawData, 1, 1, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 1, 1));
            rwt.ModifyBlock(rwt.rawData, 0, 2, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 0, 2));
            rwt.ModifyBlock(rwt.rawData, 1, 3, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 1, 3));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, false);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            rwt.ExportRawDataXiaomi(rwt.rawData,@textBox_Import_DNG_Text.Text);
        }

        private void button_EXP_EOS_Click(object sender, EventArgs e)
        {
            rwt.ExportRawData14bitUncompressed(rwt.rawData, textBox_Import_DNG_Text.Text);
        }
    }   
}