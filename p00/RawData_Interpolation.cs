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
        public bool IsEmptyRow(int[,] input, int rownumber)
        {
            bool temp = true;
            for (int i = 0; i < input.GetLength(0); i++)
            {
                if (input[i, rownumber] == ushort.MaxValue)
                {
                    temp = temp && true;
                }
                else
                {
                    temp = temp && false;
                }
            }
            return temp;
        }

        public bool IsEmptyColumn(int[,] input, int columnnumber)
        {
            bool temp = true;
            for (int i = 0; i < input.GetLength(1); i++)
            {
                if (input[columnnumber,i] == ushort.MaxValue)
                {
                    temp = temp && true;
                }
                else
                {
                    temp = temp && false;
                }
            }
            return temp;
        }

        public double calculateMSE(int[] input)
        {
            double sum = 0;
            for (int i = 0; i < 5; i++)
            {
                sum += input[i];
            }
            double average = sum / 5;
            sum = 0;
            for (int i = 0; i < 5; i++)
            {
                sum += Math.Pow((average - input[i]), 2);
            }



            return sum;
        }


        public struct ouputValues
        {
            public int x;
            public int y;
            public int pixelvalue;
            public double MSEvalue;
        }

        public struct CorrectedValue
        {
            public int pixelvalue;
            public double MSEvalue;
        }

        public ouputValues[] CorrectLine(int[,] data, int[,] map, int line)
        {
            int counter = 0;
            for (int m = 0; m < data.GetLength(0); m++)
            {
                if (map[m, line] == ushort.MaxValue)
                {
                    counter++;
                }
            }

            double[] x_double = new double[counter];
            double[] y_double = new double[counter];
            ouputValues[] values = new ouputValues[counter];
            counter = 0;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (map[i, line] == ushort.MaxValue)
                {
                    x_double[counter] = i;
                    y_double[counter] = data[i, line];
                    counter++;
                }
            }

            CubicSpline cs = CubicSpline.InterpolateNatural(x_double, y_double);
            int arrayLength = data.GetLength(0);
            counter = 0;

            int[] dataMSE;
            for (int i = 0; i < arrayLength; i++)
            {
                dataMSE = new int[5];
                if (map[i, line] == ushort.MinValue)
                {


                    if (i - 2 > 0 && i + 2 <= arrayLength)
                    {
                        dataMSE[0] = data[i - 2, line];
                        dataMSE[1] = data[i - 1, line];
                        dataMSE[2] = data[i, line];
                        dataMSE[3] = data[i + 1, line];
                        dataMSE[4] = data[i + 2, line];
                    }
                    else
                    {
                        if (i == 0)
                        {
                            dataMSE[0] = data[i, line];
                            dataMSE[1] = data[i + 1, line];
                            dataMSE[2] = data[i + 2, line];
                            dataMSE[3] = data[i + 3, line];
                            dataMSE[4] = data[i + 4, line];
                        }
                        if (i == 1)
                        {
                            dataMSE[0] = data[i - 1, line];
                            dataMSE[1] = data[i, line];
                            dataMSE[2] = data[i + 1, line];
                            dataMSE[3] = data[i + 2, line];
                            dataMSE[4] = data[i + 3, line];
                        }
                        if (i == arrayLength - 1)
                        {
                            dataMSE[0] = data[i - 3, line];
                            dataMSE[1] = data[i - 2, line];
                            dataMSE[2] = data[i - 1, line];
                            dataMSE[3] = data[i, line];
                            dataMSE[4] = data[i + 1, line];
                        }
                        if (i == arrayLength)
                        {
                            dataMSE[0] = data[i - 4, line];
                            dataMSE[1] = data[i - 3, line];
                            dataMSE[2] = data[i - 2, line];
                            dataMSE[3] = data[i - 1, line];
                            dataMSE[4] = data[i, line];
                        }
                    }
                    values[counter] = new ouputValues
                    {
                        x = i,
                        y = line,
                        pixelvalue = (int)cs.Interpolate(i),
                        MSEvalue = calculateMSE(dataMSE)
                    };
                    counter++;
                }
            }
            return values;
        }

       // public int[,] CorrectArea(int[,]raw, int )
        public int[,] CorrectArea(int[,]rawData, int [,]mapData, int slicesX, int slicesY,int indexX, int indexY)
        {
            int[,] raw = SliceBlock(rawData, slicesX, slicesY, indexX, indexY);
            int[,] map = SliceBlock(mapData, slicesX, slicesY, indexX, indexY);
            int lengthX = raw.GetLength(0);
            int lengthY = raw.GetLength(1);
            CorrectedValue[,] zone1 = new CorrectedValue[lengthX, lengthY];
            for (int i = 0; i < lengthY; i++)
            {
                for (int j = 0; j < lengthX; j++)
                {
                    zone1[j, i] = new CorrectedValue
                    {
                        pixelvalue = int.MaxValue,
                        MSEvalue = double.MaxValue
                    };
                }
            }
            for (int yyy = 0; yyy < lengthY; yyy++)
            {
                if (!IsEmptyRow(map, yyy))
                {
                    ouputValues[] horizontal = CorrectLine(raw, map, yyy);
                    for (int counter = 0; counter < lengthX; counter++)
                    {

                        ouputValues temp = horizontal[counter];
                        if (zone1[temp.x, temp.y].MSEvalue > temp.MSEvalue)
                        {
                            zone1[temp.x, temp.y] = new CorrectedValue
                            {
                                pixelvalue = temp.pixelvalue,
                                MSEvalue = temp.MSEvalue
                            };
                        }

                    }
                }
            }

            for (int xxx = 0; xxx < lengthX; xxx++)
            {
                if (!IsEmptyColumn(map, xxx))
                {
                    ouputValues[] vertical = CorrectLine(TransposeArray(raw), TransposeArray(map), xxx);
                    for (int counter = 0; counter < lengthX; counter++)
                    {

                        ouputValues temp = vertical[counter];
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

            for (int yyy = 0; yyy < map.GetLength(1); yyy++)
            {
                for (int xxx = 0; xxx < map.GetLength(0); xxx++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        raw[xxx, yyy] = zone1[xxx, yyy].pixelvalue;
                    }
                }
            }
            //rwt.ModifyBlock(rwt.rawData, 0, 0, raw);
            return raw;
        }

    }
}
