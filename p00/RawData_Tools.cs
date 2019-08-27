namespace p00
{
    partial class RawData
    {
        public int DeinterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((arraylength / sensels) * (input % sensels)) + (input / sensels);
        }
        public int[,] DeinterlaceUniversal(int[,] input, bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
            int resolutionX = input.GetLength(0);
            int resolutionY = input.GetLength(1);
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            int[,] output = new int[resolutionX, resolutionY];
            for (int xxx = 0; xxx < resolutionX; xxx++)
            {
                int tempx = DeinterlaceCoordinate(resolutionX, UniqueSenselsX, xxx);
                for (int yyy = 0; yyy < resolutionY; yyy++)
                {
                    int tempy = DeinterlaceCoordinate(resolutionY, UniqueSenselsY, yyy);
                    output[tempx, tempy] = input[xxx, yyy];
                }

            }
            return output;
        }
        public int InterlaceCoordinate(int arraylength, int sensels, int input)
        {
            return ((input % (arraylength / sensels)) * sensels) + (input / (arraylength / sensels));
        }
        public int[,] InterlaceUniversal(int[,] input, bool IsDualISO)
        {
            int UniqueSenselsX = 2;
            int UniqueSenselsY;
            if (IsDualISO) { UniqueSenselsY = 4; }
            else { UniqueSenselsY = 2; }
            int resolutionX = input.GetLength(0);
            int resolutionY = input.GetLength(1);
            int blockSizeX = resolutionX / UniqueSenselsX;
            int blockSizeY = resolutionY / UniqueSenselsY;
            int[,] output = new int[resolutionX, resolutionY];
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
        public void SwapSides(int[,] input1, int[,] input2)
        {
            int[,] temp;
            temp = input1;
            input1 = input2;
            input2 = temp;
        }
        public int[,] SliceBlock(int[,] input, int slicesX, int slicesY, int x, int y)
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
                    output[xxx, yyy] = input[debugX, debugY];
                }
            }
            return output;
        }
        public int[,] ModifyBlock(int[,] input, int x, int y, int[,] blockData)
        {
            int sliceWidth = blockData.GetLength(0);
            int sliceHeight = blockData.GetLength(1);

            int[,] output = input;

            for (int yyy = 0; yyy < sliceHeight; yyy++)
            {
                for (int xxx = 0; xxx < sliceWidth; xxx++)
                {
                    output[x * sliceWidth + xxx, y * sliceHeight + yyy] = blockData[xxx, yyy];
                }
            }
            return output;
        }
    }
}
