# NetCrawlerDetect
A .net standard port of JayBizzle's CrawlerDetect project [(https://github.com/JayBizzle/Crawler-Detect)](https://github.com/JayBizzle/Crawler-Detect).

## About NetCrawlerDetect

NetCrawlerDetect is a .net standard class for detecting bots/crawlers/spiders via the user agent and/or http "from" header. Currently able to detect 1,000's of bots/spiders/crawlers.

### Usage
Like its' originator, you can either pass in a collection of web headers (from which the user agent will be extracted) or pass in the user agent string directly.

The simplest use of this object is as follows:

```csharp
// Pass in the user agent directly
var detector = new CrawlerDetect();
var result = detector.IsCrawler("Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)");

// Do we have a bot/crawler/spider?
if(result == true)
{
	// Yes. Fetch the name of the bot (optional)
	var bot = detector.Matches[0].Value;
}
```

### Contributing
If you find a bot/spider/crawler user agent that NetCrawlerDetect fails to detect, please submit a pull request with the regex pattern added to the `_data` List in `Fixtures/Crawlers.cs` and add the failing user agent to `NetCrawlerDetect.Tests/crawlers.txt`.

Please also consider submitting a pull request to our parent project :)