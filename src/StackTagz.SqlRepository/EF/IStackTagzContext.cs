using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace StackTagz.SqlRepository.EF {
	public interface IStackTagzContext {
		IObjectSet<Question> Questions { get; }
		IObjectSet<TSResult> TSResults { get; }
		IObjectSet<TSPeriod> TSPeriods { get; }

		Question CreateQuestion();
		TSResult CreateTSResult();
		TSPeriod CreateTSPeriod(TSResult parent);

		void AddQuestion(Question item);
		void AddTSResult(TSResult item);
		void AddTSPeriod(TSPeriod item);

		void SaveChanges();
	}
}
