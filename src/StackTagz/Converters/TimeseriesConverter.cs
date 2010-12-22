using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using StackTagz.Infrastructure;
using StackTagz.Model;
using StackTagz.Model.Data;

namespace StackTagz.Converters {
	public class TimeseriesConverter : JsonConverter{
		public override bool CanConvert(Type objectType) {
			return objectType == typeof(Timeseries);
		}

		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer) {
			throw new NotSupportedException("Deserialization of type {0} is not supported".FormatString(objectType));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			var ts = value as Timeseries;

			if(ts == null)
				throw new InvalidOperationException("Cannot serialize type {0}".FormatString(value.GetType()));

			writer.WriteStartArray();
			foreach(var kv in ts) {
				writer.WriteStartArray();
				writer.WriteValue(kv.Key.ToString("yyyy-MM-dd"));
				writer.WriteValue(kv.Value.Count);
				writer.WriteEndArray();
			}
			writer.WriteEndArray();
		}
	}
}
