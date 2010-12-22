using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackTagz.Model;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.Model.Sites;
using StackTagz.Filters;
using StackTagz.ServerRepository;

namespace StackTagz.Controllers {
	public class HomeController : ControllerBase {

		ISitesRepository m_SitesRepository;
		public HomeController(ISitesRepository sitesRepo, IErrorController errorController)
			: base(errorController) {
			m_SitesRepository = sitesRepo;
		}

		public ActionResult Index() {
			var sites = m_SitesRepository.GetSites();

			return View(new MainViewModel {
				ApiKey = ApiSettings.Key,
				SiteInfoList = sites,
				ApiVersion = ApiSettings.Version
			});
		}

		public ActionResult About() {
			return View();
		}
	}
}