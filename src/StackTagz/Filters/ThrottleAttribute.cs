using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Caching;
using StackTagz.Model;
using StackTagz.Infrastructure;

namespace StackTagz.Filters {

	public class ThrottleCacheItem {
		public int RequestsRemaining { get; set; }
	}
	
	/// <summary>
	/// Decorates any MVC route that needs to have client requests limited by time.
	/// Based on http://stackoverflow.com/questions/33969/best-way-to-implement-request-throttling-in-asp-net-mvc
	/// </summary>
	/// <remarks>
	/// Uses the current System.Web.Caching.Cache to store each client request to the decorated route.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class ThrottleAttribute : ActionFilterAttribute {
		public ThrottleAttribute(DateTimeBase dtWrapper, ICache cacheWrapper) {
			m_Cache = cacheWrapper;
			m_DateTimeWrapper = dtWrapper;
		}
		public ThrottleAttribute() {

		}

		/// <summary>
		/// A unique name for this Throttle.
		/// </summary>
		/// <remarks>
		/// We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1"
		/// </remarks>
		public string Name { get; set; }

		/// <summary>
		/// The number of seconds clients must wait before executing this decorated route again.
		/// </summary>
		public int Seconds { get; set; }

		/// <summary>
		/// Number of requests a client is allowed to execute in the amount of time specified by Seconds
		/// </summary>
		public int Requests { get; set; }

		public bool Json { get; set; }

		/// <summary>
		/// Wrapper around datetime for testing purposes
		/// </summary>
		DateTimeBase GetDateTimeWrapper() {
			if (m_DateTimeWrapper == null)
				m_DateTimeWrapper = new DateTimeBase();

			return m_DateTimeWrapper;
		}
		private DateTimeBase m_DateTimeWrapper;

		ICache GetCache(Cache c) {
			if(m_Cache == null)
				m_Cache = new CacheWrapper();

			return m_Cache;
		}
		private ICache m_Cache;
		/// <summary>
		/// A text message that will be sent to the client upon throttling.  You can include the token {n} to
		/// show this.Seconds in the message, e.g. "Wait {n} seconds before trying again".
		/// </summary>
		public string Message { get; set; }

		public override void OnActionExecuting(ActionExecutingContext c) {
			var key = string.Concat(Name, "-", c.HttpContext.Request.UserHostAddress);
			var allowExecute = false;
			var cache = GetCache(c.HttpContext.Cache);
			var item = cache[key] as ThrottleCacheItem;

			if(item == null) {
				cache.Add(key,
					new ThrottleCacheItem { RequestsRemaining = Requests },
					GetDateTimeWrapper().Now.AddSeconds(Seconds),
					Cache.NoSlidingExpiration); // no callback

				allowExecute = true;
			} else {
				if(item.RequestsRemaining > 0) {
					item.RequestsRemaining--;
					allowExecute = true;
				}
			}

			if(!allowExecute) {
				if(String.IsNullOrEmpty(Message))
					Message = "You may only perform this action {n} times every {s} seconds.";

				var content = Message.Replace("{s}", Seconds.ToString()).Replace("{n}", Requests.ToString());
				if (!Json)
					c.Result = new ContentResult { Content = content };
				else
					c.Result = new JsonResult { Data = new DataResult { success = false, message = content }, JsonRequestBehavior=JsonRequestBehavior.AllowGet};

				// see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
				c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
			}
		}
	}
}