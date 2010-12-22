using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using StackTagz.Model;
using StackTagz.Infrastructure;
using StackTagz.Model.Data;


namespace StackTagz.Converters {
	public class TimeseriesPeriodConverter : JsonConverter{
		public override bool CanConvert(Type objectType) {
			return objectType == typeof(TimeseriesPeriod);
		}

		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer) {
			throw new InvalidOperationException();
		}

		/*
		public int Count { get { return m_Count; } }
		public Rollup RollupType { get { return m_Rollup; } }
		public int QuestionCount { get { return m_QuestionCount; } }
		public int AnswerCount { get { return m_AnswerCount; } }
		public int CommentCount { get { return m_CommentCount; } }
		public int VoteCount { get { return m_VoteCount; } }
*/

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			
			if(!typeof(TimeseriesPeriod).IsAssignableFrom(value.GetType()))
				throw new InvalidOperationException("Cannot serialize type {0}".FormatString(value.GetType()));

			var tsp = (TimeseriesPeriod)value;
			
			serializer.Serialize(writer, new { 
			 tsp.Count,
			 tsp.QuestionCount, 
			 tsp.AnswerCount, 
			 tsp.CommentCount
			});
		}
	}
}