using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace p00
{
    partial class BP_Data
    {
        public int DeinterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((arraylength / sensels) * (input % sensels)) + (input / sensels);
        }
        public Matrix<ushort> DeinterlaceUniversal(Matrix<ushort> input, bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
            int resolutionX = input.ColumnCount;
            int resolutionY = input.RowCount;
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            Matrix<ushort> output =Matrix<ushort>.Build.Dense(resolutionX, resolutionY);
            for (int xxx = 0; xxx < resolutionX; xxx++)
            {
                int tempx = DeinterlaceCoordinate(resolutionX, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < resolutionY; yyy++)
                {
                    int tempy = DeinterlaceCoordinate(resolutionY, UniqueSenselsY, yyy);
                    output[tempx, tempy] = (ushort)input[xxx, yyy];
                }

            }
            return output;
        }
        public int InterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((input % (arraylength / sensels)) * sensels) + (input / (arraylength / sensels));
        }
        public Matrix<ushort> InterlaceUniversal(Matrix<ushort> input, bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
            int resolutionX = input.ColumnCount;
            int resolutionY = input.RowCount;
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            Matrix<ushort> output =Matrix<ushort>.Build.Dense(resolutionX, resolutionY);
            for (int xxx = 0; xxx < resolutionX; xxx++)
            {
                int tempx = InterlaceCoordinate(resolutionX, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < resolutionY; yyy++)
                {
                    int tempy = InterlaceCoordinate(resolutionY, UniqueSenselsY, yyy);
                    output[tempx, tempy] = input[xxx, yyy];
                }

            }
            return output;
        }
        public Matrix<ushort> TransposeArray(Matrix<ushort> input)
        {
            return input.Transpose();
        }
        public void SwapSides(Matrix<ushort> input1, Matrix<ushort> input2)
        {
            Matrix<ushort> temp;
            temp = input1;
            input1 = input2;
            input2 = temp;
        }
        public Matrix<ushort> SliceBlock(Matrix<ushort> input, int slicesX, int slicesY, int x, int y)
        {
            int sliceWidth = input.ColumnCount / slicesX;
            int sliceHeight = input.RowCount / slicesY;
            Matrix<ushort> output = Matrix<ushort>.Build.Dense( sliceWidth, sliceHeight);

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    int debugX = x * sliceWidth + xxx;
                    int debugY = y * sliceHeight + yyy;
                    output[xxx, yyy] = input[debugX, debugY];
                }
            }
            return output;
        }
        public Matrix<ushort> ModifyBlock(Matrix<ushort> input, int x, int y, Matrix<ushort> blockData)
        {
            int sliceWidth = blockData.ColumnCount;
            int sliceHeight = blockData.RowCount;

            Matrix<ushort> output = input;

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    output[x * sliceWidth + xxx, y * sliceHeight + yyy] = blockData[xxx, yyy];
                }
            }
            return output;
        }

        public ushort[] copyRow(Matrix<ushort> input, int rowNumber)
        {
            ushort[] output = new ushort[input.ColumnCount];
            int rowLength = output.Length;
            for (int i = 0; i < rowLength; i++)
            {
                output[i] = input[i, rowNumber];
            }
            return output;
        }

        public ushort[] copyColumn(Matrix<ushort> input, int columnNumber)
        {
            ushort[] output = new ushort[input.RowCount];
            int columnLength = output.Length;
            for (int i = 0; i < columnLength; i++)
            {
                output[i] = input[columnLength, i];
            }
            return output;
        }
    }
}
