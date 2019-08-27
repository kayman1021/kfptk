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
        private void button_ImportRawXiaomi_Click(object sender, EventArgs e) { rwt.ImportRawDataXiaomi(@textBox_ImportRaw.Text); }
        private void button_ImportRaw14bit_Click(object sender, EventArgs e) { rwt.ImportRawData14bitUncompressed(@textBox_ImportRaw.Text); }
        private void button_ExportTiff_Click(object sender, EventArgs e) { rwt.ExportRawDataTiff(@textBox_ExportTiff.Text); }
        private void button_ExportIntArray_Click(object sender, EventArgs e) { rwt.ExportRawDataArray(@textBox_ExportIntArray.Text); }
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
        private void button_DeinterlaceVertical_Click(object sender, EventArgs e) { rwt.Deinterlace(rwt.rawData, false); }
        private void button_DeinterlaceHorizontal_Click(object sender, EventArgs e) { rwt.Deinterlace(rwt.rawData, true); }
        private void button_InterlaceVertical_Click(object sender, EventArgs e) {/* rwt.InterlaceVertical(); */}
        private void button_InterlaceHorizontal_Click(object sender, EventArgs e) {/* rwt.InterlaceHorizontal(); */}
        private void button_SeperateDualISO_Click(object sender, EventArgs e) {/* rwt.SeperateDualISO(); */}
        private void button_Transpose_Click(object sender, EventArgs e)
        {
            Console.WriteLine();
            rwt.rawData= rwt.TransposeArray(rwt.rawData);
            rwt.rawWidth = rwt.rawData.GetLength(0);
            rwt.rawHeight = rwt.rawData.GetLength(1);
            Console.WriteLine();
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

        public void Deinterlace(int[,] input, bool direction)
        {
            int[,] output;
            int width;
            int height;
            int halfres;
            if (direction)
            {
                input=TransposeArray(input);
            }
            width = input.GetLength(0);
            height = input.GetLength(1);
            output = new int[width,height];
            halfres = height / 2;

            

            int temp;
            for (int yyy = 0; yyy < height; yyy++)
            {
                temp = yyy / 2;
                if (yyy % 2 == 1) { temp += halfres; }
                for (int xxx = 0; xxx < width; xxx++) { output[xxx, temp] = input[xxx, yyy]; }
            }

            if (direction)
            {
                output = TransposeArray(output);
            }


            rawData = output;
            rawWidth = rawData.GetLength(0);
            rawHeight = rawData.GetLength(1);
        }


        public int[,] TransposeArray(int[,]input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int[,] output = new int[height, width];
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[yyy,xxx]= input[xxx,yyy];
                }
            }
            return output;
        }

/*

        public void DeinterlaceHorizontal(int[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int halfres = width / 2;
            int[,] output = new int[width, height];
            int temp;
            for (int xxxxx = 0; xxxxx < width; xxxxx++)
            {
                temp = xxxxx / 2;
                if (xxxxx % 2 == 1)
                {
                    temp += halfres;
                }
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, temp] = rawData[xxx, xxxxx];
                }
            }
            rawData = output;
        }

        public void InterlaceVertical()
        {
            int[,] output = new int[rawWidth, rawHeight];
            int halfres = rawHeight / 2;
            int temp;
            for (int yyy = 0; yyy < rawHeight; yyy++)
            {
                if (yyy < halfres)
                {
                    temp = 2 * yyy;
                }
                else
                {
                    temp = (2 * yyy) - rawHeight + 1;
                }
                for (int xxx = 0; xxx < rawWidth; xxx++)
                {
                    output[xxx, temp] = rawData[xxx, yyy];
                }
            }
            rawData = output;
        }

        public void InterlaceHorizontal()
        {
            int[,] output = new int[rawWidth, rawHeight];
            int halfres = rawWidth / 2;
            int temp;
            for (int xxx = 0; xxx < rawWidth; xxx++)
            {
                if (xxx < halfres)
                {
                    temp = 2 * xxx;
                }
                else
                {
                    temp = (2 * xxx) - rawWidth + 1;
                }
                for (int yyy = 0; yyy < rawHeight; yyy++)
                {
                    output[temp, yyy] = rawData[xxx, yyy];
                }
            }
            rawData = output;
        }
        public void DeinterlaceVertical()
        {
            int[,] output = new int[rawWidth, rawHeight];
            int halfres = rawHeight / 2;
            int temp;
            for (int yyy = 0; yyy < rawHeight; yyy++)
            {
                if (yyy % 2 == 0)
                {
                    temp = yyy / 2;
                }
                else
                {
                    temp = halfres + ((yyy - 1) / 2);
                }
                for (int xxx = 0; xxx < rawWidth; xxx++)
                {
                    output[xxx, temp] = rawData[xxx, yyy];
                }
            }
            rawData = output;
        }
        public void DeinterlaceHorizontal()
        {
            int[,] output = new int[rawWidth, rawHeight];
            int halfres = rawWidth / 2;
            int temp;
            for (int xxx = 0; xxx < rawWidth; xxx++)
            {
                if (xxx % 2 == 0)
                {
                    temp = xxx / 2;
                }
                else
                {
                    temp = halfres + ((xxx - 1) / 2);
                }
                for (int yyy = 0; yyy < rawHeight; yyy++)
                {
                    output[temp, yyy] = rawData[xxx, yyy];
                }
            }
            rawData = output;
        }
        public void SkipEmptyColumns()
        {
            ArrayList emptyColumns = new ArrayList();
            bool isEmptyColumn;
            for (int i = 0; i < rawWidth; i++)
            {
                isEmptyColumn = true;
                for (int j = 0; j < rawHeight; j++)
                {
                    if (rawData[i, j] != 0)
                    {
                        isEmptyColumn = false;
                    }
                }
                if (isEmptyColumn)
                {
                    emptyColumns.Add(i);
                }
            }
            int[,] temp = new int[rawWidth - emptyColumns.Count, rawHeight];
            int counter = 0;
            for (int i = 0; i < rawWidth; i++)
            {
                if (!emptyColumns.Contains(i))
                {
                    for (int j = 0; j < rawHeight; j++)
                    {
                        temp[counter, j] = rawData[i, j];
                    }
                }
            }
            rawWidth = emptyColumns.Count;
            rawData = temp;
        }
        public void SkipEmptyRows()
        {
        }
        public void SeperateDualISO()
        {
            int[,] output = new int[rawWidth, rawHeight];
            int halfres = rawHeight / 2;
            int temp;
            for (int yyy = 0; yyy < rawHeight; yyy++)
            {
                temp = (yyy / 2) + (yyy % 2);
                if (yyy % 4 >= 2)//BOTTOM
                {
                    temp += halfres - 1;
                }
                for (int xxx = 0; xxx < rawWidth; xxx++)
                {
                    output[xxx, temp] = rawData[xxx, yyy];
                }
            }
            rawData = output;
        }


        
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