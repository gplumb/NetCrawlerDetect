using System.Collections.Generic;

namespace NetCrawlerDetect.Fixtures
{
    /// <summary>
    /// Generic provider of data
    /// </summary>
    public abstract class AbstractProvider<T>
    {
        /// <summary>
        /// The data
        /// </summary>
        protected List<T> _data;


        /// <summary>
        /// Get an enumeration of the data
        /// </summary>
        public IEnumerable<T> GetAll()
        {
            return _data;
        }
    }
}
