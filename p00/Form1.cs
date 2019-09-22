using System;
using System.Windows.Forms;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace p00
{
    public partial class Form1 : Form
    {

        //RawData rwt = new RawData();
        BP_Data rrwwtt = new BP_Data();


        public Form1()
        {
            InitializeComponent();
            comboBox_Import_DNG_Select.Items.Add(DngFileType.MLVApp14bit);
            comboBox_Import_DNG_Select.Items.Add(DngFileType.Xiaomi16bit);
            comboBox_Import_DNG_Select.SelectedIndex = 0;
        }

        private void button_Import_DNG_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "DNG file|*.dng";
                if (open.ShowDialog() == DialogResult.OK) { textBox_Import_DNG_Text.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_Import_DNG_Import_Click(object sender, EventArgs e)
        {
            if ((DngFileType)comboBox_Import_DNG_Select.SelectedItem == DngFileType.MLVApp14bit)
            {
                //rwt.rawData = rwt.ImportRawData14bitUncompressed(textBox_Import_DNG_Text.Text);
                rrwwtt.Left = rrwwtt.ImportRawData14bitUncompressed(textBox_Import_DNG_Text.Text);
            }
            if ((DngFileType)comboBox_Import_DNG_Select.SelectedItem == DngFileType.Xiaomi16bit)
            {
                //rwt.rawData = rwt.ImportRawDataXiaomi(textBox_Import_DNG_Text.Text);
                rrwwtt.Left = rrwwtt.ImportRawDataXiaomi(textBox_Import_DNG_Text.Text);
            }
        }
        private void button_Import_FPM_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "FPM file|*.fpm";
                if (open.ShowDialog() == DialogResult.OK) { textBox_Import_FPM_Text.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_Import_FPM_Import_Click(object sender, EventArgs e)
        {
            //rwt.mapData = rwt.ImportPixelMapFromFPM(textBox_Import_FPM_Text.Text, rwt.rawData);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////    PUSH ONLY DIMENSIONS? NOT THE WHOLE MATRIX!
            rrwwtt.Left = rrwwtt.ImportPixelMapFromFPM(textBox_Export_FPM_Text.Text, rrwwtt.Left);
        }
        private void button_Import_TIFF_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "DNG file|*.dng";
                if (open.ShowDialog() == DialogResult.OK) { textBox_Import_TIFF_Text.Text = open.FileName; }
            }
            catch (Exception) { throw new ApplicationException("Failed loading file"); }
        }
        private void button_Import_TIFF_Import_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.ImportRawDataTiff(textBox_Import_TIFF_Text.Text);
            rrwwtt.Left = rrwwtt.ImportRawDataTiff(textBox_Export_TIFF_Text.Text);
        }
        private void button_Import_Mapped_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif; *.tiff;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.tif; *.tiff";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    textBox_Import_Mapped_Text.Text = open.FileName;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading file");
            }
        }
        private void button_Import_Mapped_Import_Click(object sender, EventArgs e)
        {
            //rwt.mapData = rwt.ImportPixelMapFromPicture(textBox_Import_Mapped_Text.Text);
            rrwwtt.Right = rrwwtt.ImportPixelMapFromPicture(textBox_Import_Mapped_Text.Text);
            //Console.WriteLine();
        }
        private void button_Tools_Deinterlace_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, false);
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, false);
        }
        private void button_Tools_DeinterlaceDualISO_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, true);
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, true);
            //rwt.makeSplineHorizontal();
            //rwt.makeSplineVertical();
            //Console.WriteLine();
        }
        private void button_Tools_Transpose_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.TransposeArray(rwt.rawData);
            rrwwtt.Left = rrwwtt.TransposeArray(rrwwtt.Left);
        }
        private void button_Tools_Interlace_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.InterlaceUniversal(rwt.rawData, false);
            rrwwtt.Left = rrwwtt.InterlaceUniversal(rrwwtt.Left, false);
        }
        private void button_Tools_InterlaceDualISO_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.InterlaceUniversal(rwt.rawData, true);
            rrwwtt.Left = rrwwtt.InterlaceUniversal(rrwwtt.Left, true);
        }
        private void button_Tools_SwapSides_Click(object sender, EventArgs e)
        {
            //int[,] temp;
            //temp = rwt.rawData;
            //rwt.rawData = rwt.mapData;
            //rwt.mapData = temp;
            //rrwwtt.SwapSides(rrwwtt.Left, rrwwtt.Right);
            Matrix<double> temp = Matrix<double>.Build.Dense(rrwwtt.Left.RowCount, rrwwtt.Left.ColumnCount);
            temp = rrwwtt.Left;
            rrwwtt.Left = rrwwtt.Right;
            rrwwtt.Right = temp;
        }
        private void button_Export_TIFF_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "tiff file|*.tiff";
                save.ShowDialog();
                if (save.FileName != "") { textBox_Export_TIFF_Text.Text = save.FileName; }
            }
            catch (Exception) { throw new ApplicationException(""); }
        }
        private void button_Export_TIFF_Export_Click(object sender, EventArgs e)
        {
            //rwt.ExportRawDataTiff(@textBox_Export_TIFF_Text.Text, rwt.rawData);
            rrwwtt.ExportRawDataTiff(@textBox_Export_TIFF_Text.Text, rrwwtt.Left);
        }
        private void button_Export_FPM_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "fpm file|*.fpm";
                save.ShowDialog();
                if (save.FileName != "") { textBox_Export_FPM_Text.Text = save.FileName; }
            }
            catch (Exception) { throw new ApplicationException(""); }
        }
        private void button_Export_FPM_Export_Click(object sender, EventArgs e)
        {
            //rwt.ExportFPM(@textBox_Export_FPM_Text.Text, rwt.mapData);
            rrwwtt.ExportFPM(@textBox_Export_FPM_Text.Text, rrwwtt.Left);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*//rwt.ModifyBlock(rwt.rawData, 0, 0, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 2, 0, 0));
            //rwt.ModifyBlock(rwt.rawData, 1, 1, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 2, 1, 1));
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.CorrectArea(rrwwtt.Left, rrwwtt.Right, 2, 2, 0, 0));
            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 1, rrwwtt.CorrectArea(rrwwtt.Left, rrwwtt.Right, 2, 2, 1, 1));*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*rwt.ModifyBlock(rwt.rawData, 0, 0, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 0, 0));
            rwt.ModifyBlock(rwt.rawData, 1, 1, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 1, 1));
            rwt.ModifyBlock(rwt.rawData, 0, 2, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 0, 2));
            rwt.ModifyBlock(rwt.rawData, 1, 3, rwt.CorrectArea(rwt.rawData, rwt.mapData, 2, 4, 1, 3));*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, true);
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //rwt.rawData = rwt.DeinterlaceUniversal(rwt.rawData, false);
            rrwwtt.Left = rrwwtt.DeinterlaceUniversal(rrwwtt.Left, false);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //rwt.ExportRawDataXiaomi(rwt.rawData,@textBox_Import_DNG_Text.Text);
            rrwwtt.ExportRawDataXiaomi(rrwwtt.Left, @textBox_Import_DNG_Text.Text);
        }

        private void button_EXP_EOS_Click(object sender, EventArgs e)
        {
            //rwt.ExportRawData14bitUncompressed(rwt.rawData, textBox_Import_DNG_Text.Text);
            rrwwtt.ExportRawData14bitUncompressed(rrwwtt.Left, @textBox_Import_DNG_Text.Text);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            System.IO.Stream myStream;
            OpenFileDialog thisDialog = new OpenFileDialog();

            //thisDialog.InitialDirectory = "d:\\";
            thisDialog.Filter = "All files (*.*)|*.*";
            thisDialog.FilterIndex = 2;
            thisDialog.RestoreDirectory = true;
            thisDialog.Multiselect = true;
            thisDialog.Title = "Please Select Source File(s) for Conversion";

            if (thisDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in thisDialog.FileNames)
                {
                    try
                    {
                        if ((myStream = thisDialog.OpenFile()) != null)
                        {
                            using (myStream)
                            {
                                listBox_MASS_DUAL_ISO.Items.Add(file);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }

                foreach (string path in listBox_MASS_DUAL_ISO.Items)
                {
                    // RawData mass = new RawData();
                    // mass.mapData = rwt.mapData;
                    // mass.rawData = mass.ImportRawData14bitUncompressed(@path);
                    //mass.rawData = mass.DeinterlaceUniversal(mass.rawData, true);
                    //mass.ModifyBlock(mass.rawData, 0, 0, mass.CorrectArea2(mass.rawData, mass.mapData, 2, 4, 0, 0));
                    //mass.ModifyBlock(mass.rawData, 1, 1, mass.CorrectArea2(mass.rawData, mass.mapData, 2, 4, 1, 1));
                    //mass.ModifyBlock(mass.rawData, 0, 2, mass.CorrectArea2(mass.rawData, mass.mapData, 2, 4, 0, 2));
                    //mass.ModifyBlock(mass.rawData, 1, 3, mass.CorrectArea2(mass.rawData, mass.mapData, 2, 4, 1, 3));
                    // mass.rawData = mass.InterlaceUniversal(mass.rawData, true);
                    // mass.ExportRawData14bitUncompressed(mass.rawData, @path);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //rwt.FindValueAtDegree(rwt.SliceBlock(rwt.rawData,2,4,0,0),rwt.SliceBlock(rwt.mapData,2,4,0,0),(double)numericUpDown_angle.Value,3,0);
        }

        private void button_correctVertical_Click(object sender, EventArgs e)
        {
            /*//int width = rwt.rawData.GetLength(0) / 2;
            //int height = rwt.rawData.GetLength(1) / 4;
            Point[] corruptZones = new Point[4] {
                new Point(0,0),
                new Point(1,1),
                new Point(0,2),
                new Point(1,3)
            };

            int[,] data;
            int[,] map;
            Point zone;
            CubicSpline[] cs;

            for (int i = 0; i < 4; i++)
            {
                zone = corruptZones[i];
                data = rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, zone.X,zone.Y);
                map = rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, zone.X, zone.Y);
                cs = rrwwtt.InterpolateColumn(data, map);
                for (int xxx = 0; xxx < width; xxx++)
                {
                    for (int yyy = 0; yyy < height; yyy++)
                    {
                        if (map[xxx, yyy] == 0)
                        {
                            data[xxx, yyy] = (int)rwt.interpolateFromColumn(xxx, yyy, cs);
                        }
                    }
                }
                rwt.ModifyBlock(rwt.rawData, zone.X,zone.Y, data);
            }
        */
        }

        private void button_correctHorizontal_Click(object sender, EventArgs e)
        {/*
            int width = rwt.rawData.GetLength(0) / 2;
            int height = rwt.rawData.GetLength(1) / 4;
            Point[] corruptZones = new Point[4] {
                new Point(0,0),
                new Point(1,1),
                new Point(0,2),
                new Point(1,3)
            };

            int[,] data;
            int[,] map;
            Point zone;
            CubicSpline[] cs;

            for (int i = 0; i < 4; i++)
            {
                zone = corruptZones[i];
                data = rwt.SliceBlock(rwt.rawData, 2, 4, zone.X, zone.Y);
                map = rwt.SliceBlock(rwt.mapData, 2, 4, zone.X, zone.Y);
                cs = rwt.InterpolateRow(data, map);
                for (int yyy = 0; yyy < height; yyy++)
                {
                    for (int xxx = 0; xxx < width; xxx++)
                    {
                        if (map[xxx, yyy] == 0)
                        {
                            data[xxx, yyy] = (int)rwt.interpolateFromRow(xxx, yyy, cs);
                        }
                    }
                }
                rwt.ModifyBlock(rwt.rawData, zone.X, zone.Y, data);
            }
        */
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // rwt.CA(rwt.rawData, rwt.mapData, 2, 4, 0, 0);

            /*
                        rwt.ModifyBlock(rwt.rawData, 0, 0, rwt.CA(rwt.rawData, rwt.mapData, 2, 4, 0, 0));
                        rwt.ModifyBlock(rwt.rawData, 1, 1, rwt.CA(rwt.rawData, rwt.mapData, 2, 4, 1, 1));
                        rwt.ModifyBlock(rwt.rawData, 0, 2, rwt.CA(rwt.rawData, rwt.mapData, 2, 4, 0, 2));
                        rwt.ModifyBlock(rwt.rawData, 1, 3, rwt.CA(rwt.rawData, rwt.mapData, 2, 4, 1, 3));*/
        }

        private void button8_Click(object sender, EventArgs e)
        {

            rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 3);
            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 1, 3);
            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 1, 3);

            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 2, 3);
            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 2, 3);
            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 2, 3);

            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 3, 3);
            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 3, 3);
            //rrwwtt.Left = rrwwtt.ggg(rrwwtt.Left, rrwwtt.Right, 3, 3);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rrwwtt.Left = rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0);
            rrwwtt.Right = rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 0);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int radius = 3;
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 0)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 0, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 0), radius));

            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 1, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 1)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 1, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 1), radius));

            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 2, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 2)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 0, 2, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 0, 2), radius));

            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 3, rrwwtt.Prefit(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 3)));
            rrwwtt.ModifyBlock(rrwwtt.Left, 1, 3, rrwwtt.Collector(rrwwtt.SliceBlock(rrwwtt.Left, 2, 4, 0, 0), rrwwtt.SliceBlock(rrwwtt.Right, 2, 4, 1, 3), radius));

            //rrwwtt.Left = rrwwtt.Prefit(rrwwtt.Left, rrwwtt.Right);
            //rrwwtt.Left = rrwwtt.Collector(rrwwtt.Left, rrwwtt.Right, 3);
            //rrwwtt.Left = rrwwtt.Collector(rrwwtt.Left, rrwwtt.Right, 3);
            //rrwwtt.Left = rrwwtt.Collector(rrwwtt.Left, rrwwtt.Right, 3);
        }
    }
}