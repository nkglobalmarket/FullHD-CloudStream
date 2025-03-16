// CloudStream plugin template
using System.Net;
using System.Text.RegularExpressions;

public class FullHDPlugin : MainAPI
{
    public override string Name => "FullHD Film İzle";
    public override string Description => "Fullhdfilmizlesene.de kaynağından filmleri çeker.";
    public override string Author => "Kemal Abi";
    public override string Language => "tr";
    public override string Domain => "fullhdfilmizlesene.de";

    public override List<MainPageSection> GetMainPage()
    {
        var sections = new List<MainPageSection>();

        // Aksiyon filmleri çekiliyor
        var aksiyonUrl = "https://www.fullhdfilmizlesene.de/film-turleri/aksiyon/";
        var aksiyonDoc = GetWebView(aksiyonUrl);

        var aksiyonItems = aksiyonDoc.Select("div.poster").Select(element =>
        {
            var link = element.SelectFirst("a")?.GetAttribute("href");
            var title = element.SelectFirst("img")?.GetAttribute("alt") ?? "Film";
            var poster = element.SelectFirst("img")?.GetAttribute("src");

            return new SearchResult
            {
                Name = title,
                Url = link,
                PosterUrl = poster,
                Type = "movie"
            };
        }).ToList();

        sections.Add(new MainPageSection
        {
            Title = "Aksiyon Filmleri",
            Items = aksiyonItems
        });

        return sections;
    }

    public override Dictionary<string, string> LoadVideoUrls(string url)
    {
        var doc = GetWebView(url);

        // Sitedeki iframe çekiliyor
        var iframeSrc = doc.SelectFirst("iframe")?.GetAttribute("src");

        if (string.IsNullOrEmpty(iframeSrc))
        {
            return null;
        }

        // Şu an iframe linkini direkt video player olarak ekliyoruz
        return new Dictionary<string, string>
        {
            { "FullHD Player", iframeSrc }
        };
    }
}
