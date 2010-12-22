using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Sites {
	public class SiteInfo {
		public string Site { get; set; }
		public string ApiAddress { get; set; }
		public string SiteImage { get; set; }

		public override bool Equals(object obj) {

			var other = obj as SiteInfo;
			if(other == null) {
				return false;
			}

			return
				Site == other.Site &&
				ApiAddress == other.ApiAddress &&
				SiteImage == other.SiteImage;
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + (Site != null ? Site.GetHashCode()        : 0);
			hash = hash * 23 + (Site != null ? ApiAddress.GetHashCode()	: 0 );
			hash = hash * 23 + (Site != null ? SiteImage.GetHashCode() : 0  );
			return hash;
		}
    
    
	}
}
