using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Collections;
using System.IO;
using BitMiracle.LibTiff.Classic;
using MathNet.Numerics.Interpolation;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace p00
{
    public partial class BP_Data
    {
        public double minimum, maximum;

        public oMatrix LLL;
        public oMatrix RRR;

        public BP_Data()
        {
            LLL = new oMatrix(new ushort[0, 0]);
            RRR = new oMatrix(new ushort[0, 0]);
        }


        public byte[] byteLoader(string filename, int startAddress, int length)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                reader.ReadBytes(startAddress);
                return reader.ReadBytes(length);
            }
        }

        public oMatrix _ImportRawDataXiaomi(string filename)
        {
            //const int bitsPerSample = 16;
            //const int bitsPerByte = 8;
            int width, height;
            ushort[,] output;
            int pixelcount;
            int datalength;
            int start_addr;
            byte[] bytebuffer;
            int filelength;
            filelength = (int)new System.IO.FileInfo(@filename).Length;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                //width = 4096;
                //height = 3072;
                Console.WriteLine();
            }
            output = new ushort[width, height];
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                pixelcount = width * height;
                //int datalength = (width * height * bitsPerSample / bitsPerByte);
                datalength = (pixelcount) << 1;
                start_addr = filelength - datalength;
                reader.ReadBytes(start_addr);
            }
            bytebuffer = byteLoader(filename, start_addr, datalength);
            for (int i = 0; i < pixelcount; i++)
            {
                output[i % width, i / width] = (ushort)(bytebuffer[i << 1] + ((bytebuffer[(i << 1) + 1]) << 8));
            }
            return new oMatrix(output);
        }

        public void _ExportRawDataXiaomi(ushort[,] input, string filename)
        {

            int width = input.GetLength(0);
            int height = input.GetLength(1);


            byte[] bytedump;
            int filelength;
            int pixelcount = width * height;
            int datalength = pixelcount << 1;
            int start_addr;
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                filelength = (int)(new System.IO.FileInfo(@filename).Length);
                start_addr = filelength - datalength;
                bytedump = reader.ReadBytes(filelength);
                Console.WriteLine();
            }

            for (int i = 0; i < pixelcount; i++)
            {
                bytedump[(i * 2) + start_addr] = (byte)(input[i % width, i / width] % 256);
                bytedump[(i * 2) + 1 + start_addr] = (byte)(input[i % width, i / width] / 256);
            }
            _byteArrayWriter(bytedump, filename, "_corrected");
        }

        public void _byteArrayWriter(byte[] input, string filename, string additionToName)
        {
            string outputFilename = filename.Substring(0, filename.Length - 4) + additionToName + filename.Substring(filename.Length - 4, 4);
            using (BinaryWriter writer = new BinaryWriter(File.Open((@outputFilename), FileMode.Create))) { writer.Write(input); }
        }

        /*public byte _BoolArrayToByte(bool[] input)
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
        }///////////////////////////////////////////////////////////////////////////////meg kellene szuntetni*/

        public ushort[] _ArrayConvert2Dto1D(ushort[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int arraylength = width * height;
            ushort[] output = new ushort[arraylength];
            for (int i = 0; i < arraylength; i++)
            {
                output[i] = input[i % width, i / width];
            }
            return output;
        }

        /*public bool[] _IntegerTo14Bit(int input)
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
        }///////////////////////////////////////////////////////////////////////////////meg kellene szuntetni*/

        public void _ExportRawData14bitUncompressed(ushort[,] input, string filename)/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int pixelcount = width * height; ;
            int filelength = (int)(new System.IO.FileInfo(filename).Length);
            int datalength;
            int start_addr;
            ushort[] values;
            byte[] inputByte;
            byte[] outputByte;

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                datalength = (int)(pixelcount * 14 / 8f);
                start_addr = filelength - datalength;
                inputByte = reader.ReadBytes(filelength);
            }

            outputByte = new byte[datalength];
            values = _ArrayConvert2Dto1D(input);
            int temp;
            for (int i = 0; i < pixelcount; i = i + 4)
            {
                temp = (i / 4) * 7;
                /*outputByte[temp + 0] = (byte)(values[i] / 64);
                outputByte[temp + 1] = (byte)(((values[i] % 64) * 4) + (values[i + 1] / 4096));
                outputByte[temp + 2] = (byte)((values[i + 1] % 4096) / 16);
                outputByte[temp + 3] = (byte)(((values[i + 1] % 16) * 16) + (values[i + 2] / 1024));
                outputByte[temp + 4] = (byte)((values[i + 2] % 1024) / 4);
                outputByte[temp + 5] = (byte)(((values[i + 2] % 4) * 64) + (values[i + 3] / 256));
                outputByte[temp + 6] = (byte)(values[i + 3] % 256);*/
                outputByte[temp + 0] = (byte)(values[i] >> 6);
                outputByte[temp + 1] = (byte)(((byte)((values[i] % 64) << 2)) + (byte)(values[i + 1] >> 12));
                outputByte[temp + 2] = (byte)((values[i + 1] % 4096) >> 4);
                outputByte[temp + 3] = (byte)(((values[i + 1] % 16) * 16) + (values[i + 2] >> 10));
                outputByte[temp + 4] = (byte)((values[i + 2] % 1024) / 4);
                outputByte[temp + 5] = (byte)(((values[i + 2] % 4) * 64) + (values[i + 3] >> 8));
                outputByte[temp + 6] = (byte)(values[i + 3] % 256);
            }
            Array.Copy(outputByte, 0, inputByte, start_addr, datalength);
            _byteArrayWriter(inputByte, filename, "_corrected");
        }

        public oMatrix _OpenAsPixelmap(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            ushort[,] values = new ushort[width, height];
            BitmapData imageData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            byte[] imageBytes = new byte[Math.Abs(imageData.Stride) * height];
            IntPtr scan0 = imageData.Scan0;

            Marshal.Copy(scan0, imageBytes, 0, imageBytes.Length);

            for (int i = 0; i < imageBytes.Length; i += 3)
            {
                if (imageBytes[i] + imageBytes[i + 1] + imageBytes[i + 2] == 0)
                {
                    values[(i / 3) % width, (i / 3) / width] = 0;
                }
                else
                {
                    values[(i / 3) % width, (i / 3) / width] = ushort.MaxValue;
                }
            }
            image.UnlockBits(imageData);
            return new oMatrix(values);
        }

        public ushort[,] _ImportAdobeConverted(string filename)
        {
            int width = 5360;
            int height = 3465;
            int bitsPerSample = 16;
            ushort[,] output = new ushort[width, height];
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int filelength = (int)(new System.IO.FileInfo(filename).Length);
                int datalength = (int)(width * height * bitsPerSample / 8f);
                int start_addr = filelength - datalength;
                reader.ReadBytes(start_addr);
                byte[] bytebuffer = reader.ReadBytes(datalength);
                Console.WriteLine();
                for (int i = 0; i < (bytebuffer.Length / 2); i++)
                {
                    output[(i) % width, (i) / width] = (ushort)(bytebuffer[2 * i] + (bytebuffer[(2 * i) + 1] * 256));
                }
                Console.WriteLine();
            }

            return output;
        }

        public void _ExportAdobeConverted(oMatrix input, string filename)
        {
            int width = input.Width();
            int height = input.Height();
            List<byte> rawdata = new List<byte>();

            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    rawdata.Add(Convert.ToByte((Convert.ToInt32(input.GetValue(xxx, yyy))) % 256));
                    rawdata.Add(Convert.ToByte((Convert.ToInt32(input.GetValue(xxx, yyy))) / 256));
                }
            }

            byte[] output = rawdata.ToArray();

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int filelength = (int)(new System.IO.FileInfo(filename).Length);
                int datalength = rawdata.Count;
                int start_addr = filelength - datalength;
                byte[] bytebuffer = reader.ReadBytes(filelength);
                Console.WriteLine();
                for (int i = start_addr; i < filelength; i++)
                {
                    bytebuffer[i] = output[i - start_addr];
                }
                Console.WriteLine();
                _byteArrayWriter(bytebuffer, filename, "_corrected");
                Console.WriteLine();
            }
        }

        public oMatrix _ImportRawData14bitUncompressed(string filename)
        {
            int width, height, bitsPerSample;
            int filelength;
            int datalength;
            int start_addr;
            int pixelcount;
            ushort[,] output;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                bitsPerSample = input.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
            }
            output = new ushort[width, height];
            pixelcount = width * height;
            datalength = (int)(width * height * bitsPerSample / 8f);
            byte[] inputByte;
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                filelength = (int)(new System.IO.FileInfo(filename).Length);
                start_addr = filelength - datalength;
                reader.ReadBytes(start_addr);
                inputByte = reader.ReadBytes(datalength);
            }

            int temp;
            for (int i = 0; i < datalength; i = i + 7)
            {
                //tempbuffer = reader.ReadBytes(7);
                /*buff.Add((ushort)((tempbuffer[0] * 64) + (tempbuffer[1] >> 2)));
                buff.Add((ushort)(((((byte)(tempbuffer[1] << 6)) >> 6) * 4096) + (tempbuffer[2] * 16) + (tempbuffer[3] >> 4)));
                buff.Add((ushort)(((((byte)(tempbuffer[3] << 4)) >> 4) * 1024) + (tempbuffer[4] * 4) + (tempbuffer[5] >> 6)));
                buff.Add((ushort)(((((byte)(tempbuffer[5] << 2)) >> 2) * 256) + (tempbuffer[6])));*/

                temp = (i / 7) << 2;

                output[(temp + 0) % width, (temp + 0) / width] = (ushort)((inputByte[i + 0] << 6) + (inputByte[i + 1] >> 2));
                output[(temp + 1) % width, (temp + 1) / width] = (ushort)((((byte)(inputByte[i + 1] << 6)) << 6) + (inputByte[i + 2] << 4) + (inputByte[i + 3] >> 4));
                output[(temp + 2) % width, (temp + 2) / width] = (ushort)((((byte)(inputByte[i + 3] << 4)) << 6) + (inputByte[i + 4] << 2) + (inputByte[i + 5] >> 6));
                output[(temp + 3) % width, (temp + 3) / width] = (ushort)((((byte)(inputByte[i + 5] << 2)) << 6) + (inputByte[i + 6]));
            }
            return new oMatrix(output);
        }

        public void _ExportRawDataTiff(oMatrix input, string filename)
        {
            int width = input.Width();
            int height = input.Height();
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
                        rawValue = input.GetValue(xxx, yyy);
                        bytebuffer[xxx << 1] = (byte)(rawValue % 256);
                        bytebuffer[(xxx << 1) + 1] = (byte)(rawValue / 256);
                    }
                    output.WriteScanline(bytebuffer, yyy);
                }
            }
        }

        public oMatrix _BitmapToIntArray(Bitmap input)
        {
            int width = input.Width;
            int height = input.Height;
            ushort[,] output = new ushort[width, height];
            Color color;
            ushort temp;
            for (int yyy = 0; yyy < input.Height; yyy++)
            {
                for (int xxx = 0; xxx < input.Width; xxx++)
                {
                    color = input.GetPixel(xxx, yyy);
                    temp = (ushort)(color.R * color.G * color.B);
                    if (temp > 0)
                    {
                        temp = (ushort.MaxValue);
                    }
                    else
                    {
                        temp = (ushort.MinValue);
                    }
                    output[yyy, xxx] = temp;
                }
            }
            return new oMatrix(output);
        }

        public oMatrix _ImportPixelMapFromPicture(string filename)
        {
            return _BitmapToIntArray(new Bitmap(@filename));
        }

        public oMatrix _ImportPixelMapFromFPM(int resolutionX, int resolutionY, string filename)
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

            ushort[,] output = new ushort[resolutionX, resolutionY];
            for (int i = 0; i < pointArray.Length; i++)
            {
                output[pointArray[i].X, pointArray[i].Y] = ushort.MinValue;
            }

            return new oMatrix(output);
        }


        public oMatrix _ImportRawDataTiff(string filename)
        {
            return null;
        }
        public void _ExportFPM(oMatrix input, string filename)
        {
            StringBuilder sb = new StringBuilder();
            int temp;
            int height = input.Height();
            int width = input.Width();
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    temp = input.GetValue(xxx, yyy);
                    if (temp <= 0)
                    {
                        sb.Append(xxx + " \t " + yyy + "\n");
                    }
                }
            }
            System.IO.File.WriteAllText(@filename, sb.ToString());
        }

        public void _SwapSides()
        {
            oMatrix temp = LLL;
            LLL = RRR;
            RRR = temp;
        }

        public ushort[,] _Collector(ushort[,] data, ushort[,] map, int radius)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            ushort[,] output = data;
            List<InterpolatedUnit>[,] allValue = new List<InterpolatedUnit>[width, height];

            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    /*if (xxx == 73 && yyy == 469)
                    {
                        Console.WriteLine();
                    }*/
                    List<InterpolatedUnit> results = new List<InterpolatedUnit>();
                    if (map[xxx, yyy] == 0)
                    {

                        if (yyy - radius >= 0 && yyy + radius < height && xxx - radius >= 0 && xxx + radius < width)
                        {
                            if (xxx == 367 && yyy == 871)
                            {
                                Console.WriteLine();
                            }

                            int size = (radius << 1) + 1;

                            oMatrix subData = new oMatrix(data);
                            subData = subData.Submatrix(xxx - radius, yyy - radius, size, size);
                            Console.WriteLine();
                            double[] tempData = new double[size];
                            double[] places = new double[tempData.Length];
                            for (int i = 0; i < places.Length; i++)
                            {
                                places[i] = i;
                            }

                            int orders = 2;

                            tempData = subData.ReturnRowAsDoubleArray(radius);
                            for (int i = 1; i <= orders; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }

                            tempData = subData.ReturnColumnAsDoubleArray(radius);
                            for (int i = 1; i <= orders; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }

                            tempData = subData.ReturnDiagonalAsDoubleArray1();
                            for (int i = 1; i <= orders; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }

                            tempData = subData.ReturnDiagonalAsDoubleArray2();
                            for (int i = 1; i <= orders; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }
                        }

                        else
                        {
                            //BORDER, ONLY VERTICAL OR HORIZONTAL
                        }
                    }
                    else
                    {
                        //Nothing to do
                    }
                    allValue[xxx, yyy] = results;
                }
            }
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (allValue[xxx, yyy].Count != 0)
                    {
                        InterpolatedUnit bestGoodness = new InterpolatedUnit { goodnessOfFit = ushort.MaxValue };
                        List<InterpolatedUnit> temp3 = allValue[xxx, yyy];
                        for (int i = 0; i < temp3.Count; i++)
                        {
                            if (temp3[i].goodnessOfFit <= bestGoodness.goodnessOfFit)
                            {
                                bestGoodness = temp3[i];
                            }
                        }
                        output[xxx, yyy] = (ushort)bestGoodness.value;
                    }
                }
            }

            return output;
        }

        public InterpolatedUnit _Fitter(double[] values, double[] places, int order)
        {
            int arrayLength = values.Length;
            double[] fit = Fit.Polynomial(places, values, order);
            double[] fittedValues = new double[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                fittedValues[i] = Polynomial.Evaluate(i, fit);
            }
            double error = _ErrorOfFit(values, fittedValues);
            return new InterpolatedUnit { value = (int)Polynomial.Evaluate(arrayLength >> 1, fit), goodnessOfFit = error };
        }

        public ushort[,] _Prefit(ushort[,] data, ushort[,] map)
        {
            int height = data.GetLength(1);
            int width = data.GetLength(0);
            ushort[,] output = data;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == 0)
                    {
                        if (xxx == 0)
                        {
                            if (yyy == 0) { output[xxx, yyy] = (ushort)((data[xxx, yyy + 1] + data[xxx + 1, yyy]) / 2d); }//borderType = BorderType.LowerLeft;   
                            else
                            {
                                if (yyy == height - 1) { output[xxx, yyy] = (ushort)((data[xxx, yyy - 1] + data[xxx + 1, yyy]) / 2d); }//borderType = BorderType.UpperLeft;
                                else { output[xxx, yyy] = (ushort)((data[xxx, yyy + 1] + data[xxx, yyy - 1] + data[xxx + 1, xxx]) / 3d); }//borderType = BorderType.Left;
                            }
                        }
                        else
                        {
                            if (xxx == width - 1)
                            {
                                if (yyy == 0) { output[xxx, yyy] = (ushort)((data[xxx, yyy + 1] + data[xxx - 1, yyy]) / 2d); }//borderType = BorderType.LowerRight;
                                else
                                {
                                    if (yyy == height - 1) { output[xxx, yyy] = (ushort)((data[xxx, yyy - 1] + data[xxx - 1, yyy]) / 2d); }//borderType = BorderType.UpperRight;
                                    else { output[xxx, yyy] = (ushort)((data[xxx, yyy + 1] + data[xxx, yyy - 1] + data[xxx - 1, yyy]) / 3d); }//borderType = BorderType.Right;
                                }
                            }
                            else
                            {
                                if (yyy == 0) { output[xxx, yyy] = (ushort)((data[xxx, yyy + 1] + data[xxx + 1, yyy] + data[xxx - 1, yyy]) / 3d); }//borderType = BorderType.Lower;     
                                else
                                {
                                    if (yyy == height - 1) { output[xxx, yyy] = (ushort)((data[xxx, yyy - 1] + data[xxx + 1, yyy] + data[xxx - 1, yyy]) / 3d); }//borderType = BorderType.Upper;
                                    else { output[xxx, yyy] = (ushort)((data[xxx, yyy + 1] + data[xxx, yyy - 1] + data[xxx + 1, yyy] + data[xxx - 1, yyy]) / 4d); }//borderType = BorderType.None;
                                }
                            }
                        }
                    }
                }
            }
            return output;
        }

        public double _ErrorOfFit(double[] measured, double[] fitted)
        {
            double sum = 0;
            /*for (int i = 1; i < measured.Length - 1; i++)
            {
                sum += (measured[i] - fitted[i]) * (measured[i] - fitted[i]);
            }*/
            for (int i = 0; i < measured.Length; i++)
            {
                sum += (measured[i] - fitted[i]) * (measured[i] - fitted[i]);
            }

            return Math.Sqrt(sum) / (measured.Length);
        }

        public ushort[,] _Prefit2(ushort[,] data, ushort[,] map)
        {
            int height = data.GetLength(1);
            int width = data.GetLength(0);
            ushort[,] output = data;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == 0 && xxx > 0 && xxx < width - 1)
                    {
                        output[xxx, yyy] = (ushort)((data[xxx + 1, yyy] + data[xxx - 1, yyy]) / 2);
                    }
                }
            }
            return output;
        }

        public oMatrix _Collector2(ushort[,] data, ushort[,] map, int radius)
        {
            int height = data.GetLength(1);
            int width = data.GetLength(0);
            ushort[,] output = new ushort[width, height];

            List<InterpolatedUnit>[,] allValue = new List<InterpolatedUnit>[width, height];

            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    List<InterpolatedUnit> results = new List<InterpolatedUnit>();
                    if (map[xxx, yyy] == 0)
                    {
                        if (yyy - radius >= 0 && yyy + radius < height && xxx - radius >= 0 && xxx + radius < width)
                        {
                            int size = (radius * 2) + 1;

                            oMatrix subData = new oMatrix(data);
                            subData = subData.Submatrix(yyy - radius, xxx - radius, size, size);

                            //data.Submatrix(yyy - radius, xxx - radius, size, size);

                            double[] tempData = new double[size];
                            double[] places = new double[tempData.Length];
                            for (int i = 0; i < places.Length; i++)
                            {
                                places[i] = i;
                            }

                            tempData = _MakeCenterAverage((subData.ReturnRowAsDoubleArray(radius)));
                            results.Add(_Fitter(tempData, places, 1));
                            results.Add(_Fitter(tempData, places, 2));
                            tempData = _MakeCenterAverage(tempData);

                            tempData = _MakeCenterAverage((subData.ReturnDiagonalAsDoubleArray1()));
                            results.Add(_Fitter(tempData, places, 1));
                            results.Add(_Fitter(tempData, places, 2));

                            tempData = _MakeCenterAverage((subData.ReturnDiagonalAsDoubleArray2()));
                            results.Add(_Fitter(tempData, places, 1));
                            results.Add(_Fitter(tempData, places, 2));

                        }

                        else
                        {
                            //BORDER, ONLY VERTICAL OR HORIZONTAL
                        }
                    }
                    else
                    {
                        //Nothing to do
                    }
                    allValue[xxx, yyy] = results;
                }
            }
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (allValue[xxx, yyy].Count != 0)
                    {
                        InterpolatedUnit bestGoodness = new InterpolatedUnit { goodnessOfFit = 99999999999999 };
                        List<InterpolatedUnit> temp3 = allValue[xxx, yyy];
                        for (int i = 0; i < temp3.Count; i++)
                        {
                            if (temp3[i].goodnessOfFit <= bestGoodness.goodnessOfFit)
                            {
                                bestGoodness = temp3[i];
                            }
                        }
                        output[xxx, yyy] = (ushort)bestGoodness.value;
                    }
                }
            }

            return new oMatrix(output);
        }


        public double[] _MakeCenterAverage(double[] input)
        {
            int center = input.Length / 2;
            input[center] = (int)((input[center - 1] + input[center + 1]) / 2);
            return input;
        }


























    }

    public class oMatrix
    {
        ushort[,] data;

        int width; public int Width() { return width; }
        int height; public int Height() { return height; }
        public ushort[,] Data() { return data; }

        public oMatrix(int dimX, int dimY, ushort fillingValue)
        {
            if (fillingValue == ushort.MinValue)
            {
                UpdateData(new ushort[dimX, dimY]);
            }
            else
            {
                UpdateData(new ushort[dimX, dimY]);
                for (int xxx = 0; xxx < dimX; xxx++)
                {
                    for (int yyy = 0; yyy < dimY; yyy++)
                    {
                        data[xxx, yyy] = fillingValue;
                    }
                }
            }
        }

        public oMatrix(ushort[,] input)
        {
            UpdateData(input);
        }

        /*public void UpdateDimensions()
        {
            width = data.GetLength(0);
            height = data.GetLength(1);
        }*/

        public void UpdateData(ushort[,] input)
        {
            data = input;
            width = input.GetLength(0);
            height = input.GetLength(1);
        }

        public ushort GetMinimum()
        {
            ushort output = ushort.MaxValue;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    ushort value = data[xxx, yyy];
                    if (value < output)
                    {
                        output = value;
                    }
                }
            }
            return output;
        }

        public ushort GetMaximum()
        {
            ushort output = ushort.MinValue;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    ushort value = data[xxx, yyy];
                    if (value > output)
                    {
                        output = value;
                    }
                }
            }
            return output;
        }

        public ushort GetValue(int positionX, int positionY)
        {
            return data[positionX, positionY];
        }

        public ushort[,] Transpose()
        {
            ushort[,] output = new ushort[height, width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    output[yyy, xxx] = data[xxx, yyy];
                }
            }
            return output;
        }

        public ushort[,] FlipHorizontal()
        {
            ushort[,] output = new ushort[width, height];
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    output[width - 1, yyy] = data[xxx, yyy];
                }
            }
            return output;
        }

        public ushort[,] FlipVertical()
        {
            ushort[,] output = new ushort[width, height];
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    output[xxx, (height - 1) - yyy] = data[xxx, yyy];
                }
            }
            return output;
        }

        public void ModifyBlock(int slicePositionX, int slicePositionY, ushort[,] slice)
        {
            int sliceWidth = slice.GetLength(0);
            int sliceHeight = slice.GetLength(1);
            for (int xxx = 0; xxx < sliceWidth; xxx++)
            {
                for (int yyy = 0; yyy < sliceHeight; yyy++)
                {
                    data[slicePositionX * sliceWidth + xxx, slicePositionY * sliceHeight + yyy] = slice[xxx, yyy];
                }
            }
        }

        public ushort[,] SliceBlock(int sliceCountX, int sliceCountY, int slicePosX, int slicePosY)
        {
            int sliceWidth = width / sliceCountX;
            int sliceHeight = height / sliceCountY;
            ushort[,] output = new ushort[sliceWidth, sliceHeight];
            for (int xxx = 0; xxx < sliceWidth; xxx++)
            {
                for (int yyy = 0; yyy < sliceHeight; yyy++)
                {
                    output[xxx, yyy] = data[(slicePosX * sliceWidth) + xxx, (slicePosY * sliceHeight) + yyy];
                }
            }
            return output;
        }

        public int InterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((input % (arraylength / sensels)) * sensels) + (input / (arraylength / sensels));
        }

        public ushort[,] InterlaceUniversal(int UniqueSenselsX, int UniqueSenselsY)
        {
            /*int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }*/
            int resolutionX = width;
            int resolutionY = height;
            int blockSizeX = width / UniqueSenselsX;
            int blockSizeY = height / UniqueSenselsY;
            ushort[,] output = new ushort[width, height];
            for (int xxx = 0; xxx < width; xxx++)
            {
                int tempX = InterlaceCoordinate(width, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < height; yyy++)
                {
                    output[tempX, InterlaceCoordinate(height, UniqueSenselsY, yyy)] = data[xxx, yyy];
                }
                if (height % UniqueSenselsY != 0)
                {
                    for (int i = 0; i < height % UniqueSenselsY; i++)
                    {
                        output[xxx, ((height / UniqueSenselsY) * UniqueSenselsY) + i] = data[xxx, ((height / UniqueSenselsY) * UniqueSenselsY) + i];
                    }
                }
            }
            return output;
        }

        public int DeinterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((arraylength / sensels) * (input % sensels)) + (input / sensels);
        }

        public ushort[,] DeinterlaceUniversal(int UniqueSenselsX, int UniqueSenselsY)
        {
            /*int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }*/
            int blockSizeX = width / UniqueSenselsX;
            int blockSizeY = height / UniqueSenselsY;
            ushort[,] output = new ushort[width, height];
            for (int xxx = 0; xxx < width; xxx++)
            {
                int tempx = DeinterlaceCoordinate(width, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < height; yyy++)
                {
                    output[tempx, DeinterlaceCoordinate(height, UniqueSenselsY, yyy)] = (ushort)data[xxx, yyy];
                }
                if (height % UniqueSenselsY != 0)
                {
                    for (int i = 0; i < height % UniqueSenselsY; i++)
                    {
                        output[xxx, ((height / UniqueSenselsY) * UniqueSenselsY) + i] = data[xxx, ((height / UniqueSenselsY) * UniqueSenselsY) + i];
                    }
                }
            }
            return output;
        }

        public oMatrix Submatrix(int startPosX, int startPosY, int sizeX, int sizeY)
        {
            ushort[,] output = new ushort[sizeX, sizeY];
            for (int xxx = 0; xxx < sizeX; xxx++)
            {
                for (int yyy = 0; yyy < sizeY; yyy++)
                {
                    output[xxx, yyy] = data[startPosX + xxx, startPosY + yyy];
                }
            }
            return new oMatrix(output);
        }

        public ushort[] ReturnColumnAsUshortArray(int columnNumber)
        {
            ushort[] output = new ushort[height];
            for (int yyy = 0; yyy < height; yyy++)
            {
                output[yyy] = data[columnNumber, yyy];
            }
            return output;
        }

        public ushort[] ReturnRowAsUshortArray(int rowNumber)
        {
            ushort[] output = new ushort[width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                output[xxx] = data[xxx, rowNumber];
            }
            return output;
        }

        public double[] ReturnColumnAsDoubleArray(int columnNumber)
        {
            double[] output = new double[height];
            for (int yyy = 0; yyy < height; yyy++)
            {
                output[yyy] = data[columnNumber, yyy];
            }
            return output;
        }

        public double[] ReturnRowAsDoubleArray(int rowNumber)
        {
            double[] output = new double[width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                output[xxx] = data[xxx, rowNumber];
            }
            return output;
        }

        public ushort[] ReturnDiagonalAsUshortArray1()
        {
            ushort[] output = new ushort[width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                output[xxx] = data[xxx, xxx];
            }
            return output;
        }

        public ushort[] ReturnDiagonalAsUshortArray2()
        {
            ushort[,] temp = FlipVertical();
            ushort[] output = new ushort[width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                output[xxx] = temp[xxx, xxx];
            }
            return output;
        }

        public double[] ReturnDiagonalAsDoubleArray1()
        {
            double[] output = new double[width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                output[xxx] = data[xxx, xxx];
            }
            return output;
        }

        public double[] ReturnDiagonalAsDoubleArray2()
        {
            ushort[,] temp = FlipVertical();
            double[] output = new double[width];
            for (int xxx = 0; xxx < width; xxx++)
            {
                output[xxx] = temp[xxx, xxx];
            }
            return output;
        }
    }
}
