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

        public bool isArrayListEqual(ArrayList left, ArrayList right)
        {
            if (left.Count != right.Count)
            {
                return false;
            }
            else
            {
                outputDouble tempL;
                outputDouble tempR;
                for (int i = 0; i < left.Count; i++)
                {
                    tempL = (outputDouble)left[i];
                    tempR = (outputDouble)right[i];
                    if (tempL.x != tempR.x || tempL.y != tempR.y)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public struct outputValues
        {
            public int x;
            public int y;
            public int value;
            public double error;
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

        public struct optValue
        {
            public int value;
            public double error;
        }

        public double degToRad(double angleInDegree)
        {
            return angleInDegree * System.Math.PI / 180;
        }

        public outputValues FindValueAtDegree(int[,] data, int[,] map, double degree, int xxx, int yyy, CubicSpline[] helperHOR, CubicSpline[] helperVER)
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
                    valueY = (System.Math.Tan(DegInRad) * i) + yyy;
                    if (valueX >= 0 && valueX < width && valueY >= 0 && valueY < height && valueY - yyy >= limitLower && valueY - yyy <= limitUpper)
                    {
                        tempPoints.Add(new outputDouble() { x = valueX, y = valueY, value = interpolateFromColumn((int)valueX, valueY, helperVER), radius = i });

                    }
                    valueX = (i / System.Math.Tan(DegInRad)) + xxx;
                    valueY = i + yyy;
                    if (valueX >= 0 && valueX < width && valueY >= 0 && valueY < height && valueX - xxx >= limitLower && valueX - xxx <= limitUpper)
                    {
                        tempPoints.Add(new outputDouble() { x = valueX, y = valueY, value = interpolateFromRow(valueX, (int)valueY, helperHOR), radius = i });
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
                return new outputValues() { x = xxx, y = yyy, value = 0, error = double.MaxValue };
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
                double thismofo = GoodnessOfFit.RSquared(aAxis.Select(x => q1 + q2 * x + q3 * x * x), aValues);
                Console.WriteLine();
                return new outputValues() { x = xxx, y = yyy, value = (int)interpolatedValue, error = thismofo };
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


        public int[,] CA(int[,] inputData, int[,] inputMap, int slicesX, int slicesY, int indexX, int indexY)
        {
            int[,] data = SliceBlock(rawData, slicesX, slicesY, indexX, indexY);
            int[,] map = SliceBlock(mapData, slicesX, slicesY, indexX, indexY);
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            CubicSpline[] helperHOR = InterpolateRow(data, map);
            CubicSpline[] helperVER = InterpolateColumn(data, map);
            ArrayList angles = new ArrayList();
            for (int i = 0; i < 180; i = i + 15)
            {
                angles.Add((double)i);
            }

            int counter = 0;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == 0)
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
                        result.Add(new ArrayList() { new pointDouble() { x = xxx, y = yyy } });
                    }
                }
            }
            ArrayList values = CB(data, map, helperHOR, helperVER, angles, result);
            int[,] optimizedSlice = optimizer(data, map, values);
            Console.WriteLine();
            return optimizedSlice;
        }


        public ArrayList CB(int[,] data, int[,] map, CubicSpline[] helperHOR, CubicSpline[] helperVER, ArrayList angles, ArrayList results)
        {
            ArrayList output = new ArrayList();
            int oldvalue;
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            double DegInRad;
            int xxx, yyy;
            int limitUpper = 5;
            int limitLower = -limitUpper;
            int badpixelsCount = results.Count;
            for (int i = 0; i < angles.Count; i++)
            {
                DegInRad = degToRad((double)angles[i]);

                double localX;
                double localY;
                double globalX;
                double globalY;
                ArrayList anglePoints_XisINT = new ArrayList();
                ArrayList anglePoints_YisINT = new ArrayList();
                for (int radius = limitLower; radius <= limitUpper; radius++)
                {
                    if (radius != 0)
                    {
                        if ((double)angles[i] % 90 == 0)
                        {
                            if ((double)angles[i] == 0)
                            {
                                localX = radius;
                                localY = 0;
                            }
                            else
                            {
                                localX = 0;
                                localY = radius;
                            }
                        }
                        else
                        {
                            localX = radius;
                            localY = (System.Math.Tan(DegInRad) * radius);
                        }
                        if (localY >= limitLower && localY <= limitUpper)
                        {
                            anglePoints_XisINT.Add(new outputDouble() { x = localX, y = localY, radius = radius });
                        }




                        if ((double)angles[i] == 0)
                        {
                            if ((double)angles[i] == 0)
                            {
                                localX = radius;
                                localY = 0;
                            }
                            else
                            {
                                localX = 0;
                                localY = radius;
                            }
                        }
                        else
                        {
                            localX = radius / System.Math.Tan(DegInRad);
                            localY = radius;
                        }

                        if (localX >= limitLower && localX <= limitUpper)
                        {
                            anglePoints_YisINT.Add(new outputDouble() { x = localX, y = localY, radius = radius });
                        }
                    }
                }

                Console.WriteLine();

                for (int j = 0; j < badpixelsCount; j++)
                {

                    ArrayList temp1 = (ArrayList)results[j];
                    pointDouble temp2 = (pointDouble)temp1[0];
                    xxx = (int)temp2.x;
                    yyy = (int)temp2.y;
                    oldvalue = data[xxx, yyy];
                    ArrayList tempPoints = new ArrayList();
                    int radius;


                    for (int k = 0; k < anglePoints_XisINT.Count; k++)
                    {
                        localX = ((outputDouble)anglePoints_XisINT[k]).x;
                        localY = ((outputDouble)anglePoints_XisINT[k]).y;
                        radius = ((outputDouble)anglePoints_XisINT[k]).radius;
                        globalX = localX + xxx;
                        globalY = localY + yyy;
                        if (globalX >= 0 && globalX < width && globalY >= 0 && globalY < height)
                        {
                            tempPoints.Add(new outputDouble() { x = globalX, y = globalY, value = interpolateFromColumn((int)globalX, globalY, helperVER), radius = radius });
                        }
                    }

                    if (!isArrayListEqual(anglePoints_XisINT, anglePoints_YisINT))
                    {
                        for (int k = 0; k < anglePoints_YisINT.Count; k++)
                        {
                            localX = ((outputDouble)anglePoints_YisINT[k]).x;
                            localY = ((outputDouble)anglePoints_YisINT[k]).y;
                            radius = ((outputDouble)anglePoints_YisINT[k]).radius;
                            globalX = localX + xxx;
                            globalY = localY + yyy;
                            if (globalX >= 0 && globalX < width && globalY >= 0 && globalY < height)
                            {
                                tempPoints.Add(new outputDouble() { x = globalX, y = globalY, value = interpolateFromRow(globalX, (int)globalY, helperHOR), radius = radius });
                            }
                        }
                    }


                    Console.WriteLine();

                    ArrayList arrayAxis = new ArrayList();
                    ArrayList arrayValues = new ArrayList();
                    for (int ggg = 0; ggg < tempPoints.Count; ggg++)
                    {
                        outputDouble temp = ((outputDouble)tempPoints[ggg]);
                        arrayAxis.Add(temp.radius / Math.Abs(temp.radius) * getVectorLength(xxx - temp.x, yyy - temp.y));
                        arrayValues.Add(temp.value);
                    }
                    double[] aAxis = arrayAxis.ToArray(typeof(double)) as double[];
                    double[] aValues = arrayValues.ToArray(typeof(double)) as double[];

                    //Tuple<double, double> fitLine = Fit.Line(aAxis, aValues);
                    //double goodnessLine = GoodnessOfFit.RSquared(aAxis.Select(x => fitLine.Item2 + fitLine.Item1 * x), aValues);
                    if (aAxis.Length < 4)
                    {
                        output.Add(new outputValues() { x = xxx, y = yyy, value = 0, error = 0 });
                    }
                    else
                    {
                        double[] fitPolynomial = Fit.Polynomial(aAxis, aValues, 3);
                        //double goodnessPolynomial = GoodnessOfFit.RSquared(aAxis.Select(x => fitPolynomial[0] + fitPolynomial[1] * x + fitPolynomial[2] * x * x + fitPolynomial[3] * x * x * x), aValues);
                        double goodnessPolynomial = GoodnessOfFit.RSquared(aAxis.Select(x => fitPolynomial[0] + fitPolynomial[1] * x + fitPolynomial[2] * x * x), aValues);
                        Console.WriteLine();
                        Func<double, double> f = Fit.LinearCombinationFunc(aAxis, aValues, x => 1.0, x => x, x => x * x);
                        double res = f(0);

                        Console.WriteLine();
                        output.Add(new outputValues() { x = xxx, y = yyy, value = (int)res, error = goodnessPolynomial });
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            return output;
        }




        public int[,] optimizer(int[,] data, int[,] map, ArrayList input)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            ArrayList output = new ArrayList();
            optValue[,] pic = new optValue[width, height];
            for (int i = 0; i < input.Count; i++)
            {
                outputValues temp = (outputValues)input[i];
                if (pic[temp.x, temp.y].error <= temp.error)
                {
                    pic[temp.x, temp.y].error = temp.error;
                    pic[temp.x, temp.y].value = temp.value;
                }
            }
            Console.WriteLine();
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == 0)
                    {
                        Console.WriteLine();
                        data[xxx, yyy] = pic[xxx, yyy].value;
                    }
                }
            }
            return data;
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
            for (int i = 0; i < 180; i = i + 15)
            {
                angles.Add((double)i);
            }
            ArrayList likelyValues = new ArrayList();
            likelyValues = FindValueAtArea(data, map, angles, helperHOR, helperVER);

            int width = data.GetLength(0);
            int height = data.GetLength(1);
            outputValues[,] optimals = new outputValues[width, height];

            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        optimals[xxx, yyy] = new outputValues() { x = xxx, y = yyy, error = double.MaxValue, value = 0 };
                    }
                }
            }

            for (int i = 0; i < likelyValues.Count; i++)
            {
                outputValues temp = (outputValues)likelyValues[i];
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
                        outputValues temp = (outputValues)optimals[xxx, yyy];
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
