using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackTagz.Filters;
using System.Web.Mvc;
using Moq;
using System.Web;
using System.Web.Caching;
using System.Net;
using StackTagz.Model;
using StackTagz.Infrastructure;

namespace StackTagz.Tests.Filters {
	[TestClass]
	public class ThrottleAttributeTest {


		Mock<ICache> m_cacheMock;
		Mock<DateTimeBase> m_dtWrapperMock;
		Mock<HttpContextBase> m_ContextMock;
		ActionExecutingContext m_ActionContext;
		Mock<HttpRequestBase> m_RequestMock;
		Mock<HttpResponseBase> m_ResponseMock;

		[TestInitialize]
		public void TestInit() {
			m_cacheMock = new Mock<ICache>();
			m_dtWrapperMock = new Mock<DateTimeBase>();
			m_ActionContext = new ActionExecutingContext();
			m_ContextMock = new Mock<HttpContextBase>();
			m_RequestMock = new Mock<HttpRequestBase>();
			m_ResponseMock = new Mock<HttpResponseBase>();

			m_ActionContext.HttpContext = m_ContextMock.Object;
			m_ContextMock.Setup(c => c.Request).Returns(m_RequestMock.Object);
			m_ContextMock.Setup(c => c.Response).Returns(m_ResponseMock.Object);
			m_RequestMock.Setup(r=>r.UserHostAddress).Returns("ipaddress");
			m_dtWrapperMock.Setup(dt => dt.Now).Returns(new DateTime(2010, 06, 23));
		}

		[TestMethod]
		public void OnActionExecutingShouldAddCacheItemIfNotExists() {

			ThrottleAttribute target = new ThrottleAttribute(m_dtWrapperMock.Object, m_cacheMock.Object) { Requests = 10, Seconds = 20, Name="TestAction" }; // TODO: Initialize to an appropriate value

			m_cacheMock.Setup(c=>c["TestAction-ipaddress"]).Returns(null);
			m_cacheMock.Setup(c =>
				c.Add("TestAction-ipaddress",
					It.Is<ThrottleCacheItem>(tci=>tci.RequestsRemaining == 10),
					new DateTime(2010, 06, 23,0,0,20),
					Cache.NoSlidingExpiration)
				).Returns(null);
			
			target.OnActionExecuting(m_ActionContext);

			m_cacheMock.VerifyAll();
		}
		[TestMethod]
		public void OnActionExecutingShouldDecrementRequests() {

			ThrottleAttribute target = new ThrottleAttribute(m_dtWrapperMock.Object, m_cacheMock.Object) { Requests = 10, Seconds = 20, Name = "TestAction" }; // TODO: Initialize to an appropriate value

			var tci = new ThrottleCacheItem { RequestsRemaining = 10 };
			m_cacheMock.Setup(c => c["TestAction-ipaddress"]).Returns(tci);

			target.OnActionExecuting(m_ActionContext);

			Assert.AreEqual(9, tci.RequestsRemaining);
		}
		[TestMethod]
		public void OnActionExecutingShouldRejectRequestWhenNotRequestsRemaining() {

			ThrottleAttribute target = new ThrottleAttribute(m_dtWrapperMock.Object, m_cacheMock.Object) { Requests = 10, Seconds = 20, Name = "TestAction" }; // TODO: Initialize to an appropriate value

			var tci = new ThrottleCacheItem { RequestsRemaining = 0 };
			m_cacheMock.Setup(c => c["TestAction-ipaddress"]).Returns(tci);
			m_ResponseMock.SetupSet(r => { r.StatusCode = (int)HttpStatusCode.Conflict; });
			target.OnActionExecuting(m_ActionContext);

			m_ResponseMock.VerifyAll();
			Assert.IsNotNull(m_ActionContext.Result);
		}

		[TestMethod]
		public void OnActionExecutingShouldFormatString() {

			ThrottleAttribute target = new ThrottleAttribute(m_dtWrapperMock.Object, m_cacheMock.Object) { 
				Requests = 10, 
				Seconds = 20, 
				Name = "TestAction",
				Message = "Message {n} {s}"
			}; // TODO: Initialize to an appropriate value

			var tci = new ThrottleCacheItem { RequestsRemaining = 0 };
			m_cacheMock.Setup(c => c["TestAction-ipaddress"]).Returns(tci);
			m_ResponseMock.SetupSet(r => { r.StatusCode = (int)HttpStatusCode.Conflict; });
			target.OnActionExecuting(m_ActionContext);

			
			Assert.AreEqual("Message 10 20",((ContentResult)m_ActionContext.Result).Content);
		}

		[TestMethod]
		public void OnActionExecutingShouldDoJsonResultWhenJson() {

			ThrottleAttribute target = new ThrottleAttribute(m_dtWrapperMock.Object, m_cacheMock.Object) {
				Requests = 10,
				Seconds = 20,
				Name = "TestAction",
				Message = "Message {n} {s}",
				Json = true
			}; // TODO: Initialize to an appropriate value

			var tci = new ThrottleCacheItem { RequestsRemaining = 0 };
			m_cacheMock.Setup(c => c["TestAction-ipaddress"]).Returns(tci);
			m_ResponseMock.SetupSet(r => { r.StatusCode = (int)HttpStatusCode.Conflict; });
			target.OnActionExecuting(m_ActionContext);


			Assert.AreEqual("Message 10 20", ((DataResult)((JsonResult)m_ActionContext.Result).Data).message);
			Assert.IsFalse(((DataResult)((JsonResult)m_ActionContext.Result).Data).success);
			Assert.AreEqual(JsonRequestBehavior.AllowGet, ((JsonResult)m_ActionContext.Result).JsonRequestBehavior);
		}
	}
}
