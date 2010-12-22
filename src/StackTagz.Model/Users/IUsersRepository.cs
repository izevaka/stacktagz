using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Users {
	public interface IUsersRepository {
		IEnumerable<UserInfo> Search(string site, string userString);
	}
}
