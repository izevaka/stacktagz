using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Infrastructure {
	public interface ICache {
		object this[string key] { get; }
		object Add(string key, object value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration);
	}
}
