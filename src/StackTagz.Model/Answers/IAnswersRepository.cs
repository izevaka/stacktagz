using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StackTagz.Model.Answers{
	public interface IAnswersRepository : IDataRepository<AnswerInfo> {
		List<AnswerInfo> Get(string site, IEnumerable<int> ids);
	}
}
