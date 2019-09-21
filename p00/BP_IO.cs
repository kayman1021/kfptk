using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.IO;
using BitMiracle.LibTiff.Classic;
using MathNet.Numerics.LinearAlgebra;

namespace p00
{
    public partial class BP_Data
    {
        public Matrix<double> ImportRawDataXiaomi(string filename)
        {
            const int bitsPerSample = 16;
            const int bitsPerByte = 8;
            int width, height;
            Matrix<double> output;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                output = Matrix<double>.Build.Dense(height, width);
            }
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                int filelength = (int)new System.IO.FileInfo(@filename).Length;
                int datalength = (width * height * bitsPerSample / bitsPerByte);
                int start_addr = filelength - datalength;
                int pixelcount = width * height;
                reader.ReadBytes(start_addr);
                Byte[] bytebuffer = reader.ReadBytes(datalength);
                for (int i = 0; i < pixelcount; i++) { output[i / width, i % width] = (double)(bytebuffer[i << 1] + ((bytebuffer[(i << 1) + 1]) << 8)); }
            }
            //Console.WriteLine();
            return output;
        }
        public void ExportRawDataXiaomi(Matrix<double> data, string filename)
        {
            int bitsPerSample = 16;
            int width = data.ColumnCount;
            int height = data.RowCount;


            byte[] bytedump;
            int[] intdump;
            int filelength;
            int datalength;
            int start_addr;
            int pixelcount;
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                filelength = (int)(new System.IO.FileInfo(@filename).Length);
                datalength = (int)(width * height * bitsPerSample / 8);
                start_addr = filelength - datalength;
                pixelcount = width * height;
                intdump = new int[filelength / 2];

                bytedump = reader.ReadBytes(filelength);
                for (int i = 0; i < filelength >> 1; i++) { intdump[i] = bytedump[i << 1] + (bytedump[(i << 1) + 1] << 8); }
            }

            //Array.Copy(rawData, 0, intdump, start_addr / 2, pixelcount);

            int counter = 0;
            for (int i = intdump.Length - pixelcount; i < intdump.Length; i++)
            {
                intdump[i] = (int)Left.At(counter % Left.ColumnCount, counter / Left.ColumnCount);
                counter++;
            }

            bool[] boolA = new bool[8];
            bool[] boolB = new bool[8];
            int temp;

            for (int i = 0; i < intdump.Length; i++)
            {
                temp = intdump[i];
                boolB[0] = Convert.ToBoolean(temp >> 15);
                temp = temp % 32768;
                boolB[1] = Convert.ToBoolean(temp >> 14);
                temp = temp % 16384;
                boolB[2] = Convert.ToBoolean(temp >> 13);
                temp = temp % 8192;
                boolB[3] = Convert.ToBoolean(temp >> 12);
                temp = temp % 4096;
                boolB[4] = Convert.ToBoolean(temp >> 11);
                temp = temp % 2048;
                boolB[5] = Convert.ToBoolean(temp >> 10);
                temp = temp % 1024;
                boolB[6] = Convert.ToBoolean(temp >> 9);
                temp = temp % 512;
                boolB[7] = Convert.ToBoolean(temp >> 8);
                temp = temp % 256;

                boolA[0] = Convert.ToBoolean(temp >> 7);
                temp = temp % 128; ;
                boolA[1] = Convert.ToBoolean(temp >> 6);
                temp = temp % 64;
                boolA[2] = Convert.ToBoolean(temp >> 5);
                temp = temp % 32;
                boolA[3] = Convert.ToBoolean(temp >> 4);
                temp = temp % 16;
                boolA[4] = Convert.ToBoolean(temp >> 3);
                temp = temp % 8;
                boolA[5] = Convert.ToBoolean(temp >> 2);
                temp = temp % 4;
                boolA[6] = Convert.ToBoolean(temp >> 1);
                temp = temp % 2;
                boolA[7] = Convert.ToBoolean(temp);
                temp = temp % 1;


                bytedump[i << 1] = BoolArrayToByte(boolA);
                bytedump[(i << 1) + 1] = BoolArrayToByte(boolB);
            }

            byteArrayWriter(bytedump, filename);
        }
        public void byteArrayWriter(byte[] input, string filename)
        {
            string outputFilename = filename.Substring(0, filename.Length - 4) + "_corrected.dng";
            using (BinaryWriter writer = new BinaryWriter(File.Open((@outputFilename), FileMode.Create))) { writer.Write(input); }
        }
        public byte BoolArrayToByte(bool[] input)
        {
            byte outut = 0;
            if (input[0])
            {
                outut += 128;
            }
            if (input[1])
            {
                outut += 64;
            }
            if (input[2])
            {
                outut += 32;
            }
            if (input[3])
            {
                outut += 16;
            }
            if (input[4])
            {
                outut += 8;
            }
            if (input[5])
            {
                outut += 4;
            }
            if (input[6])
            {
                outut += 2;
            }
            if (input[7])
            {
                outut++;
            }
            return outut;
        }
        public int[] ArrayConvert2Dto1D(Matrix<double> input)
        {
            int xxx = input.ColumnCount;
            int yyy = input.RowCount;
            int arraylength = xxx * yyy;
            int[] output = new int[arraylength];
            for (int i = 0; i < arraylength; i++)
            {
                output[i] = (int)input[i / xxx, i % xxx];
            }
            return output;
        }
        public bool[] IntegerTo14Bit(int input)
        {
            bool[] output = new bool[14];
            int divider = 8192;
            int remainder = input;
            for (int i = 0; i < 14; i++)
            {
                output[i] = Convert.ToBoolean(remainder / divider);
                remainder = remainder % divider;
                divider = divider >> 1;
            }
            return output;
        }
        public void ExportRawData14bitUncompressed(Matrix<double> data, string filename)
        {
            int filelength;
            int datalength;
            int start_addr;
            int pixelcount;
            byte[] bytebuffer_filecopy;
            byte[] bytebuffer_data;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                pixelcount = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt() * input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            }
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                filelength = (int)(new System.IO.FileInfo(filename).Length);
                datalength = (int)(pixelcount * 14 / 8f);
                start_addr = filelength - datalength;
                bytebuffer_filecopy = reader.ReadBytes(filelength);
                bytebuffer_data = new byte[datalength];

                int[] intArray = ArrayConvert2Dto1D(data);
                BitArray bitbuffer = new BitArray(bytebuffer_filecopy);
                bool[] intDataAsBoolArray = new bool[pixelcount * 14];
                for (int i = 0; i < pixelcount; i++)
                {
                    Array.Copy(IntegerTo14Bit(intArray[i]), 0, intDataAsBoolArray, i * 14, 14);
                }

                bool[] tmp = new bool[8];
                for (int i = 0; i < datalength; i++)
                {
                    Array.Copy(intDataAsBoolArray, i << 3, tmp, 0, 8);
                    bytebuffer_data[i] = BoolArrayToByte(tmp);
                }

                Array.Copy(bytebuffer_data, 0, bytebuffer_filecopy, start_addr, datalength);
                byteArrayWriter(bytebuffer_filecopy, filename);
            }
        }
        public BitArray Reverse_Bitarray(BitArray input)
        {
            int length = input.Length;
            BitArray output = input;
            bool tmp;
            for (int i = 0; i < length; i = i + 8)
            {
                tmp = output[i];
                output[i] = output[i + 7];
                output[i + 7] = tmp;

                tmp = output[i + 1];
                output[i + 1] = output[i + 6];
                output[i + 6] = tmp;

                tmp = output[i + 2];
                output[i + 2] = output[i + 5];
                output[i + 5] = tmp;

                tmp = output[i + 3];
                output[i + 3] = output[i + 4];
                output[i + 4] = tmp;
            }
            return output;
        }
        public Matrix<double> ImportRawData14bitUncompressed(string filename)
        {
            int width, height;
            Matrix<double> output;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                //output  = Matrix<ushort>.Build.Diagonal( width, height);
                output = CreateMatrix.Dense<double>(height, width);
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


                bitbuffer = Reverse_Bitarray(bitbuffer);

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
                //Console.WriteLine();
                for (int i = 0; i < intbuffer.Length; i++) { output[i / width, i % width] = (double)intbuffer[i]; }
                //Console.WriteLine();
                return output;
            }
        }
        public void ExportRawDataTiff(string filename, Matrix<double> input)
        {
            int width = input.ColumnCount;
            int height = input.RowCount;
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
                output.SetField(TiffTag.COMPRESSION, Compression.LZW);
                output.SetField(TiffTag.ROWSPERSTRIP, height);
                output.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);
                byte[] bytebuffer = new byte[width * 16 / 8];
                double rawValue;
                for (int yyy = 0; yyy < height; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        rawValue = input[yyy, xxx];
                        bytebuffer[xxx << 1] = (byte)(rawValue % 256);
                        bytebuffer[(xxx << 1) + 1] = (byte)(rawValue / 256);
                    }
                    output.WriteScanline(bytebuffer, yyy);
                }
            }
        }
        public void ExportRawDataArray(string filename, Matrix<double> input)
        {
            int width = input.ColumnCount;
            int height = input.RowCount;
            StringBuilder sb = new StringBuilder();
            string spacer = " ";
            string newline = "\r\n";
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++) { sb.Append(input[yyy, xxx] + spacer); }
                sb.Append(newline);
            }
            File.WriteAllText(@filename, sb.ToString());
        }
        public Matrix<double> BitmapToIntArray(Bitmap input)
        {
            Matrix<double> output = Matrix<double>.Build.Dense(input.Height, input.Width);
            Color color;
            double temp;
            for (int yyy = 0; yyy < input.Height; yyy++)
            {
                for (int xxx = 0; xxx < input.Width; xxx++)
                {
                    color = input.GetPixel(xxx, yyy);
                    temp = (double)(color.R * color.G * color.B);
                    if (temp > 0)
                    {
                        temp = (double)(ushort.MaxValue);
                    }
                    else
                    {
                        temp = (double)(ushort.MinValue);
                    }
                    output[yyy, xxx] = temp;
                }
            }
            //Console.WriteLine();



            return output;
        }
        public Matrix<double> ImportPixelMapFromPicture(string filename)
        {
            //Matrix<ushort> output = BitmapToIntArray(new Bitmap(@filename));
            return BitmapToIntArray(new Bitmap(@filename));
        }
        public Matrix<double> ImportPixelMapFromFPM(string filename, Matrix<double> getResolutionOfThis)
        {
            string input = System.IO.File.ReadAllText(@filename);
            string[] stringArray = input.Split(new string[] { "\n" }, StringSplitOptions.None);
            Point[] pointArray = new Point[stringArray.GetLength(0)];
            string[] coordinates;
            int maximumXXX = int.MinValue;
            int maximumYYY = int.MinValue;
            int xxx, yyy;
            for (int i = 0; i < stringArray.GetLength(0); i++)
            {
                coordinates = stringArray[i].Split(new string[] { " \r " }, StringSplitOptions.None);
                xxx = int.Parse(coordinates[0]);
                yyy = int.Parse(coordinates[1]);
                if (xxx > maximumXXX)
                {
                    maximumXXX = xxx;
                }
                if (yyy > maximumYYY)
                {
                    maximumYYY = yyy;
                }
                pointArray[i] = new Point(xxx, yyy);
            }

            Matrix<double> output = Matrix<double>.Build.Dense(getResolutionOfThis.RowCount, getResolutionOfThis.ColumnCount);
            for (int i = 0; i < pointArray.Length; i++)
            {
                output[pointArray[i].Y, pointArray[i].X] = ushort.MinValue;
            }

            return output;
        }
        public Matrix<double> ImportRawDataTiff(string filename)
        {
            return null;
        }
        public void ExportFPM(string filename, Matrix<double> data)
        {
            StringBuilder sb = new StringBuilder();
            int temp;
            int height = data.RowCount;
            int width = data.ColumnCount;
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    temp = (int)data[yyy, xxx];
                    if (temp <= 0)
                    {
                        sb.Append(xxx + " \t " + yyy + "\n");
                    }
                }
            }
            System.IO.File.WriteAllText(@filename, sb.ToString());
        }
    }
}
