using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackTagz.Model;
using Newtonsoft.Json;
using StackTagz.Converters;
using StackTagz.Model.Data;
using StackTagz.Model.Sites;
using StackTagz.Model.Users;
using StackTagz.Filters;

namespace StackTagz.Controllers
{
    public class DataController : ControllerBase
    {

		public DataController(JsonConverter[] converters, ISummarizer repository, ISitesRepository sitesRepository, IErrorController errorController, IUsersRepository userRepository)
			: base(errorController) {
			m_Converters = converters;
			m_TimeSeriesRepository = repository;
			m_SiteRepository = sitesRepository;
			m_userRepository = userRepository;
		}

		//Dependencies
		private readonly JsonConverter[] m_Converters;
		private readonly ISummarizer m_TimeSeriesRepository;
		private readonly ISitesRepository m_SiteRepository;
		private IUsersRepository m_userRepository;

        //
        // GET: /data/graph/13
		[Throttle(Name="graph", Order=1, Requests=20, Seconds=60,Json=true)]
        public ActionResult Index(string site, int userid)
        {
			site = m_SiteRepository.GetApiAddress(site);
			if (site == null)
				return m_errorController.InvokeDataHttp404(HttpContext, "Invalid site api address");
			
			var data = m_TimeSeriesRepository.GetTimeSeries(site, userid, null, null,Rollup.Weekly);
			return Content(JsonConvert.SerializeObject(data, m_Converters), "application/json");
        }
    }
}
