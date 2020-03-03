using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Xunit;

namespace NetCrawlerDetect.Tests
{
    /// <summary>
    /// Ported unit tests with some additional coverage
    /// </summary>
    public class Tests
    {
        CrawlerDetect _detector;


        /// <summary>
        /// Test setup
        /// </summary>
        public Tests()
        {
            _detector = new CrawlerDetect();
        }


        [Fact]
        public void UserAgentsAreBots()
        {
            bool result = false;

            using (var filestream = File.OpenRead(@"crawlers.txt"))
            {
                using (var reader = new StreamReader(filestream))
                {
                    var entry = reader.ReadLine();
                    result = _detector.IsCrawler(entry);

                    Assert.True(result, $"Misidentified bot: {entry}");
                }
            }
        }


        [Fact]
        public void UserAgentsAreDevices()
        {
            bool result = false;

            using (var filestream = File.OpenRead(@"devices.txt"))
            {
                using (var reader = new StreamReader(filestream))
                {
                    var entry = reader.ReadLine();
                    result = _detector.IsCrawler(entry);

                    Assert.False(result, $"Misidentified device: {entry}");
                }
            }
        }


        [Fact]
        public void ReturnCorrectlyMatchedBotName()
        {
            var result = _detector.IsCrawler("Mozilla/5.0 (iPhone; CPU iPhone OS 7_1 like Mac OS X) AppleWebKit (KHTML, like Gecko) Mobile (compatible; Yahoo Ad monitoring; https://help.yahoo.com/kb/yahoo-ad-monitoring-SLN24857.html)");
            Assert.True(result, "Yahoo Ad monitoring IS a bot!");
            Assert.Equal("monitoring", _detector.Matches[0].Value);
        }


        [Fact]
        public void NoMatchesWhenNoBotMatched()
        {
            var result = _detector.IsCrawler("nothing to see here!");
            Assert.False(result);
            Assert.Equal(0, _detector.Matches.Count);
        }


        [Fact]
        public void EmptyUserAgent()
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                var result = _detector.IsCrawler("    \t");
            });
        }


        [Fact]
        public void NullAllTheWay()
        {
            var cd = new CrawlerDetect(null, null);

            Assert.Throws(typeof(ArgumentException), () =>
            {
                var result = cd.IsCrawler(null);
            });
        }


        [Fact]
        public void EmptyAllTheWay()
        {
            var cd = new CrawlerDetect(null, string.Empty);

            Assert.Throws(typeof(ArgumentException), () =>
            {
                var result = cd.IsCrawler(string.Empty);
            });
        }


        [Fact]
        public void InferBotViaUserAgentHeader()
        {
            var headers = new WebHeaderCollection()
            {
                {"accept", "*/*"},
                {"accept-encoding", "DEFLATE"},
                {"cache-control", "no-cache"},
                {"connection", "Keep-Alive"},
                // {"from", "bingbot(at)microsoft.com"},
                {"host", "www.test.com"},
                {"pragma", "no-cache"},
                {"user-agent", "Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)"}
            };

            var cd = new CrawlerDetect(headers);
            var result = cd.IsCrawler();
            Assert.True(result);
        }


        [Fact]
        public void BotUserAgentPassedViaConstructor()
        {
            var cd = new CrawlerDetect(null, "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1 like Mac OS X) AppleWebKit (KHTML, like Gecko) Mobile (compatible; Yahoo Ad monitoring; https://help.yahoo.com/kb/yahoo-ad-monitoring-SLN24857.html)");
            var result = cd.IsCrawler();
            Assert.True(result);
        }


        [Fact]
        public void InferBotViaFromHeader()
        {
            var headers = new WebHeaderCollection()
            {
                {"accept", "*/*"},
                {"accept-encoding", "DEFLATE"},
                {"cache-control", "no-cache"},
                {"connection", "Keep-Alive"},
                {"from", "googlebot(at)googlebot.com"},
                {"host", "www.test.com"},
                {"pragma", "no-cache"},
                {"user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.71 Safari/537.36"}
            };

            var cd = new CrawlerDetect(headers);
            var result = cd.IsCrawler();
            Assert.True(result);
        }


        [Fact]
        public void NoRegexCollisions()
        {
            var crawlers = new Fixtures.Crawlers();

            foreach (var key1 in crawlers.GetAll())
            {
                foreach (var key2 in crawlers.GetAll())
                {
                    if (key1 == key2)
                        continue;

                    var regex = new Regex(key1);
                    Assert.False(regex.IsMatch(key2));
                }
            }
        }
    }
}
