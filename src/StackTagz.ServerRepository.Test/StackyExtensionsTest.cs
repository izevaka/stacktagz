using StackTagz.ServerRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Stacky;
using StackTagz.Model.Answers;
using StackTagz.Model.Questions;
using System.Collections.Generic;
using stacktagzusers = StackTagz.Model.Users;

namespace StackTagz.Repository.Test
{
    
    
    /// <summary>
    ///This is a test class for StackyExtensionsTest and is intended
    ///to contain all StackyExtensionsTest Unit Tests
    ///</summary>
	[TestClass()]
	public class StackyExtensionsTest {


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


		/// <summary>
		///A test for ToAnswerInfo
		///</summary>
		[TestMethod()]
		public void ToAnswerInfoShouldCopyMembers() {
			Answer answer = new Answer { CreationDate = new DateTime(2009, 06, 23, 1, 2, 3), QuestionId = 123, Id = 456 };
			AnswerInfo expected = new AnswerInfo { CreationDate = new DateTime(2009,06,23,1,2,3), QuestionId = 123, AnswerId = 456}; 
			AnswerInfo actual;
			actual = StackyExtensions.ToAnswerInfo(answer);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for ToQuestionInfo
		///</summary>
		[TestMethod()]
		public void ToQuestionInfoTest() {
			Question question = new Question { CreationDate = new DateTime(2009, 06, 23, 1, 2, 3), Id = 123, Tags = new List<string> { "tag1", "tag2" } }; 
			QuestionInfo expected = new QuestionInfo { CreationDate = new DateTime(2009, 06, 23, 1, 2, 3), Id = 123, Tags = new List<string> { "tag1", "tag2"} }; 
			QuestionInfo actual;
			actual = StackyExtensions.ToQuestionInfo(question);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for ToUserInfo
		///</summary>
		[TestMethod()]
		public void ToUserInfoTest() {
			User user = new User { BadgeCounts = new BadgeCounts { Bronze = 1, Silver = 2, Gold = 3 }, Reputation = 123, DisplayName = "Name", Id = 456}; // TODO: Initialize to an appropriate value
			stacktagzusers.UserInfo expected = new stacktagzusers.UserInfo { UserId = 456, BronzeBadges = 1, SilverBadges = 2, GoldBadges = 3, Rep = 123, Name = "Name", GravatarUrl = user.GravatarUrl }; // TODO: Initialize to an appropriate value
			stacktagzusers.UserInfo actual;
			actual = StackyExtensions.ToUserInfo(user);
			Assert.AreEqual(expected, actual);
		}
	}
}
