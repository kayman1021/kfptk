using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace p00
{
    public partial class BP_Data
    {
        public int DeinterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((arraylength / sensels) * (input % sensels)) + (input / sensels);
        }
        public Matrix<double> DeinterlaceUniversal(Matrix<double> input, bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
            int resolutionX = input.ColumnCount;
            int resolutionY = input.RowCount;
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            Matrix<double> output = Matrix<double>.Build.Dense(resolutionY, resolutionX);
            for (int xxx = 0; xxx < resolutionX; xxx++)
            {
                int tempx = DeinterlaceCoordinate(resolutionX, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < resolutionY; yyy++)
                {
                    int tempy = DeinterlaceCoordinate(resolutionY, UniqueSenselsY, yyy);
                    output[tempy, tempx] = (double)input[yyy, xxx];
                }

            }
            return output;
        }
        public int InterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((input % (arraylength / sensels)) * sensels) + (input / (arraylength / sensels));
        }
        public Matrix<double> InterlaceUniversal(Matrix<double> input, bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
            int resolutionX = input.ColumnCount;
            int resolutionY = input.RowCount;
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            Matrix<double> output = Matrix<double>.Build.Dense(resolutionY, resolutionX);
            for (int xxx = 0; xxx < resolutionX; xxx++)
            {
                int tempx = InterlaceCoordinate(resolutionX, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < resolutionY; yyy++)
                {
                    int tempy = InterlaceCoordinate(resolutionY, UniqueSenselsY, yyy);
                    output[tempy, tempx] = input[yyy, xxx];
                }

            }
            return output;
        }
        public Matrix<double> TransposeArray(Matrix<double> input)
        {
            return input.Transpose();
        }
        public void SwapSides(Matrix<double> input1, Matrix<double> input2)
        {
            Matrix<double> temp;
            temp = input1;
            input1 = input2;
            input2 = temp;
        }
        public Matrix<double> SliceBlock(Matrix<double> input, int slicesX, int slicesY, int x, int y)
        {
            int sliceWidth = input.ColumnCount / slicesX;
            int sliceHeight = input.RowCount / slicesY;
            Matrix<double> output = Matrix<double>.Build.Dense(sliceHeight, sliceWidth);

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    int debugX = x * sliceWidth + xxx;
                    int debugY = y * sliceHeight + yyy;
                    output[yyy, xxx] = input[debugY, debugX];
                }
            }
            return output;
        }

        public Matrix<double> ModifyBlock(Matrix<double> input, int x, int y, Matrix<double> blockData)
        {
            int sliceWidth = blockData.ColumnCount;
            int sliceHeight = blockData.RowCount;

            Matrix<double> output = input;

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    output[y * sliceHeight + yyy, x * sliceWidth + xxx] = blockData[yyy, xxx];
                }
            }
            return output;
        }
    }
}
