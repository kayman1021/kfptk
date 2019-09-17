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

    public struct InterpolatedUnit
    {
        public int x;
        public int y;
        public int location;
        public double value;
        public double goodnessOfFit;
        //public InterpolationMethod method;
    }

    public partial class BP_Data
    {
        public Matrix<double> ggg(Matrix<double> input, Matrix<double> map,int order,int radius)
        {
            int width = input.ColumnCount;
            int height = input.RowCount;
            //List<InterpolatedUnit> tempList = new List<InterpolatedUnit>();
            List<InterpolatedUnit>[] arrayOfList = new List<InterpolatedUnit>[width * height];
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    if (map[yyy,xxx]==0)
                    {
                        arrayOfList[yyy * width + xxx] = new List<InterpolatedUnit>();
                    }
                }
            }
            arrayOfList = getvalues(radius, order, input, map, true, arrayOfList);
            //arrayOfList = getvalues(radius, order, input, map, true, arrayOfList);
            //tempList.AddRange(getvalues(radius, order, input, map, true));
            arrayOfList = getvalues(radius, order, TransposeArray(input), TransposeArray(map), false,arrayOfList);

            for (int i = 0; i < arrayOfList.Length; i++)
            {
                if (arrayOfList[i]!=null)
                {
                    List<InterpolatedUnit> temp = arrayOfList[i];
                    InterpolatedUnit bestGoodness = new InterpolatedUnit { };
                    InterpolatedUnit temp3;
                    for (int j = 0; j < temp.Count; j++)
                    {
                        temp3 = temp[j];
                        if (temp3.goodnessOfFit>bestGoodness.goodnessOfFit)
                        {
                            bestGoodness = temp3;
                        }
                    }
                    input[bestGoodness.y,bestGoodness.x]= bestGoodness.value;
                }
            }

            return input;
        }

        public List<InterpolatedUnit>[] getvalues(int radius, int order, Matrix<double> input, Matrix<double> map, bool direction, List<InterpolatedUnit>[]arrayOfList)
        {

            if (!direction)
            {
                Console.WriteLine();
            }



            int height = input.RowCount;
            int width = input.ColumnCount;
            int upper = 0;
            int lower = 0;
            List<InterpolatedUnit> output = new List<InterpolatedUnit>();

            Matrix<double> mmm = input;







            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >=0 + radius)
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


                        if (xxx==0||xxx==width-1)
                        {
                            arrayOfList[(yyy * width) + xxx].Add(new InterpolatedUnit { x = xxx, y = yyy, value = 0, goodnessOfFit = (double)0, location = (yyy * width) + xxx });
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

                            goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values2);
                            if (direction)
                            {
                                arrayOfList[(yyy * width) + xxx].Add(new InterpolatedUnit { x = (int)xxx, y = (int)yyy, value = calculatedValue, goodnessOfFit = goodnessOfFit, location = (yyy * width) + xxx });
                            }
                            else
                            {
                                Console.WriteLine();
                                arrayOfList[(xxx * height) + yyy].Add(new InterpolatedUnit { x = (int)yyy, y = (int)xxx, value = calculatedValue, goodnessOfFit = goodnessOfFit, location = (xxx * height) + yyy });
                            }

                            Console.WriteLine();
                        }
                    }
                }
            }

            return arrayOfList;
        }
    }
}
