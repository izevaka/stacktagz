using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Data {
	public class TimeseriesResult {

		public Dictionary<string, Timeseries> Timeseries { get; set; }
		private Rollup m_Rollup;

		public TimeseriesResult() {
			Timeseries = new Dictionary<string, Timeseries>();
		}
		public TimeseriesResult(Rollup rollup):this() {
			m_Rollup = rollup;
		}

		public TimeseriesResult(Rollup rollup, Dictionary<string, Timeseries> values): this(rollup) {
			Timeseries = values;
		}

		public Rollup RollupType {
			get { return m_Rollup; }
			set { m_Rollup = value; }
		}


		public Timeseries CreateTimeseries() {
			return new Timeseries(m_Rollup);
		}

	}
}
