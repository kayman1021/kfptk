using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Collections;
using System.IO;
using BitMiracle.LibTiff.Classic;
using MathNet.Numerics.Interpolation;

namespace p00
{
    public partial class BP_Data
    {
        public Matrix<double> Left;
        public Matrix<double> Right;
        public double minimum, maximum;
    }
}
