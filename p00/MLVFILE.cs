using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p00
{
    public enum MLV_frameTypes
    {
        VIDF, AUDF, RAWI, WAVI, EXPO, LENS, RTCI, IDNT, XREF, INFO, DISO, MARK, STYL, ELVL, WBAL, DEBG
    }
    class MLVFILE
    {
        public void StringFinder(string filename)
        {
            string results = "";
            int fileLength;
            string blockName;
            uint blockLength;
            uint blockPosition;

            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                fileLength = (int)new System.IO.FileInfo(filename).Length;
                uint counter = 0;
                do
                {
                    blockPosition = counter;
                    blockName = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4));
                    blockLength = get32bit(reader.ReadBytes(4));
                    reader.ReadBytes((int)blockLength - 8);
                    results += blockName + "\n";
                    counter += blockLength;
                } while (counter < fileLength - 4);
            }
        }

        public uint get32bit(byte[] input)
        {
            return (uint)((input[0]) + (input[1] << 8) + (input[2] << 16) + (input[3] << 24));
        }

        public ushort[] FrameDataToUshort(byte[] input)
        {
            ushort[] output = new ushort[8];

            /*output[0] = (ushort)((input[0] >> 2) + (input[1] * 64));
            output[1] = (ushort)((((input[0] << 6) >> 6) * 4096) + (input[2] >> 4) + (input[3] * 16));
            output[2] = (ushort)((((input[2] << 4) >> 4) * 1024) + (input[4] >> 6) + (input[5] * 4));
            output[3] = (ushort)((((input[4] << 2) >> 2) * 256) + input[7]);

            output[4] = (ushort)((input[6] * 64) + (input[9] >> 2));
            output[5] = (ushort)((input[8] * 16) + (((input[9] << 6) >> 6) * 4096) + (input[11] >> 4));
            output[6] = (ushort)((input[10] * 4) + (((input[11] << 4) >> 4) * 1024) + (input[13] >> 6));
            output[7] = (ushort)((input[12]) + (((input[13] << 2) >> 2) * 256));*/


            output[0] = (ushort)((input[0] >> 2) + (input[1] >> 6));
            output[1] = (ushort)(((input[0] << 6) >> 18) + (input[2] >> 4) + (input[3] >> 4));
            output[2] = (ushort)(((input[2] << 4) >> 14) + (input[4] >> 6) + (input[5] >> 2));
            output[3] = (ushort)(((input[4] << 2) >> 10) + input[7]);

            output[4] = (ushort)((input[6] >> 6) + (input[9] >> 2));
            output[5] = (ushort)((input[8] >> 4) + ((input[9] << 6) >> 18) + (input[11] >> 4));
            output[6] = (ushort)((input[10] >> 2) + ((input[11] << 4) >> 14) + (input[13] >> 6));
            output[7] = (ushort)((input[12]) + ((input[13] << 2) >> 10));

            return output;
        }
    }
}
