using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace StackTagz.Converters {
	public class SafeStringConverter : JsonConverter{

		public override bool CanConvert(Type objectType) {
			return objectType == typeof(string);
		}

		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer) {
			throw new NotSupportedException("{0} does not support converting from URL-encoded string");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			writer.WriteValue(HttpUtility.HtmlEncode(value));
		}
	}
}