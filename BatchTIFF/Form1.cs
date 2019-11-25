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
                string[] coords = GDALInfoGetPosition(tiff_image, 0.0, 0.0).Split(new string[] {", " }, StringSplitOptions.None);
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
                tab_strings.Add("  ("+X+","+Y+ ") (0,0) Label \"Точка 1\",");

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
                tab_strings.Add("  ("+ X+","+Y + ") (0,"+ tiff_image.RasterYSize.ToString()+ ")  Label \"Точка 2\",");

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
                tab_strings.Add("  (" + X + "," + Y + ") (" + tiff_image.RasterXSize.ToString() + ","+ tiff_image.RasterYSize.ToString()+") Label \"Точка 3\",");

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
                File.WriteAllLines(Path.GetDirectoryName(tiff_file)+"\\"+Path.GetFileNameWithoutExtension(tiff_file) + ".TAB", tab_strings);

                if (checkBox2.Checked)
                {
                    using (MagickImage image = new MagickImage(tiff_file))
                    {
                        string fullPath = Path.GetDirectoryName(tiff_file).TrimEnd(Path.DirectorySeparatorChar);
                        string folder_name = fullPath.Split(Path.DirectorySeparatorChar).Last();
                        if (image.Depth == 1 || folder_name== "Gray_images")
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

        private void button3_Click(object sender, EventArgs e)
        {
            string[] sqlite_files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "raster_points.sqlite", SearchOption.AllDirectories);
            int files_count = sqlite_files.Length;
            List<string> msk_strings;
            List<string> sk42_strings;
            foreach (string sqlite_file in sqlite_files)
            {
                msk_strings = new List<string>();
                sk42_strings = new List<string>();
                string fullPath = Path.GetDirectoryName(sqlite_file).TrimEnd(Path.DirectorySeparatorChar);
                //MessageBox.Show(fullPath);
                string settlement = fullPath.Split(Path.DirectorySeparatorChar).Last();
                //MessageBox.Show(settlement);
                var con = new SQLiteConnection("Data Source="+sqlite_file);
                con.Open();
                string stm = "SELECT * FROM raster_points";

                var cmd = new SQLiteCommand(stm, con);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    msk_strings.Add("Имя_пункта 	X 	Y ");
                    while (rdr.Read())
                    {
                        msk_strings.Add(rdr.GetInt32(0)+"\t"+ rdr.GetDouble(1)+"\t"+ rdr.GetDouble(2));
                        //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                    }
                }
                File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_МСК.txt", msk_strings, Encoding.ASCII);
                con.Close();

                var con_13 = new SQLiteConnection("Data Source=" + Path.GetDirectoryName(sqlite_file) + "\\satellite_points\\sat_pnts_z13.sqlite");
                con_13.Open();
                stm = "SELECT * FROM sat_points_zone13";
                cmd = new SQLiteCommand(stm, con_13);
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        sk42_strings.Add("Имя_пункта 	X 	Y ");
                        while (rdr.Read())
                        {
                            sk42_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                        }
                        File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-13.txt", sk42_strings, Encoding.ASCII);
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
                        sk42_strings.Add("Имя_пункта 	X 	Y ");
                        while (rdr.Read())
                        {
                            sk42_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                        }
                        File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-14.txt", sk42_strings, Encoding.ASCII);
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
                        sk42_strings.Add("Имя_пункта 	X 	Y ");
                        while (rdr.Read())
                        {
                            sk42_strings.Add(rdr.GetInt32(0) + "\t" + rdr.GetDouble(1) + "\t" + rdr.GetDouble(2));
                            //MessageBox.Show($"{rdr.GetInt32(0)} {rdr.GetDouble(1)} {rdr.GetDouble(2)}");
                        }
                        File.WriteAllLines(Path.GetDirectoryName(sqlite_file) + "\\" + settlement + "_СК42-15.txt", sk42_strings, Encoding.ASCII);
                    }

                }
                con_15.Close();
            }
            MessageBox.Show("Создание файлов завершено");

        }
    }

}
