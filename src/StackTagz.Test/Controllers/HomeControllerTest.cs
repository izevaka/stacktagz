using StackTagz.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using Moq;
using StackTagz.Model.Sites;
using StackTagz.Model;

namespace StackTagz.Tests.Controllers
{
 
    
    /// <summary>
    ///This is a test class for HomeControllerTest and is intended
    ///to contain all HomeControllerTest Unit Tests
    ///</summary>
	[TestClass()]
	public class HomeControllerTest {
		private Mock<ISitesRepository> m_SitesRepoMock;
		[TestInitialize]
		public void TestInit() {
			m_SitesRepoMock = new Mock<ISitesRepository>();
		}

		[TestMethod()]
		public void IndexShouldSetApiSettings() {

			HomeController target = new HomeController(m_SitesRepoMock.Object, null); // TODO: Initialize to an appropriate value
			ViewResult actual;
			actual = target.Index() as ViewResult;

			Assert.IsTrue(!string.IsNullOrEmpty(((MainViewModel)actual.ViewData.Model).ApiKey));
			Assert.IsTrue(!string.IsNullOrEmpty(((MainViewModel)actual.ViewData.Model).ApiVersion));
		}

		[TestMethod()]
		public void IndexShouldCallGetSites() {

			HomeController target = new HomeController(m_SitesRepoMock.Object, null); // TODO: Initialize to an appropriate value
			ActionResult actual;
			actual = target.Index();

			m_SitesRepoMock.Verify(s => s.GetSites(), Times.Exactly(1));
		}
	}
}