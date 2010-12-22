using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackTagz.Infrastructure;
using System.Reflection;
using StackTagz.Controllers;
using StackTagz.Infrastructure.DI;

namespace StackTagz {
	public class ControllerFactory : IControllerFactory {
		public ControllerFactory(IDIContainer container) {
			m_Container = container;
		}
		
		#region IControllerFactory Members

		private readonly IDIContainer m_Container;

		public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName) {
			if(!m_Container.IsRegistered<IController>(controllerName.ToLower())) {
				var nullController = m_Container.Resolve<IController>("null");
				return nullController;
			}
			
			return m_Container.Resolve<IController>(controllerName.ToLower());
		}

		public void ReleaseController(IController controller) {
			controller = null;
		}

		#endregion
	}
}