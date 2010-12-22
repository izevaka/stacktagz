using StackTagz.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Newtonsoft.Json;
using StackTagz.Model;
using System.IO;
using System.Text;
using System.Collections.Generic;
using StackTagz.Model.Data;

namespace StackTagz.Tests.Converters
{
 	[TestClass()]
	public class TimeseriesResultConverterTest {


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
		TimeseriesResult m_Result;
		[TestInitialize()]
		public void MyTestInitialize() {
			m_Result = new TimeseriesResult(Rollup.Weekly);

			m_Result.Timeseries.Add("Label1", new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod> { 
				{new DateTime(2009, 04, 23), new TimeseriesPeriod(1,0,0) },
				{new DateTime(2009, 04, 24), new TimeseriesPeriod(6,0,0) },
				{new DateTime(2009, 04, 25), new TimeseriesPeriod(3,0,0) }})
			);

			m_Result.Timeseries.Add("Label2", new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod> { 
				{new DateTime(2009, 04, 23), new TimeseriesPeriod(4,0,0) },
				{new DateTime(2009, 04, 24), new TimeseriesPeriod(5,0,0) }})
			);
		}

		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		[TestMethod()]
		public void CanConvertTimeseriesResult() {
			TimeseriesResultConverter target = new TimeseriesResultConverter(); // TODO: Initialize to an appropriate value
			Type objectType = typeof(TimeseriesResult);
			bool actual;
			actual = target.CanConvert(objectType);
			Assert.IsTrue(actual);
		}

		[TestMethod()]
		public void CanConvertSomethingElse() {
			TimeseriesResultConverter target = new TimeseriesResultConverter(); // TODO: Initialize to an appropriate value
			Type objectType = typeof(object);
			bool actual;
			actual = target.CanConvert(objectType);
			Assert.IsFalse(actual);
		}

		[TestMethod()]
		[ExpectedException(typeof(NotSupportedException))]
		public void ReadJsonTest() {
			TimeseriesResultConverter target = new TimeseriesResultConverter(); // TODO: Initialize to an appropriate value
			var actual = target.ReadJson(null, typeof(TimeseriesResult), null);
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidOperationException))]
		public void WriteJsonShouldThrowExceptionOnNotTimeseriesResult() {
			TimeseriesResultConverter target = new TimeseriesResultConverter(); // TODO: Initialize to an appropriate value
			target.WriteJson(null, new object(), null);
		}

		[TestMethod()]
		public void WriteJsonShouldConvert() {
			TimeseriesResultConverter target = new TimeseriesResultConverter(); // TODO: Initialize to an appropriate value
			using (var ms = new MemoryStream()) {
				var tw = new StreamWriter(ms);

				JsonWriter writer = new JsonTextWriter(tw); // TODO: Initialize to an appropriate value				
				JsonSerializer serializer = new JsonSerializer(); // TODO: Initialize to an appropriate value
				
				
				target.WriteJson(writer, m_Result, serializer);
				writer.Flush();
				
				ms.Position = 0;
				var result = new StreamReader(ms).ReadToEnd();
				Assert.AreEqual("{\"RollupType\":\"Weekly\",\"Timeseries\":[[[\"2009-04-23\",1],[\"2009-04-24\",6],[\"2009-04-25\",3]],[[\"2009-04-23\",4],[\"2009-04-24\",5]]],\"Labels\":[{\"label\":\"Label1\"},{\"label\":\"Label2\"}],\"TimeseriesInfo\":[[{\"Count\":1,\"QuestionCount\":1,\"AnswerCount\":0,\"CommentCount\":0},{\"Count\":6,\"QuestionCount\":6,\"AnswerCount\":0,\"CommentCount\":0},{\"Count\":3,\"QuestionCount\":3,\"AnswerCount\":0,\"CommentCount\":0}],[{\"Count\":4,\"QuestionCount\":4,\"AnswerCount\":0,\"CommentCount\":0},{\"Count\":5,\"QuestionCount\":5,\"AnswerCount\":0,\"CommentCount\":0}]]}", result);
			}
		}
	}
}
