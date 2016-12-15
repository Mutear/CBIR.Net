using CBIR.Net.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBIR.Net.Feature
{
    /// <summary>
    /// <para>Annular Color Layout Histogram</para>
    /// <para>References:</para>
    /// <para>Rao A, Srihari R K, Zhang Z. Spatial color histograms for content-based image retrieval[J]. 1999:183-186.</para>
    /// <para>孙君顶, 毋小省. 基于颜色分布特征的图像检索[J]. 光电子·激光, 2006, 17(8):1009-1013.</para>
    /// </summary>
    public class AnnularColorLayoutHistogram : IFeature
    {
        public const string FeatureName = "ACLH";
        /// <summary>
        /// <para>Number of concentric circles</para>
        /// <para>Default value is 10</para>
        /// </summary>
        protected const int N = 10;
        /// <summary>
        /// <para>The size of the image when the feature is extracted</para>
        /// </summary>
        protected const int Width = 200, Height = 200;
        /// <summary>
        /// The matrix of this image feature
        /// </summary>
        protected int[][] featureMatrix = null;

        public virtual void Extract(System.Drawing.Bitmap bitmap)
        {
            if (bitmap.Width < Width || bitmap.Height < Height)
            {
                throw new Exception(string.Format("The size of the image must be greater than {0}*{1}", Width, Height));
            }

            // Get gray pixel matrix
            int[][] grayPixelMatrix = ImageUtil.GetGrayPixelMatrix(bitmap, Width, Height);
            if (grayPixelMatrix != null && grayPixelMatrix.Length != 0 && grayPixelMatrix[0].Length != 0)
            {
                // Group pixels according to the gray value of pixel
                Dictionary<int, List<Tuple<int, int>>> groupedPixels = new Dictionary<int, List<Tuple<int, int>>>();
                Tuple<double, double>[] centroids = new Tuple<double, double>[256];
                double[][] distances = new double[grayPixelMatrix.Length][];
                for (int i = 0; i < grayPixelMatrix.Length; i++)
                {
                    distances[i] = new double[grayPixelMatrix[i].Length];
                    for (int j = 0; j < grayPixelMatrix[i].Length; j++)
                    {
                        int gray = grayPixelMatrix[i][j];
                        Tuple<int, int> tuple = new Tuple<int, int>(i, j);
                        List<Tuple<int, int>> list = null;
                        if (groupedPixels.ContainsKey(gray))
                        {
                            list = groupedPixels[gray];
                            list.Add(tuple);
                            var c = centroids[gray];
                            centroids[gray] = new Tuple<double, double>(c.Item1 + tuple.Item1, c.Item2 + tuple.Item2);
                        }
                        else
                        {
                            list = new List<Tuple<int, int>>();
                            list.Add(tuple);
                            groupedPixels.Add(gray, list);
                            centroids[gray] = new Tuple<double,double>(tuple.Item1, tuple.Item2);
                        }
                    }
                }
                // Calculate the centroid for different gray values
                this.featureMatrix = new int[256][];
                for (int i = 0; i < 256; i++)
                {
                    this.featureMatrix[i] = new int[N];
                    if (centroids[i] != null)
                    {
                        var list = groupedPixels[i];
                        var c = centroids[i];
                        centroids[i] = new Tuple<double, double>(c.Item1 / list.Count, c.Item2 / list.Count);

                        double max = 0;
                        // Calculate the distance to the centroid for each pixel and select the max distance.
                        foreach (var tuple in list)
                        {
                            double dis = Math.Sqrt(Math.Pow(tuple.Item1 - centroids[i].Item1, 2) + Math.Pow(tuple.Item2 - centroids[i].Item2, 2));
                            distances[tuple.Item1][tuple.Item2] = dis;
                            if (dis > max)
                            {
                                max = dis;
                            }
                        }

                        // Counts the number of pixels contained in a concentric circle with different distances
                        for (int j = 0; j < list.Count; j++)
                        {
                            double dis = distances[list[j].Item1][list[j].Item2];
                            if (max > 0)
                            {
                                double quot = dis / (max / N);
                                // When quot equals 10.0, it will throw a exception: array index out of range
                                if (quot == 10.0)
                                {
                                    quot = 9.0;
                                }
                                this.featureMatrix[i][(int)Math.Floor(quot)]++;
                            }
                            else
                            {
                                if (max == 0 && list.Count == 1)
                                {
                                    this.featureMatrix[i][0] = 1;
                                }
                                else
                                {
                                    throw new Exception("Logic erroe");
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual double CalculateSimilarity(IFeature feature)
        {
            if (this.featureMatrix == null)
            {
                throw new Exception("This object has not yet extracted the image feature");
            }
            else if(feature is AnnularColorLayoutHistogram)
            {
                return ImageUtil.CalculateSimilarity(this.featureMatrix, (feature as AnnularColorLayoutHistogram).featureMatrix);
            }
            else
            {
                throw new ArgumentException("The type of feature does not match this object");
            }
        }

        public virtual string GenerateIndexWithFeature()
        {
            if (this.featureMatrix == null)
            {
                throw new Exception("This object has not yet extracted the image feature");
            }
            else
                return ImageUtil.ConvertMatrixToString(this.featureMatrix);
        }

        public virtual void GenerateFeatureWithIndex(string index)
        {
            this.featureMatrix = ImageUtil.ConvertStringToMatrix(index, 256, N);
        }

        public virtual string GetFeatureName()
        {
            return FeatureName;
        }
    }
}
