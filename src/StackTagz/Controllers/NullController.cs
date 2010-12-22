using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackTagz.Controllers
{
    public class NullController : ControllerBase
    {
		public NullController(IErrorController errController)
			: base(errController) {
		}
    }
}
