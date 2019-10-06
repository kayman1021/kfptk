using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p00
{
    class BP_Pixel
    {
        public int x;
        public int y;
        public int location;
        public double value;
        public double goodnessOfFit;
        public double[] valueArray;

        public BP_Pixel(int x, int y, ushort value, double goodnessOfFit)
        {
            this.x = x;
            this.y = y;
            this.value = value;
            this.goodnessOfFit = goodnessOfFit;
        }
    }

    class BP_PixelList
    {
        List<List<BP_Pixel>> aasasasas = new List<List<BP_Pixel>>();
        public BP_PixelList(int length)
        {
            for (int i = 0; i < length; i++)
            {
                aasasasas[i] = new List<BP_Pixel>();
            }
        }
    }

    class raw_info
    {
        public raw_info()
        {

        }
    }



    class VideoFrame
    {
        vidf_hdr_t info;
        public int frameStart;
        oMatrix framedata;

        public ushort get16bit(byte[]input)
        {
            return (ushort)((input[0])+(input[1]<<8));
        }
        public uint get32bit(byte[]input)
        {
            return (uint)((input[0]) + (input[1] << 8) +(input[2] << 16) + (input[3] << 24));
        }
        public ulong get64bit(byte[] input)
        {
            return 9999999999;
        }

        public VideoFrame(int start,byte[] byteArray)
        {
            frameStart = start;


            string blockType;
            uint blockSize;
            ulong timestamp;
            uint frameNumber;
            ushort cropPosX;
            ushort cropPosY;
            ushort panPosX;
            ushort panPosY;
            uint frameSpace;
            byte[] rest;

            blockType = System.Text.Encoding.ASCII.GetString(byteArray.Take<byte>(4).ToArray());
            blockSize = get32bit(byteArray.Skip(4).Take(4).ToArray());
            timestamp = get64bit(byteArray.Skip(8).Take(8).ToArray());
            frameNumber = get32bit(byteArray.Skip(16).Take(4).ToArray());
            cropPosX = get16bit(byteArray.Skip(20).Take(2).ToArray());
            cropPosY = get16bit(byteArray.Skip(22).Take(2).ToArray());
            panPosX = get16bit(byteArray.Skip(24).Take(2).ToArray());
            panPosY = get16bit(byteArray.Skip(26).Take(2).ToArray());
            frameSpace = get16bit(byteArray.Skip(28).Take(4).ToArray());
            rest = byteArray.Skip(32).ToArray();

            Console.WriteLine();

        }
    }

    class MLV_DATA
    {
        //string stringData;
        List<MLV> mlvData;
        List<VideoFrame> frames;
        byte[] bytebuffer;

        public MLV_DATA(string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                mlvData = new List<MLV>();
                frames = new List<VideoFrame>();
                bytebuffer = reader.ReadBytes((int)(new System.IO.FileInfo(filename).Length));
                //stringData = System.Text.Encoding.ASCII.GetString(bytebuffer);
                bbbb();
                Console.WriteLine();
            }
        }

        public uint get32bit(byte[] input)
        {
            return (uint)((input[0]) + (input[1] << 8) + (input[2] << 16) + (input[3] << 24));
        }

        public void ExtractData(int i)
        {

        }

        public void bbbb()
        {
            for (int i = 0; i < bytebuffer.Length-4; i++)
            {
                switch (System.Text.Encoding.ASCII.GetString(bytebuffer.Skip(i).Take(4).ToArray()))
                {
                    case "MLVI":
                        mlvData.Add(new file_hdr_t());
                        break;

                    case "VIDF":
                        mlvData.Add(new vidf_hdr_t());
                        int start =  i;
                        byte[] lengthInByte = bytebuffer.Skip(start).Take(4).ToArray();
                        uint lengthInUint = get32bit(lengthInByte);
                        frames.Add(new VideoFrame(start,bytebuffer.Skip(start).Take((int)lengthInUint).ToArray()));
                        break;
                    case "AUDF":
                        mlvData.Add(new audf_hdr_t());
                        break;
                    case "RAWI":
                        mlvData.Add(new rawi_hdr_t());
                        break;
                    case "WAVI":
                        mlvData.Add(new wavi_hdr_t());
                        break;
                    case "EXPO":
                        mlvData.Add(new expo_hdr_t());
                        break;
                    case "LENS":
                        mlvData.Add(new lens_hdr_t());
                        break;
                    case "RTCI":
                        mlvData.Add(new rtci_hdr_t());
                        break;
                    case "IDNT":
                        mlvData.Add(new idnt_hdr_t());
                        break;
                    case "XREF":
                        mlvData.Add(new xref_hdr_t());
                        break;
                    case "INFO":
                        mlvData.Add(new info_hdr_t());
                        break;
                    case "DISO":
                        mlvData.Add(new diso_hdr_t());
                        break;
                    case "MARK":
                        mlvData.Add(new mark_hdr_t());
                        break;
                    case "STYL":
                        mlvData.Add(new styl_hdr_t());
                        break;
                    case "ELVL":
                        mlvData.Add(new elvl_hdr_t());
                        break;
                    case "WBAL":
                        mlvData.Add(new wbal_hdr_t());
                        break;
                    case "DEBG":
                        mlvData.Add(new debg_hdr_t());
                        break;
                        //default:
                       // break;
                }
            }
            Console.WriteLine();
        }
    }


    class MLV
    {

    }

    class xref_t
    {
        ushort fileNumber;          /* the logical file number as specified in header */
        byte empty;                 /* for future use. set to zero. */
        byte frameType;             /* 1 for VIDF, 2 for AUDF, 0 otherwise */
        ulong frameOffset;          /* the file offset at which the frame is stored (VIDF/AUDF) */
        public xref_t()
        {

        }
    }

    class hdr_t : MLV
    {
        string blockType;           /*[4]*/
        uint blockSize;
        ulong timestamp;
        public hdr_t()
        {

        }
    }

    class file_hdr_t : MLV
    {
        public string fileMagic;           /* [4] Magic Lantern Video file header */
        public uint blockSize;             /* size of the whole header */
        public string versionString;       /* [8] null-terminated C-string of the exact revision of this format */
        public ulong fileGuid;             /* UID of the file (group) generated using hw counter, time of day and PRNG */
        public ushort fileNum;             /* the ID within fileCount this file has (0 to fileCount-1) */
        public ushort fileCount;           /* how many files belong to this group (splitting or parallel) */
        public uint fileFlags;             /* 1=out-of-order data, 2=dropped frames, 4=single image mode, 8=stopped due to error */
        public ushort videoClass;          /* 0=none, 1=RAW, 2=YUV, 3=JPEG, 4=H.264 */
        public ushort audioClass;          /* 0=none, 1=WAV */
        public uint videoFrameCount;       /* number of video frames in this file. set to 0 on start, updated when finished. */
        public uint audioFrameCount;       /* number of audio frames in this file. set to 0 on start, updated when finished. */
        public uint sourceFpsNom;          /* configured fps in 1/s multiplied by sourceFpsDenom */
        public uint sourceFpsDenom;        /* denominator for fps. usually set to 1000, but may be 1001 for NTSC */
        public file_hdr_t()
        {
            fileMagic = "VIDF";
        }
    }

    class vidf_hdr_t : MLV
    {
        string blockType;           /* [4] this block contains one frame of video data */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        uint frameNumber;           /* unique video frame number */
        ushort cropPosX;            /* specifies from which sensor row/col the video frame was copied (8x2 blocks) */
        ushort cropPosY;            /* (can be used to process dead/hot pixels) */
        ushort panPosX;             /* specifies the panning offset which is cropPos, but with higher resolution (1x1 blocks) */
        ushort panPosY;             /* (it's the frame area from sensor the user wants to see) */
        uint frameSpace;            /* size of dummy data before frameData starts, necessary for EDMAC alignment */
                                    /* uint8_t     frameData[variable]; */
        public vidf_hdr_t()
        {

        }
    }

    class audf_hdr_t : MLV
    {
        string blockType;           /* [4] this block contains audio data */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        uint frameNumber;           /* unique audio frame number */
        uint frameSpace;            /* size of dummy data before frameData starts, necessary for EDMAC alignment */
                                    /* uint8_t     frameData[variable]; */
        public audf_hdr_t()
        {

        }
    }

    class rawi_hdr_t : MLV
    {
        string blockType;           /* [4] when videoClass is RAW, this block will contain detailed format information */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        ushort xRes;                /* Configured video resolution, may differ from payload resolution */
        ushort yRes;                /* Configured video resolution, may differ from payload resolution */
        raw_info raw_info;          /* the raw_info structure delivered by raw.c of ML Core */
        public rawi_hdr_t()
        {
            raw_info = new raw_info();
        }
    }

    class wavi_hdr_t : MLV
    {
        string blockType;           /* [4] when audioClass is WAV, this block contains format details  compatible to RIFF */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        ushort format;              /* 1=Integer PCM, 6=alaw, 7=mulaw */
        ushort channels;            /* audio channel count: 1=mono, 2=stereo */
        uint samplingRate;          /* audio sampling rate in 1/s */
        uint bytesPerSecond;        /* audio data rate */
        ushort blockAlign;          /* see RIFF WAV hdr description */
        ushort bitsPerSample;       /* audio ADC resolution */
        public wavi_hdr_t()
        {

        }
    }

    class expo_hdr_t : MLV
    {
        string blockType;           /* [4] */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        uint isoMode;               /* 0=manual, 1=auto */
        uint isoValue;              /* camera delivered ISO value */
        uint isoAnalog;             /* ISO obtained by hardware amplification (most full-stop ISOs, except extreme values) */
        uint digitalGain;           /* digital ISO gain (1024 = 1 EV) - it's not baked in the raw data, so you may want to scale it or adjust the white level */
        ulong shutterValue;         /* exposure time in microseconds */
        public expo_hdr_t()
        {

        }
    }

    class lens_hdr_t : MLV
    {
        string blockType;           /* [4] */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        ushort focalLength;         /* in mm */
        ushort focalDist;           /* in mm (65535 = infinite) */
        ushort aperture;            /* f-number * 100 */
        byte stabilizerMode;        /* 0=off, 1=on, (is the new L mode relevant) */
        byte autofocusMode;         /* 0=off, 1=on */
        uint flags;                 /* 1=CA avail, 2=Vign avail, ... */
        uint lensID;                /* hexadecimal lens ID (delivered by properties?) */
        string lensName;            /* [32] full lens string */
        string lensSerial;          /* [32] full lens serial number */
        public lens_hdr_t()
        {

        }
    }

    class rtci_hdr_t : MLV
    {
        string blockType;
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        ushort tm_sec;              /* seconds (0-59) */
        ushort tm_min;              /* minute (0-59) */
        ushort tm_hour;             /* hour (0-23) */
        ushort tm_mday;             /* day of month (1-31) */
        ushort tm_mon;              /* month (0-11) */
        ushort tm_year;             /* year since 1900 */
        ushort tm_wday;             /* day of week */
        ushort tm_yday;             /* day of year */
        ushort tm_isdst;            /* daylight saving */
        ushort tm_gmtoff;           /* GMT offset */
        string tm_zone;             /* [8] time zone string */
        public rtci_hdr_t()
        {

        }
    }

    class idnt_hdr_t : MLV
    {
        string blockType;           /* [4] */
        uint blockSize;             /* total frame size */
        ulong timestamp;            /* hardware counter timestamp for this frame (relative to recording start) */
        string cameraName;          /* [32] PROP (0x00000002), offset 0, length 32 */
        uint cameraModel;           /* PROP (0x00000002), offset 32, length 4 */
        string cameraSerial;        /* [32] Camera serial number (if available) */
        public idnt_hdr_t()
        {

        }
    }

    class xref_hdr_t : MLV
    {
        string blockType;           /* [4] can be added in post processing when out of order data is present */
        uint blockSize;             /* this can also be placed in a separate file with only file header plus this block */
        ulong timestamp;
        uint frameType;             /* bitmask: 1=video, 2=audio */
        uint entryCount;            /* number of xrefs that follow here */
        xref_t xrefEntries;         /* this structure refers to the n'th video/audio frame offset in the files */
        public xref_hdr_t()
        {

        }
    }

    class info_hdr_t : MLV
    {
        string blockType;           /* [4] user definable info string. take number, location, etc. */
        uint blockSize;
        ulong timestamp;
        string stringData;          /**/
        public info_hdr_t()
        {

        }
    }
    class diso_hdr_t : MLV
    {
        string blockType;           /* [4] Dual-ISO information */
        uint blockSize;
        ulong timestamp;
        uint dualMode;              /* bitmask: 0=off, 1=odd lines, 2=even lines, upper bits may be defined later */
        uint isoValue;
        public diso_hdr_t()
        {

        }
    }

    class mark_hdr_t : MLV
    {
        string blockType;           /* [4] markers set by user while recording */
        uint blockSize;
        ulong timestamp;
        uint type;                  /* value may depend on the button being pressed or counts up (t.b.d) */
        public mark_hdr_t()
        {

        }
    }

    class styl_hdr_t : MLV
    {
        string blockType;           /* [4] */
        uint blockSize;
        ulong timestamp;
        uint picStyleId;
        int contrast;
        int sharpness;
        int saturation;
        int colortone;
        string picStyleName;        /* [16] */
        public styl_hdr_t()
        {

        }
    }

    class elvl_hdr_t : MLV
    {
        string blockType;           /* [4] Electronic level (orientation) data */
        uint blockSize;
        ulong timestamp;
        uint roll;                  /* degrees x100 (here, 45.00 degrees) */
        uint pitch;                 /* 10.00 degrees */
        public elvl_hdr_t()
        {

        }
    }

    class wbal_hdr_t : MLV
    {
        string blockType;           /* [4] White balance info */
        uint blockSize;
        ulong timestamp;
        uint wb_mode;               /* WB_AUTO 0, WB_SUNNY 1, WB_SHADE 8, WB_CLOUDY 2, WB_TUNGSTEN 3, WB_FLUORESCENT 4, WB_FLASH 5, WB_CUSTOM 6, WB_KELVIN 9 */
        uint kelvin;                /* only when wb_mode is WB_KELVIN */
        uint wbgain_r;              /* only when wb_mode is WB_CUSTOM */
        uint wbgain_g;              /* 1024 = 1.0 */
        uint wbgain_b;              /* note: it's 1/canon_gain (uses dcraw convention) */
        uint wbs_gm;                /* WBShift (no idea how to use these in post) */
        uint wbs_ba;                /* range: -9...9 */
        public wbal_hdr_t()
        {

        }
    }

    class debg_hdr_t : MLV
    {
        string blockType;           /* [4] DEBG - debug messages for development use, contains no production data */
        uint blockSize;
        ulong timestamp;
        uint type;                  /* debug data type, for now 0 - text log */
        uint length;                /* data can be of arbitrary length and blocks are padded to 32 bits, so store real length */
        string stringData;          /* uint8_t     stringData[variable]; */
        public debg_hdr_t()
        {

        }
    }
}