using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackTagz.Controllers;
using Moq;
using StackTagz.Model;
using StackTagz.Converters;
using Newtonsoft.Json;
using System.Web.Mvc;
using StackTagz.Model.Data;
using StackTagz.Model.Sites;
using System.Web;

namespace StackTagz.Tests.Controllers {
	/// <summary>
	/// Summary description for DataControllerTest
	/// </summary>
	[TestClass]
	public class DataControllerIntegrationTest {
		public DataControllerIntegrationTest() {
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 

		[TestInitialize()]
		public void MyTestInitialize() {
			m_SitesRepoMock = new Mock<ISitesRepository>();
			m_SitesRepoMock.Setup(i=>i.GetApiAddress(It.IsAny<string>())).Returns<string>(prop=>prop);
		}
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		Mock<ISitesRepository> m_SitesRepoMock;
		#endregion

		[TestMethod]
		public void DataControllerShouldCallRepository() {
			
			var timeseriesRepo = new Mock<ISummarizer>();
			timeseriesRepo.Setup(t => t.GetTimeSeries("siteaddr", 123, null, null, Rollup.Weekly)).Returns((TimeseriesResult)null);
			var dataController = new DataController(new JsonConverter[] { }, timeseriesRepo.Object, m_SitesRepoMock.Object, null, null);
			var result = dataController.Index("siteaddr",123);
			
			timeseriesRepo.VerifyAll();
			
		}
		[TestMethod]
		public void DataControllerShouldCallGetApiAddress() {

			var timeseriesRepo = new Mock<ISummarizer>();
			timeseriesRepo.Setup(t => t.GetTimeSeries("siteaddr", 123, null, null, Rollup.Weekly)).Returns((TimeseriesResult)null);
			var dataController = new DataController(new JsonConverter[] { }, timeseriesRepo.Object, m_SitesRepoMock.Object, null, null);
			var result = dataController.Index("siteaddr", 123);

			m_SitesRepoMock.Verify(s => s.GetApiAddress("siteaddr"), Times.Once());

		}

		[TestMethod]
		public void DataControllerShouldReturnContentResult() {

			var timeseriesRepo = new Mock<ISummarizer>();
			timeseriesRepo.Setup(t => t.GetTimeSeries("siteaddr", 123, null, null, Rollup.Weekly)).Returns((TimeseriesResult)null);
			var dataController = new DataController(new JsonConverter[] { }, timeseriesRepo.Object, m_SitesRepoMock.Object, null, null);
			var result = dataController.Index("siteaddr", 123);

			Assert.IsInstanceOfType(result, typeof(ContentResult));

		}
		[TestMethod]
		public void DataControllerShouldCallTimeSeriesFormatter() {

			var tsr = new TimeseriesResult(Rollup.Weekly);

			tsr.Timeseries.Add("Label1", new Timeseries(Rollup.Weekly));


			var timeseriesRepo = new Mock<ISummarizer>();
			timeseriesRepo.Setup(t => t.GetTimeSeries("siteaddr", 123, null, null, Rollup.Weekly)).Returns(tsr);

			var converter = new Mock<JsonConverter>();
			converter.Setup(c=>c.CanConvert(It.IsAny<Type>())).Returns(false);
			converter.Setup(c => c.CanConvert(typeof(TimeseriesResult))).Returns(true);

			var dataController = new DataController(new[] { converter.Object }, timeseriesRepo.Object, m_SitesRepoMock.Object, null, null);
			var result = dataController.Index("siteaddr", 123) as ContentResult;

			Assert.AreEqual("application/json", result.ContentType);

			converter.Verify(c => c.WriteJson(It.IsAny<JsonWriter>(), It.IsAny<TimeseriesResult>(), It.IsAny<JsonSerializer>()), Times.Exactly(1));
		}

		[TestMethod]
		public void GraphShouldDo404WhenNoSite() {

			var errorController = new Mock<IErrorController>();
			
			var timeseriesRepo = new Mock<ISummarizer>();
			timeseriesRepo.Setup(t => t.GetTimeSeries("siteaddr", 123, null, null, Rollup.Weekly)).Returns((TimeseriesResult)null);
			var dataController = new DataController(new JsonConverter[] { }, timeseriesRepo.Object, m_SitesRepoMock.Object, errorController.Object, null);
			m_SitesRepoMock.Setup(s => s.GetApiAddress("siteaddr")).Returns((string)null);
						
			var result = dataController.Index("siteaddr", 123);

			errorController.Verify(e => e.InvokeDataHttp404(It.IsAny<HttpContextBase>(), It.IsAny<string>()), Times.Once());
		}
	}
}
