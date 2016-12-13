using CBIR.Net.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBIR.Net.Feature
{
    public class MeanHash : IFeature
    {
        public const string FeatureName = "MeanHash";
        protected string featureValue = null;

        public virtual void Extract(System.Drawing.Bitmap bitmap)
        {
            int[][] grayPixelMatrix = ImageUtil.GetGrayPixelMatrix(bitmap, 8, 8);
            if (grayPixelMatrix != null && grayPixelMatrix.Length != 0 && grayPixelMatrix[0].Length != 0)
            {
                double average = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        average += grayPixelMatrix[i][j];
                    }
                }
                average /= 64.0;
                this.featureValue = GetFeature(grayPixelMatrix, average);
            }
        }

        public virtual double CalculateSimilarity(IFeature feature)
        {
            if (string.IsNullOrEmpty(this.featureValue))
            {
                throw new Exception("This object has not yet extracted the image feature");
            }
            else if (feature is MeanHash)
            {
                return ImageUtil.CalculateSimilarity(this.featureValue, (feature as MeanHash).featureValue);
            }
            else
            {
                throw new ArgumentException("The type of feature does not match this object");
            }
        }

        public virtual string GenerateIndexWithFeature()
        {
            return this.featureValue;
        }

        public virtual void GenerateFeatureWithIndex(string index)
        {
            this.featureValue = index;
        }

        public virtual string GetFeatureName()
        {
            return FeatureName;
        }

        private String GetFeature(int[][] matrix, double average)
        {
            String featureValue = "";
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] < average)
                    {
                        featureValue += '0';
                    }
                    else
                    {
                        featureValue += '1';
                    }
                }
            }
            return featureValue;
        }
    }
}
