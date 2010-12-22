using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Questions;
using StackTagz.SqlRepository.EF;

namespace StackTagz.SqlRepository {
	public class QuestionsRepository : IPersistQuestionsRepository{
		private IStackTagzContext m_Entities;

		public QuestionsRepository(IStackTagzContext entities) {
			m_Entities = entities;
		}
		
		
		#region IQuestionsRepository Members

		public List<QuestionInfo> Get(string site, IEnumerable<int> itemIds) {
			return Get(site, itemIds, 0);
		}

		public List<QuestionInfo> Get(string site, IEnumerable<int> itemIds, int pageSize) {
			return (from q in m_Entities.Questions
					where q.Site == site && itemIds.Contains(q.QuestionId)
					select q).ToList().Select(q => q.ToQuestionInfo()).ToList();

		}

		#endregion

		#region IDataRepository<QuestionInfo> Members

		public List<QuestionInfo> Get(string site, int userId) {
			throw new NotImplementedException();
		}

		#endregion

		#region IPersistQuestionsRepository Members

		public void SaveQuestions(string site, List<QuestionInfo> questionInfos) {
			//var questions = questionInfos.Where(qI=>!m_Entities.Questions.Any(q=>q.QuestionId == qI.Id));
			var questionIds = questionInfos.Select(qI=>qI.Id);

			var existingQuestions = (from q in m_Entities.Questions
									 where questionIds.Contains(q.QuestionId)
									 select q.QuestionId).ToList();

			var questions = questionInfos.Where(qI => !existingQuestions.Contains(qI.Id));

			foreach(var q in questions) {
				var question = m_Entities.CreateQuestion();
				question.SetFromQuestionInfo(site, q);
				m_Entities.AddQuestion(question);
			}
			m_Entities.SaveChanges();
		}

		#endregion

	}
}
