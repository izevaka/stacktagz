using StackTagz;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using System.Web.Mvc;
using Moq;
using StackTagz.Controllers;
using StackTagz.Infrastructure.DI;

namespace StackTagz.Tests
{
 	[TestClass()]
	public class ControllerFactoryTest {


		private TestContext testContextInstance;

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
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		[TestMethod()]
		public void CreateControllerShouldCallResolveWithControllerName() {

			var containerMock = new Mock<IDIContainer>();
			ControllerFactory target = new ControllerFactory(containerMock.Object); // TODO: Initialize to an appropriate value
			RequestContext requestContext = null; // TODO: Initialize to an appropriate value
			string controllerName = "foobar"; // TODO: Initialize to an appropriate value

			containerMock.Setup(c => c.Resolve<IController>("foobar")).Returns(default(IController));
			containerMock.Setup(c => c.IsRegistered<IController>("foobar")).Returns(true);

			IController actual;
			actual = target.CreateController(requestContext, controllerName);

			containerMock.VerifyAll();
		}

		[TestMethod()]
		public void CreateControllerShouldReturnErrorControllerForUnregisteredController() {

			var containerMock = new Mock<IDIContainer>();
			ControllerFactory target = new ControllerFactory(containerMock.Object); // TODO: Initialize to an appropriate value
			RequestContext requestContext = null; // TODO: Initialize to an appropriate value
			string controllerName = "foobar"; // TODO: Initialize to an appropriate value

			containerMock.Setup(c => c.IsRegistered<IController>(controllerName)).Returns(false);
			containerMock.Setup(c => c.Resolve<IController>("null")).Returns(new NullController(null));

			IController actual;
			actual = target.CreateController(requestContext, controllerName);
			Assert.IsInstanceOfType(actual, typeof(NullController));
		}
	}
}
