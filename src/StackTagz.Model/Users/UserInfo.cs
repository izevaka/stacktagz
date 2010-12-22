using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Users {
	public class UserInfo {
		public string Name { get; set; }
		public string GravatarUrl { get; set; }
		public int GoldBadges { get; set; }
		public int SilverBadges { get; set; }
		public int BronzeBadges { get; set; }
		public int Rep { get; set; }
		public long UserId { get; set; }
		public string RepString { 
			get {
				return FormatNumber(Rep);
			} 
		}

		static string FormatNumber(int num) {
			if(num >= 100000)
				return FormatNumber(num / 1000) + "K";
			if(num >= 10000) {
				return (num / 1000D).ToString("0.#") + "K";
			}
			return num.ToString("#,0");
		}

		public override bool Equals(object obj) {

			var other = obj as UserInfo;
			if(other == null) {
				return false;
			}

			return
				Name == other.Name &&
				//Not settable in the stacky client, inited automatically, ignore in comparison
				//GravatarUrl == other.GravatarUrl &&
				GoldBadges == other.GoldBadges &&
				SilverBadges == other.SilverBadges &&
				BronzeBadges == other.BronzeBadges &&
				Rep == other.Rep;
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + (Name != null ? Name.GetHashCode() : 0);
			//Not settable in the stacky client, inited automatically, ignore in comparison
			//hash = hash * 23 + (GravatarUrl != null ? GravatarUrl.GetHashCode() : 0);
			hash = hash * 23 + GoldBadges.GetHashCode();
			hash = hash * 23 + SilverBadges.GetHashCode();
			hash = hash * 23 + BronzeBadges.GetHashCode();
			hash = hash * 23 + Rep.GetHashCode();
			return hash;
		}
    
    
	}
}
