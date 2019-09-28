using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentModerator.Moderation
{
    public class ImageModerationResult : ModerationResultBase
    {
        public bool? Result { get; set; }
        public double? AdultClassificationScore { get; set; }
        public bool? IsImageAdultClassified { get; set; }
        public double? RacyClassificationScore { get; set; }
        public bool? IsImageRacyClassified { get; set; }
        public TextModerationResult TextModerationResult { get; set; }
    }

    public class ImageProperties
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public double Weight { get; set; }
    }
}
