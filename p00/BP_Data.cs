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
        //public Matrix<double> Left;
        //public Matrix<double> Right;
        public double minimum, maximum;

        public oMatrix LLL;
        public oMatrix RRR;

        public BP_Data()
        {
            LLL = new oMatrix(new ushort[0,0]);
            RRR = new oMatrix(new ushort[0, 0]);
        }

        public oMatrix _ImportRawDataXiaomi(string filename)
        {
            const int bitsPerSample = 16;
            const int bitsPerByte = 8;
            int width, height;
            ushort[,] output;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();

            }
            output = new ushort[width, height];
            using (BinaryReader reader = new BinaryReader(File.Open(@filename, FileMode.Open)))
            {
                int filelength = (int)new System.IO.FileInfo(@filename).Length;
                int datalength = (width * height * bitsPerSample / bitsPerByte);
                int start_addr = filelength - datalength;
                int pixelcount = width * height;
                reader.ReadBytes(start_addr);
                byte[] bytebuffer = reader.ReadBytes(datalength);
                for (int i = 0; i < pixelcount; i++) { output[i % width, i / width] = (ushort)(bytebuffer[i << 1] + ((bytebuffer[(i << 1) + 1]) << 8)); }
            }
            return new oMatrix(output);
        }

        public void _ExportRawDataXiaomi(oMatrix input, string filename)
        {
            int bitsPerSample = 16;
            int width = input.Width();
            int height = input.Height();


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

            int counter = 0;
            for (int i = intdump.Length - pixelcount; i < intdump.Length; i++)
            {
                intdump[i] = input.GetValue(counter % width, counter / width);
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


                bytedump[i << 1] = _BoolArrayToByte(boolA);
                bytedump[(i << 1) + 1] = _BoolArrayToByte(boolB);
            }

            _byteArrayWriter(bytedump, filename, "_corrected");
        }//////////////////////////////////////////////////////////////////////////////////////////////////////////gázos

        public void _byteArrayWriter(byte[] input, string filename, string additionToName)
        {
            string outputFilename = filename.Substring(0, filename.Length - 4) + additionToName + filename.Substring(filename.Length - 4,4);
            using (BinaryWriter writer = new BinaryWriter(File.Open((@outputFilename), FileMode.Create))) { writer.Write(input); }
        }

        public byte _BoolArrayToByte(bool[] input)
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
        }///////////////////////////////////////////////////////////////////////////////meg kellene szuntetni

        public ushort[] _ArrayConvert2Dto1D(oMatrix input)
        {
            int width = input.Width();
            int height = input.Height();
            int arraylength = width * height;
            ushort[] output = new ushort[arraylength];
            for (int i = 0; i < arraylength; i++)
            {
                output[i] = input.GetValue(i % width, i / width);
            }
            return output;
        }///////////////////////////////////////////////////////////////////////////////meg kellene szuntetni

        public bool[] _IntegerTo14Bit(int input)
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
        }///////////////////////////////////////////////////////////////////////////////meg kellene szuntetni

        public void _ExportRawData14bitUncompressed(oMatrix data, string filename)
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

                ushort[] intArray = _ArrayConvert2Dto1D(data);
                BitArray bitbuffer = new BitArray(bytebuffer_filecopy);
                bool[] intDataAsBoolArray = new bool[pixelcount * 14];
                for (int i = 0; i < pixelcount; i++)
                {
                    Array.Copy(_IntegerTo14Bit(intArray[i]), 0, intDataAsBoolArray, i * 14, 14);
                }

                bool[] tmp = new bool[8];
                for (int i = 0; i < datalength; i++)
                {
                    Array.Copy(intDataAsBoolArray, i << 3, tmp, 0, 8);
                    bytebuffer_data[i] = _BoolArrayToByte(tmp);
                }

                Array.Copy(bytebuffer_data, 0, bytebuffer_filecopy, start_addr, datalength);
                _byteArrayWriter(bytebuffer_filecopy, filename, "_corrected");
            }
        }

        public BitArray _Reverse_Bitarray(BitArray input)
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

        /*public void _ImportRawData14bitUncompressed2(string filename)
        {
            int width, height;
            ushort[,] output;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                output = new ushort[width, height];
            }
            int bitsPerSample = 14;
            int filelength = (int)(new System.IO.FileInfo(filename).Length);
            int datalength = (int)(width * height * bitsPerSample / 8f);
            int start_addr = filelength - datalength;
            Console.WriteLine();
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                Console.WriteLine();
                int[] bbb = new int[filelength];
                for (int i = 0; i < filelength; i++)
                {
                    if (i >= start_addr)
                    {
                        Console.WriteLine();
                        bbb[i - start_addr] = fs.ReadByte();
                    }
                }
                Console.WriteLine();
            }

        }*/

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

        public oMatrix _ImportRawData14bitUncompressed(string filename)///////////////////////////////////////////////////////////////////////////////////////////vissza kell allitani regebbrol
        {
            int width, height, bitsPerSample;
            ushort[,] output;
            using (Tiff input = Tiff.Open(@filename, "r"))
            {
                width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                //height = 3465;
                //width = 5202;
                //bitsPerSample = 16;
                bitsPerSample = input.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
                //output  = Matrix<ushort>.Build.Diagonal( width, height);
                output = new ushort[width, height];
            }
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                Console.WriteLine();
                int filelength = (int)(new System.IO.FileInfo(filename).Length);
                int datalength = (int)(width * height * bitsPerSample / 8f);
                int start_addr = filelength-datalength;
                int pixelcount = width * height;
                reader.ReadBytes(start_addr);
                byte[] bytebuffer = new byte[datalength];
                bytebuffer = reader.ReadBytes(datalength);
                BitArray bitbuffer = new BitArray(bytebuffer);
                int[] intbuffer = new int[pixelcount];

                Console.WriteLine();
                bitbuffer = _Reverse_Bitarray(bitbuffer);

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
                for (int i = 0; i < intbuffer.Length; i++) { output[i % width, i / width] = (ushort)intbuffer[i]; }
                //Console.WriteLine();
                return new oMatrix(output);
            }
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
        }///////////////////////////////opt

        public void _ExportRawDataArray(oMatrix input, string filename)
        {
            int width = input.Width();
            int height = input.Height();
            StringBuilder sb = new StringBuilder();
            string spacer = " ";
            string newline = "\r\n";
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++) { sb.Append(input.GetValue(xxx, yyy) + spacer); }
                sb.Append(newline);
            }
            File.WriteAllText(@filename, sb.ToString());
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
            //Console.WriteLine();



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
                    if (xxx == 73 && yyy == 469)
                    {
                        Console.WriteLine();
                    }
                    List<InterpolatedUnit> results = new List<InterpolatedUnit>();
                    if (map[xxx, yyy] == 0)
                    {
                        
                        if (yyy - radius >= 0 && yyy + radius < height && xxx - radius >= 0 && xxx + radius < width)
                        {
                            int size = (radius * 2) + 1;

                            //oMatrix subData = data.Submatrix(xxx - radius, yyy - radius, size, size);

                            oMatrix subData = new oMatrix(data);
                            subData = subData.Submatrix(xxx - radius, yyy - radius, size, size);

                            double[] tempData = new double[size];
                            double[] places = new double[tempData.Length];
                            for (int i = 0; i < places.Length; i++)
                            {
                                places[i] = i;
                            }


                            tempData = subData.ReturnRowAsDoubleArray(radius);
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }

                            tempData = subData.ReturnColumnAsDoubleArray(radius);
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }

                            tempData = subData.ReturnDiagonalAsDoubleArray1();
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(_Fitter(tempData, places, i));
                            }

                            tempData = subData.ReturnDiagonalAsDoubleArray2();
                            for (int i = 1; i <= 2; i++)
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

        /*public Matrix<double> _FlipMatrixHorizontally(Matrix<double> data)
        {
            Matrix<double> output = Matrix<double>.Build.Dense(data.RowCount, data.ColumnCount);
            int counter = data.ColumnCount - 1;
            for (int i = 0; i < data.ColumnCount; i++)
            {
                output.SetColumn(0, data.Row(counter));
                counter--;
            }
            return output;
        }*/

        public InterpolatedUnit _Fitter(double[] values, double[] places, int order)
        {
            double[] fit = Fit.Polynomial(places, values, order);
            double[] fittedValues = new double[places.Length];
            for (int i = 0; i < fittedValues.Length; i++)
            {
                fittedValues[i] = Polynomial.Evaluate(i, fit);
            }
            double error = _ErrorOfFit(values, fittedValues);
            return new InterpolatedUnit { value = (int)Polynomial.Evaluate(values.Length / 2, fit), goodnessOfFit = error };
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
            if (measured.Length == fitted.Length)
            {
                for (int i = 1; i < measured.Length - 1; i++)
                {
                    sum += (measured[i] - fitted[i]) * (measured[i] - fitted[i]);
                }
            }
            else
            {
                //Console.WriteLine();
            }
            return Math.Sqrt(sum) / (measured.Length - 2);
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
                            subData=subData.Submatrix(yyy - radius, xxx - radius, size, size);

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
        public ushort[,] Data(){ return data; }

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
                    output[xxx, height - 1] = data[xxx, yyy];
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

        public ushort[,] InterlaceUniversal(bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
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
            }
            return output;
        }

        public int DeinterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((arraylength / sensels) * (input % sensels)) + (input / sensels);
        }

        public ushort[,] DeinterlaceUniversal(bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
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
                    output[xxx,yyy] = data[startPosX+ xxx,startPosY+ yyy];
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
