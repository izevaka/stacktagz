using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackTagz.Model;

namespace StackTagz.Model.Data {
	public interface ISummarizer {
		TimeseriesResult GetTimeSeries(string site, int userId, DateTime? start, DateTime? end, Rollup rollup);
	}
}
