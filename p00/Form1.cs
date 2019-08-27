using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BitMiracle.LibTiff.Classic;
namespace p00
{
    public partial class Form1 : Form
    {
        RawDataTool rwt = new RawDataTool();
        public Form1() { InitializeComponent(); }
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
        private void button_ImportRaw14bit_Click(object sender, EventArgs e) { rwt.ImportRawData14bitUncompressed(@textBox_ImportRaw.Text); }

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
        private void button_ExportTiff_Click(object sender, EventArgs e) { rwt.ExportRawDataTiff(@textBox_ExportTiff.Text); }
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
        private void button_ExportIntArray_Click(object sender, EventArgs e) { rwt.ExportRawDataArray(@textBox_ExportIntArray.Text); }

        private void button_Deinterlace_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.Deinterlace(rwt.rawData, false);
            rwt.rawData = rwt.Deinterlace(rwt.rawData, true);
            rwt.UpdateDimensions();
        }
        private void button_Interlace_Click(object sender, EventArgs e)
        {
            rwt.rawData= rwt.Interlace(rwt.rawData, false);
            rwt.rawData= rwt.Interlace(rwt.rawData, true);
            rwt.UpdateDimensions();
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
            top = rwt.Deinterlace(top,false);
            top = rwt.Deinterlace(top, true);
            int[,] bottom = rwt.SplitBottom(rwt.rawData);
            bottom = rwt.Deinterlace(bottom, false);
            bottom = rwt.Deinterlace(bottom, true);

            rwt.rawData = rwt.JoinHalves(top, bottom);
            rwt.UpdateDimensions();

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
            rwt.UpdateDimensions();
        }


        private void button_Transpose_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.TransposeArray(rwt.rawData);
            rwt.UpdateDimensions();
        }
        private void button_KeepTopHalf_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.SplitTop(rwt.rawData);
            rwt.UpdateDimensions();
        }
        private void button_KeepBottomHalf_Click(object sender, EventArgs e)
        {
            rwt.rawData = rwt.SplitBottom(rwt.rawData);
            rwt.UpdateDimensions();
        }
    }
    class RawDataTool
    {
        public int[,] rawData;
        public int rawWidth;
        public int rawHeight;
        public void ImportRawDataXiaomi(string filename)
        {
            int bitsPerSample = 16;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                rawWidth = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                rawHeight = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                rawData = new int[rawWidth, rawHeight];
            }
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                int filelength = (int)(new System.IO.FileInfo(@filename).Length);
                int datalength = (int)(rawWidth * rawHeight * bitsPerSample / 8);
                int start_addr = filelength - datalength;
                int pixelcount = rawWidth * rawHeight;
                reader.ReadBytes(start_addr);
                Byte[] bytebuffer = reader.ReadBytes(datalength);
                for (int i = 0; i < pixelcount; i++) { rawData[i % rawWidth, i / rawWidth] = (int)(bytebuffer[2 * i]) + (int)(bytebuffer[2 * i + 1] * 256); }
            }
            Console.WriteLine();
        }
        public void ImportRawData14bitUncompressed(string filename)
        {
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                rawWidth = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                rawHeight = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                rawData = new int[rawWidth, rawHeight];
            }
            int bitsPerSample = 14;
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int filelength = (int)(new System.IO.FileInfo(filename).Length);
                int datalength = (int)(rawWidth * rawHeight * bitsPerSample / 8f);
                int start_addr = filelength - datalength;
                int pixelcount = rawWidth * rawHeight;
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
                for (int i = 0; i < intbuffer.Length; i++) { rawData[i % rawWidth, i / rawWidth] = intbuffer[i]; }
            }
        }
        public void ExportRawDataTiff(string filename)
        {
            using (Tiff output = Tiff.Open(@filename, "w"))
            {
                output.SetField(TiffTag.IMAGEWIDTH, rawWidth);
                output.SetField(TiffTag.IMAGELENGTH, rawHeight);
                output.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                output.SetField(TiffTag.BITSPERSAMPLE, 16);
                output.SetField(TiffTag.ORIENTATION, BitMiracle.LibTiff.Classic.Orientation.TOPLEFT);
                output.SetField(TiffTag.XRESOLUTION, 72);
                output.SetField(TiffTag.YRESOLUTION, 72);
                output.SetField(TiffTag.RESOLUTIONUNIT, ResUnit.CENTIMETER);
                output.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                output.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                output.SetField(TiffTag.COMPRESSION, Compression.NONE);
                output.SetField(TiffTag.ROWSPERSTRIP, rawHeight);
                output.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
                byte[] bytebuffer = new byte[rawWidth * 16 / 8];
                int rawValue;
                for (int yyy = 0; yyy < rawHeight; yyy++)
                {
                    for (int xxx = 0; xxx < rawWidth; xxx++)
                    {
                        rawValue = rawData[xxx, yyy];
                        bytebuffer[xxx * 2] = (byte)(rawValue % 256);
                        bytebuffer[xxx * 2 + 1] = (byte)(rawValue / 256);
                    }
                    output.WriteScanline(bytebuffer, yyy);
                }
            }
        }
        public void ExportRawDataArray(string filename)
        {
            StringBuilder sb = new StringBuilder();
            string spacer = " ";
            string newline = "\r\n";
            for (int yyy = 0; yyy < rawHeight; yyy++)
            {
                for (int xxx = 0; xxx < rawWidth; xxx++) { sb.Append(rawData[xxx, yyy] + spacer); }
                sb.Append(newline);
            }
            File.WriteAllText(@filename, sb.ToString());
        }

        public void UpdateDimensions()
        {
            rawWidth = rawData.GetLength(0);
            rawHeight = rawData.GetLength(1);
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
                //temp = yyy / 2 + (yyy % 2 * halfres);
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
                //temp = (yyy - ((yyy / halfres) * halfres)) * 2 + (yyy / halfres);
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
                    temp = (yyy*2) - (yyy % 2);
                }
                else
                {
                    temp = ((yyy - halfres)*2) - (yyy % 2)+2;
                }
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, temp] = input[xxx, yyy];
                }
            }
            return output;
        }



        public int[,] SplitTop(int[,]input)
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

        public int[,]SplitBottom(int[,]input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int halfres = height / 2;
            int[,] output = new int[width, halfres];

            for (int yyy = halfres; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, yyy-halfres] = input[xxx, yyy];
                }
            }
            return output;
        }

        public int[,]JoinHalves(int[,]top,int[,]bottom)
        {
            int width = top.GetLength(0);
            int height = top.GetLength(1)*2;
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

        /*



                public void ImportFocusPixelMap(string filename)
                {
                }
                public RawDataTool()
                {
                    focusPixelPositions = new ArrayList();
                }
                void SwapColumn(int column1, int column2)
                {
                    int[] temp = new int[rawHeight];
                    for (int i = 0; i < rawHeight; i++)
                    {
                        temp[i] = rawData[column1, i];
                        rawData[column1, i] = rawData[column2, i];
                        rawData[column2, i] = temp[i];
                    }
                }
                void SwapRow(int row1, int row2)
                {
                    int[] temp = new int[rawWidth];
                    for (int i = 0; i < rawWidth; i++)
                    {
                        temp[i] = rawData[i, row1];
                        rawData[i, row1] = rawData[i, row2];
                        rawData[i, row2] = temp[i];
                    }
                }
                public void ImportRawDataArray(string filename)
                {
                    string inputdata = File.ReadAllText(@filename);
                }
                */
    }
}