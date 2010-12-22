using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;
using StackTagz.Infrastructure;

namespace StackTagz {
	public class CacheWrapper : ICache {
		private readonly Cache m_Cache;

		public CacheWrapper() {
			m_Cache = HttpContext.Current.Cache;
		}


		#region ICacheWrapper Members

		public object this[string key] {
			get { return m_Cache[key]; }
		}

		public object Add(string key, object value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration) {
			return m_Cache.Add(key, value, null, absoluteExpiration ?? Cache.NoAbsoluteExpiration, slidingExpiration ?? Cache.NoSlidingExpiration, CacheItemPriority.Low, null);
		}

		#endregion
	}
}

