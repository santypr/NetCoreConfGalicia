using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.Collections.Generic;

namespace Console.Moderator
{
    public static class ModerationLib
    {
    }

    // Wraps the creation and configuration of a Content Moderator client.
    public static class Clients
    {
        // The base URL fragment for Content Moderator calls.
        // Add your Azure Content Moderator endpoint to your environment variables.
        private static readonly string AzureBaseURL = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_ENDPOINT");

        // Your Content Moderator subscription key.
        // Add your Azure Content Moderator subscription key to your environment variables.
        private static readonly string CMSubscriptionKey = Environment.GetEnvironmentVariable("CONTENT_MODERATOR_SUBSCRIPTION_KEY");

        // Returns a new Content Moderator client for your subscription.
        public static ContentModeratorClient NewClient()
        {
            // Create and initialize an instance of the Content Moderator API wrapper.
            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(CMSubscriptionKey));

            client.Endpoint = AzureBaseURL;
            return client;
        }
    }

    public class ContentModerationConfig
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public int MaxRetries { get; set; }
        public LimitsConfig Limits { get; set; }
    }

    public class LimitsConfig
    {
        public int ImgSize { get; set; }
        public int TextLength { get; set; }
        public double FileWeight { get; set; }
    }

    public class ModerationResultBase
    {
        public string Language { get; set; }
    }

    public class TextModerationResult : ModerationResultBase
    {
        public Classification Classification { get; set; }
        public List<DetectedTerms> Terms { get; set; }
    }

}
