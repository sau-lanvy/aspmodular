using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspModular.Infrastructure.Extensions
{
    public static class EnumerableExtension
    {
        /// <summary>
	    /// Determines whether the collection is null or contains no elements.
	    /// </summary>
	    /// <typeparam name="T">The IEnumerable type.</typeparam>
	    /// <param name="enumerable">The enumerable, which may be null or empty.</param>
	    /// <returns>
	    ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
	    /// </returns>
	    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            return !enumerable.Any();
        }
    }
}
