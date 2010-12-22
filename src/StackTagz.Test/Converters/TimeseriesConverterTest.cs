using StackTagz.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Newtonsoft.Json;
using StackTagz.Model;
using System.IO;
using System.Collections.Generic;
using StackTagz.Model.Data;

namespace StackTagz.Tests
{
    
    
    /// <summary>
    ///This is a test class for TimeseriesConverterTest and is intended
    ///to contain all TimeseriesConverterTest Unit Tests
    ///</summary>
	[TestClass()]
	public class TimeseriesConverterTest {


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
		public void CanConvertShouldBeTrueForTimeSeries() {
			TimeseriesConverter target = new TimeseriesConverter(); 
			Type objectType = typeof(Timeseries);
			bool actual;
			actual = target.CanConvert(objectType);
			Assert.IsTrue(actual);
		}
		[TestMethod()]
		public void CanConvertShouldBeFalseForObject() {
			TimeseriesConverter target = new TimeseriesConverter(); 
			Type objectType = typeof(object);
			bool actual;
			actual = target.CanConvert(objectType);
			Assert.IsFalse(actual);
		}

		[TestMethod()]
		[ExpectedException(typeof(NotSupportedException))]
		public void ReadJsonTest() {
			TimeseriesConverter target = new TimeseriesConverter(); 
			JsonSerializer serializer = new JsonSerializer(); 

			var actual = target.ReadJson(null, typeof(Timeseries), null);
		}

		[TestMethod()]
		public void WriteJsonTest() {
			TimeseriesConverter target = new TimeseriesConverter();
			using(var ms = new MemoryStream()) {
				var tw = new StreamWriter(ms);

				JsonWriter writer = new JsonTextWriter(tw); // TODO: Initialize to an appropriate value

				JsonSerializer serializer = new JsonSerializer(); // TODO: Initialize to an appropriate value

				target.WriteJson(writer, new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod> { 
					{ new DateTime(2009, 04, 23), new TimeseriesPeriod(1,2,3) }, 
					{ new DateTime(2009, 04, 24), new TimeseriesPeriod(5,6,7) } }), serializer);
				writer.Flush();

				ms.Position = 0;
				var result = new StreamReader(ms).ReadToEnd();
				Assert.AreEqual("[[\"2009-04-23\",6],[\"2009-04-24\",18]]", result);
			}

		}
	}
}
