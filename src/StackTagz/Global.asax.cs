using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StackTagz.Controllers;
using Newtonsoft.Json;
using StackTagz.Converters;
using System.Reflection;
using StackTagz.Infrastructure.DI;
using StackTagz.Infrastructure;
using StackTagz.ServerRepository;
using StackTagz.ServerRepository.StackClientFactory;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.Model.Sites;
using StackTagz.Model.Data;
using StackTagz.SqlRepository.EF;
using StackTagz.Model.Users;
using System.Web.Configuration;
using System.Configuration;
using Common.Logging;
using StackTagz.Filters;
using StackTagz.Summarizer;
using StackTagz.ServerRepository.Answers;
using StackTagz.ServerRepository.Questions;
using StackTagz.Model.Comments;
using StackTagz.ServerRepository.Comments;


namespace StackTagz {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	

	public class MvcApplication : System.Web.HttpApplication {

		private readonly IDIContainer m_Container;

		public MvcApplication() {
			m_Container = new DIContainer();
			var configSection = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");

			if (!configSection.Debug)
				Error += new EventHandler(MvcApplication_Error);
			
			BeginRequest += new EventHandler(MvcApplication_BeginRequest);

			this.GetLogger().Trace(m => m("Application startup"));
		}

		void MvcApplication_BeginRequest(object sender, EventArgs e) {
			//<!--@ip_address, @referrer, @url-->
			log4net.ThreadContext.Properties["ip_address"] = Request.UserHostAddress;
			log4net.ThreadContext.Properties["referrer"] = Request.UrlReferrer;
			log4net.ThreadContext.Properties["url"] = Request.Url.OriginalString;
		}

		void MvcApplication_Error(object sender, EventArgs e) {
			
			var exception = Server.GetLastError();
			var httpException = exception as HttpException;
			this.GetLogger().Error("Unhandled error",exception);

			var errorRoute = new RouteData();
			errorRoute.Values.Add("controller", "Error");

			var contextBase = new HttpContextWrapper(Context);
			var routeData = RouteTable.Routes.GetRouteData(contextBase);
			var isData = (string)routeData.Values["controller"] == "Data" || (string)routeData.Values["action"] == "DataNotFound";
			var notFoundAction = isData ? "DataNotFound" : "NotFound";
			var errorAction = isData ? "DataError" : "ApplicationError";
			
			if(httpException != null) {
				//Unsage characters in the URL
				if(httpException.GetHttpCode() == 400) {
					errorRoute.Values.Add("action", notFoundAction);
					errorRoute.Values.Add("path", Context.Request.Url.OriginalString);
					errorRoute.Values.Add("referrer", Context.Request.UrlReferrer);
				}
				if(httpException.GetHttpCode() == 500) {
					errorRoute.Values.Add("action", errorAction);
				}
			} else {
				errorRoute.Values.Add("action", errorAction);
			}
			
			IController errorController = ControllerBuilder.Current.GetControllerFactory().CreateController(null, "error");
			try {
				errorController.Execute(new RequestContext(
					 contextBase, errorRoute));
			} catch(Exception) {
				//If we are here it means that the URL is unsafe and the only way to handle it gracefully is to redirect.
				Response.Redirect("~/Error/" + notFoundAction);
			}
			Server.ClearError();
		}
		
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Graph", // Route name
				"data/graph/{site}/{userid}", // URL with parameters
				new { controller = "Data", action = "Index" }, new { userid = @"\d{1,}", site = @"[a-zA-Z0-9\.]*"}  // Parameter defaults
			);
			routes.MapRoute(
				"Users", // Route name
				"data/users/{site}/{user}", // URL with parameters
				new { controller = "Data", action = "SearchUsers", user = "user" }, new { site = @"[a-zA-Z0-9\.]*" }  // Parameter defaults
			);

			routes.MapRoute(
				"Data catchall", // Route name
				"data/{*path}", // URL with parameters
				new { controller = "Error", action = "DataNotFound" }  // Parameter defaults
			);


			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new { } // Parameter defaults
			);

			routes.MapRoute(
				"Catch all", // Route name
				"{*path}", // URL with parameters
				new { controller = "Error", action = "NotFound" }, new { } // Parameter defaults
			);


		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
			//RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);

			RegisterContainer();

			ControllerBuilder.Current.SetControllerFactory(m_Container.Resolve<IControllerFactory>());
		}

		private void RegisterContainer() {

			m_Container.RegisterInstance<IDIContainer>(m_Container);
			m_Container.RegisterInstance<IControllerFactory>(new ControllerFactory(m_Container));
			m_Container.RegisterInstance<JsonConverter[]>(new []{new TimeseriesResultConverter()});
			m_Container.RegisterType<ISummarizer, Summarizer.Summarizer>();
			//m_Container.RegisterType<ITimeseriesRepository, DummyTimeseriesRepository>();

			m_Container.RegisterType<IQuestionsRepository, QuestionsRepository>();
			m_Container.RegisterType<IAnswersRepository, AnswersRepository>();
			m_Container.RegisterType<IStackClientFactory, StackClientFactory>();
			m_Container.RegisterType<ISitesRepository, SitesRepository>();
			//m_Container.RegisterType<ISitesRepository, DummySitesRepository>();
			m_Container.RegisterType<IPersistTimeseriesRepository, SqlRepository.TimeseriesRepository>();
			m_Container.RegisterType<IPersistQuestionsRepository, SqlRepository.QuestionsRepository>();
			m_Container.RegisterType<ICommentsRepository, CommentsRepository>();
			m_Container.RegisterType<ICommentQuestionProcessor, CommentQuestionProcessor>();
			m_Container.RegisterType<IStackTagzContext, SqlRepository.EF.StackTagzContext>();
			m_Container.RegisterType<IErrorController, ErrorController>();
			m_Container.RegisterType<IUsersRepository, UsersRepository>();
			//m_Container.RegisterType<IUsersRepository, DummyUsersRepository>();

			m_Container.RegisterType<ICache, CacheWrapper>();
			
		
			var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.ToLower().EndsWith("controller") && typeof(IController).IsAssignableFrom(t));
			
			foreach(var type in types) {
				m_Container.RegisterType(typeof(IController), type, type.Name.ToLower().Replace("controller", ""));
			}
		}
	}
}