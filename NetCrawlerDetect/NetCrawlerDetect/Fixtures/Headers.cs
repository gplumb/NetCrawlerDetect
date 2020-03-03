using System.Collections.Generic;

namespace NetCrawlerDetect.Fixtures
{
    /// <summary>
    /// A provider of HTTP headers that may contain a user-agent string
    /// </summary>
    public class Headers : AbstractProvider<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Headers()
        {
            // All possible HTTP headers that represent the user agent string
            _data = new List<string>()
            {
                // The default User-Agent string
                @"user-agent",

                // Header can occur on devices using Opera Mini
                @"x-operamini-phone-ua",

                // Vodafone specific header: http://www.seoprinciple.com/mobile-web-community-still-angry-at-vodafone/24/
                @"x-device-user-agent",
                @"x-original-user-agent",
                @"x-skyfire-phone",
                @"x-bolt-phone-ua",
                @"device-stock-ua",
                @"x-ucbrowser-device-ua",

                // Sometimes, bots (especially Google) use a genuine user agent, but fill this header in with their email address
                @"from",

                // Seen in use by Netsparker
                @"x-scanner",
            };
        }
    }
}
