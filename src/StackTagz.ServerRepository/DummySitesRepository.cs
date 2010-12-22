using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Sites;

namespace StackTagz.ServerRepository {
	public class DummySitesRepository : ISitesRepository{
		#region ISitesRepository Members

		public IEnumerable<SiteInfo> GetSites() {
			return new List<SiteInfo> { new SiteInfo { ApiAddress = "http://api.stackoverflow.com", Site="stackoverflow.com"} };
		}

		public string GetApiAddress(string site) {
			return "http://api.stackoverflow.com";
		}

		#endregion
	}
}
