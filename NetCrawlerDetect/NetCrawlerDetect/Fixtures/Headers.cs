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
                @"USER_AGENT",

                // Header can occur on devices using Opera Mini
                @"X_OPERAMINI_PHONE_UA",

                // Vodafone specific header: http://www.seoprinciple.com/mobile-web-community-still-angry-at-vodafone/24/
                @"X_DEVICE_USER_AGENT",
                @"X_ORIGINAL_USER_AGENT",
                @"X_SKYFIRE_PHONE",
                @"X_BOLT_PHONE_UA",
                @"DEVICE_STOCK_UA",
                @"X_UCBROWSER_DEVICE_UA",

                // Sometimes, bots (especially Google) use a genuine user agent, but fill this header in with their email address
                @"FROM",

                // Seen in use by Netsparker
                @"X_SCANNER",
            };
        }
    }
}
