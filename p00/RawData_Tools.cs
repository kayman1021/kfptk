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
        public int[,] Deinterlace(int[,] input, bool direction)
        {
            int[,] output;
            int width;
            int height;
            int halfres;
            if (direction) { input = TransposeArray(input); }
            width = input.GetLength(0);
            height = input.GetLength(1);
            output = new int[width, height];
            halfres = height / 2;

            int temp;
            for (int yyy = 0; yyy < height; yyy++)
            {
                temp = yyy / 2;
                if (yyy % 2 == 1) { temp += halfres; }
                for (int xxx = 0; xxx < width; xxx++) { output[xxx, temp] = input[xxx, yyy]; }
            }

            if (direction) { output = TransposeArray(output); }

            return output;
        }

        public int[,] Interlace(int[,] input, bool direction)
        {
            int[,] output;
            int width;
            int height;
            int halfres;
            if (direction) { input = TransposeArray(input); }
            width = input.GetLength(0);
            height = input.GetLength(1);
            output = new int[width, height];
            halfres = height / 2;

            int temp;
            for (int yyy = 0; yyy < height; yyy++)
            {
                if (yyy < halfres) { temp = yyy * 2; }
                else { temp = (yyy - halfres) * 2 + 1; }
                for (int xxx = 0; xxx < width; xxx++) { output[xxx, temp] = input[xxx, yyy]; }
            }

            if (direction) { output = TransposeArray(output); }

            return output;
        }

        public int[,] TransposeArray(int[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int[,] output = new int[height, width];
            for (int yyy = 0; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[yyy, xxx] = input[xxx, yyy];
                }
            }
            return output;
        }

        public int[,] SeperateDualISO(int[,] input)
        {
            int[,] output;
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            output = new int[width, height];
            int halfres = height / 2;
            int temp;
            for (int yyy = 0; yyy < height; yyy++)
            {
                temp = (yyy / 2) + (yyy % 2);
                if (yyy % 4 >= 2)
                {
                    temp += halfres - 1;
                }
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, temp] = input[xxx, yyy];
                }
            }
            return output;
        }

        public int[,] DeinterlaceDualISO(int[,] input)
        {
            int[,] output = new int[input.GetLength(0), input.GetLength(1)];
            for (int xxx = 0; xxx < input.GetLength(0); xxx++)
            {
                for (int yyy = 0; yyy < input.GetLength(1); yyy++)
                {
                    output[xxx, (((input.GetLength(1) / 4) * (yyy % 4)) + yyy / 4)] = input[xxx, yyy];
                }

            }
            output = Deinterlace(output, true);
            return output;
        }

        public int[,] InterlaceDualISO(int[,] input)
        {
            int[,] output;
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            output = new int[width, height];
            //int halfres = height / 2;
            int slices = 4;
            int temp;
            for (int xxx = 0; xxx < width; xxx++)
            {

                for (int yyy = 0; yyy < height; yyy++)
                {
                    temp = ((yyy % (height / slices)) * slices) + (yyy / (height / slices));
                    output[xxx, temp] = input[xxx, yyy];
                }
            }
            output = Interlace(output, true);
            return output;
        }



        public int[,] SplitTop(int[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int halfres = height / 2;
            int[,] output = new int[width, halfres];

            for (int yyy = 0; yyy < halfres; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, yyy] = input[xxx, yyy];
                }
            }
            return output;
        }

        public int[,] SplitBottom(int[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            int halfres = height / 2;
            int[,] output = new int[width, halfres];

            for (int yyy = halfres; yyy < height; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, yyy - halfres] = input[xxx, yyy];
                }
            }
            return output;
        }

        public int[,] JoinHalves(int[,] top, int[,] bottom)
        {
            int width = top.GetLength(0);
            int height = top.GetLength(1) * 2;
            int halfres = top.GetLength(1);
            int[,] output = new int[width, height];

            for (int yyy = 0; yyy < halfres; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    output[xxx, yyy] = top[xxx, yyy];
                }
            }
            for (int yyy = 0; yyy < halfres; yyy++)
            {
                for (int xxx = 0; xxx < width; xxx++)
                {
                    int temp = yyy + halfres;
                    output[xxx, temp] = bottom[xxx, yyy];
                }
            }


            return output;
        }



        public void SwapSides(int[,] input1, int[,] input2)
        {
            int[,] temp;
            temp = input1;
            input1 = input2;
            input2 = temp;
        }

        public int[,] SliceBlock(int[,] input,int slicesX,int slicesY, int x, int y)
        {
            int sliceWidth = input.GetLength(0) / slicesX;
            int sliceHeight = input.GetLength(1) / slicesY;
            int[,] output = new int[sliceWidth, sliceHeight];

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    output[xxx, yyy] = input[x * sliceWidth + xxx, y * sliceHeight + yyy];
                }
            }
            return output;
        }

        public int[,] ModifyBlock(int[,] input, int x, int y,int[,]blockData)
        {
            int sliceWidth = blockData.GetLength(0);
            int sliceHeight = blockData.GetLength(1);

            int[,] output = input;

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    output[x*sliceWidth+xxx,y*sliceHeight+yyy]= blockData[xxx, yyy];
                }
            }
            return output;
        }




    }
}
