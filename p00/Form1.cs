﻿using System;
using System.Windows.Forms;

namespace p00
{
    public partial class Form1 : Form
    {
        public enum dngFileType { Xiaomi16bit, MLVApp14bit };
        public enum mapFileType { Image, FMP };
        RawDataTool rwt = new RawDataTool();

        public Form1() { InitializeComponent(); }

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
            rwt.pixelData = rwt.ImportPixelMapFromFPM(textBox_Import_FPM_Text.Text);
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
            input = rwt.Deinterlace(input, true);

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

            input = rwt.Interlace(input, true);
            input = rwt.DeinterlaceDualISO(input);

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
            rwt.ExportFPM(@textBox_Export_FPM_Text.Text, rwt.rawData);
        }

        /*
        private void button_ImportRawBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "DNG file|*.dng";
                if (open.ShowDialog() == DialogResult.OK) { textBox_ImportRaw.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_ImportRawXiaomi_Click(object sender, EventArgs e) { rwt.ImportRawDataXiaomi(@textBox_ImportRaw.Text); }
        private void button_ImportRaw14bit_Click(object sender, EventArgs e) { rwt.rawData=rwt.ImportRawData14bitUncompressed(@textBox_ImportRaw.Text); }

        private void button_ExportTiffBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "tiff file|*.tiff";
                save.ShowDialog();
                if (save.FileName != "") { textBox_ExportTiff.Text = save.FileName; }
            }
            catch (Exception) { throw new ApplicationException(""); }
        }
        private void button_ExportTiff_Click(object sender, EventArgs e) { rwt.ExportRawDataTiff(@textBox_ExportTiff.Text, rwt.rawData); }
        private void button_ExportIntArrayBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "txt file|*.txt";
                save.ShowDialog();
                if (save.FileName != "") { textBox_ExportIntArray.Text = save.FileName; }
            }
            catch (Exception) { throw new ApplicationException(""); }
        }
        private void button_ExportIntArray_Click(object sender, EventArgs e) { rwt.ExportRawDataArray(@textBox_ExportIntArray.Text, rwt.rawData); }

        private void button_Deinterlace_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.Deinterlace(rwt.rawData, false);
            rwt.rawData = rwt.Deinterlace(rwt.rawData, true);
        }
        private void button_Interlace_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.Interlace(rwt.rawData, false);
            rwt.rawData = rwt.Interlace(rwt.rawData, true);
        }

        private void button_SeperateDualISO_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.SeperateDualISO(rwt.rawData);
        }
        private void button_AlternateDualISO_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.AlternateDualISO(rwt.rawData);
        }
        private void button_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            int[,] top = rwt.SplitTop(rwt.rawData);
            top = rwt.Deinterlace(top, false);
            top = rwt.Deinterlace(top, true);
            int[,] bottom = rwt.SplitBottom(rwt.rawData);
            bottom = rwt.Deinterlace(bottom, false);
            bottom = rwt.Deinterlace(bottom, true);

            rwt.rawData = rwt.JoinHalves(top, bottom);

        }
        private void button_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            int[,] top = rwt.SplitTop(rwt.rawData);
            top = rwt.Interlace(top, false);
            top = rwt.Interlace(top, true);
            int[,] bottom = rwt.SplitBottom(rwt.rawData);
            bottom = rwt.Interlace(bottom, false);
            bottom = rwt.Interlace(bottom, true);

            rwt.rawData = rwt.JoinHalves(top, bottom);
        }


        private void button_Transpose_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.TransposeArray(rwt.rawData);
        }
        private void button_KeepTopHalf_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.SplitTop(rwt.rawData);
        }
        private void button_KeepBottomHalf_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.SplitBottom(rwt.rawData);
        }

        private void button_ImportFPMFromImage_Click(object sender, EventArgs e)
        {
            rwt.ImportPixelMapFromPicture(textBox_ImportFPMFromImage.Text);
        }

        private void button_BrowseFPMImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif; *.tiff;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif; *.tiff";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    textBox_ImportFPMFromImage.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading file");
            }
        }

        */
    }

    /*    class RawDataTool
        {

            public int[,] rawData;
            public int[,] pixelData;
            public int[,] ImportRawDataXiaomi(string filename)
            {
                int bitsPerSample = 16;
                int width, height;
                int[,] output;
                using (Tiff input = Tiff.Open(@filename, "r"))
                {
                    width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                    height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                    output = new int[width, height];
                }
                using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
                {
                    int filelength = (int)(new System.IO.FileInfo(@filename).Length);
                    int datalength = (int)(width * height * bitsPerSample / 8);
                    int start_addr = filelength - datalength;
                    int pixelcount = width * height;
                    reader.ReadBytes(start_addr);
                    Byte[] bytebuffer = reader.ReadBytes(datalength);
                    for (int i = 0; i < pixelcount; i++) { output[i % width, i / width] = (int)(bytebuffer[2 * i]) + (int)(bytebuffer[2 * i + 1] * 256); }
                }
                Console.WriteLine();
                return output;
            }
            public int[,] ImportRawData14bitUncompressed(string filename)
            {
                int width, height;
                int[,] output;
                using (Tiff input = Tiff.Open(@filename, "r"))
                {
                    width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                    height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                    output = new int[width, height]; 
                }
                int bitsPerSample = 14;
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    int filelength = (int)(new System.IO.FileInfo(filename).Length);
                    int datalength = (int)(width * height * bitsPerSample / 8f);
                    int start_addr = filelength - datalength;
                    int pixelcount = width * height;
                    reader.ReadBytes(start_addr);
                    byte[] bytebuffer = new byte[datalength];
                    bytebuffer = reader.ReadBytes(datalength);
                    BitArray bitbuffer = new BitArray(bytebuffer);
                    int[] intbuffer = new int[pixelcount];
                    bool ttt;
                    for (int i = 0; i < bitbuffer.Length; i += 8)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            ttt = bitbuffer[i + j];
                            bitbuffer[i + j] = bitbuffer[i + (8 - j - 1)];
                            bitbuffer[i + (8 - j - 1)] = ttt;
                        }
                    }
                    bool temp;
                    int sum;
                    for (int bbb = 0; bbb < bitbuffer.Length; bbb += bitsPerSample)
                    {
                        sum = 0;
                        for (int aaa = 0; aaa < bitsPerSample; aaa++)
                        {
                            temp = bitbuffer[aaa + bbb];
                            if (temp) { sum += (int)(Math.Pow((double)(2), (double)(bitsPerSample - 1 - aaa))); }
                        }
                        intbuffer[bbb / bitsPerSample] = sum;
                    }
                    Console.WriteLine();
                    for (int i = 0; i < intbuffer.Length; i++) { output[i % width, i / height] = intbuffer[i]; }
                    return output;
                }
            }
            public void ExportRawDataTiff(string filename, int[,] input)
            {
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                using (Tiff output = Tiff.Open(@filename, "w"))
                {
                    output.SetField(TiffTag.IMAGEWIDTH, width);
                    output.SetField(TiffTag.IMAGELENGTH, height);
                    output.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                    output.SetField(TiffTag.BITSPERSAMPLE, 16);
                    output.SetField(TiffTag.ORIENTATION, BitMiracle.LibTiff.Classic.Orientation.TOPLEFT);
                    output.SetField(TiffTag.XRESOLUTION, 72);
                    output.SetField(TiffTag.YRESOLUTION, 72);
                    output.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                    output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                    output.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                    output.SetField(TiffTag.COMPRESSION, Compression.NONE);
                    output.SetField(TiffTag.ROWSPERSTRIP, height);
                    output.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
                    byte[] bytebuffer = new byte[width * 16 / 8];
                    int rawValue;
                    for (int yyy = 0; yyy < height; yyy++)
                    {
                        for (int xxx = 0; xxx < width; xxx++)
                        {
                            rawValue = input[xxx, yyy];
                            bytebuffer[xxx * 2] = (byte)(rawValue % 256);
                            bytebuffer[xxx * 2 + 1] = (byte)(rawValue / 256);
                        }
                        output.WriteScanline(bytebuffer, yyy);
                    }
                }
            }
            public void ExportRawDataArray(string filename, int[,] input)
            {
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                StringBuilder sb = new StringBuilder();
                string spacer = " ";
                string newline = "\r\n";
                for (int yyy = 0; yyy < height; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++) { sb.Append(input[xxx, yyy] + spacer); }
                    sb.Append(newline);
                }
                File.WriteAllText(@filename, sb.ToString());
            }

            public int[,] Deinterlace(int[,] input, bool direction)
            {
                int[,] output;
                int width;
                int height;
                int halfres;
                if (direction) { input = TransposeArray(input); }
                width = input.GetLength(0);
                height = input.GetLength(1);
                output = new int[width, height];
                halfres = height / 2;

                int temp;
                for (int yyy = 0; yyy < height; yyy++)
                {
                    temp = yyy / 2;
                    if (yyy % 2 == 1) { temp += halfres; }
                    for (int xxx = 0; xxx < width; xxx++) { output[xxx, temp] = input[xxx, yyy]; }
                }

                if (direction) { output = TransposeArray(output); }

                return output;
            }

            public int[,] Interlace(int[,] input, bool direction)
            {
                int[,] output;
                int width;
                int height;
                int halfres;
                if (direction) { input = TransposeArray(input); }
                width = input.GetLength(0);
                height = input.GetLength(1);
                output = new int[width, height];
                halfres = height / 2;

                int temp;
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (yyy < halfres) { temp = yyy * 2; }
                    else { temp = (yyy - halfres) * 2 + 1; }
                    for (int xxx = 0; xxx < width; xxx++) { output[xxx, temp] = input[xxx, yyy]; }
                }

                if (direction) { output = TransposeArray(output); }

                return output;
            }

            public int[,] TransposeArray(int[,] input)
            {
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                int[,] output = new int[height, width];
                for (int yyy = 0; yyy < height; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        output[yyy, xxx] = input[xxx, yyy];
                    }
                }
                return output;
            }

            public int[,] SeperateDualISO(int[,] input)
            {
                int[,] output;
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                output = new int[width, height];
                int halfres = height / 2;
                int temp;
                for (int yyy = 0; yyy < height; yyy++)
                {
                    temp = (yyy / 2) + (yyy % 2);
                    if (yyy % 4 >= 2)
                    {
                        temp += halfres - 1;
                    }
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        output[xxx, temp] = input[xxx, yyy];
                    }
                }
                return output;
            }

            public int[,] AlternateDualISO(int[,] input)
            {
                int[,] output;
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                output = new int[width, height];
                int halfres = height / 2;
                int temp;
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (yyy / halfres == 0)
                    {
                        temp = (yyy * 2) - (yyy % 2);
                    }
                    else
                    {
                        temp = ((yyy - halfres) * 2) - (yyy % 2) + 2;
                    }
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        output[xxx, temp] = input[xxx, yyy];
                    }
                }
                return output;
            }



            public int[,] SplitTop(int[,] input)
            {
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                int halfres = height / 2;
                int[,] output = new int[width, halfres];

                for (int yyy = 0; yyy < halfres; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        output[xxx, yyy] = input[xxx, yyy];
                    }
                }
                return output;
            }

            public int[,] SplitBottom(int[,] input)
            {
                int width = input.GetLength(0);
                int height = input.GetLength(1);
                int halfres = height / 2;
                int[,] output = new int[width, halfres];

                for (int yyy = halfres; yyy < height; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        output[xxx, yyy - halfres] = input[xxx, yyy];
                    }
                }
                return output;
            }

            public int[,] JoinHalves(int[,] top, int[,] bottom)
            {
                int width = top.GetLength(0);
                int height = top.GetLength(1) * 2;
                int halfres = top.GetLength(1);
                int[,] output = new int[width, height];

                for (int yyy = 0; yyy < halfres; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        output[xxx, yyy] = top[xxx, yyy];
                    }
                }
                for (int yyy = 0; yyy < halfres; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        int temp = yyy + halfres;
                        output[xxx, temp] = bottom[xxx, yyy];
                    }
                }


                return output;
            }

            public int[,] BitmapToIntArray(Bitmap input)
            {
                int[,] output = new int[input.Width, input.Height];
                Color color;
                int temp;
                for (int yyy = 0; yyy < input.Height; yyy++)
                {
                    for (int xxx = 0; xxx < input.Width; xxx++)
                    {
                        color = input.GetPixel(xxx, yyy);
                        temp = color.R * color.G * color.B;
                        if (temp > 0)
                        {
                            temp = (int)(ushort.MaxValue);
                        }
                        else
                        {
                            temp = 0;
                        }
                        output[xxx, yyy] = temp;
                    }
                }
                Console.WriteLine();



                return output;
            }

            public int[,] ImportPixelMapFromPicture(string filename)
            {
                int[,] output = BitmapToIntArray(new Bitmap(@filename));
                return output;
            }

            public void SwapSides(int[,]input1,int[,]input2)
            {
                int[,] temp;
                temp = input1;
                input1 = input2;
                input2 = temp;
            }

            public int[,] ImportPixelMapFromFPM(string filename)
            {
                return null;
            }

            public int[,] ImportRawDataTiff(string filename)
            {
                return null;
            }
        }*/
}