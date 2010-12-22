using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using StackTagz.Model;
using StackTagz.Infrastructure;
using StackTagz.Model.Data;

namespace StackTagz.Converters {
	public class TimeseriesResultConverter : JsonConverter {
		public override bool CanConvert(Type objectType) {
			if(objectType == typeof(TimeseriesResult))
				return true;

			return false;
		}

		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer) {
			throw new NotSupportedException("Deserialization of type {0} is not supported".FormatString(objectType));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			var tsr = value as TimeseriesResult;

			if(tsr == null)
				throw new InvalidOperationException("Cannot serialize type {0}".FormatString(value.GetType()));

			writer.WriteRaw(JsonConvert.SerializeObject(new { 
				RollupType = tsr.RollupType.ToString(),
				Timeseries = tsr.Timeseries.Values,
				Labels = from k in tsr.Timeseries.Keys select new {label = k},
				TimeseriesInfo = tsr.Timeseries.Values.Select(ts => ts.Select(t => t.Value))
			}, new JsonConverter[] { new TimeseriesConverter(), new TimeseriesPeriodConverter() }));

		}
	}
}
