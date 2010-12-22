using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model;

namespace StackTagz.Model.Sites{
	public interface ISitesRepository {
		IEnumerable<SiteInfo> GetSites();
		string GetApiAddress(string site);
	}
}
