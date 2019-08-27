using MathNet.Numerics.Interpolation;
using System.Collections;
using System.Numerics;

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
                if (input[columnnumber, i] == ushort.MaxValue)
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

        public double CalculateMSE(int[] input)
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



            return sum/input.Length;
        }

        public double CalculateMSE_double(double[] input)
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



            return sum/input.Length;
        }


        public struct ouputValues
        {
            public int x;
            public int y;
            public int pixelvalue;
            public double MSEvalue;
        }

        public struct pointDouble
        {
            public double x;
            public double y;
        }

        public struct CorrectedValue
        {
            public int pixelvalue;
            public double MSEvalue;
        }


        public ouputValues FindFunction(int[,]data,int[,]map,double degree, int xxx, int yyy)
        {
            int oldvalue = data[xxx, yyy];
            double cos = (float)System.Math.Cos(degree * System.Math.PI / 180);
            double sin = (float)System.Math.Sin(degree * System.Math.PI / 180);

            double function_findY =(float) sin/cos;
            double function_findX = (float)cos/sin;

            ArrayList toInterpolateHorizontal = new ArrayList();
            ArrayList toInterpolateVertical = new ArrayList();
            double valY, valX;
            for (int i = -5; i <= 5; i++)
            {
                valY = i * function_findY;
                valX = i * function_findX;
                if (i != 0)
                {
                    if (valY <= 5 && valY >= -5 && i + xxx >= 0 && valY + yyy < data.GetLength(1))
                    {
                        toInterpolateVertical.Add(new pointDouble() { x = i+xxx, y = valY+yyy });

                    }
                    if (valX <= 5 && valX >= -5&&valX+xxx>=0&&i+yyy<data.GetLength(0))
                    {
                        toInterpolateHorizontal.Add(new pointDouble() { x = valX+xxx, y = i+yyy });

                    }
                }
            }
            System.Console.WriteLine();
            System.Console.WriteLine();
            ArrayList arrayCoordinates=new ArrayList();
            ArrayList arrayValues = new ArrayList();
            pointDouble temp;
            for (int i = 0; i < toInterpolateVertical.Count; i++)
            {
                temp =(pointDouble)toInterpolateVertical[i];
                arrayCoordinates.Add(System.Math.Sqrt((temp.x * temp.x )+ (temp.y* temp.y)));
                arrayValues.Add(makeSplineVertical(data,map,temp.x, temp.y));
            }
            if (!isArrayListEqual(toInterpolateVertical,toInterpolateHorizontal))
            {
                for (int i = 0; i < toInterpolateHorizontal.Count; i++)
                {
                    temp = (pointDouble)toInterpolateHorizontal[i];
                    arrayCoordinates.Add(System.Math.Sqrt((temp.x * temp.x) + (temp.y * temp.y)));
                    arrayValues.Add(makeSplineHorizontal(data, map, temp.x, temp.y));
                }
            }
            



            System.Console.WriteLine();
            CubicSpline notFinal = CubicSpline.InterpolateNaturalInplace(arrayCoordinates.ToArray(typeof(double)) as double[], arrayValues.ToArray(typeof(double)) as double[]);
            double nf = notFinal.Interpolate(System.Math.Sqrt((xxx * xxx) + (yyy * yyy)));
            arrayValues.Add(nf);
            System.Console.WriteLine();
            double MSE=CalculateMSE_double(arrayValues.ToArray(typeof(double)) as double[]);
            System.Console.WriteLine();
            return new ouputValues() {x=xxx,y=yyy,MSEvalue=MSE,pixelvalue=(int)nf };
        }

        public bool isArrayListEqual(ArrayList left, ArrayList right)
        {
            if (left.Count!=right.Count)
            {
                return false;
            }
            else
            {
                pointDouble tempL;
                pointDouble tempR;
                for (int i = 0; i < left.Count; i++)
                {
                    tempL = (pointDouble)left[i];
                    tempR = (pointDouble)right[i];
                    if (tempL.x!=tempR.x||tempL.y!=tempR.y)
                    {
                        return false;
                    }
                }
                return true;
            }
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


                    if (i >= 2 && i + 2 <= arrayLength - 1)
                    {
                        //dataMSE[0] = (int)cs.Interpolate(i - 2);
                        //dataMSE[1] = (int)cs.Interpolate(i - 1);
                        dataMSE[0] = data[i - 2, line];
                        dataMSE[1] = data[i - 1, line];
                        dataMSE[2] = (int)cs.Interpolate(i);
                        dataMSE[3] = data[i + 1, line];
                        dataMSE[4] = data[i + 2, line];
                        //dataMSE[3] = (int)cs.Interpolate(i + 1);
                        //dataMSE[4] = (int)cs.Interpolate(i + 2);
                    }
                    else
                    {
                        if (i == 0)
                        {
                            dataMSE[0] = (int)cs.Interpolate(i);

                            dataMSE[1] = data[i + 1, line];
                            dataMSE[2] = data[i + 2, line];
                            dataMSE[3] = data[i + 3, line];
                            dataMSE[4] = data[i + 4, line];


                            //dataMSE[1] = (int)cs.Interpolate(i + 1);
                            //dataMSE[2] = (int)cs.Interpolate(i + 2);
                            //dataMSE[3] = (int)cs.Interpolate(i + 3);
                            //dataMSE[4] = (int)cs.Interpolate(i + 4);
                        }
                        if (i == 1)
                        {
                            //dataMSE[0] = (int)cs.Interpolate(i - 1);
                            dataMSE[0] = data[i - 1, line];
                            dataMSE[1] = (int)cs.Interpolate(i);
                            dataMSE[2] = data[i + 1, line];
                            dataMSE[3] = data[i + 2, line];
                            dataMSE[4] = data[i + 3, line];


                            //dataMSE[2] = (int)cs.Interpolate(i + 1);
                            //dataMSE[3] = (int)cs.Interpolate(i + 2);
                            //dataMSE[4] = (int)cs.Interpolate(i + 3);
                        }
                        if (i == arrayLength - 1)
                        {
                            //dataMSE[0] = (int)cs.Interpolate(i - 4);
                            //dataMSE[1] = (int)cs.Interpolate(i - 3);
                            //dataMSE[2] = (int)cs.Interpolate(i - 2);
                            //dataMSE[3] = (int)cs.Interpolate(i - 1);
                            dataMSE[0] = data[i - 4, line];
                            dataMSE[1] = data[i - 3, line];
                            dataMSE[2] = data[i - 2, line];
                            dataMSE[3] = data[i - 1, line];



                            dataMSE[4] = (int)cs.Interpolate(i);
                        }
                        if (i == arrayLength - 2)
                        {
                            //dataMSE[0] = (int)cs.Interpolate(i - 3);
                            //dataMSE[1] = (int)cs.Interpolate(i - 2);
                            //dataMSE[2] = (int)cs.Interpolate(i - 1);

                            dataMSE[0] = data[i - 3, line];
                            dataMSE[1] = data[i - 2, line];
                            dataMSE[2] = data[i - 1, line];

                            dataMSE[3] = (int)cs.Interpolate(i);

                            dataMSE[4] = data[i, line];
                            //dataMSE[4] = (int)cs.Interpolate(i + 1);
                        }
                    }
                    values[counter] = new ouputValues
                    {
                        x = i,
                        y = line,
                        pixelvalue = (int)cs.Interpolate(i),
                        MSEvalue = CalculateMSE(dataMSE)
                    };
                    counter++;
                }
            }
            return values;
        }

        public int[,] CorrectArea(int[,] rawData, int[,] mapData, int slicesX, int slicesY, int indexX, int indexY)
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





        public ouputValues[] CorrectLine2(int[] dataLine, int[] mapLine, int number, bool direction)
        {
            ArrayList output = new ArrayList();


            int counter = 0;
            for (int m = 0; m < dataLine.Length; m++)
            {
                if (mapLine[m] == ushort.MaxValue)
                {
                    counter++;
                }
            }



            double[] posotion = new double[counter];
            double[] rawValue = new double[counter];
            ouputValues[] values = new ouputValues[counter];
            counter = 0;

            for (int i = 0; i < dataLine.Length; i++)
            {
                if (mapLine[i] == ushort.MaxValue)
                {
                    posotion[counter] = i;
                    rawValue[counter] = dataLine[i];
                    counter++;
                }
            }



            CubicSpline cs = CubicSpline.InterpolateNatural(posotion, rawValue);
            int arrayLength = dataLine.Length;
            counter = 0;

            int[] dataMSE;
            for (int i = 0; i < dataLine.Length; i++)
            {
                dataMSE = new int[5];
                if (mapLine[i] == ushort.MinValue)
                {


                    if (i >= 2 && i + 2 <= dataLine.Length - 1)
                    {
                        dataMSE[0] = dataLine[i - 2];
                        dataMSE[1] = dataLine[i - 1];
                        dataMSE[2] = (int)cs.Interpolate(i);
                        dataMSE[3] = dataLine[i + 1];
                        dataMSE[4] = dataLine[i + 2];
                    }
                    else
                    {
                        if (i == 0)
                        {
                            dataMSE[0] = (int)cs.Interpolate(i);

                            dataMSE[1] = dataLine[i + 1];
                            dataMSE[2] = dataLine[i + 2];
                            dataMSE[3] = dataLine[i + 3];
                            dataMSE[4] = dataLine[i + 4];
                        }
                        if (i == 1)
                        {
                            dataMSE[0] = dataLine[i - 1];
                            dataMSE[1] = (int)cs.Interpolate(i);
                            dataMSE[2] = dataLine[i + 1];
                            dataMSE[3] = dataLine[i + 2];
                            dataMSE[4] = dataLine[i + 3];
                        }
                        if (i == dataLine.Length - 1)
                        {
                            dataMSE[0] = dataLine[i - 4];
                            dataMSE[1] = dataLine[i - 3];
                            dataMSE[2] = dataLine[i - 2];
                            dataMSE[3] = dataLine[i - 1];
                            dataMSE[4] = (int)cs.Interpolate(i);
                        }
                        if (i == dataLine.Length - 2)
                        {
                            dataMSE[0] = dataLine[i - 3];
                            dataMSE[1] = dataLine[i - 2];
                            dataMSE[2] = dataLine[i - 1];
                            dataMSE[3] = (int)cs.Interpolate(i);
                            dataMSE[4] = dataLine[i];
                        }
                    }
                    if (direction)
                    {
                        values[counter] = new ouputValues
                        {
                            x = number,
                            y = i,
                            pixelvalue = (int)cs.Interpolate(i),
                            MSEvalue = CalculateMSE(dataMSE)
                        };
                        counter++;
                    }
                    else
                    {
                        values[counter] = new ouputValues
                        {
                            x = i,
                            y = number,
                            pixelvalue = (int)cs.Interpolate(i),
                            MSEvalue = CalculateMSE(dataMSE)
                        };
                        counter++;
                    }
                }
            }
            return values;
        }



        public int[,] CorrectArea2(int[,] rawData, int[,] mapData, int slicesX, int slicesY, int indexX, int indexY)
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
                    ouputValues[] horizontal = CorrectLine2(copyRow(raw, yyy), copyRow(map, yyy), yyy, false);
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
                    ouputValues[] vertical = CorrectLine2(copyColumn(raw, xxx), copyColumn(map, xxx), xxx, true);
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




        public double makeSplineHorizontal(int[,] data, int[,] map, double yyy, double input)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            ArrayList coordinateList = new ArrayList();
            ArrayList valueList = new ArrayList();
            for (int xxx = 0; xxx < width; xxx++)
            {
                if (map[xxx, (int)yyy] == ushort.MaxValue)
                {
                    coordinateList.Add((double)xxx);
                    valueList.Add((double)data[xxx, (int)yyy]);
                }
            }
            CubicSpline splineHorizontal = CubicSpline.InterpolateNatural(coordinateList.ToArray(typeof(double)) as double[], valueList.ToArray(typeof(double)) as double[]);
            return splineHorizontal.Interpolate(input);
        }

        public double makeSplineVertical(int[,]data, int[,]map,double xxx, double input)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            ArrayList coordinateList = new ArrayList();
            ArrayList valueList = new ArrayList();
            for (int yyy = 0; yyy < height; yyy++)
            {
                if (map[(int)xxx, yyy] == ushort.MaxValue)
                {
                    coordinateList.Add((double)yyy);
                    valueList.Add((double)data[(int)xxx, yyy]);
                }
            }
            CubicSpline splineHorizontal = CubicSpline.InterpolateNatural(coordinateList.ToArray(typeof(double)) as double[], valueList.ToArray(typeof(double)) as double[]);
            System.Console.WriteLine();
            double temp= splineHorizontal.Interpolate(input);
            return temp;
        }
    }
}
