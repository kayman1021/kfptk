using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace BP
{
    //public enum dngFileType { Xiaomi16bit, MLVApp14bit };
    partial class BP_Data
    {
        public Matrix<ushort> Left;
        public Matrix<ushort> Right;
    }
}
