namespace ContentModerator.Moderation
{
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
}
