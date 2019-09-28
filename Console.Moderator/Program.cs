using AutoMapper;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Console.Moderator
{
    class Program
    {
        // The name of the file that contains the text to evaluate.
        private static string TextFile = "TextFile.txt";

        // The name of the file to contain the output from the evaluation.
        private static string OutputFile = "TextModerationOutput.txt";


        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello Santiago de Compostela!");

            ModerateText();
            //ModerateImage();
        }

        private static void ModerateText()
        {
            // Load the input text.
            string text = File.ReadAllText(TextFile);
            System.Console.WriteLine("Screening {0}", TextFile);

            text = text.Replace(System.Environment.NewLine, " ");
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);

            // Save the moderation results to a file.
            using (StreamWriter outputWriter = new StreamWriter(OutputFile, false))
            {
                // Create a Content Moderator client and evaluate the text.
                using (var client = GetClient())
                {
                    // Screen the input text: check for profanity,
                    // autocorrect text, check for personally identifying
                    // information (PII), and classify the text into three categories
                    outputWriter.WriteLine("Autocorrect typos, check for matching terms, PII, and classify.");

                    var rawResult = client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);

                    outputWriter.WriteLine(JsonConvert.SerializeObject(rawResult, Formatting.Indented));
                }
                outputWriter.Flush();
                outputWriter.Close();
            }
        }

        private static ContentModeratorClient GetClient()
        {
            var apiKey = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_SUBSCRIPTION_KEY");
            var endpoint = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_ENDPOINT");
            var limits = new LimitsConfig
            {
                ImgSize = 128,
                TextLength = 1024,
                FileWeight = 4.0
            };
            var moderatorConfig = new ContentModerationConfig
            {
                ApiKey = apiKey,
                BaseUrl = endpoint,
                Limits = limits,
                MaxRetries = 5
            };
            var client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(apiKey), 
                new RetryHandler(moderatorConfig.MaxRetries));
            client.Endpoint = endpoint;

            return client;
        }

    }
}
