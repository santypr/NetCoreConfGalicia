using AutoMapper;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ContentModerator.Moderation
{
    public class ModerationLib
    {
        private readonly IMapper mapper;
        private readonly ContentModerationConfig contentModerationConfig;

        public ModerationLib()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Screen, TextModerationResult>();
                cfg.CreateMap<Evaluate, ImageModerationResult>().ForMember(m => m.TextModerationResult, ac => ac.Ignore());
            });
            mapper = config.CreateMapper();
        }

        public async Task<ImageModerationResult> ModerateImage(Uri imageUri, string language, ImageProperties imageProperties)
        {
            if (imageProperties.Width < contentModerationConfig.Limits.ImgSize || imageProperties.Height < contentModerationConfig.Limits.ImgSize)
            {
                throw new Exception("Cognitive Services API cannot moderate images smaller than 128px");
            }

            if (imageProperties.Weight > contentModerationConfig.Limits.FileWeight)
            {
                throw new Exception("Cognitive Services API cannot moderate images bigger than 4MB");
            }

            using (var moderatorClient = GetClient())
            {
                var bodyModel = new BodyModel("URL", imageUri.ToString());
                var imageEvaluationResult = await moderatorClient.ImageModeration.EvaluateUrlInputAsync("application/json", bodyModel);
                var isInnapropriated = imageEvaluationResult.Result.Value;
                var textModerationResult = new TextModerationResult();
                if (!isInnapropriated)
                {
                    var imgOCRText = await moderatorClient.ImageModeration.OCRUrlInputAsync(language, "application/json", bodyModel, true);
                    var text = imgOCRText.Text.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim().Trim('/');
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        textModerationResult = await ModerateText(text, language);
                        var reviewRecommended = textModerationResult.Classification?.ReviewRecommended.Value ?? false;
                        var hasInappropriateWords = textModerationResult.Terms != null ? textModerationResult.Terms.Count > 0 : false;
                        isInnapropriated = reviewRecommended || hasInappropriateWords;

                        imageEvaluationResult.Result = isInnapropriated;
                    }
                }

                var result = mapper.Map<Evaluate, ImageModerationResult>(imageEvaluationResult);
                result.TextModerationResult = textModerationResult;
                result.Language = language;

                return result;
            }
        }

        public async Task<TextModerationResult> ModerateText(string text, string language)
        {
            if (text.Length > contentModerationConfig.Limits.TextLength)
            {
                throw new Exception("Cognitive Services API cannot moderate texts longer than 1024 characters");
            }

            using (var stream = GetStreamFromText(text))
            {
                using (var moderatorClient = GetClient())
                {
                    var moderationResult = await moderatorClient.TextModeration.ScreenTextAsync("text/plain", stream, language, classify: true);
                    var result = mapper.Map<Screen, TextModerationResult>(moderationResult);
                    result.Language = language;
                    return result;
                }
            }
        }

        private ContentModeratorClient GetClient()
        {
            var client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(contentModerationConfig.ApiKey), new RetryHandler(contentModerationConfig.MaxRetries));
            client.Endpoint = contentModerationConfig.BaseUrl;

            return client;
        }

        private Stream GetStreamFromText(string text)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
