using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics;

namespace p00
{
    public enum InterpolationMethod { Constant = 0, Linear = 1, Quadratic = 2, Cubic = 3, Quartic = 4 };
    public enum Direction { Horizontal = 0, Vertical = 1, Diagonal1 = 2, Diagonal2 = 3 };
    public enum DngFileType { Xiaomi16bit, MLVApp14bit };
    public enum BorderType { UpperLeft, Upper, UpperRight, Right, LowerRight, Lower, LowerLeft, Left, None }
    public struct InterpolatedUnit { public int x; public int y; public int location; public double value; public double goodnessOfFit; public Direction direction; public double[] valueArray; public InterpolationMethod method; }

    public partial class BP_Data
    {
        public Matrix<double> Collector(Matrix<double> data, Matrix<double> map, int radius)
        {
            int height = data.RowCount;
            int width = data.ColumnCount;

            List<InterpolatedUnit>[,] allValue = new List<InterpolatedUnit>[width, height];
            //Matrix<double> data = data;
            //Matrix<double> map = map;
            //data = data;

            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    List<InterpolatedUnit> results = new List<InterpolatedUnit>();
                    if (map[yyy, xxx] == 0)
                    {
                        if (yyy - radius >= 0 && yyy + radius < height && xxx - radius >= 0 && xxx + radius < width)
                        {
                            int size = (radius * 2) + 1;
                            /*if (xxx==560&&yyy==76)
                            {
                                Console.WriteLine();
                            }*/
                            Matrix<double> subData = data.SubMatrix(yyy - radius, size, xxx - radius, size);
                            Matrix<double> subMap = map.SubMatrix(yyy - radius, size, xxx - radius, size);

                            double[] tempData = new double[size];
                            double[] places = new double[tempData.Length];
                            for (int i = 0; i < places.Length; i++)
                            {
                                places[i] = i;
                            }


                            tempData = (subData.Row(radius)).AsArray();
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(Fitter(tempData, places, i));
                            }

                            tempData = (subData.Column(radius)).AsArray();
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(Fitter(tempData, places, i));
                            }

                            tempData = (subData.Diagonal()).AsArray();
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(Fitter(tempData, places, i));
                            }

                            tempData = (FlipMatrixHorizontally(subData).Diagonal()).AsArray();
                            for (int i = 1; i <= 2; i++)
                            {
                                results.Add(Fitter(tempData, places, i));
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
                    //Console.WriteLine();
                }
            }
            //Console.WriteLine();
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (allValue[xxx, yyy].Count != 0)
                    {
                        InterpolatedUnit bestGoodness = new InterpolatedUnit { goodnessOfFit = 99999999 };
                        List<InterpolatedUnit> temp3 = allValue[xxx, yyy];
                        for (int i = 0; i < temp3.Count; i++)
                        {
                            if (temp3[i].goodnessOfFit <= bestGoodness.goodnessOfFit)
                            {
                                bestGoodness = temp3[i];
                            }
                        }
                        data[yyy, xxx] = bestGoodness.value;
                    }
                }
            }

            return data;
        }

        public Matrix<double> FlipMatrixHorizontally(Matrix<double> data)
        {
            Matrix<double> output = Matrix<double>.Build.Dense(data.RowCount, data.ColumnCount);
            int counter = data.ColumnCount - 1;
            for (int i = 0; i < data.ColumnCount; i++)
            {
                output.SetColumn(0, data.Row(counter));
                counter--;
            }
            return output;
        }

        public InterpolatedUnit Fitter(double[] values, double[] places, int order)
        {
            double[] fit = Fit.Polynomial(places, values, order);
            double[] fittedValues = new double[places.Length];
            for (int i = 0; i < fittedValues.Length; i++)
            {
                fittedValues[i] = Polynomial.Evaluate(i, fit);
            }
            double error = ErrorOfFit(values, fittedValues);
            return new InterpolatedUnit { value = (int)Polynomial.Evaluate(values.Length / 2, fit), goodnessOfFit = error };
        }

        public Matrix<double> Prefit(Matrix<double> data, Matrix<double> map)
        {
            int height = data.RowCount;
            int width = data.ColumnCount;
            Matrix<double> output = data;
            for (int xxx = 0; xxx < width; xxx++)
            {
                for (int yyy = 0; yyy < height; yyy++)
                {
                    if (map[yyy, xxx] == 0)
                    {
                        if (xxx == 0)
                        {
                            if (yyy == 0) { output[yyy, xxx] = (int)((data[yyy + 1, xxx] + data[yyy, xxx + 1]) / 2d); }//borderType = BorderType.LowerLeft;   
                            else
                            {
                                if (yyy == height - 1) { output[yyy, xxx] = (int)((data[yyy - 1, xxx] + data[yyy, xxx + 1]) / 2d); }//borderType = BorderType.UpperLeft;
                                else { output[yyy, xxx] = (int)((data[yyy + 1, xxx] + data[yyy - 1, xxx] + data[yyy, xxx + 1]) / 3d); }//borderType = BorderType.Left;
                            }
                        }
                        else
                        {
                            if (xxx == width - 1)
                            {
                                if (yyy == 0) { output[yyy, xxx] = (int)((data[yyy + 1, xxx] + data[yyy, xxx - 1]) / 2d); }//borderType = BorderType.LowerRight;
                                else
                                {
                                    if (yyy == height - 1) { data[yyy, xxx] = (int)((data[yyy - 1, xxx] + data[yyy, xxx - 1]) / 2d); }//borderType = BorderType.UpperRight;
                                    else { output[yyy, xxx] = (int)((data[yyy + 1, xxx] + data[yyy - 1, xxx] + data[yyy, xxx - 1]) / 3d); }//borderType = BorderType.Right;
                                }
                            }
                            else
                            {
                                if (yyy == 0) { output[yyy, xxx] = (int)((data[yyy + 1, xxx] + data[yyy, xxx + 1] + data[yyy, xxx - 1]) / 3d); }//borderType = BorderType.Lower;     
                                else
                                {
                                    if (yyy == height - 1) { data[yyy, xxx] = (int)((data[yyy - 1, xxx] + data[yyy, xxx + 1] + data[yyy, xxx - 1]) / 3d); }//borderType = BorderType.Upper;
                                    else { output[yyy, xxx] = (int)((data[yyy + 1, xxx] + data[yyy - 1, xxx] + data[yyy, xxx + 1] + data[yyy, xxx - 1]) / 4d); }//borderType = BorderType.None;
                                }
                            }
                        }
                    }
                }
            }
            return output;
        }


        public double ErrorOfFit(double[] measured, double[] fitted)
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


        public Vector<double> ReturnDiagonalVector(Matrix input, int line)
        {
            int width = input.ColumnCount;
            int height = input.RowCount;

            int square = Math.Min(width, height);

            Vector<double> output;
            output = Vector.Build.Dense(square);

            if (line == 0)
            {
                for (int i = 0; i < square; i++)
                {
                    output[i] = input[i, i];
                }
            }
            else
            {
                if (line < 0)
                {
                    if (height + line >= square)
                    {
                        for (int i = 0; i < square; i++)
                        {
                            output[i] = input[i - line, i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < height + line; i++)
                        {
                            output[i] = input[i - line, i];
                        }
                    }
                }
                else
                {
                    if (true)
                    {

                    }



                    for (int i = 0; i < square; i++)
                    {
                        output[i] = input[i + line, i];
                    }
                }
            }
            return output;


        }

        public Matrix<double> ggg(Matrix<double> input, Matrix<double> map, int radius)
        {
            int width = input.ColumnCount;
            int height = input.RowCount;
            //List<InterpolatedUnit> tempList = new List<InterpolatedUnit>();
            List<InterpolatedUnit>[] arrayOfList = new List<InterpolatedUnit>[width * height];
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    if (map[yyy, xxx] == 0)
                    {
                        arrayOfList[yyy * width + xxx] = new List<InterpolatedUnit>();
                    }
                }
            }
            /*arrayOfList = getvalues(radius, order, input, map, true, arrayOfList);
            arrayOfList = getvalues(radius, order, TransposeArray(input), TransposeArray(map), false,arrayOfList);*/

            arrayOfList = getvalues(radius, 1, input, map, true, arrayOfList);
            arrayOfList = getvalues(radius, 1, TransposeArray(input), TransposeArray(map), false, arrayOfList);
            arrayOfList = getvalues(radius, 2, input, map, true, arrayOfList);
            arrayOfList = getvalues(radius, 2, TransposeArray(input), TransposeArray(map), false, arrayOfList);
            //arrayOfList = getvalues(radius, 3, input, map, true, arrayOfList);
            //arrayOfList = getvalues(radius, 3, TransposeArray(input), TransposeArray(map), false, arrayOfList);



            //Console.WriteLine();

            for (int i = 0; i < arrayOfList.Length; i++)
            {
                if (arrayOfList[i] != null)
                {
                    List<InterpolatedUnit> temp = arrayOfList[i];
                    if (i == (237 * 564) + 295)
                    {
                        //Console.WriteLine();
                    }
                    InterpolatedUnit bestGoodness = new InterpolatedUnit { goodnessOfFit = 99999999 };
                    InterpolatedUnit temp3;
                    for (int j = 0; j < temp.Count; j++)
                    {
                        temp3 = temp[j];
                        /*if (temp3.location==95666)
                        {
                            Console.WriteLine();
                        }*/
                        if (temp3.goodnessOfFit <= bestGoodness.goodnessOfFit)
                        {
                            bestGoodness = temp3;
                        }
                    }
                    input[bestGoodness.y, bestGoodness.x] = bestGoodness.value;
                }
            }

            return input;
        }

        public List<InterpolatedUnit>[] getvalues(int radius, int order, Matrix<double> input, Matrix<double> map, bool direction, List<InterpolatedUnit>[] arrayOfList)
        {

            if (!direction)
            {
                //Console.WriteLine();
            }



            int height = input.RowCount;
            int width = input.ColumnCount;
            int upper = 0;
            int lower = 0;
            List<InterpolatedUnit> output = new List<InterpolatedUnit>();

            //Matrix<double> mmm = input;







            for (int yyy = 0; yyy < input.RowCount; yyy++)
            {
                Vector<double> vec = input.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {



                    if (mappam[xxx] == 0)
                    {
                        if ((xxx == 415 && yyy == 132) || (yyy == 415 && xxx == 132))
                        {
                            //Console.WriteLine();
                        }
                        if (xxx >= 0 + radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                lower = xxx - radius;
                                upper = vec.Count - 1;
                            }
                        }
                        else
                        {
                            lower = 0;
                            upper = xxx + radius;
                        }

                        //Console.WriteLine();

                        if (xxx == 0 || xxx == width - 1)
                        {
                            arrayOfList[(yyy * width) + xxx].Add(new InterpolatedUnit { x = xxx, y = yyy, value = 0, goodnessOfFit = (double)99999999, location = (yyy * width) + xxx });
                        }
                        else
                        {
                            double[] values = new double[upper - lower];
                            double[] values2 = new double[(upper - lower) + 1];
                            double[] fittedValues = new double[(upper - lower) + 1];
                            double[] places = new double[upper - lower];
                            int counter = 0;
                            int middle = 0;
                            for (int vvv = lower; vvv <= upper; vvv++)
                            {
                                if (vvv != xxx)
                                {
                                    values[counter] = vec[vvv];
                                    places[counter] = vvv;
                                    counter++;
                                }
                                else
                                {
                                    middle = vvv - lower;
                                }
                            }
                            double goodnessOfFit;
                            double calculatedValue;
                            double[] FIT;

                            FIT = Fit.Polynomial(places, values, order);
                            for (int i = 0; i < places.Length + 1; i++)
                            {
                                fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                            }
                            calculatedValue = (int)Polynomial.Evaluate(xxx, FIT);

                            for (int i = 0; i < middle; i++)
                            {
                                values2[i] = values[i];
                            }
                            values2[middle] = calculatedValue;
                            for (int i = middle + 1; i < values2.Length; i++)
                            {
                                values2[i] = values[i - 1];
                            }

                            goodnessOfFit = ErrorOfFit(values2, fittedValues);
                            if (direction)
                            {
                                arrayOfList[(yyy * width) + xxx].Add(new InterpolatedUnit { x = (int)xxx, y = (int)yyy, value = calculatedValue, goodnessOfFit = goodnessOfFit, location = (yyy * width) + xxx, direction = Direction.Horizontal, valueArray = values2, method = (InterpolationMethod)order });
                            }
                            else
                            {
                                arrayOfList[(xxx * height) + yyy].Add(new InterpolatedUnit { x = (int)yyy, y = (int)xxx, value = calculatedValue, goodnessOfFit = goodnessOfFit, location = (xxx * height) + yyy, direction = Direction.Vertical, valueArray = values2, method = (InterpolationMethod)order });
                            }
                        }
                    }
                }
            }

            return arrayOfList;
        }
    }
}
