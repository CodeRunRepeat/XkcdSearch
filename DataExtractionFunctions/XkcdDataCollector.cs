using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XkcdSearch.DataExtractionFunctions
{
    class XkcdDataCollector
    {
        private const string rootUrl = "https://xkcd.com/";
        private const string permalinkHint = "Permanent link to this comic:";
        private const string imagelinkHint = "Image URL (for hotlinking/embedding):";

        private const string permalinkRegex = @"https://xkcd\.com/(\d+)/";
        private const string imagelinkRegex = @"https://imgs\.xkcd\.com/comics/[\w\-\(\)\.]+";

        private const string titleHint = @"<title>";
        private const string titleRegex = @"xkcd:([^\<]+)\</title\>";

        private async Task<string> GetStringFromPage(
            string url,
            string hint,
            string regex,
            int capture)
        {
            using (var client = new HttpClient())
            {
                string result = await client.GetStringAsync(url);
                return GetStringFromContent(result, hint, regex, capture);               
            }
        }

        private string GetStringFromContent(
            string content,
            string hint,
            string regex,
            int capture)
        {
            var text = content.Substring(content.IndexOf(hint) + hint.Length).TrimStart();

            Regex r = new Regex(regex);
            var match = r.Match(text);
            return match.Groups[capture].Value;
        }

        public async Task<string> GetLatestComic()
        {
            return await GetStringFromPage(
                rootUrl,
                permalinkHint,
                permalinkRegex,
                1);
        }

        public async Task<XkcdComicInformation> GetComicInformation(string comicId)
        {
            using (var client = new HttpClient())
            {
                string pageSource = await client.GetStringAsync(rootUrl + comicId);
                var imageUrl = GetStringFromContent(
                    pageSource,
                    imagelinkHint,
                    imagelinkRegex,
                    0);

                var title = GetStringFromContent(
                    pageSource,
                    titleHint,
                    titleRegex,
                    1);

                return new XkcdComicInformation()
                {
                    ComicUrl = imageUrl,
                    Title = title.Trim(),
                };
            }
        }
    }
}
