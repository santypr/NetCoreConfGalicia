using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using System.Collections.Generic;

namespace ContentModerator.Moderation
{
    public class TextModerationResult : ModerationResultBase
    {
        public Classification Classification { get; set; }
        public List<DetectedTerms> Terms { get; set; }
    }
}
