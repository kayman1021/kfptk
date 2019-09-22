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
    //public enum InterpolationMethod { Constant = 0, Linear = 1, Quadratic = 2, Cubic = 3, Quartic = 4 };
    //public enum Direction { Horizontal = 0, Vertical = 1, Diagonal1 = 2, Diagonal2 = 3 };
    public enum DngFileType { Xiaomi16bit, MLVApp14bit };
    //public enum BorderType { UpperLeft, Upper, UpperRight, Right, LowerRight, Lower, LowerLeft, Left, None }
    public struct InterpolatedUnit { public int x; public int y; public int location; public double value; public double goodnessOfFit; public double[] valueArray; }

    public partial class BP_Data
    {
        public Matrix<double> Collector(Matrix<double> data, Matrix<double> map, int radius)
        {
            int height = data.RowCount;
            int width = data.ColumnCount;

            List<InterpolatedUnit>[,] allValue = new List<InterpolatedUnit>[width, height];

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
                }
            }
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
    }
}
