using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model.Data;
using StackTagz.SqlRepository.EF;

namespace StackTagz.SqlRepository {

	public class TimeseriesRepository : IPersistTimeseriesRepository{

		public TimeseriesRepository(IStackTagzContext entities) {
			m_Entities = entities;
		}

		private readonly IStackTagzContext m_Entities;
		
		#region ITimeseriesRepository Members

		public TimeseriesResult GetTimeSeries(string site, int userId, DateTime? start, DateTime? end, Rollup rollup) {

			IQueryable<TSResult> query = from r in m_Entities.TSResults
										 where r.Site == site && r.UserId == userId
										 select r;


			var result = (from q in query
						  select new { result = new TimeseriesResult { RollupType = (Rollup)q.Rollup }, dbresult = q })
						.FirstOrDefault();

			if(result != null) {
				var tags = result.dbresult.TSPeriods.Select(p=>p.tag).Distinct();
				var periods = tags.ToDictionary<string, string, Timeseries>(key => key, value => new Timeseries(rollup,
					result.dbresult.TSPeriods.Where(p => p.tag == value).ToDictionary<TSPeriod, DateTime, TimeseriesPeriod>(periodKey => periodKey.Date, periodValue => periodValue.ToTimeseriesPeriod())
				));
				result.result.Timeseries = periods;
				return result.result;
			}

			return null;
		}


		public void SaveTimeseries(string site, int userId, TimeseriesResult result) {
			var tsResult = m_Entities.CreateTSResult();
			tsResult.Rollup = (int)result.RollupType;
			tsResult.UserId = userId;
			tsResult.Site = site;
			tsResult.LastFetched = DateTime.Now;

			SavePeriods(tsResult, result.Timeseries);
			
			m_Entities.AddTSResult(tsResult);
			m_Entities.SaveChanges();
		}

		internal void SavePeriods(TSResult tsresult, Dictionary<string, Timeseries> timeseries) {

			foreach(var ts in timeseries) {
				foreach(var period in ts.Value) {
					var tsPeriod = m_Entities.CreateTSPeriod(tsresult);
					tsPeriod.SetFromTimeseriesPeriod(ts.Key, period.Key, period.Value);
					tsresult.TSPeriods.Add(tsPeriod);
				}
			}
		}

		#endregion
	}
}
