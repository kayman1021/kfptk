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
        public Form1()
        {
            InitializeComponent();

        }

        //GUI INTERACTION 
        private void button_ImportRawXiaomi_Click(object sender, EventArgs e)
        {
            rwt.ImportRawDataXiaomi(@textBox_ImportRaw.Text);
        }
        private void button_ImportRaw14bit_Click(object sender, EventArgs e)
        {
            rwt.ImportRawData14bitUncompressed(@textBox_ImportRaw.Text);
        }
        private void button_ExportTiff_Click(object sender, EventArgs e)
        {
            rwt.ExportRawDataTiff(@textBox_ExportTiff.Text);
        }
        private void button_ExportIntArray_Click(object sender, EventArgs e)
        {
            rwt.ExportRawDataArray(@textBox_ExportIntArray.Text);
        }

        private void button_ImportRawBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "DNG file|*.dng";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    textBox_ImportRaw.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading file");
            }
        }
        private void button_ExportIntArrayBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "txt file|*.txt";
                save.ShowDialog();
                if (save.FileName != "")
                {
                    textBox_ExportIntArray.Text = save.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("");
            }
        }
        private void button_ExportTiffBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "tiff file|*.tiff";
                save.ShowDialog();
                if (save.FileName != "")
                {
                    textBox_ExportTiff.Text = save.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("");
            }
        }

        private void button_DeinterlaceVertical_Click(object sender, EventArgs e)
        {
            rwt.DeinterlaceVertical();
        }
        private void button_DeinterlaceHorizontal_Click(object sender, EventArgs e)
        {
            rwt.DeinterlaceHorizontal();
        }
        private void button_InterlaceVertical_Click(object sender, EventArgs e)
        {
            rwt.InterlaceVertical();
        }
        private void button_InterlaceHorizontal_Click(object sender, EventArgs e)
        {
            rwt.InterlaceHorizontal();
        }
    }



    class RawDataTool
    {
        int[,] rawData;
        int rawWidth;
        int rawHeight;



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
                //int temp;
                for (int i = 0; i < pixelcount; i++)
                {
                    rawData[i % rawWidth, i / rawWidth] = (int)(bytebuffer[2 * i]) + (int)(bytebuffer[2 * i + 1] * 256);
                }
            }
            Console.WriteLine();
        }//WORKS

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
                int[]intbuffer = new int[pixelcount];


                bool ttt;
                for (int i = 0; i < bitbuffer.Length; i+=8)//Reverse byte order, so 14bit can be gained more easily
                {
                    for (int j = 0; j < 4; j++)
                    {
                        ttt = bitbuffer[i + j];
                        bitbuffer[i + j] = bitbuffer[i + (8 - j-1)];
                        bitbuffer[i + (8 - j - 1)] = ttt;
                    }
                }


                bool temp;
                int sum;
                for (int bbb = 0; bbb < bitbuffer.Length; bbb+=bitsPerSample)
                {
                    sum = 0;
                    for (int aaa = 0; aaa < bitsPerSample; aaa++)//Calculate every bit's power, write sum into array
                    {
                        temp = bitbuffer[aaa+bbb];
                        if (temp)
                        {
                            sum += (int)(Math.Pow((double)(2), (double)(bitsPerSample-1- aaa)));
                        }
                    }
                    intbuffer[bbb/bitsPerSample] = sum;
                }
                Console.WriteLine();

                for (int i = 0; i < intbuffer.Length; i++)//Convert 1 dimension array to 2 dimension array
                {
                    rawData[i % rawWidth, i / rawWidth] = intbuffer[i];
                }
            }
        }//BIG MESS

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
        }//WORKS

        public void ExportRawDataArray(string filename)
        {
            StringBuilder sb = new StringBuilder();
            string spacer = " ";
            string newline = "\r\n";
            for (int yyy = 0; yyy < rawHeight; yyy++)
            {
                for (int xxx = 0; xxx < rawWidth; xxx++)
                {
                    sb.Append(rawData[xxx, yyy] + spacer);
                }
                sb.Append(newline);
            }
            File.WriteAllText(@filename, sb.ToString());
        }//MAY WORK

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
        }//WORKS

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
        }//WORKS

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
        }//WORKS

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
        }//WORKS








        //Commented out methods
        /*public void ImportFocusPixelMap(string filename)
        {

        }*///TO DO TO DO

        /*public RawDataTool()
        {
            focusPixelPositions = new ArrayList();
        }*///TO DO TO DO

        /*void SwapColumn(int column1, int column2)
        {
            int[] temp = new int[rawHeight];
            for (int i = 0; i < rawHeight; i++)
            {
                temp[i] = rawData[column1, i];
                rawData[column1, i] = rawData[column2, i];
                rawData[column2, i] = temp[i];
            }
        }*///NEEDS TEST, BUT NOT IN INTEREST

        /*void SwapRow(int row1, int row2)
        {
            int[] temp = new int[rawWidth];
            for (int i = 0; i < rawWidth; i++)
            {
                temp[i] = rawData[i, row1];
                rawData[i, row1] = rawData[i, row2];
                rawData[i, row2] = temp[i];
            }
        }*///NEEDS TEST, BUT NOT IN INTEREST

        /*public void ImportRawDataArray(string filename)
        {
            string inputdata = File.ReadAllText(@filename);
        }*///TO DO TO DO
    }
}
