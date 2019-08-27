using System;
using System.Windows.Forms;

namespace p00
{
    public partial class Form1 : Form
    {
        public enum dngFileType { Xiaomi16bit, MLVApp14bit };
        RawData rwt = new RawData();


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
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.mapData; }
            input = rwt.Deinterlace(input, false);
            input = rwt.Deinterlace(input, true);
            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.mapData = input; }
        }
        private void button_Tools_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.mapData; }

            input = rwt.DeinterlaceDualISO(input);
            //input = rwt.Deinterlace(input, false);

            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.mapData = input; }
        }
        private void button_Tools_Transpose_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.mapData; }

            input = rwt.TransposeArray(input);

            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.mapData = input; }
        }
        private void button_Tools_Interlace_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.mapData; }
            input = rwt.Interlace(input, false);
            input = rwt.Interlace(input, true);
            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.mapData = input; }
        }
        private void button_Tools_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            int[,] input;
            if (radioButton_Select_Left.Checked) { input = rwt.rawData; }
            else { input = rwt.mapData; }

            //input = rwt.Interlace(input, true);
            input = rwt.InterlaceDualISO(input);

            if (radioButton_Select_Left.Checked) { rwt.rawData = input; }
            else { rwt.mapData = input; }
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
            int[,] data;
            int[,] pixel;

            data= rwt.SliceBlock(rwt.rawData,2,2, 0, 0);
            pixel= rwt.SliceBlock(rwt.mapData,2,2, 0, 0);
            RawData.CorrectedValue[,]zone1 = new RawData.CorrectedValue[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    zone1[j, i] = new RawData.CorrectedValue
                    {
                        pixelvalue = int.MaxValue,
                        MSEvalue=double.MaxValue
                    };
                }
            }
            for (int yyy = 0; yyy < data.GetLength(1); yyy++)
            {
                if (!rwt.IsEmptyRow(rwt.mapData,yyy))
                {
                    RawData.ouputValues[] horizontal = rwt.CorrectLine(data, pixel, yyy);
                    for (int counter = 0; counter < horizontal.GetLength(0); counter++)
                    {

                        RawData.ouputValues temp = horizontal[counter];
                        if (zone1[temp.x,temp.y].MSEvalue>temp.MSEvalue)
                        {
                            zone1[temp.x, temp.y] = new RawData.CorrectedValue
                            {
                                pixelvalue = temp.pixelvalue,
                                MSEvalue = temp.MSEvalue
                            };
                        }
                        
                    }
                }
            }

            for (int xxx = 0; xxx < data.GetLength(0); xxx++)
            {
                if (!rwt.IsEmptyColumn(rwt.mapData, xxx))
                {
                    RawData.ouputValues[] vertical = rwt.CorrectLine(rwt.TransposeArray(data),rwt.TransposeArray( pixel), xxx);
                    for (int counter = 0; counter < vertical.GetLength(0); counter++)
                    {

                        RawData.ouputValues temp = vertical[counter];
                        if (zone1[temp.y, temp.x].MSEvalue > temp.MSEvalue)
                        {
                            zone1[temp.y, temp.x] = new RawData.CorrectedValue
                            {
                                pixelvalue = temp.pixelvalue,
                                MSEvalue = temp.MSEvalue
                            };
                        }

                    }
                }
            }

            for (int yyy = 0; yyy < pixel.GetLength(1); yyy++)
            {
                for (int xxx = 0; xxx < pixel.GetLength(0); xxx++)
                {
                    if (pixel[xxx,yyy]==ushort.MinValue)
                    {
                        data[xxx, yyy] = zone1[xxx, yyy].pixelvalue;
                    }
                }
            }
            rwt.ModifyBlock(rwt.rawData,0,0,data);









            data = rwt.SliceBlock(rwt.rawData, 2, 2, 1, 1);
            pixel = rwt.SliceBlock(rwt.mapData, 2, 2, 1, 1);
            RawData.CorrectedValue[,] zone2 = new RawData.CorrectedValue[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    zone2[j, i] = new RawData.CorrectedValue
                    {
                        pixelvalue = int.MaxValue,
                        MSEvalue = double.MaxValue
                    };
                }
            }
            for (int yyy = 0; yyy < data.GetLength(1); yyy++)
            {
                if (!rwt.IsEmptyRow(rwt.mapData, yyy))
                {
                    RawData.ouputValues[] horizontal = rwt.CorrectLine(data, pixel, yyy);
                    for (int counter = 0; counter < horizontal.GetLength(0); counter++)
                    {

                        RawData.ouputValues temp = horizontal[counter];
                        if (zone2[temp.x, temp.y].MSEvalue > temp.MSEvalue)
                        {
                            zone2[temp.x, temp.y] = new RawData.CorrectedValue
                            {
                                pixelvalue = temp.pixelvalue,
                                MSEvalue = temp.MSEvalue
                            };
                        }

                    }
                }
            }

            for (int xxx = 0; xxx < data.GetLength(0); xxx++)
            {
                if (!rwt.IsEmptyColumn(rwt.mapData, xxx))
                {
                    RawData.ouputValues[] vertical = rwt.CorrectLine(rwt.TransposeArray(data), rwt.TransposeArray(pixel), xxx);
                    for (int counter = 0; counter < vertical.GetLength(0); counter++)
                    {

                        RawData.ouputValues temp = vertical[counter];
                        if (zone2[temp.y, temp.x].MSEvalue > temp.MSEvalue)
                        {
                            zone2[temp.y, temp.x] = new RawData.CorrectedValue
                            {
                                pixelvalue = temp.pixelvalue,
                                MSEvalue = temp.MSEvalue
                            };
                        }

                    }
                }
            }

            for (int yyy = 0; yyy < pixel.GetLength(1); yyy++)
            {
                for (int xxx = 0; xxx < pixel.GetLength(0); xxx++)
                {
                    if (pixel[xxx, yyy] == ushort.MinValue)
                    {
                        data[xxx, yyy] = zone2[xxx, yyy].pixelvalue;
                    }
                }
            }
            rwt.ModifyBlock(rwt.rawData, 1, 1, data);


        }
    }   
}