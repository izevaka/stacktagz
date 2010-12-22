using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model;
using StackTagz.Model.Data;

namespace StackTagz.Summarizer {
	public class DummySummarizer : ISummarizer {
		private IPersistTimeseriesRepository m_timeseriesPersistRepo;
		public DummySummarizer(IPersistTimeseriesRepository timeseriesPersistRepo) {
			m_timeseriesPersistRepo = timeseriesPersistRepo;
		}

		#region ITimeSeriesRepository Members

		public TimeseriesResult GetTimeSeries(string site, int userId, DateTime? start, DateTime? end, Rollup rollup) {

			//See if the DB contains the whole TimeseriesResult
			var result = m_timeseriesPersistRepo.GetTimeSeries(site, userId, start, end, rollup);
			if(result != null)
				return result;

			var tsr = GetDummyTimeSeries();
			return tsr;
		}

		TimeseriesResult GetDummyTimeSeries() {
			var result = new TimeseriesResult(Rollup.Weekly);

			result.Timeseries.Add("C#", new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod> { 
				{DateTime.Now.Date.AddDays(-3), new TimeseriesPeriod( 1,2,0)}, 
				{DateTime.Now.Date.AddDays(-2), new TimeseriesPeriod( 0,6,1)},
				{DateTime.Now.Date.AddDays(-1), new TimeseriesPeriod( 3,0,4)} 
			}
			));

			result.Timeseries.Add("C++", new Timeseries(Rollup.Weekly, new Dictionary<DateTime, TimeseriesPeriod> { 
				{DateTime.Now.Date.AddDays(-3), new TimeseriesPeriod(1,7,5)}, 
				{DateTime.Now.Date.AddDays(-2), new TimeseriesPeriod(5,4,1)},
				{DateTime.Now.Date.AddDays(-1), new TimeseriesPeriod(1,1,8)}
			}
			));

			return result;

		}

		#endregion
	}
}
