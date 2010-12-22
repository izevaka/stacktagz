using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Infrastructure;
using StackTagz.ServerRepository.StackClientFactory;
using Stacky;
using StackTagz.Model;
using StackTagz.Model.Questions;
using StackTagz.Model.Answers;


namespace StackTagz.ServerRepository.Questions{
	
	public class QuestionsRepository : ServerListRepositoryBase<Question, QuestionInfo>, IQuestionsRepository{
		private IDataConverter<Question, QuestionInfo> m_Converter = new QuestionToQuestionInfoConverter();

		public QuestionsRepository(IStackClientFactory factory): base(factory){
		}
		

		protected override IPagedList<Question> RequestData(StackyClient client, int userId, int page) {
			return client.GetQuestionsByUser(userId, page: page, pageSize: ApiSettings.PageSize);
		}

		protected override IPagedList<Question> RequestData(StackyClient client, IEnumerable<int> idList) {
			return client.GetQuestions(idList,pageSize: idList.Count());
		}

		protected override IDataConverter<Question, QuestionInfo> Converter {
			get { return m_Converter; }
		}
	}
}
