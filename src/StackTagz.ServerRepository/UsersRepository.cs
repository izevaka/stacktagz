using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Users;
using StackTagz.ServerRepository.StackClientFactory;

namespace StackTagz.ServerRepository {
	public class UsersRepository : ServerRepositoryBase, IUsersRepository {

		public UsersRepository(IStackClientFactory factory) : base(factory){

		}

		#region IUsersRepository Members

		public IEnumerable<StackTagz.Model.Users.UserInfo> Search(string site, string userString) {
			var client = ClientFactory.GetClient(site);

			var users = client.GetUsers(filter: userString, pageSize: ApiSettings.MaxUsers, page: 1);
			return users.Select(u=>u.ToUserInfo());
		}

		#endregion
	}
}
