namespace p00
{
    partial class RawData
    {
        public int[,] DeinterlaceUniversal(int[,] input,bool IsDualISO)
        {
            
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }

            int resolutionX = input.GetLength(0);
            int resolutionY = input.GetLength(1);
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            int[,] output = new int[resolutionX,resolutionY];
            for (int xxx = 0; xxx < resolutionX; xxx++)
            {
                for (int yyy = 0; yyy < resolutionY; yyy++)
                {
                    output[xxx, ((blockSizeY * (yyy % UniqueSenselsY)) + yyy / UniqueSenselsY)] = input[xxx, yyy];
                }

            }
            output = Deinterlace(output, true);
            return output;
        }



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
                    int debugX = x * sliceWidth + xxx;
                    int debugY = y * sliceHeight + yyy;
                    output[xxx, yyy] = input[debugX,debugY];
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
