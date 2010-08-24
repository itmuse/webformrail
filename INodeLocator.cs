using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsistentHashingDemo
{
    public interface INodeLocator<T> where T : INodeIndentity
    {
        /// <summary>
        /// Initializes the locator.
        /// </summary>
        /// <param name="nodes">The nodes defined in the configuration.</param>
        void Initialize(IList<T> nodes);
        /// <summary>
        /// Returns the node the specified key belongs to.
        /// </summary>
        /// <param name="key">The key of the item to be located.</param>
        /// <returns>The T the specifed item belongs to</returns>
        T Locate(string key);
    }
}
