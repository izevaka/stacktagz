using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;

namespace StackTagz.Infrastructure {

	public static class LoggingExtensions {
		private static ILog g_Log;
		
		public static void SetLogger(ILog log) {
			g_Log = log;
		}
		
		public static ILog GetLogger<T>(this T obj) where T : class {
			return g_Log ?? LogManager.GetLogger<T>();
		}
	}
	
	
	public static class ObjectExtensions {

		public static string FormatString(this string val, params object[] args) {
			return string.Format(val, args);
		}

		public static bool IsEqual<T>(this IEnumerable<T> l, IEnumerable<T> r) {
			if(r == null)
				return false;
			if(Object.ReferenceEquals(l, r))
				return true;
			using(var lenum = l.GetEnumerator()) {
				using(var renum = r.GetEnumerator()) {

					do {
						var lend = lenum.MoveNext();
						var rend = renum.MoveNext();

						if(lend != rend)
							return false;

						if(!lend)
							break;

						if(!lenum.Current.Equals(renum.Current))
							return false;

					} while(true);
				}
			}
			return true;
		}
	}
}
