/*using System;
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
using BitMiracle.LibTiff.Classic;*/
using MathNet.Numerics.Interpolation;

namespace p00
{
    public enum dngFileType { Xiaomi16bit, MLVApp14bit };
    partial class RawData
    {
        public int[,] rawData;
        public int[,] mapData;
    }
}
