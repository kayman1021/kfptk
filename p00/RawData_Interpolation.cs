﻿using MathNet.Numerics.Interpolation;

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
            double temp = 0;
            for (int i = 0; i < 5; i++)
            {
                temp = average - input[i];
                sum += temp * temp;
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


                    if (i >= 2 && i + 2 <= arrayLength-1)
                    {
                        dataMSE[0] = (int)cs.Interpolate(i - 2);
                        dataMSE[1] = (int)cs.Interpolate(i - 1);
                        dataMSE[2] = (int)cs.Interpolate(i);
                        dataMSE[3] = (int)cs.Interpolate(i +1);
                        dataMSE[4] = (int)cs.Interpolate(i +2);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            dataMSE[0] = (int)cs.Interpolate(i);
                            dataMSE[1] = (int)cs.Interpolate(i+1);
                            dataMSE[2] = (int)cs.Interpolate(i+ 2);
                            dataMSE[3] = (int)cs.Interpolate(i +3);
                            dataMSE[4] = (int)cs.Interpolate(i+4);
                        }
                        if (i == 1)
                        {
                            dataMSE[0] = (int)cs.Interpolate(i - 1);
                            dataMSE[1] = (int)cs.Interpolate(i);
                            dataMSE[2] = (int)cs.Interpolate(i +1);
                            dataMSE[3] = (int)cs.Interpolate(i +2);
                            dataMSE[4] = (int)cs.Interpolate(i +3);
                        }
                        if (i == arrayLength - 1)
                        {
                            dataMSE[0] = (int)cs.Interpolate(i - 4);
                            dataMSE[1] = (int)cs.Interpolate(i - 3);
                            dataMSE[2] = (int)cs.Interpolate(i - 2);
                            dataMSE[3] = (int)cs.Interpolate(i - 1);
                            dataMSE[4] = (int)cs.Interpolate(i);
                        }
                        if (i == arrayLength-2)
                        {
                            dataMSE[0] = (int)cs.Interpolate(i - 3);
                            dataMSE[1] = (int)cs.Interpolate(i - 2);
                            dataMSE[2] = (int)cs.Interpolate(i - 1);
                            dataMSE[3] = (int)cs.Interpolate(i);
                            dataMSE[4] = (int)cs.Interpolate(i+ 1);
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
                    for (int counter = 0; counter < horizontal.GetLength(0); counter++)
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
                    for (int counter = 0; counter < vertical.GetLength(0); counter++)
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
            return raw;
        }

    }
}
