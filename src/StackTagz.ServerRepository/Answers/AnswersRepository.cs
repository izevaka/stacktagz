using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stacky;
using StackTagz.Model.Answers;
using StackTagz.ServerRepository.StackClientFactory;

namespace StackTagz.ServerRepository.Answers {
	public class AnswersRepository : ServerListRepositoryBase<Answer, AnswerInfo>, IAnswersRepository {
		private IDataConverter<Answer, AnswerInfo> m_Converter = new AnswerToAnswerInfoConverter();

		public AnswersRepository(IStackClientFactory factory)
			: base(factory) {
		}

		protected override IPagedList<Answer> RequestData(StackyClient client, int userId, int page) {
			return client.GetUsersAnswers(userId, page: page, pageSize: ApiSettings.PageSize);
		}

		protected override IPagedList<Answer> RequestData(StackyClient client, IEnumerable<int> idList) {
			return client.GetAnswers(idList, pageSize: idList.Count());
		}

		protected override IDataConverter<Answer, AnswerInfo> Converter {
			get { return m_Converter; }
		}
	}
}
