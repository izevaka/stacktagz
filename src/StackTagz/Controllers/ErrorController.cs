using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.Routing;
using StackTagz.Model;
using StackTagz.Infrastructure;

namespace StackTagz.Controllers
{
	public class ErrorModel {
		public string Url { get; set; }
		public string Message { get; set; }
	}
    public class ErrorController : Controller, IErrorController
    {
		protected override void HandleUnknownAction(string actionName) {
			new ErrorController().InvokeHttp404(HttpContext);
		}

		//Shows some UI, 500
        public ActionResult ApplicationError()
        {
			Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("ApplicationError");
        }
		//Shows some UI, 404
		public ActionResult NotFound(string path,string referrer) {
			this.GetLogger().Warn(m=>m("Page not found"));
			Response.StatusCode = (int)HttpStatusCode.NotFound;
			return View("NotFound", new ErrorModel { Url = path });
		}
		//API method, no need to show UI
		public ActionResult DataNotFound(string path, string message) {
			Response.StatusCode = (int)HttpStatusCode.NotFound;
			Response.TrySkipIisCustomErrors = true;
			return Json(new DataResult { success = false, message = message }, "application/JSON", JsonRequestBehavior.AllowGet);
		}
		//API method, no need to show UI
		
		public ActionResult DataError() {
			Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			Response.TrySkipIisCustomErrors = true;
			return Json(new DataResult { success = false, message = "Server error" },"application/JSON", JsonRequestBehavior.AllowGet);
		}

		private ActionResult InvokeHttp404(string action, HttpContextBase httpContext, string message) {
			var errorRoute = new RouteData();
			errorRoute.Values.Add("controller", "Error");
			errorRoute.Values.Add("action", action);
			errorRoute.Values.Add("path", httpContext.Request.Url.OriginalString);
			errorRoute.Values.Add("referrer", httpContext.Request.UrlReferrer);
			errorRoute.Values.Add("message", message);
			Execute(new RequestContext(
				 httpContext, errorRoute));

			return new EmptyResult();
		}

		#region IErrorController Members
		public ActionResult InvokeHttp404(HttpContextBase httpContext) {
			return InvokeHttp404("NotFound", httpContext, null);
		}


		public ActionResult InvokeHttp404(HttpContextBase context, string message) {
			return InvokeHttp404("NotFound", context, message);
		}

		public ActionResult InvokeDataHttp404(HttpContextBase context) {
			return InvokeHttp404("DataNotFound", context, null);
		}

		public ActionResult InvokeDataHttp404(HttpContextBase context, string message) {
			return InvokeHttp404("DataNotFound", context, message);
		}

		#endregion
	}
}
