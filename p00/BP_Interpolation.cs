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
        public uint x;
        public uint y;
        public double value;
        public double goodnessOfFit;
        public InterpolationMethod method;
    }

    public partial class BP_Data
    {
        public Matrix<double> ggg(Matrix<double> input, Matrix<double> map)
        {
            int radius = 3;
            int upper = 0;
            int lower = 0;
            List<InterpolatedUnit> tempList = new List<InterpolatedUnit>();

            Matrix<double> mmm = input;

            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, 1);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        input[yyy, xxx] = calculatedValue;
                        Console.WriteLine();
                    }
                }
            }

            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, 2);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        input[yyy, xxx] = calculatedValue;
                        Console.WriteLine();
                    }
                }
            }

            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, 2);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        input[yyy, xxx] = calculatedValue;
                        Console.WriteLine();
                    }
                }
            }

            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, 3);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        input[yyy, xxx] = calculatedValue;
                        Console.WriteLine();
                    }
                }
            }

            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, 3);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        input[yyy, xxx] = calculatedValue;
                        Console.WriteLine();
                    }
                }
            }

            for (int yyy = 0; yyy < mmm.RowCount; yyy++)
            {
                Vector<double> vec = mmm.Row(yyy);
                Vector<double> mappam = map.Row(yyy);
                for (int xxx = 0; xxx < vec.Count; xxx++)
                {
                    if (mappam[xxx] == 0)
                    {
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, 3);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        input[yyy, xxx] = calculatedValue;
                        Console.WriteLine();
                    }
                }
            }

            return input;
        }

        public List<InterpolatedUnit> getvalues(int radius, int order, Matrix<double> input, Matrix<double> map, bool direction)
        {
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
                        if (xxx >= radius)
                        {
                            if (xxx <= vec.Count - 1 - radius)
                            {
                                lower = xxx - radius;
                                upper = xxx + radius;
                                ///////////////////////////////////full radius
                            }
                            else
                            {
                                if (xxx != vec.Count - 1)
                                {
                                    lower = xxx - radius;
                                    upper = vec.Count - 1;
                                    ///////////upper limit
                                }
                            }
                        }
                        else
                        {
                            if (xxx != 0)
                            {
                                lower = 0;
                                upper = xxx + radius;
                            }
                        }
                        double[] values = new double[upper - lower + 1];
                        double[] fittedValues = new double[upper - lower + 1];
                        double[] places = new double[upper - lower + 1];
                        int counter = 0;
                        for (int vvv = lower; vvv <= upper; vvv++)
                        {
                            values[counter] = vec[vvv];
                            places[counter] = vvv;
                            counter++;
                        }
                        double goodnessOfFit;
                        double calculatedValue;
                        double[] FIT;

                        FIT = Fit.Polynomial(places, values, order);
                        for (int i = 0; i < places.Length; i++)
                        {
                            fittedValues[i] = Polynomial.Evaluate(lower + i, FIT);
                        }
                        calculatedValue = Polynomial.Evaluate(xxx, FIT);
                        goodnessOfFit = GoodnessOfFit.RSquared(fittedValues, values);
                        output.Add(new InterpolatedUnit { x = (uint)xxx, y = (uint)yyy, value = calculatedValue, goodnessOfFit = goodnessOfFit, method = (InterpolationMethod)order });
                        Console.WriteLine();
                    }
                }
            }

            return output;
        }
    }
}
