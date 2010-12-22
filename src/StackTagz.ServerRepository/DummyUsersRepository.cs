using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Users;

namespace StackTagz.ServerRepository {
	public class DummyUsersRepository : IUsersRepository{
		#region IUsersRepository Members

		public IEnumerable<UserInfo> Search(string site, string userString) {
			return new List<UserInfo>{
				new UserInfo{UserId = 1, Rep = 6346, GoldBadges = 1, SilverBadges = 2, BronzeBadges = 12, GravatarUrl = "/stacktagz/content/gravatar1.jpeg",Name = "Jonh Smith"}, 
				new UserInfo{UserId = 2, Rep = 6346, GoldBadges = 1, SilverBadges = 2, BronzeBadges = 12, GravatarUrl = "/stacktagz/content/gravatar2.png",Name = "Jane Doe"},
				new UserInfo{UserId = 3, Rep = 6346, GoldBadges = 1, SilverBadges = 2, BronzeBadges = 12, GravatarUrl = "/stacktagz/content/gravatar1.png",Name = "Jane Doe"}
			}.Where(ui=>ui.Name.Contains(userString));
		}

		#endregion
	}
}
