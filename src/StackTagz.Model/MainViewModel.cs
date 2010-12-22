using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Sites;

namespace StackTagz.Model {
	public class MainViewModel {
		public string ApiKey { get; set; }
		public IEnumerable<SiteInfo> SiteInfoList { get; set; }
		public string ApiVersion { get; set; }
	}
}
