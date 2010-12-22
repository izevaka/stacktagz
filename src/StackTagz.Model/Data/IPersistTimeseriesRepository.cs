using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model.Data {
	/// <summary>
	/// Interface that fetches data from the database
	/// Doesn't add any functionality at this point, 
	/// Only exists to disambiguate Server interface and DB interface
	/// </summary>
	public interface IPersistTimeseriesRepository : ISummarizer{
		void SaveTimeseries(string site, int userId, TimeseriesResult result);
	}
}
