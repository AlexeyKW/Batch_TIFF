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
using OSGeo.GDAL;
using OSGeo.OSR;

namespace BatchTIFF
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                    X = X.Replace(',', '.');
                    int X_int = System.Convert.ToInt32(X);
                    X = X_int.ToString() + ".0";
                    Y = Y.Replace(',', '.');
                    int Y_int = System.Convert.ToInt32(Y);
                    Y = Y_int.ToString() + ".0";
                }
                else
                {
                    X = X.Replace(',', '.');
                    Y = Y.Replace(',', '.');
                }
                tab_strings.Add("  ("+X+","+Y+ ") (0,0) Label \"Точка 1\",");
                coords = GDALInfoGetPosition(tiff_image, tiff_image.RasterXSize, 0.0).Split(new string[] { ", " }, StringSplitOptions.None);
                X = coords[0];
                Y = coords[1];
                if (checkBox1.Checked)
                {
                    X = X.Replace(',', '.');
                    int X_int = System.Convert.ToInt32(X);
                    X = X_int.ToString() + ".0";
                    Y = Y.Replace(',', '.');
                    int Y_int = System.Convert.ToInt32(Y);
                    Y = Y_int.ToString() + ".0";
                }
                else
                {
                    X = X.Replace(',', '.');
                    Y = Y.Replace(',', '.');
                }
                tab_strings.Add("  ("+ X+","+Y + ") ("+ tiff_image.RasterXSize.ToString()+ ",0) Label \"Точка 2\",");
                coords = GDALInfoGetPosition(tiff_image, 0.0, tiff_image.RasterYSize).Split(new string[] { ", " }, StringSplitOptions.None);
                X = coords[0];
                Y = coords[1];
                if (checkBox1.Checked)
                {
                    X = X.Replace(',', '.');
                    int X_int = System.Convert.ToInt32(X);
                    X = X_int.ToString() + ".0";
                    Y = Y.Replace(',', '.');
                    int Y_int = System.Convert.ToInt32(Y);
                    Y = Y_int.ToString() + ".0";
                }
                else
                {
                    X = X.Replace(',', '.');
                    Y = Y.Replace(',', '.');
                }
                tab_strings.Add("  ("+ X+","+Y + ") (0,"+ tiff_image.RasterYSize.ToString()+ ")  Label \"Точка 3\"");
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
            }
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
    }

}
