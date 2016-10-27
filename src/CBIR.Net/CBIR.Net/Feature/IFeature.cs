﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBIR.Net.Feature
{
    /// <summary>
    /// <para>The basic interface of image feature</para>
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// <para>Image feature extraction method</para>
        /// <para>This method will extract the feature of bitmap, and store in the instance for subsequent operations</para>
        /// <para>For example, CalculateSimilarity and GenerateIndexWithFeature</para>
        /// </summary>
        /// <param name="bitmap">The source image</param>
        public void Extract(Bitmap bitmap);
        /// <summary>
        /// <para>Calculate the similarity between two image features</para>
        /// </summary>
        /// <param name="feature">The image feature that will compare with it</param>
        /// <returns></returns>
        public double CalculateSimilarity(IFeature feature);
        /// <summary>
        /// <para>Generate index with feature</para>
        /// <para>Not all data structures of image feature are suitable for storing</para>
        /// <para>So we need this method to generate a index string</para>
        /// <para>And this index can be stored in database or document</para>
        /// </summary>
        /// <returns></returns>
        public string GenerateIndexWithFeature();
        /// <summary>
        /// <para>This method is the inverse operation of GenerateIndexWithFeature</para>
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void GenerateFeatureWithIndex(string index);
        /// <summary>
        /// <para>Return the name of this image feature</para>
        /// </summary>
        public string GetFeatureName();
    }
}
