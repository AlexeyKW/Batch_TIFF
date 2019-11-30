using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using OSGeo.GDAL;
using OSGeo.OSR;
using ImageMagick;
using System.Data.SQLite;
using MathNet.Numerics.Statistics;
using System.Globalization;

namespace BatchTIFF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            string[] tiff_files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.tif", SearchOption.AllDirectories);
            int files_count = tiff_files.Length;
            List<string> tab_strings;
            var regex = new Regex(Regex.Escape(","));
            foreach (string tiff_file in tiff_files)
            {
                MagickImage image_check = new MagickImage(tiff_file);
                string fullPath_check = Path.GetDirectoryName(tiff_file).TrimEnd(Path.DirectorySeparatorChar);
                string folder_name_check = fullPath_check.Split(Path.DirectorySeparatorChar).Last();
                if (image_check.Depth == 1 || folder_name_check == "Gray_images")
                {

                }
                else
                {
                    tab_strings = new List<string>();
                    string file_path = Path.GetDirectoryName(tiff_file);
                    string file_name = Path.GetFileName(tiff_file);
                    Dataset tiff_image = Gdal.Open(tiff_file, Access.GA_ReadOnly);
                    Driver drv = tiff_image.GetDriver();
                    //MessageBox.Show("Corner Coordinates:");
                    //MessageBox.Show("  Upper Left (" + GDALInfoGetPosition(tiff_image, 0.0, 0.0) + ")"+ "(0.0, 0.0)");
                    //MessageBox.Show("  Lower Right (" + GDALInfoGetPosition(tiff_image, tiff_image.RasterXSize, tiff_image.RasterYSize) + ")" + "(" + tiff_image.RasterXSize.ToString() + ", " + tiff_image.RasterYSize.ToString() + ")");
                    //MessageBox.Show("  Upper Right (" + GDALInfoGetPosition(tiff_image, tiff_image.RasterXSize, 0.0) + ")" + "(" + tiff_image.RasterXSize.ToString() + ", 0.0)");
                    //MessageBox.Show("  Lower Left (" + GDALInfoGetPosition(tiff_image, 0.0, tiff_image.RasterYSize) + ")"+ "(0.0, "+ tiff_image.RasterYSize.ToString() + ")");
                    //MessageBox.Show("  Center (" + GDALInfoGetPosition(tiff_image, tiff_image.RasterXSize / 2, tiff_image.RasterYSize / 2) + ")");
                    tab_strings.Add("!table");
                    tab_strings.Add("!version 300");
                    tab_strings.Add("!charset WindowsCyrillic");
                    tab_strings.Add("");
                    tab_strings.Add("Definition Table");
                    tab_strings.Add("  File \"" + file_name + "\"");
                    tab_strings.Add("  Type \"RASTER\"");
                    string[] coords = GDALInfoGetPosition(tiff_image, 0.0, 0.0).Split(new string[] { ", " }, StringSplitOptions.None);
                    string X = coords[0];
                    string Y = coords[1];
                    if (checkBox1.Checked)
                    {
                        //X = X.Replace(',', '.');
                        double X_float = System.Convert.ToDouble(X);
                        int X_int = System.Convert.ToInt32(X_float);
                        X = X_int.ToString() + ".0";
                        //Y = Y.Replace(',', '.');
                        double Y_float = System.Convert.ToDouble(Y);
                        int Y_int = System.Convert.ToInt32(Y_float);
                        Y = Y_int.ToString() + ".0";
                    }
                    else
                    {
                        X = X.Replace(',', '.');
                        Y = Y.Replace(',', '.');
                    }
                    tab_strings.Add("  (" + X + "," + Y + ") (0,0) Label \"Точка 1\",");

                    coords = GDALInfoGetPosition(tiff_image, 0.0, tiff_image.RasterYSize).Split(new string[] { ", " }, StringSplitOptions.None);
                    X = coords[0];
                    Y = coords[1];
                    if (checkBox1.Checked)
                    {
                        //X = X.Replace(',', '.');
                        double X_float = System.Convert.ToDouble(X);
                        int X_int = System.Convert.ToInt32(X_float);
                        X = X_int.ToString() + ".0";
                        //Y = Y.Replace(',', '.');
                        double Y_float = System.Convert.ToDouble(Y);
                        int Y_int = System.Convert.ToInt32(Y_float);
                        Y = Y_int.ToString() + ".0";
                    }
                    else
                    {
                        X = X.Replace(',', '.');
                        Y = Y.Replace(',', '.');
                    }
                    tab_strings.Add("  (" + X + "," + Y + ") (0," + tiff_image.RasterYSize.ToString() + ")  Label \"Точка 2\",");

                    coords = GDALInfoGetPosition(tiff_image, tiff_image.RasterXSize, tiff_image.RasterYSize).Split(new string[] { ", " }, StringSplitOptions.None);
                    X = coords[0];
                    Y = coords[1];
                    if (checkBox1.Checked)
                    {
                        //X = X.Replace(',', '.');
                        double X_float = System.Convert.ToDouble(X);
                        int X_int = System.Convert.ToInt32(X_float);
                        X = X_int.ToString() + ".0";
                        //Y = Y.Replace(',', '.');
                        double Y_float = System.Convert.ToDouble(Y);
                        int Y_int = System.Convert.ToInt32(Y_float);
                        Y = Y_int.ToString() + ".0";
                    }
                    else
                    {
                        X = X.Replace(',', '.');
                        Y = Y.Replace(',', '.');
                    }
                    tab_strings.Add("  (" + X + "," + Y + ") (" + tiff_image.RasterXSize.ToString() + "," + tiff_image.RasterYSize.ToString() + ") Label \"Точка 3\",");

                    coords = GDALInfoGetPosition(tiff_image, tiff_image.RasterXSize, 0.0).Split(new string[] { ", " }, StringSplitOptions.None);
                    X = coords[0];
                    Y = coords[1];
                    if (checkBox1.Checked)
                    {
                        //X = X.Replace(',', '.');
                        double X_float = System.Convert.ToDouble(X);
                        int X_int = System.Convert.ToInt32(X_float);
                        X = X_int.ToString() + ".0";
                        //Y = Y.Replace(',', '.');
                        double Y_float = System.Convert.ToDouble(Y);
                        int Y_int = System.Convert.ToInt32(Y_float);
                        Y = Y_int.ToString() + ".0";
                    }
                    else
                    {
                        X = X.Replace(',', '.');
                        Y = Y.Replace(',', '.');
                    }
                    tab_strings.Add("  (" + X + "," + Y + ") (" + tiff_image.RasterXSize.ToString() + ",0) Label \"Точка 4\"");
                    tab_strings.Add("  CoordSys NonEarth Units \"m\"");
                    tab_strings.Add("  Units \"m\"");
                    //string projection = tiff_image.GetProjectionRef();
                    //if (projection != null)
                    //{
                    //    SpatialReference srs = new SpatialReference(null);
                    //    if (srs.ImportFromWkt(ref projection) == 0)
                    //    {
                    //        string wkt;
                    //        srs.ExportToPrettyWkt(out wkt, 0);
                    //        MessageBox.Show("Coordinate System is: "+ wkt);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Coordinate System is: "+ projection);
                    //    }
                    //}
                    File.WriteAllLines(Path.GetDirectoryName(tiff_file) + "\\" + Path.GetFileNameWithoutExtension(tiff_file) + ".TAB", tab_strings);

                    if (checkBox2.Checked)
                    {
                        using (MagickImage image = new MagickImage(tiff_file))
                        {
                            string fullPath = Path.GetDirectoryName(tiff_file).TrimEnd(Path.DirectorySeparatorChar);
                            string folder_name = fullPath.Split(Path.DirectorySeparatorChar).Last();
                            if (image.Depth == 1 || folder_name == "Gray_images")
                            {

                            }
                            else
                            {
                                if (Directory.Exists(Path.GetDirectoryName(tiff_file) + "\\Gray_images"))
                                {

                                }
                                else
                                {
                                    //MessageBox.Show("");
                                    Directory.CreateDirectory(Path.GetDirectoryName(tiff_file) + "\\Gray_images");
                                }
                                File.Copy(tiff_file, Path.GetDirectoryName(tiff_file) + "\\Gray_images\\" + Path.GetFileNameWithoutExtension(tiff_file) + "_gray.tif", true);
                                image.ColorType = ColorType.Bilevel;
                                image.Quantize(new QuantizeSettings()
                                {
                                    Colors = 2,
                                    //ColorSpace = ColorSpace.RGB,
                                    DitherMethod = DitherMethod.FloydSteinberg
                                });
                                image.Depth = 1;
                                byte[] result = image.ToByteArray(MagickFormat.Tiff);
                                File.WriteAllBytes(tiff_file, result);
                                tab_strings[5] = "  File \"" + Path.GetFileNameWithoutExtension(tiff_file) + "_gray.tif" + "\"";
                                File.WriteAllLines(Path.GetDirectoryName(tiff_file) + "\\Gray_images\\" + Path.GetFileNameWithoutExtension(tiff_file) + "_gray.TAB", tab_strings);
                            }

                        }
                    }
                }
            }
            MessageBox.Show("Обработка завершена");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                label1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private static string GDALInfoGetPosition(Dataset ds, double x, double y)
        {
            double[] adfGeoTransform = new double[6];
            double dfGeoX, dfGeoY;
            ds.GetGeoTransform(adfGeoTransform);

            dfGeoX = adfGeoTransform[0] + adfGeoTransform[1] * x + adfGeoTransform[2] * y;
            dfGeoY = adfGeoTransform[3] + adfGeoTransform[4] * x + adfGeoTransform[5] * y;

            return dfGeoX.ToString() + ", " + dfGeoY.ToString();
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] sqlite_files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "raster_points.sqlite", SearchOption.AllDirectories);
            int files_count = sqlite_files.Length;
            List<string> msk_strings;
            List<string> msk_strings_panorama;
            List<int> msk_indexes;
            List<double> msk_X;
            List<double> msk_Y;
            List<string> sk42_strings;
            List<string> sk42_strings_panorama;
            List<int> sk42_indexes;
            List<double> sk42_X;
            List<double> sk42_Y;
            List<double> distances;
            foreach (string sqlite_file in sqlite_files)
            {
                msk_strings = new List<string>();
                sk42_strings = new List<string>();
                msk_strings_panorama = new List<string>();
                sk42_strings_panorama = new List<string>();
                msk_indexes = new List<int>();
                sk42_indexes = new List<int>();
                msk_X = new List<double>();
                msk_Y = new List<double>();
                sk42_X = new List<double>();
                sk42_Y = new List<double>();
                distances = new List<double>();
                int error = 0;
                int msk_pnts;
                int sk42_pnts;
                string fullPath = Path.GetDirectoryName(sqlite_file).TrimEnd(Path.DirectorySeparatorChar);
                //MessageBox.Show(fullPath);
                string settlement = fullPath.Split(Path.DirectorySeparatorChar).Last();
                //MessageBox.Show(settlement);
                var con = new SQLiteConnection("Data Source=" + sqlite_file);
                con.Open();
                string stm = "SELECT * FROM raster_points";

                var cmd = new SQLiteCommand(stm, con);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    msk_pnts = 0;
                    msk_strings.Add("Имя_пункта 	X 	Y ");
                    msk_strings_panorama.Add("Имя_пункта 	X 	Y ");
                    while (rdr.Read())
                    {
                        msk_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                        msk_strings_panorama.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(2) + "\t" + rdr.GetDouble(1));
                        msk_pnts = msk_pnts + 1;
                        msk_indexes.Add(rdr.GetInt32(0));
                        msk_X.Add(rdr.GetDouble(1));
                        msk_Y.Add(rdr.GetDouble(2));
                        //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                    }
                }
                File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_МСК.txt", msk_strings, Encoding.ASCII);
                File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_МСК_panorama.txt", msk_strings_panorama, Encoding.ASCII);
                con.Close();

                var con_13 = new SQLiteConnection("Data Source=" + Path.GetDirectoryName(sqlite_file) + "\\satellite_points\\sat_pnts_z13.sqlite");
                con_13.Open();
                stm = "SELECT * FROM sat_points_zone13";
                cmd = new SQLiteCommand(stm, con_13);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        sk42_pnts = 0;
                        sk42_strings.Add("Имя_пункта 	X 	Y ");
                        sk42_strings_panorama.Add("Имя_пункта 	X 	Y ");
                        while (rdr.Read())
                        {
                            sk42_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                            sk42_strings_panorama.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(2) + "\t" + rdr.GetDouble(1));
                            sk42_pnts = sk42_pnts + 1;
                            sk42_indexes.Add(rdr.GetInt32(0));
                            if (msk_indexes[sk42_pnts - 1] != sk42_indexes[sk42_pnts - 1])
                            {
                                error = 1;
                                textBox1.AppendText("Различаются номера точек " + msk_indexes[sk42_pnts - 1].ToString() + " - " + sk42_indexes[sk42_pnts - 1].ToString() + "\n");
                            }
                            sk42_X.Add(rdr.GetDouble(1));
                            sk42_Y.Add(rdr.GetDouble(2));
                            distances.Add(GetDistance(msk_X[sk42_pnts - 1], msk_Y[sk42_pnts - 1], sk42_X[sk42_pnts - 1], sk42_Y[sk42_pnts - 1]));
                            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                        }
                        if (msk_pnts != sk42_pnts)
                        {
                            error = 1;
                            textBox1.AppendText("Различается количество точек для " + settlement + "\n");
                        }
                        double med_distance = distances.Median();
                        int k = 0;
                        foreach (double distance in distances)
                        {
                            if (Math.Abs(distance - med_distance) > 15)
                            {
                                textBox1.AppendText("Возможно некорректно указана пара точек " + msk_indexes[k] + " - " + sk42_indexes[k] + "\n");
                            }
                            k = k + 1;
                            error = 1;
                        }
                        if (error > 0)
                        {
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-13_error.txt", sk42_strings, Encoding.ASCII);
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-13_panorama_error.txt", sk42_strings_panorama, Encoding.ASCII);
                        }
                        else
                        {
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-13.txt", sk42_strings, Encoding.ASCII);
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-13_panorama.txt", sk42_strings_panorama, Encoding.ASCII);
                        }
                    }

                }
                con_13.Close();

                var con_14 = new SQLiteConnection("Data Source=" + Path.GetDirectoryName(sqlite_file) + "\\satellite_points\\sat_pnts_z14.sqlite");
                con_14.Open();
                stm = "SELECT * FROM sat_points_zone14";
                cmd = new SQLiteCommand(stm, con_14);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        sk42_pnts = 0;
                        sk42_strings.Add("Имя_пункта 	X 	Y ");
                        sk42_strings_panorama.Add("Имя_пункта 	X 	Y ");
                        while (rdr.Read())
                        {
                            sk42_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                            sk42_strings_panorama.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(2) + "\t" + rdr.GetDouble(1));
                            sk42_pnts = sk42_pnts + 1;
                            sk42_indexes.Add(rdr.GetInt32(0));
                            if (msk_indexes[sk42_pnts - 1] != sk42_indexes[sk42_pnts - 1])
                            {
                                error = 1;
                                textBox1.AppendText("Различаются номера точек " + msk_indexes[sk42_pnts - 1].ToString() + " - " + sk42_indexes[sk42_pnts - 1].ToString() + "\n");
                            }
                            sk42_X.Add(rdr.GetDouble(1));
                            sk42_Y.Add(rdr.GetDouble(2));
                            distances.Add(GetDistance(msk_X[sk42_pnts - 1], msk_Y[sk42_pnts - 1], sk42_X[sk42_pnts - 1], sk42_Y[sk42_pnts - 1]));
                            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                        }
                        if (msk_pnts != sk42_pnts)
                        {
                            error = 1;
                            textBox1.AppendText("Различается количество точек для " + settlement + "\n");
                        }
                        double med_distance = distances.Median();
                        int k = 0;
                        foreach (double distance in distances)
                        {
                            if (Math.Abs(distance - med_distance) > 15)
                            {
                                textBox1.AppendText("Возможно некорректно указана пара точек " + msk_indexes[k] + " - " + sk42_indexes[k] + "\n");
                            }
                            k = k + 1;
                            error = 1;
                        }
                        if (error > 0)
                        {
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-14_error.txt", sk42_strings, Encoding.ASCII);
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-14_panorama_error.txt", sk42_strings_panorama, Encoding.ASCII);
                        }
                        else
                        {
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-14.txt", sk42_strings, Encoding.ASCII);
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-14_panorama.txt", sk42_strings_panorama, Encoding.ASCII);
                        }
                    }

                }
                con_14.Close();

                var con_15 = new SQLiteConnection("Data Source=" + Path.GetDirectoryName(sqlite_file) + "\\satellite_points\\sat_pnts_z15.sqlite");
                con_15.Open();
                stm = "SELECT * FROM sat_points_zone15";
                cmd = new SQLiteCommand(stm, con_15);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        sk42_pnts = 0;
                        sk42_strings.Add("Имя_пункта 	X 	Y ");
                        sk42_strings_panorama.Add("Имя_пункта 	X 	Y ");
                        while (rdr.Read())
                        {
                            sk42_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                            sk42_strings_panorama.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(2) + "\t" + rdr.GetDouble(1));
                            sk42_pnts = sk42_pnts + 1;
                            sk42_indexes.Add(rdr.GetInt32(0));
                            if (msk_indexes[sk42_pnts - 1] != sk42_indexes[sk42_pnts - 1])
                            {
                                error = 1;
                                textBox1.AppendText("Различаются номера точек " + msk_indexes[sk42_pnts - 1].ToString() + " - " + sk42_indexes[sk42_pnts - 1].ToString() + "\n");
                            }
                            sk42_X.Add(rdr.GetDouble(1));
                            sk42_Y.Add(rdr.GetDouble(2));
                            distances.Add(GetDistance(msk_X[sk42_pnts - 1], msk_Y[sk42_pnts - 1], sk42_X[sk42_pnts - 1], sk42_Y[sk42_pnts - 1]));
                            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                        }
                        if (msk_pnts != sk42_pnts)
                        {
                            error = 1;
                            textBox1.AppendText("Различается количество точек для " + settlement + "\n");
                        }
                        double med_distance = distances.Median();
                        int k = 0;
                        foreach (double distance in distances)
                        {
                            if (Math.Abs(distance - med_distance) > 15)
                            {
                                textBox1.AppendText("Возможно некорректно указана пара точек " + msk_indexes[k] + " - " + sk42_indexes[k] + "\n");
                            }
                            k = k + 1;
                            error = 1;
                        }
                        if (error > 0)
                        {
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-15_error.txt", sk42_strings, Encoding.ASCII);
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-15_panorama_error.txt", sk42_strings_panorama, Encoding.ASCII);
                        }
                        else
                        {
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-15.txt", sk42_strings, Encoding.ASCII);
                            File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-15_panorama.txt", sk42_strings_panorama, Encoding.ASCII);
                        }
                    }

                }
                con_15.Close();

                if (checkBox3.Checked)
                {
                    //double[] x1 = { 83.786, 109.929, 1038.000, 539.107, 831.036, 632.786 };
                    double[] x1 = msk_X.ToArray();
                    //double[] y1 = { -36.107, -582.929, -434.786, -694.036, -352.000, -219.107 };
                    double[] y1 = msk_Y.ToArray();
                    //double[] x2 = { 557124.596, 564344.898, 646174.994, 603772.500, 626857.500, 607905.000 };
                    double[] x2 = sk42_X.ToArray();
                    //double[] y2 = { 5479746.857, 5376737.207, 5421503.083, 5363472.000, 5433468.000, 5455042.500 };
                    double[] y2 = sk42_Y.ToArray();
                    int n = 6;
                    //int p = 6;
                    int p = msk_X.Count;

                    double[,] m = new double[n + 1, n + 1];
                    double[] bForX = new double[n + 1];
                    double[] bForY = new double[n + 1];
                    double[] a = new double[n + 1];
                    double[] b = new double[n + 1];

                    for (int i = 0; i < p; i++)
                    {

                        m[1, 1] = n;
                        m[1, 2] = m[2, 1] += x1[i];
                        m[1, 3] = m[3, 1] += y1[i];
                        m[1, 4] = m[4, 1] = m[2, 2] += Math.Pow(x1[i], 2);
                        m[1, 5] = m[5, 1] = m[2, 3] = m[3, 2] += x1[i] * y1[i];
                        m[1, 6] = m[6, 1] = m[3, 3] += Math.Pow(y1[i], 2);
                        m[2, 4] = m[4, 2] += Math.Pow(x1[i], 3);
                        m[2, 5] = m[5, 2] = m[3, 4] = m[4, 3] += Math.Pow(x1[i], 2) * y1[i];
                        m[2, 6] = m[6, 2] = m[3, 5] = m[5, 3] += x1[i] * Math.Pow(y1[i], 2);
                        m[3, 6] = m[6, 3] += Math.Pow(y1[i], 3);
                        m[4, 4] += Math.Pow(x1[i], 4);
                        m[4, 5] = m[5, 4] += Math.Pow(x1[i], 3) * y1[i];
                        m[4, 6] = m[6, 4] = m[5, 5] += Math.Pow(x1[i], 2) * Math.Pow(y1[i], 2);
                        m[5, 6] = m[6, 5] += x1[i] * Math.Pow(y1[i], 3);
                        m[6, 6] += Math.Pow(y1[i], 4);


                        bForX[1] += x2[i];
                        bForX[2] += x1[i] * x2[i];
                        bForX[3] += y1[i] * x2[i];
                        bForX[4] += Math.Pow(x1[i], 2) * x2[i];
                        bForX[5] += x1[i] * y1[i] * x2[i];
                        bForX[6] += Math.Pow(y1[i], 2) * x2[i];

                        bForY[1] += y2[i];
                        bForY[2] += x1[i] * y2[i];
                        bForY[3] += y1[i] * y2[i];
                        bForY[4] += Math.Pow(x1[i], 2) * y2[i];
                        bForY[5] += x1[i] * y1[i] * y2[i];
                        bForY[6] += Math.Pow(y1[i], 2) * y2[i];
                    }


                    if (!linsolve.solvesystem(m, bForX, n, ref a))
                    {
                        //System.Console.Write("Error! Degenerate matrix A!");
                        //System.Console.WriteLine();
                        //System.Console.ReadKey();
                        MessageBox.Show("Ошибка. Вырожденная матрица А");
                        return;
                    }

                    if (!linsolve.solvesystem(m, bForY, n, ref b))
                    {
                        //System.Console.Write("Error! Degenerate matrix A!");
                        //System.Console.WriteLine();
                        //System.Console.ReadKey();
                        MessageBox.Show("Ошибка. Вырожденная матрица А");
                        return;
                    }

                    double testPointX1 = 500;
                    double testPointY1 = -300;
                    double testPointX2 = a[1] + a[2] * testPointX1 + a[3] * testPointY1 + a[4] * Math.Pow(testPointX1, 2) + a[5] * testPointX1 * testPointY1 + a[6] * Math.Pow(testPointY1, 2);
                    double testPointY2 = b[1] + b[2] * testPointX1 + b[3] * testPointY1 + b[4] * Math.Pow(testPointX1, 2) + b[5] * testPointX1 * testPointY1 + b[6] * Math.Pow(testPointY1, 2);

                    MessageBox.Show("X = " + testPointX2);
                    //System.Console.WriteLine();
                    MessageBox.Show("Y = " + testPointY2);
                    //System.Console.WriteLine();

                    for (int i = 0; i < n; i++)
                    {
                        //System.Console.WriteLine();
                        MessageBox.Show("a[" + i + "] = " + a[i + 1]);
                    }

                    for (int i = 0; i < n; i++)
                    {
                        //System.Console.WriteLine();
                        MessageBox.Show("b[" + i + "] = " + b[i + 1]);

                    }

                    //System.Console.ReadKey();

                }

                if (checkBox4.Checked)
                {
                    IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                    double x0 = msk_X.Mean();
                    double y0 = msk_Y.Mean();
                    double x0_ = sk42_X.Mean();
                    double y0_ = sk42_Y.Mean();
                    List<double> deltaX = new List<double>();
                    List<double> deltaY = new List<double>();
                    List<double> deltaX_ = new List<double>();
                    List<double> deltaY_ = new List<double>();
                    List<double> KDn = new List<double>();
                    List<double> Kf1 = new List<double>();
                    List<double> Kf2 = new List<double>();
                    foreach (int list_index in msk_indexes)
                    {
                        deltaX.Add(msk_X[list_index - 1] - x0);
                        deltaY.Add(msk_Y[list_index - 1] - y0);
                        deltaX_.Add(sk42_X[list_index - 1] - x0_);
                        deltaY_.Add(sk42_Y[list_index - 1] - y0_);
                        KDn.Add(deltaX[list_index - 1] * deltaX[list_index - 1] + deltaY[list_index - 1] * deltaY[list_index - 1]);
                        Kf1.Add(deltaX[list_index - 1] * deltaX_[list_index - 1] + deltaY[list_index - 1] * deltaY_[list_index - 1]);
                        Kf2.Add(deltaX_[list_index - 1] * deltaY[list_index - 1] - deltaX[list_index - 1] * deltaY_[list_index - 1]);
                    }
                    double D = KDn.Sum();
                    double f1 = Kf1.Sum();
                    double f2 = Kf2.Sum();
                    double d = f1 / D;
                    double c = f2 / D;
                    double Qx = x0_ - x0 * d - y0 * c;
                    double Qy = y0_ - y0 * d + x0 * c;
                    char[] charSeparator_comma = new char[] { ',' };
                    char[] charSeparator_left = new char[] { '(' };
                    char[] charSeparator_right = new char[] { ')' };
                    string[] tab_files = Directory.GetFiles(Path.GetDirectoryName(sqlite_file), "*.TAB", SearchOption.TopDirectoryOnly);
                    foreach (string tab_file in tab_files)
                    {
                        if (Path.GetFileName(tab_file).Contains("_trans"))
                        {

                        }
                        else
                        {
                            List<string> tabLines = File.ReadAllLines(tab_file).ToList();
                            List<string> panoramaLines = File.ReadAllLines(tab_file).ToList();
                            string[] pre_coords;
                            double x;
                            double y;
                            double x_;
                            double y_;
                            string[] prev_str;
                            string[] x_str;
                            string[] y_str;
                            pre_coords = tabLines[7].Split(charSeparator_comma);
                            x_str = pre_coords[0].Split(charSeparator_left);
                            x = double.Parse(x_str[1], formatter);
                            y_str = pre_coords[1].Split(charSeparator_right);
                            y = double.Parse(y_str[0], formatter);
                            x_ = Qx + x * d + y * c;
                            y_ = Qy + y * d - x * c;
                            prev_str = tabLines[7].Split(new string[] { ") (" }, StringSplitOptions.None);
                            tabLines[7] = "  (" + x_.ToString().Replace(",",".") + "," + y_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            panoramaLines[7] = "  (" + y_.ToString().Replace(",", ".") + "," + x_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            pre_coords = tabLines[8].Split(charSeparator_comma);
                            x_str = pre_coords[0].Split(charSeparator_left);
                            x = double.Parse(x_str[1], formatter);
                            y_str = pre_coords[1].Split(charSeparator_right);
                            y = double.Parse(y_str[0], formatter);
                            x_ = Qx + x * d + y * c;
                            y_ = Qy + y * d - x * c;
                            prev_str = tabLines[8].Split(new string[] { ") (" }, StringSplitOptions.None);
                            tabLines[8] = "  (" + x_.ToString().Replace(",", ".") + "," + y_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            panoramaLines[8] = "  (" + y_.ToString().Replace(",", ".") + "," + x_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            pre_coords = tabLines[9].Split(charSeparator_comma);
                            x_str = pre_coords[0].Split(charSeparator_left);
                            x = double.Parse(x_str[1], formatter);
                            y_str = pre_coords[1].Split(charSeparator_right);
                            y = double.Parse(y_str[0], formatter);
                            x_ = Qx + x * d + y * c;
                            y_ = Qy + y * d - x * c;
                            prev_str = tabLines[9].Split(new string[] { ") (" }, StringSplitOptions.None);
                            tabLines[9] = "  (" + x_.ToString().Replace(",", ".") + "," + y_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            panoramaLines[9] = "  (" + y_.ToString().Replace(",", ".") + "," + x_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            pre_coords = tabLines[10].Split(charSeparator_comma);
                            x_str = pre_coords[0].Split(charSeparator_left);
                            x = double.Parse(x_str[1], formatter);
                            y_str = pre_coords[1].Split(charSeparator_right);
                            y = double.Parse(y_str[0], formatter);
                            x_ = Qx + x * d + y * c;
                            y_ = Qy + y * d - x * c;
                            prev_str = tabLines[10].Split(new string[] { ") (" }, StringSplitOptions.None);
                            tabLines[10] = "  (" + x_.ToString().Replace(",", ".") + "," + y_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            panoramaLines[10] = "  (" + y_.ToString().Replace(",", ".") + "," + x_.ToString().Replace(",", ".") + ") (" + prev_str[1];
                            File.WriteAllLines(tab_file.Replace(".TAB", "_trans.TAB"), tabLines);
                            File.WriteAllLines(tab_file.Replace(".TAB", "_trans_panorama.TAB"), panoramaLines);
                        }

                    }
                }
            }
            MessageBox.Show("Создание файлов завершено");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                label2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Gdal.AllRegister();
            textBox1.Clear();
            string[] tiff_files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*_cut.tif", SearchOption.AllDirectories);
            int files_count = tiff_files.Length;
            //textBox1.Text = textBox1.Text + files_count.ToString() + "\n";
            foreach (string tiff_file in tiff_files)
            {
                //textBox1.AppendText(tiff_file + "\n");
                string result_tiff = tiff_file.Replace(folderBrowserDialog1.SelectedPath, "");
                result_tiff = result_tiff.Replace("_cut", "");
                result_tiff = result_tiff.Replace("_modified", "");
                //textBox1.AppendText(folderBrowserDialog2.SelectedPath + result_tiff+"\n");
                Directory.CreateDirectory(Path.GetDirectoryName(folderBrowserDialog2.SelectedPath + result_tiff));
                File.Copy(tiff_file, folderBrowserDialog2.SelectedPath + result_tiff, true);
                List<string> tabLines = File.ReadAllLines(tiff_file.Replace(".tif", ".TAB")).ToList();
                tabLines[5] = "File \"" + Path.GetFileName(result_tiff) + "\"";
                File.WriteAllLines(Path.GetDirectoryName(folderBrowserDialog2.SelectedPath + result_tiff) + "\\" + Path.GetFileNameWithoutExtension(result_tiff) + ".TAB", tabLines);
            }
            MessageBox.Show("Обработка файлов завершена");
        }
    }

    class linsolve
    {
        public static bool solvesystemlu(ref double[,] a,
            ref int[] pivots,
            double[] b,
            int n,
            ref double[] x)
        {
            bool result = new bool();
            double[] y = new double[0];
            int i = 0;
            int j = 0;
            double v = 0;
            int ip1 = 0;
            int im1 = 0;
            int i_ = 0;

            b = (double[])b.Clone();

            y = new double[n + 1];
            x = new double[n + 1];
            result = true;
            for (i = 1; i <= n; i++)
            {
                if (a[i, i] == 0)
                {
                    result = false;
                    return result;
                }
            }

            //
            // pivots
            //
            for (i = 1; i <= n; i++)
            {
                if (pivots[i] != i)
                {
                    v = b[i];
                    b[i] = b[pivots[i]];
                    b[pivots[i]] = v;
                }
            }

            //
            // Ly = b
            //
            y[1] = b[1];
            for (i = 2; i <= n; i++)
            {
                im1 = i - 1;
                v = 0.0;
                for (i_ = 1; i_ <= im1; i_++)
                {
                    v += a[i, i_] * y[i_];
                }
                y[i] = b[i] - v;
            }

            //
            // Ux = y
            //
            x[n] = y[n] / a[n, n];
            for (i = n - 1; i >= 1; i--)
            {
                ip1 = i + 1;
                v = 0.0;
                for (i_ = ip1; i_ <= n; i_++)
                {
                    v += a[i, i_] * x[i_];
                }
                x[i] = (y[i] - v) / a[i, i];
            }
            return result;
        }

        public static bool solvesystem(double[,] a,
            double[] b,
            int n,
            ref double[] x)
        {
            bool result = new bool();
            int[] pivots = new int[0];
            int i = 0;

            a = (double[,])a.Clone();
            b = (double[])b.Clone();

            lu.ludecomposition(ref a, n, n, ref pivots);
            result = solvesystemlu(ref a, ref pivots, b, n, ref x);
            return result;
        }
    }

    class lu
    {
        public static void ludecomposition(ref double[,] a,
            int m,
            int n,
            ref int[] pivots)
        {
            int i = 0;
            int j = 0;
            int jp = 0;
            double[] t1 = new double[0];
            double s = 0;
            int i_ = 0;

            pivots = new int[Math.Min(m, n) + 1];
            t1 = new double[Math.Max(m, n) + 1];
            System.Diagnostics.Debug.Assert(m >= 0 & n >= 0, "Error in LUDecomposition: incorrect function arguments");

            if (m == 0 | n == 0)
            {
                return;
            }
            for (j = 1; j <= Math.Min(m, n); j++)
            {

                jp = j;
                for (i = j + 1; i <= m; i++)
                {
                    if (Math.Abs(a[i, j]) > Math.Abs(a[jp, j]))
                    {
                        jp = i;
                    }
                }
                pivots[j] = jp;
                if (a[jp, j] != 0)
                {

                    if (jp != j)
                    {
                        for (i_ = 1; i_ <= n; i_++)
                        {
                            t1[i_] = a[j, i_];
                        }
                        for (i_ = 1; i_ <= n; i_++)
                        {
                            a[j, i_] = a[jp, i_];
                        }
                        for (i_ = 1; i_ <= n; i_++)
                        {
                            a[jp, i_] = t1[i_];
                        }
                    }

                    if (j < m)
                    {

                        jp = j + 1;
                        s = 1 / a[j, j];
                        for (i_ = jp; i_ <= m; i_++)
                        {
                            a[i_, j] = s * a[i_, j];
                        }
                    }
                }
                if (j < Math.Min(m, n))
                {

                    jp = j + 1;
                    for (i = j + 1; i <= m; i++)
                    {
                        s = a[i, j];
                        for (i_ = jp; i_ <= n; i_++)
                        {
                            a[i, i_] = a[i, i_] - s * a[j, i_];
                        }
                    }
                }
            }
        }
    }

}
