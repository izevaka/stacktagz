using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace StackTagz.SqlRepository.EF {
	public class StackTagzContext : ObjectContext, IStackTagzContext {   
        public StackTagzContext() : base("name=stacktagzEntities", "stacktagzEntities")  
        {
			base.ContextOptions.LazyLoadingEnabled = true;
            m_Questions = CreateObjectSet<Question>();
			m_TSPeriods = CreateObjectSet<TSPeriod>();
			m_TSResults = CreateObjectSet<TSResult>();
        }

		#region IStackTagzContext Members
		private ObjectSet<Question> m_Questions;
		public IObjectSet<Question> Questions {
            get { return m_Questions; }
        }
        

		private ObjectSet<TSResult> m_TSResults;
		public IObjectSet<TSResult> TSResults {
			get { return m_TSResults; }
		}

		private ObjectSet<TSPeriod> m_TSPeriods;
		public IObjectSet<TSPeriod> TSPeriods {
			get { return m_TSPeriods; }
		}


		public Question CreateQuestion() {
			return m_Questions.CreateObject();
		}

		public TSResult CreateTSResult() {
			var result = m_TSResults.CreateObject();			
			return result;
		}

		public TSPeriod CreateTSPeriod(TSResult result) {
			var period = m_TSPeriods.CreateObject();
			period.TSResult = result;
			return period;
		}

		public void AddQuestion(Question item) {
			m_Questions.AddObject(item);
		}

		public void AddTSResult(TSResult item) {
			m_TSResults.AddObject(item);
		}

		public void AddTSPeriod(TSPeriod item) {
			m_TSPeriods.AddObject(item);
		}

		public new void SaveChanges() {
			base.SaveChanges();
		}

		#endregion
	}
}
