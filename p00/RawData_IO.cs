using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Collections;
using System.IO;
using BitMiracle.LibTiff.Classic;

namespace p00
{
    partial class RawData
    {
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
                for (int i = 0; i < intbuffer.Length; i++) { output[i % width, i / width] = intbuffer[i]; }
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
                output.SetField(TiffTag.COMPRESSION, Compression.LZW);
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
                        temp = (int)(ushort.MinValue);
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
        public int[,] ImportPixelMapFromFPM(string filename, int[,] getResolutionOfThis)
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

            int[,] output = new int[getResolutionOfThis.GetLength(0), getResolutionOfThis.GetLength(1)];
            for (int i = 0; i < pointArray.Length; i++)
            {
                output[pointArray[i].X, pointArray[i].Y] = ushort.MinValue;
            }

            return output;
        }
        public int[,] ImportRawDataTiff(string filename)
        {
            return null;
        }
        public void ExportFPM(string filename, int[,] data)
        {
            StringBuilder sb = new StringBuilder();
            int temp;
            int height = data.GetLength(1);
            int width = data.GetLength(0);
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    temp = data[xxx, yyy];
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
