using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StackTagz.Infrastructure;
using System.Linq;
using System.Collections.Generic;
using Stacky;
using StackTagz.Model.Sites;
using Moq;

namespace StackTagz.ServerRepository.Test
{

	[TestClass()]
	public class SitesRepositoryTest : ServerRepositoryTestBase {
		private Mock<ICache> m_CacheMock;
		private Mock<DateTimeBase> m_DateTimeMock;


		#region Additional test attributes
		
		//Use TestInitialize to run code before running each test
		[TestInitialize()]
		public void MyTestInitialize()
		{
			InitFactory();
			StackAuthClient.Setup(sac=>sac.GetSites()).Returns(new []{
				new Site{ApiEndpoint = "http://api.stackoverflow.com", SiteUrl="http://stackoverflow.com", LogoUrl = "so.png"},
				new Site{ApiEndpoint = "http://api.serverfault.com", SiteUrl="http://serverfault.com", LogoUrl = "sf.png"}
			});
			m_CacheMock = new Mock<ICache>();
			m_DateTimeMock = new Mock<DateTimeBase>();
		}
		#endregion

		[TestMethod]
		public void GetSitesShouldCacheResult() {
			m_CacheMock.Setup(c => c.Add("sites", It.IsAny<IEnumerable<SiteInfo>>(), new DateTime(2010, 06, 24, 1, 2, 3), null));
			m_DateTimeMock.Setup(dt => dt.Now).Returns(new DateTime(2010, 06, 23, 1, 2, 3));
			
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);
			target.GetSites();

			m_CacheMock.VerifyAll();
		}

		[TestMethod]
		public void GetSitesShouldRetrieveCachedResult() {
			var sites = new List<SiteInfo>();
			m_CacheMock.Setup(c=>c["sites"]).Returns(sites);

			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);
			var actual = target.GetSites();

			m_CacheMock.Verify(c => c.Add(It.IsAny<string>(), It.IsAny<IEnumerable<SiteInfo>>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>()), Times.Never());
			Assert.AreSame(sites, actual);
		}


		[TestMethod]
		public void GetSitesShouldCallGetSites() {
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);
			target.GetSites();
			base.StackAuthClient.VerifyAll();
		}
		[TestMethod]
		public void GetSitesShouldCallProcessSites() {
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);

			Assert.AreEqual(2, target.GetSites().Count());
			CollectionAssert.AreEqual(new [] { 
					new SiteInfo { ApiAddress = "api.stackoverflow.com",Site = "stackoverflow.com", SiteImage = "so.png" } ,
					new SiteInfo { ApiAddress = "api.serverfault.com",Site = "serverfault.com", SiteImage = "sf.png" } 
				}, 
				target.GetSites().ToList());
		}

		[TestMethod]
		public void GetApiAddressShouldMatchExistingSite() {
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);

			var apiaddr = target.GetApiAddress("stackoverflow.com");
			Assert.AreEqual("http://api.stackoverflow.com", apiaddr);
		}
		[TestMethod]
		public void GetApiAddressShouldMatchMixedCaseAddress() {
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);

			var apiaddr = target.GetApiAddress(" sTACKOVERFLOw.com\t");
			Assert.AreEqual("http://api.stackoverflow.com", apiaddr);
		}
		[TestMethod]
		public void GetApiAddressShouldMatchApiAddress() {
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);

			var apiaddr = target.GetApiAddress("api.stackoverflow.com");
			Assert.AreEqual("http://api.stackoverflow.com", apiaddr);
		}
		[TestMethod]
		public void GetApiAddressShouldReturnNullForNonExistantSite() {
			SitesRepository target = new SitesRepository(StackClientFactoryMock.Object, m_CacheMock.Object, m_DateTimeMock.Object);

			var apiaddr = target.GetApiAddress("foobar");
			Assert.IsNull( apiaddr);
		}
	}
}
