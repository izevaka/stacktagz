using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Questions {
	/// <summary>
	/// Interface that fetches data from the database
	/// Doesn't add any functionality at this point, 
	/// Only exists to disambiguate Server interface and DB interface
	/// </summary>
	public interface IPersistQuestionsRepository : IQuestionsRepository{
		void SaveQuestions(string site, List<QuestionInfo> questionInfos);
	}
}
