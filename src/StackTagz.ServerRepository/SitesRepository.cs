using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.Model;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;
using StackTagz.Model.Sites;
using StackTagz.ServerRepository.StackClientFactory;
using StackTagz.Infrastructure;

namespace StackTagz.ServerRepository{
	public class SitesRepository : ISitesRepository{
		private IStackClientFactory m_Factory;
		private ICache m_Cache;
		private DateTimeBase m_DateTime;

		public SitesRepository(IStackClientFactory clientFactory, ICache cache, DateTimeBase dateTime) {
			m_Factory = clientFactory;
			m_Cache = cache;
			m_DateTime = dateTime;
		}


		
		#region ISitesRepository Members

		private List<SiteInfo> CreateSiteList() {
			var list = new List<SiteInfo>();

			var authClient = m_Factory.GetAuthClient();
			var sites = authClient.GetSites();
			foreach(var site in sites) {

				list.Add(new SiteInfo {
					Site = new Uri(site.SiteUrl).Host,
					ApiAddress = new Uri(site.ApiEndpoint).Host,
					SiteImage = site.LogoUrl
				});
			}
			return list;
		}

		
		public string GetApiAddress(string site) {
			site = site.ToLower().Trim();
			var address = GetSites().Where(i => i.Site == site || i.ApiAddress == site).Select(i => i.ApiAddress).FirstOrDefault();
			if(address != null)
				address = "http://" + address;

			return address;
		}

		public IEnumerable<SiteInfo> GetSites() {
			var list = m_Cache["sites"] as List<SiteInfo>;
			if(list == null) {
				list = CreateSiteList();
				m_Cache.Add("sites", list, m_DateTime.Now.AddDays(1), null);
			}

			return list;
		}

		#endregion
	}
}
