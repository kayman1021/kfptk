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
    public enum InterpolationMethod { Linear, Quadratic,Cubic,Quartic,Quintic };

    public struct InterpolatedUnit
    {
        public uint x;
        public uint y;
        public double value;
        public double goodnessOfFit;
        public InterpolationMethod method;
    }

    partial class BP_Data
    {
        public List<InterpolatedUnit> optimalResults(Matrix<ushort>input, Matrix<ushort>map)
        {
            List<InterpolatedUnit> output = new List<InterpolatedUnit>();

            //build vertical and horizontal spline matrix

            List<List<double>> vertical_place = new List<List<double>>();
            List<List<double>> vertical_value = new List<List<double>>();
            List<List<double>> horizontal_place = new List<List<double>>();
            List<List<double>> horizontal_value = new List<List<double>>();
            //Matrix<ushort> vertical = input;
            for (int xxx = 0; xxx < input.ColumnCount; xxx++)
            {
                List<double> place = new List<double>();
                List<double> value = new List<double>();
                Vector<ushort> asvector =input.Column(xxx);
                for (int yyy = 0; yyy < asvector.Count; yyy++)
                {
                    if (asvector[yyy]!=ushort.MinValue)
                    {
                        place.Add(yyy);
                        value.Add(asvector[yyy]);
                    }
                }
                vertical_place.Add(place);
                vertical_place.Add(value);
            }
            for (int yyy = 0; yyy < input.RowCount; yyy++)
            {
                List<double> place = new List<double>();
                List<double> value = new List<double>();
                Vector<ushort> asvector = input.Column(yyy);
                for (int xxx = 0; xxx < asvector.Count; xxx++)
                {
                    if (asvector[xxx] != ushort.MinValue)
                    {
                        place.Add(xxx);
                        value.Add(asvector[xxx]);
                    }
                }
                horizontal_place.Add(place);
                horizontal_place.Add(value);
            }


            for (int xxx = 0; xxx < input.ColumnCount; xxx++)
            {
                List<double> place = vertical_place[xxx];
                List<double> value = vertical_value[xxx];
                CubicSpline cs = CubicSpline.InterpolateNatural(place, value);
                for (int yyy = 0; yyy < input.RowCount; yyy++)
                {
                    //double[] t = Fit.Polynomial(place.ToArray(), value.ToArray(), 5);
                    //GoodnessOfFit.RSquared(place_cs.Where(x => x <= 0 || x < input.RowCount||x>=yyy-2||x<=yyy+2), value_cs);
                    if (map[xxx,yyy]==ushort.MinValue)
                    {
                        place.Add(yyy);
                        value.Add(cs.Interpolate(yyy));
                        //output.Add(new InterpolatedUnit() {x=(uint)xxx,y=(uint)yyy,value=cs.Interpolate(yyy)});
                    }
                }
                cs = CubicSpline.InterpolateNatural(place, value);
                for (int yyy = 0; yyy < input.RowCount; yyy++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        if (yyy <= 0 || yyy < input.RowCount || yyy >= yyy - 2 || yyy <= yyy + 2)
                        {
                            var p = place.Skip(yyy - 2).Take(4);
                            var v = value.Skip(yyy - 2).Take(4);
                            double[] t = Fit.Polynomial(p.ToArray(), v.ToArray(), 5);
                            Func<double, double> f = Fit.PolynomialFunc(p.ToArray(), v.ToArray(), 5,MathNet.Numerics.LinearRegression.DirectRegressionMethod.NormalEquations);
                            GoodnessOfFit.RSquared(p.Select(x => t[0]+t[1]*x+t[2]*x*x+t[3]*x*x*x+t[4]*x*x*x*x+t[5]*x*x*x*x*x), v);
                        }
                    }
                }
            }
            for (int yyy = 0; yyy < input.RowCount; yyy++)
            {
                for (int xxx = 0; xxx < input.ColumnCount; xxx++)
                {
                    if (map[xxx, yyy] == ushort.MinValue)
                    {
                        //do some row interpolation
                        //add best result to output list
                    }
                }
            }
            //for trough other directions









            return output;
        }
    }
}
