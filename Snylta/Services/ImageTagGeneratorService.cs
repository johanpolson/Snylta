﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Snylta.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snylta.Services
{
    public class ImageTagGeneratorService
    {
        private string subscriptionKey;

        public ImageTagGeneratorService(AppSettings aPIKeys)
        {
            subscriptionKey = aPIKeys.ComputerVisionKey;
        }


        internal async Task<List<ImageAnalysis>> GetTagsForImages(List<string> filePaths)
        {
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
            //VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            //VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            //VisualFeatureTypes.Objects,
            VisualFeatureTypes.Tags
            };

            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = "https://northeurope.api.cognitive.microsoft.com";
            List<string> EnglishTags = new List<String>();
            List<ImageAnalysis> analysises = new List<ImageAnalysis>();

            foreach (var item in filePaths)
            {
                using (Stream imageStream = File.OpenRead(item))
                {
                    ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                        imageStream, features);
                    analysises.Add(analysis);


                    //foreach (var tag in analysis.Tags)
                    //{
                    //    if (tag.Confidence > 0.1)
                    //    {
                    //        Tag goodTag = new Tag();
                    //        goodTag.EnglishTag = tag.Name;

                    //        tags.Add(goodTag);
                    //    }
                    //}
                }

            }
            

            return analysises;
            


        }
    }
}