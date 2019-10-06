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



        public MLVFILE(string filename)
        {
            int nextField = 0;
            int fileLength;
                using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                fileLength =(int)new System.IO.FileInfo(filename).Length;
                byte[] bytebuffer = reader.ReadBytes(nextField);

            }
        }

        public uint get32bit(byte[] input)
        {
            return (uint)((input[0]) + (input[1] << 8) + (input[2] << 16) + (input[3] << 24));
        }

        void setParams(byte[]input)
        {
            string blockType = System.Text.Encoding.ASCII.GetString(input.Take<byte>(4).ToArray());
            uint blockLength = get32bit(input.Skip(4).ToArray());

            /*switch (blockType)
            {
                case "MLVI":
                    break;
                case "RAWI":
                    break;
                case "RAWC":
                    break;
                case "IDNT":
                    break;
                case "EXPO":
                    break;
                case "LENS":
                    break;
                case "WBAL":
                    break;
                case "RTCI":
                    break;
                case "DISO":
                    break;
                case "VERS":
                    break;
                case "VIDF":
                    break;
                case "NULL":
                    break;
                case MLV_frameTypes.STYL:
                    break;
                case MLV_frameTypes.ELVL:
                    break;
                case MLV_frameTypes.WBAL:
                    break;
                case MLV_frameTypes.DEBG:
                    break;
                default:
                    break;
            }*/
        }
    }

    class StringFinder
    {
        public uint get32bit(byte[] input)
        {
            return (uint)((input[0]) + (input[1] << 8) + (input[2] << 16) + (input[3] << 24));
        }
        public StringFinder(string filename)
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
                    blockName= System.Text.Encoding.ASCII.GetString(reader.ReadBytes(4));
                    blockLength =get32bit( reader.ReadBytes(4));
                    reader.ReadBytes((int)blockLength- 8);
                    results += blockName + "\n";
                    counter += blockLength;
                } while (counter<fileLength - 4);
            }
        }
    }
}
