using StackTagz.SqlRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using StackTagz.Model.Questions;
using StackTagz.SqlRepository.EF;
using Moq;
using System.Linq;
using System.Data.Objects;

namespace StackTagz.SqlRepository.Test
{
    /// <summary>
    ///This is a test class for QuestionsRepositoryTest and is intended
    ///to contain all QuestionsRepositoryTest Unit Tests
    ///</summary>
	[TestClass()]
	public class QuestionsRepositoryTest {

		[TestInitialize()]
		public void MyTestInitialize() {
			m_EntitiesMock = new Mock<IStackTagzContext>();
		}
	Mock<IStackTagzContext> m_EntitiesMock;


	/// <summary>
	///A test for Get
	///</summary>
	[TestMethod()]
	public void GetShouldFilterBySite() {
		QuestionsRepository target = new QuestionsRepository(m_EntitiesMock.Object);

		m_EntitiesMock.Setup(e=>e.Questions).Returns(new MockObjectSet<Question>(new [] {
			new Question{Site = "site1", QuestionId = 1, Date = new DateTime(2010, 06,23)},
				new Question{Site = "site1", QuestionId = 2, Date = new DateTime(2010, 06,24)},
				new Question{Site = "site1", QuestionId = 3, Date = new DateTime(2010, 06,25)},
				new Question{Site = "site2", QuestionId = 4, Date = new DateTime(2010, 06,23)},
				new Question{Site = "site3", QuestionId = 5, Date = new DateTime(2010, 06,24)},
			}));				 

			List<QuestionInfo> expected = new List<QuestionInfo> { 
				new QuestionInfo{Id = 1, CreationDate = new DateTime(2010, 06,23)},
				new QuestionInfo{Id = 2, CreationDate = new DateTime(2010, 06,24)},
				new QuestionInfo{Id = 3, CreationDate = new DateTime(2010, 06,25)}
			}; 
			var actual = target.Get("site1", new[] { 1, 2, 3 });

			Assert.AreEqual(3, actual.Count);
		}

		[TestMethod()]
		public void SaveQuestionsShouldCreateAndAddQuestion() {
			QuestionsRepository target = new QuestionsRepository(m_EntitiesMock.Object);
			List<QuestionInfo> questionInfos = new List<QuestionInfo> { 
				new QuestionInfo{CreationDate = new DateTime(2010, 06, 23), Id = 34, Tags= new List<string>{"tag1", "tag2"}}
			};

			var result = new Question();
			m_EntitiesMock.Setup(e => e.Questions).Returns(new MockObjectSet<Question>(new Question[0]));
			m_EntitiesMock.Setup(e => e.CreateQuestion()).Returns(result);
			m_EntitiesMock.Setup(e => e.AddQuestion(result));
			m_EntitiesMock.Setup(e => e.SaveChanges());
			target.SaveQuestions("site1", questionInfos);
			m_EntitiesMock.VerifyAll();
		}

		[TestMethod()]
		public void SaveQuestionsShouldAddOnlyNonExistentQuestions() {
			QuestionsRepository target = new QuestionsRepository(m_EntitiesMock.Object);
			List<QuestionInfo> questionInfos = new List<QuestionInfo> { 
				new QuestionInfo{CreationDate = new DateTime(2010, 06, 23), Id = 34, Tags= new List<string>{"tag1", "tag2"}},
				new QuestionInfo{CreationDate = new DateTime(2010, 06, 23), Id = 12, Tags= new List<string>{"tag1", "tag2"}}
			};

			var result = new Question();
			m_EntitiesMock.Setup(e => e.CreateQuestion()).Returns(result);
			m_EntitiesMock.Setup(e=>e.Questions).Returns(new MockObjectSet<Question>(new []{
				new Question{QuestionId = 12, Site = "site1"}
			}));

			target.SaveQuestions("site1", questionInfos);
			
			m_EntitiesMock.Verify(e => e.AddQuestion(result), Times.Once());
			m_EntitiesMock.Verify(e => e.AddQuestion(It.IsAny<Question>()), Times.Once());
		}

	}
}
