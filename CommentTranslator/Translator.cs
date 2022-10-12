using Amazon.Translate;
using Amazon.Translate.Model;

namespace CommentTranslator
{
    public class Translator
    {
        private IAmazonTranslate _amazonTranslate { get; set; }

        public Translator(IAmazonTranslate amazonTranslate)
        {
            _amazonTranslate = amazonTranslate;
        }

        public async Task<TranslateTextResponse> ToEn(string frText)
        {
            var request = new TranslateTextRequest
            {
                SourceLanguageCode = "auto",
                TargetLanguageCode = "en",
                Text = frText
            };
            return await _amazonTranslate.TranslateTextAsync(request);
        }
    }
}