using StackTagz.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Newtonsoft.Json;
using StackTagz.Model;
using System.IO;
using StackTagz.Model.Data;

namespace StackTagz.Tests.Controllers
{
    
    
    /// <summary>
    ///This is a test class for TimeseriesPeriodConverterTest and is intended
    ///to contain all TimeseriesPeriodConverterTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TimeseriesPeriodConverterTest {


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
		public void CanConvertTest() {
			TimeseriesPeriodConverter target = new TimeseriesPeriodConverter(); // TODO: Initialize to an appropriate value
			var actual = target.CanConvert(typeof(TimeseriesPeriod));
			Assert.IsTrue(actual);
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ReadJsonTest() {
			TimeseriesPeriodConverter target = new TimeseriesPeriodConverter(); // TODO: Initialize to an appropriate value
			var actual = target.ReadJson(null, typeof(TimeseriesPeriod), null);
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WriteJsonShouldThrowExceptionOnNotTimeseriesResult() {
			TimeseriesPeriodConverter target = new TimeseriesPeriodConverter(); // TODO: Initialize to an appropriate value
			target.WriteJson(null, new object(), null);
		}


		[TestMethod()]
		public void WriteJsonShouldConvert() {
			TimeseriesPeriodConverter target = new TimeseriesPeriodConverter(); // TODO: Initialize to an appropriate value
			using(var ms = new MemoryStream()) {
				var tw = new StreamWriter(ms);

				JsonWriter writer = new JsonTextWriter(tw); // TODO: Initialize to an appropriate value				
				JsonSerializer serializer = new JsonSerializer(); // TODO: Initialize to an appropriate value


				target.WriteJson(writer, new TimeseriesPeriod(1, 2, 3), serializer);
				writer.Flush();

				ms.Position = 0;
				var result = new StreamReader(ms).ReadToEnd();
				Assert.AreEqual("{\"Count\":6,\"QuestionCount\":1,\"AnswerCount\":2,\"CommentCount\":3}", result);
			}
		}
	}
}
