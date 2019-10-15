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
    public enum DngFileType { Xiaomi16bit = 0, MLVApp14bit = 1, AdobeExported16bit = 2 };
    public struct InterpolatedUnit { public int x; public int y; public int location; public double value; public double goodnessOfFit; public double[] valueArray; }

    public partial class BP_Data
    {

    }
}