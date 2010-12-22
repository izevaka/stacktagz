using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Questions{
	public interface IQuestionsRepository : IDataRepository<QuestionInfo> {
		[Obsolete("Should probably be IEnumerable")]
		List<QuestionInfo> Get(string site, IEnumerable<int> itemIds);
	}
}
