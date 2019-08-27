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

        public int[] CorrectRow(int[,] data, int[,] map, int row)
        {
            double[] x_double;
            double[] y_double;
            int[] output = new int[data.GetLength(0)];
            List<int> x_list = new List<int>();
            List<int> y_list = new List<int>();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (map[i, row] == ushort.MaxValue)
                {
                    x_list.Add(i);
                    y_list.Add(data[i, row]);
                }
            }
            x_double = new double[x_list.Count];
            y_double = new double[y_list.Count];
            for (int i = 0; i < x_list.Count; i++)
            {
                x_double[i] = x_list[i];
                y_double[i] = y_list[i];
            }
            CubicSpline cs = CubicSpline.InterpolateNatural(x_double, y_double);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (map[i, row] == ushort.MinValue)
                {
                    output[i] = (int)cs.Interpolate(i);
                }
                else
                {
                    output[i] = data[i, row];
                }

            }
            return output;
        }

        public int[] CorrectColumn(int[,] data, int[,] map, int column)
        {
            double[] x_double;
            double[] y_double;
            int[] output = new int[data.GetLength(1)];
            List<int> x_list = new List<int>();
            List<int> y_list = new List<int>();

            for (int i = 0; i < data.GetLength(1); i++)
            {
                if (map[column, i] == ushort.MaxValue)
                {
                    x_list.Add(i);
                    y_list.Add(data[column,i]);
                }
            }
            x_double = new double[x_list.Count];
            y_double = new double[y_list.Count];
            for (int i = 0; i < x_list.Count; i++)
            {
                x_double[i] = x_list[i];
                y_double[i] = y_list[i];
            }
            CubicSpline cs = CubicSpline.InterpolateNatural(x_double, y_double);
            for (int i = 0; i < data.GetLength(1); i++)
            {
                if (map[column,i] == ushort.MinValue)
                {
                    output[i] = (int)cs.Interpolate(i);
                }
                else
                {
                    output[i] = data[column,i];
                }

            }
            return output;
        }
    }
}
