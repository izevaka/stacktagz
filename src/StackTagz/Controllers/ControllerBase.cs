using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StackTagz.Controllers
{
    public abstract class ControllerBase : Controller
    {
		protected IErrorController m_errorController;
		public ControllerBase(IErrorController errorController) {
			m_errorController = errorController;
		}

		protected override void HandleUnknownAction(string actionName) {
			m_errorController.InvokeHttp404(HttpContext);
		}
    }
}
