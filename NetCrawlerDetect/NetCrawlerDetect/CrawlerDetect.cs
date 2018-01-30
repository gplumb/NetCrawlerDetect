using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using NetCrawlerDetect.Fixtures;

namespace NetCrawlerDetect
{
    /// <summary>
    /// Crawler Detect
    /// </summary>
    public class CrawlerDetect
    {
        /// <summary>
        /// The user-agent string to test
        /// </summary>
        protected string _userAgent = null;


        /// <summary>
        /// Headers that contain a user agent
        /// </summary>
        protected WebHeaderCollection _headers = new WebHeaderCollection();


        /// <summary>
        /// Store regex matches
        /// </summary>
        protected MatchCollection _matches = null;


        /// <summary>
        /// Crawlers object
        /// </summary>
        protected Crawlers _crawlers = new Crawlers();


        /// <summary>
        /// Exclusions object
        /// </summary>
        protected Exclusions _exclusions = new Exclusions();


        /// <summary>
        /// Headers object
        /// </summary>
        protected Headers _uaHttpHeaders = new Headers();           // TODO: Can this be reduced?


        /// <summary>
        /// A compilation of regex user agent snippets that belong to crawlers
        /// </summary>
        protected static Regex _compiledRegex = null;


        /// <summary>
        /// A compilation of regex user agent snippets to ignore
        /// </summary>
        protected static Regex _compiledExclusions = null;


        /// <summary>
        /// Expose any matches
        /// </summary>
        public MatchCollection Matches => _matches;


        /// <summary>
        /// Constructor
        /// </summary>
        public CrawlerDetect()
            : this(null, null)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public CrawlerDetect(WebHeaderCollection headers, string userAgent = null)
        {
            // Crude way to get an empty match collection
            var regex = new Regex(@".", RegexOptions.Compiled);
            _matches = regex.Matches("");

            if (_compiledRegex == null)
                _compiledRegex = CompileRegex(_crawlers.GetAll());

            if (_compiledExclusions == null)
                _compiledExclusions = CompileRegex(_exclusions.GetAll());

            SetHttpHeaders(headers);
            SetUserAgent(userAgent);
        }


        /// <summary>
        /// Compile the given list of expressions into a single Regex
        /// </summary>
        private Regex CompileRegex(IEnumerable<string> expressions)
        {
            string patterns = "(" + string.Join("|", expressions) + ")";
            return new Regex(patterns, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// Set the HTTP headers to inspect for a crawler user agent
        /// </summary>
        private void SetHttpHeaders(WebHeaderCollection headers)
        {
            // Bail if no headers passed in
            if (headers == null)
                return;

            // Only stash user agent HTTP headers
            foreach (var key in GetUserAgentHeaders())
            {
                if (Array.Exists(headers.AllKeys, (x) => x.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    _headers.Add(key, headers[key]);
                }
            }
        }


        /// <summary>
        /// Return user agent headers
        /// </summary>
        private IEnumerable<string> GetUserAgentHeaders()
        {
            return _uaHttpHeaders.GetAll();
        }


        /// <summary>
        /// Set the user agent string to evaluate
        /// </summary>
        private void SetUserAgent(string userAgent)
        {
            var result = userAgent;

            if (string.IsNullOrEmpty(userAgent))
            {
                var builder = new StringBuilder();

                // Only stash user agent HTTP headers
                foreach (var key in GetUserAgentHeaders())
                {
                    if (Array.Exists(_headers.AllKeys, (x) => x.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        builder.Append(_headers[key]);
                        builder.Append(" ");
                    }
                }

                result = builder.ToString();
            }

            _userAgent = result;
        }


        /// <summary>
        /// Check user agent string against the regex
        /// </summary>
        public bool IsCrawler(string userAgent = null)
        {
            var agent = userAgent ?? _userAgent;

            if (string.IsNullOrWhiteSpace(agent))
                throw new ArgumentException("Cannot test a null or empty user agent!");

            agent = _compiledExclusions.Replace(agent, "");

            if (agent.Length == 0)
                return false;

            _matches = _compiledRegex.Matches(agent);

            return _matches.Count > 0;
        }
    }
}
