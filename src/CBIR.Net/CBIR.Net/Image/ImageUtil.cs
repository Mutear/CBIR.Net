using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBIR.Net.Image
{
    public class ImageUtil
    {
        public static int[][] GetGrayPixelMatrix(Bitmap bitmap, int width, int height)
        {
            try
            {
                bitmap = ResizeBitmap(bitmap, width, height);
                int[][] grayPixelMatrix = new int[width][];
                for (int i = 0; i < bitmap.Height; i++)
                {
                    grayPixelMatrix[i] = new int[height];
                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        Color color = bitmap.GetPixel(i, j);
                        grayPixelMatrix[i][j] = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    }
                }
                return grayPixelMatrix;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Color[][] GetImagePixelMatrix(Bitmap bitmap, int width, int height)
        {
            try
            {
                bitmap = ResizeBitmap(bitmap, width, height);
                Color[][] pixelMatrix = new Color[width][];
                for (int i = 0; i < bitmap.Height; i++)
                {
                    pixelMatrix[i] = new Color[height];
                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        pixelMatrix[i][j] = bitmap.GetPixel(i, j);
                    }
                }
                return pixelMatrix;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static Bitmap ResizeBitmap(Bitmap bitmap, int width, int height)
        {
            Bitmap newBitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(newBitmap))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(bitmap, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                return newBitmap;
            }
        }

        public static double CalculateSimilarity(int[][] matrix1, int[][] matrix2)
        {
            return CalculateSimilarity(ConvertMatrixToVector(matrix1), ConvertMatrixToVector(matrix2));
        }

        public static double CalculateSimilarity(int[] vector1, int[] vector2)
        {
            if (vector1 == null || vector2 == null)
            {
                throw new NullReferenceException();
            }
            else if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Two vector lengths are not equal");
            }
            else
            {
                double num1 = 0, num2 = 0, numerator = 0;
                for (int i = 0; i < vector1.Length; i++)
                {
                    num1 += Math.Pow(vector1[i], 2);
                    num2 += Math.Pow(vector2[i], 2);
                    numerator += vector1[i] * vector2[i];
                }
                num1 = Math.Sqrt(num1);
                num2 = Math.Sqrt(num2);
                return numerator / (num1 * num2);
            }
        }

        public static double CalculateSimilarity(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                throw new NullReferenceException();
            }
            int num = 0;
            for (int i = 0; i < 64; i++)
            {
                if (str1[i] == str2[i])
                {
                    num++;
                }
            }
            return ((double)num) / 64.0;
        }

        public static int[] ConvertMatrixToVector(int[][] matrix)
        {
            if (matrix != null && matrix.Length != 0 && matrix[0].Length != 0)
            {
                int[] vector = new int[matrix.Length * matrix[0].Length];
                for (int i = 0, index = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[0].Length; j++, index++)
                    {
                        vector[index] = matrix[i][j];
                    }
                }
                return vector;
            }
            return null;
        }

        public static string ConvertMatrixToString(int[][] matrix)
        {
            return ConvertVectorToString(ConvertMatrixToVector(matrix));
        }

        public static string ConvertVectorToString(int[] vector)
        {
            if (vector != null && vector.Length != 0)
            {
                string str = vector[0].ToString();
                for (int i = 1; i < str.Length; i++)
                {
                    str += ' ' + vector[i];
                }
                return str;
            }
            return null;
        }

        public static int[] ConvertStringToVector(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                string[] strs = str.Split(' ');
                int[] vector = new int[strs.Length];
                for (int i = 0; i < strs.Length; i++)
                {
                    vector[i] = int.Parse(strs[i]);
                }
                return vector;
            }
        }

        public static int[][] ConvertStringToMatrix(string str, int row, int column)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            else
            {
                string[] strs = str.Split(' ');
                int[][] matrix = new int[row][];
                for (int i = 0, index = 0; i < row; i++)
                {
                    matrix[i] = new int[column];
                    for (int j = 0; j < column; j++, index++)
                    {
                        matrix[i][j] = int.Parse(strs[index]);
                    }
                }
                return matrix;
            }
        }
    }
}
