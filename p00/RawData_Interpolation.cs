using MathNet.Numerics.Interpolation;
using System.Collections;
using System.Numerics;
using MathNet.Numerics.Statistics;
using MathNet.Numerics;
using System;
using System.Linq;

namespace p00
{
    partial class RawData
    {


        public double CalculateMSE_double(double[] input)
        {
            double sum = 0;
            for (int i = 0; i < input.Length; i++)
            {
                sum += input[i];
            }
            double average = sum / input.Length;
            sum = 0;
            double temp = 0;
            for (int i = 0; i < input.Length; i++)
            {
                temp = average - input[i];
                sum += temp * temp;
            }



            return sum / (float)input.Length;
        }

        public struct ouputValues
        {
            public int x;
            public int y;
            public int value;
            public double error;
            //public int i;
        }

        public struct outputDouble
        {
            public double x;
            public double y;
            public double value;
            public int radius;
        }

        public struct pointDouble
        {
            public double x;
            public double y;
        }

        public double degToRad(double angleInDegree)
        {
            return angleInDegree * System.Math.PI / 180;
        }

        public ouputValues FindValueAtDegree(int[,] data, int[,] map, double degree, int xxx, int yyy, CubicSpline[] helperHOR, CubicSpline[] helperVER)
        {
            int oldvalue = data[xxx, yyy];
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            double DegInRad = degToRad(degree);

            ArrayList tempPoints = new ArrayList();
            double valueX;
            double valueY;
            int limitUpper = 9;
            int limitLower = -1 * limitUpper;
            for (int i = limitLower; i <= limitUpper; i++)
            {
                if (i != 0)
                {
                    valueX = i + xxx;
                    valueY = (System.Math.Tan(DegInRad)*i)+yyy;
                    if (valueX >= 0 && valueX < width && valueY >= 0 && valueY < height&& valueY-yyy>= limitLower && valueY-yyy<= limitUpper)
                    {
                        tempPoints.Add(new outputDouble() { x = valueX, y = valueY, value = interpolateFromColumn((int)valueX, valueY, helperVER), radius = i });

                    }
                    valueX = (i/System.Math.Tan(DegInRad))+xxx;
                    valueY = i + yyy;
                    if (valueX >= 0 && valueX < width && valueY >= 0 && valueY < height && valueX-xxx >= limitLower && valueX-xxx <= limitUpper)
                    {
                        tempPoints.Add(new outputDouble() { x =valueX, y = valueY, value = interpolateFromRow(valueX,(int)valueY, helperHOR), radius = i });
                    }
                }
            }
            System.Console.WriteLine();

            ArrayList arrayAxis = new ArrayList();
            ArrayList arrayValues = new ArrayList();
            outputDouble temp;
            for (int i = 0; i < tempPoints.Count; i++)
            {
                temp = (outputDouble)tempPoints[i];
                arrayAxis.Add(temp.radius / System.Math.Abs(temp.radius) * getVectorLength(xxx - temp.x, yyy - temp.y));
                arrayValues.Add(temp.value);
            }

            if (arrayAxis.Count <= 4)
            {
                return new ouputValues() { x = xxx, y = yyy, value = 0, error = double.MaxValue };
            }
            else
            {
                CubicSpline tempSpline = CubicSpline.InterpolateNatural(arrayAxis.ToArray(typeof(double)) as double[], arrayValues.ToArray(typeof(double)) as double[]);
                double interpolatedValue = tempSpline.Interpolate(0);
                arrayValues.Add(interpolatedValue);
                arrayAxis.Add((double)0);
                double[] aAxis = arrayAxis.ToArray(typeof(double)) as double[];
                double[] aValues = arrayValues.ToArray(typeof(double)) as double[];
                Tuple<double, double> p = Fit.Line(aAxis, aValues);
                double[] q = Fit.Polynomial(aAxis, aValues, 3, MathNet.Numerics.LinearRegression.DirectRegressionMethod.QR);
                double q1 = q[0];
                double q2 = q[1];
                double q3 = q[2];
                double a = p.Item1;
                double b = p.Item2;
                //double thismofo=GoodnessOfFit.RSquared(aAxis.Select(x => a + b * x), aValues);
                double thismofo = GoodnessOfFit.RSquared(aAxis.Select(x => q1+q2*x+q3*x*x), aValues);
                Console.WriteLine();
                return new ouputValues() { x = xxx, y = yyy, value = (int)interpolatedValue, error =thismofo };
                //return new ouputValues() { x = xxx, y = yyy, pixelvalue = (int)interpolatedValue, MSEvalue = ArrayStatistics.Variance(CalculateMSE_double(arrayValues.ToArray(typeof(double)) as double[]) };
            }

        }




        public ArrayList FindValueAtArea(int[,] data, int[,] map, ArrayList angles, CubicSpline[] helperHOR, CubicSpline[] helperVER)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            int variations = angles.Count;
            ArrayList output = new ArrayList();
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        for (int i = 0; i < variations; i++)
                        {
                            output.Add(FindValueAtDegree(data, map, (double)(angles[i]), xxx, yyy, helperHOR, helperVER));
                        }
                    }
                }
            }
            return output;
        }


        public void CA(int[,] inputData, int[,] inputMap, int slicesX, int slicesY, int indexX, int indexY)
        {
            int[,] data = SliceBlock(rawData, slicesX, slicesY, indexX, indexY);
            int[,] map = SliceBlock(mapData, slicesX, slicesY, indexX, indexY);
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            CubicSpline[] helperHOR = InterpolateRow(data, map);
            CubicSpline[] helperVER = InterpolateColumn(data, map);
            ArrayList angles = new ArrayList();
            for (int i = 0; i < 180; i=i+15)
            {
                angles.Add((double)i);
            }
            
            int counter = 0;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx,yyy]==0)
                    {
                        counter++;
                    }
                }
            }
            ArrayList result = new ArrayList();
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == 0)
                    {
                        //result.Add(new pointDouble() { x = xxx, y = yyy });
                        result.Add(new ArrayList() { new pointDouble() { x = xxx, y = yyy } });
                    }
                }
            }
            CB(data, map, helperHOR, helperVER, angles, result);
        }


        public void CB(int[,]data,int[,]map, CubicSpline[] helperHOR, CubicSpline[] helperVER, ArrayList angles, ArrayList results)
        {
            int oldvalue;
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            double DegInRad;
            int xxx, yyy;
            int limitUpper = 9;
            int limitLower = -limitUpper;
            int badpixelsCount = results.Count;
            for (int i = 0; i < angles.Count; i++)
            {
                DegInRad = degToRad((double)angles[i]);

                double localX;
                double localY;
                double globalX;
                double globalY;
                ArrayList anglePoints = new ArrayList();
                for (int radius = limitLower; radius < limitUpper; radius++)
                {
                    if (radius!=0)
                    {
                        localX = radius;
                        localY = (System.Math.Tan(DegInRad) * radius);
                        if (localY >= limitLower && localY <= limitUpper)
                        {
                            anglePoints.Add(new pointDouble() {x=localX,y=localY });
                        }
                        localX = radius / System.Math.Tan(DegInRad);
                        localY = radius;
                        if (localX >= limitLower && localX <= limitUpper)
                        {
                            anglePoints.Add(new pointDouble() { x = localX, y = localY });
                        }
                    }
                }

                for (int j = 0; j < badpixelsCount; j++)
                {
                    ArrayList temp1 = (ArrayList)results[j];
                    pointDouble temp2 = (pointDouble)temp1[0];
                    xxx = (int)temp2.x;
                    yyy = (int)temp2.y;
                    oldvalue = data[xxx, yyy];
                    ArrayList tempPoints = new ArrayList();


                    for (int k = 0; k < anglePoints.Count; k++)
                    {
                        localX = ((pointDouble)anglePoints[k]).x;
                        localY = ((pointDouble)anglePoints[k]).y;
                        globalX = localX + xxx;
                        globalY = localY + yyy;
                        if (globalX>=0&& globalX <width&& globalY >= 0 && globalY < height)
                        {
                            if (Math.Abs(localX % 1) <= (Double.Epsilon * 100))
                            {
                                tempPoints.Add(new outputDouble() { x = globalX, y = globalY, value = interpolateFromColumn((int)globalX, globalY, helperVER), radius = (int)localX });
                            }
                            else
                            {
                                if (Math.Abs(localY % 1) <= (Double.Epsilon * 100))
                                {
                                    tempPoints.Add(new outputDouble() { x = globalX, y = globalY, value = interpolateFromRow(globalX, (int)globalY, helperHOR), radius = (int)localY });
                                }
                                else
                                {
                                    Console.WriteLine("WRONG");
                                }
                            }
                            
                        }
                    }


                    for (int radius = limitLower; radius <= limitUpper; radius++)
                    {
                        if (radius!=0)
                        {
                            localX = radius;
                            localY = (System.Math.Tan(DegInRad) * radius);
                            globalX = localX + xxx;
                            globalY = localY + yyy;
                            if (globalX >= 0 && globalX < width && globalY >= 0 && globalY< height && localY >= limitLower && localY <= limitUpper)
                            {
                                tempPoints.Add(new outputDouble() { x = globalX, y = globalY, value = interpolateFromColumn((int)globalX, globalY, helperVER), radius = radius });
                            }

                            localX = radius / System.Math.Tan(DegInRad);
                            localY = radius;
                            globalX = localX + xxx;
                            globalY = localY + yyy;
                            if (globalX >= 0 && globalX < width && globalY >= 0 && globalY < height && localX >= limitLower && localX  <= limitUpper)
                            {
                                tempPoints.Add(new outputDouble() { x = globalX, y = globalY, value = interpolateFromRow(globalX, (int)globalY, helperHOR), radius = radius });
                            }
                        }


                    }
                }
            }
        }


        public int[,] CorrectArea(int[,] inputData, int[,] inputMap, int slicesX, int slicesY, int indexX, int indexY)
        {
            int[,] data = SliceBlock(rawData, slicesX, slicesY, indexX, indexY);
            int[,] map = SliceBlock(mapData, slicesX, slicesY, indexX, indexY);
            CubicSpline[] helperHOR = InterpolateRow(data, map);
            CubicSpline[] helperVER = InterpolateColumn(data, map);
            ArrayList angles = new ArrayList();
            /*angles.Add((double)0);
            angles.Add((double)15);
            angles.Add((double)30);
            angles.Add((double)45);
            angles.Add((double)60);
            angles.Add((double)75);
            angles.Add((double)90);
            angles.Add((double)105);
            angles.Add((double)120);
            angles.Add((double)135);
            angles.Add((double)150);
            angles.Add((double)165);*/
            for (int i = 0; i < 180; i++)
            {
                angles.Add((double)i);
            }
            ArrayList likelyValues = new ArrayList();
            likelyValues = FindValueAtArea(data, map, angles, helperHOR, helperVER);

            int width = data.GetLength(0);
            int height = data.GetLength(1);
            ouputValues[,] optimals = new ouputValues[width, height];

            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        optimals[xxx, yyy] = new ouputValues() { x = xxx, y = yyy, error = double.MaxValue, value = 0 };
                    }
                }
            }

            for (int i = 0; i < likelyValues.Count; i++)
            {
                ouputValues temp = (ouputValues)likelyValues[i];
                if (temp.error <= optimals[temp.x, temp.y].error)
                {
                    optimals[temp.x, temp.y] = temp;
                }
            }

            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        ouputValues temp = (ouputValues)optimals[xxx, yyy];
                        data[xxx, yyy] = temp.value;
                    }
                }
            }

            return data;

        }





        public CubicSpline[] InterpolateRow(int[,] data, int[,] map)
        {
            ArrayList output = new ArrayList();
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            int counter;

            for (int yyy = 0; yyy < height; yyy++)
            {
                counter = 0;
                ArrayList axis = new ArrayList();
                ArrayList value = new ArrayList();
                for (int xxx = 0; xxx < width; xxx++)
                {

                    if (map[xxx, yyy] == ushort.MaxValue)
                    {
                        axis.Add((double)counter);
                        value.Add((double)(data[xxx, yyy]));
                    }
                    counter++;
                }
                output.Add(CubicSpline.InterpolateNatural(axis.ToArray(typeof(double)) as double[], value.ToArray(typeof(double)) as double[]));
            }
            return output.ToArray(typeof(CubicSpline)) as CubicSpline[];
        }
        public CubicSpline[] InterpolateColumn(int[,] data, int[,] map)
        {
            ArrayList output = new ArrayList();
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            int counter;

            for (int xxx = 0; xxx < width; xxx++)
            {
                counter = 0;
                ArrayList axis = new ArrayList();
                ArrayList value = new ArrayList();
                for (int yyy = 0; yyy < height; yyy++)
                {

                    if (map[xxx, yyy] == ushort.MaxValue)
                    {
                        axis.Add((double)counter);
                        value.Add((double)(data[xxx, yyy]));
                        //counter++;
                    }
                    counter++;
                }
                output.Add(CubicSpline.InterpolateNatural(axis.ToArray(typeof(double)) as double[], value.ToArray(typeof(double)) as double[]));
            }
            return output.ToArray(typeof(CubicSpline)) as CubicSpline[];
        }
        public double interpolateFromRow(double axisValue, int rowValue, CubicSpline[] Rows)
        {
            return Rows[rowValue].Interpolate(axisValue);
        }
        public double interpolateFromColumn(int columnValue, double axisValue, CubicSpline[] Columns)
        {
            return Columns[columnValue].Interpolate(axisValue);
        }
        public double getVectorLength(double x, double y)
        {
            return System.Math.Sqrt((x * x) + (y * y));
        }
    }
}
