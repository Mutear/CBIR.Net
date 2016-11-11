using CBIR.Net.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBIR.Net.Feature
{
    public class RGBColorHistogram : IFeature
    {
        public const string FeatureName = "RGBCH";
        /// <summary>
        /// <para>The size of the image when the feature is extracted</para>
        /// </summary>
        protected const int Width = 200, Height = 200;
        /// <summary>
        /// The matrix of this image feature
        /// </summary>
        protected int[][] featureMatrix = null;

        public void Extract(System.Drawing.Bitmap bitmap)
        {
            Color[][] matrix = ImageUtil.GetImagePixelMatrix(bitmap, Width, Height);
            if (matrix != null && matrix.Length != 0 && matrix[0].Length != 0)
            {
                // 统计
                int[][] histogram = new int[3][];
                for (int i = 0; i < 3; i++)
                {
                    histogram[i] = new int[256];
                }
                for (int i = 0; i < matrix.Length; i++)
                {
                    for (int j = 0; j < matrix[0].Length; j++)
                    {
                        histogram[0][matrix[i][j].R]++;
                        histogram[1][matrix[i][j].G]++;
                        histogram[2][matrix[i][j].B]++;
                    }
                }
                this.featureMatrix = histogram;
            }
        }

        public double CalculateSimilarity(IFeature feature)
        {
            if (this.featureMatrix == null)
            {
                throw new Exception("This object has not yet extracted the image feature");
            }
            else if (feature is RGBColorHistogram)
            {
                return ImageUtil.CalculateSimilarity(this.featureMatrix, (feature as RGBColorHistogram).featureMatrix);
            }
            else
            {
                throw new ArgumentException("The type of feature does not match this object");
            }
        }

        public string GenerateIndexWithFeature()
        {
            if (this.featureMatrix == null)
            {
                throw new Exception("This object has not yet extracted the image feature");
            }
            else
                return ImageUtil.ConvertMatrixToString(this.featureMatrix);
        }

        public void GenerateFeatureWithIndex(string index)
        {
            this.featureMatrix = ImageUtil.ConvertStringToMatrix(index, 3, 256);
        }

        public string GetFeatureName()
        {
            return FeatureName;
        }
    }
}
