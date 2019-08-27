using System;
using System.Windows.Forms;

namespace p00
{
    public partial class Form1 : Form
    {
        public enum dngFileType { Xiaomi16bit, MLVApp14bit };
        public enum mapFileType { Image, FMP };
        RawDataTool rwt = new RawDataTool();


        public Form1()
        {
            InitializeComponent();
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
            rwt.pixelData = rwt.ImportPixelMapFromFPM(textBox_Import_FPM_Text.Text, rwt.rawData);
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
            rwt.pixelData = rwt.ImportPixelMapFromPicture(textBox_Import_Mapped_Text.Text);
        }
        private void button_Tools_Deinterlace_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.pixelData; }
            input = rwt.Deinterlace(input, false);
            input = rwt.Deinterlace(input, true);
            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.pixelData = input; }
        }
        private void button_Tools_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.pixelData; }

            input = rwt.DeinterlaceDualISO(input);
            //input = rwt.Deinterlace(input, false);

            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.pixelData = input; }
        }
        private void button_Tools_Transpose_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.pixelData; }

            input = rwt.TransposeArray(input);

            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.pixelData = input; }
        }
        private void button_Tools_Interlace_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.pixelData; }
            input = rwt.Interlace(input, false);
            input = rwt.Interlace(input, true);
            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.pixelData = input; }
        }
        private void button_Tools_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.pixelData; }

            //input = rwt.Interlace(input, true);
            input = rwt.InterlaceDualISO(input);

            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.pixelData = input; }
        }
        private void button_Tools_SwapSides_Click(object sender, EventArgs e)
        {
            int[,] temp;
            temp = rwt.rawData;
            rwt.rawData = rwt.pixelData;
            rwt.pixelData = temp;
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
            rwt.ExportFPM(@textBox_Export_FPM_Text.Text, rwt.pixelData);
        }

    }   
}