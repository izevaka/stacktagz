using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackTagz.Controllers {
	public interface IErrorController : IController {
		ActionResult InvokeHttp404(HttpContextBase context);
		ActionResult InvokeHttp404(HttpContextBase context, string message);
		ActionResult InvokeDataHttp404(HttpContextBase context);
		ActionResult InvokeDataHttp404(HttpContextBase context, string message);
	}
}