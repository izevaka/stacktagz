using StackTagz.ServerRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.ServerRepository.StackClientFactory;
using StackTagz.Model.Users;
using System.Collections.Generic;
using Moq;
using Stacky;

namespace StackTagz.ServerRepository.Test
{
	[TestClass()]
	public class UsersRepositoryTest : ServerRepositoryTestBase {

		[TestInitialize]
		public void TestInitialize() {
			InitFactory();
		}

		[TestMethod()]
		public void SearchShouldCallRespository() {

			StackClient.Setup(c=>c.GetUsers(UserSort.Reputation, SortDirection.Descending, 1, ApiSettings.MaxUsers, "searchString",null,null,null,null)).Returns(new PagedList<User>(new List<User>{new User()}));
			UsersRepository target = new UsersRepository(StackClientFactoryMock.Object); 
			
			string site = "siteaddr"; 
			string userString = "searchString"; 
			
			var actual = target.Search(site, userString);
			StackClient.VerifyAll();
		}
	}
}
