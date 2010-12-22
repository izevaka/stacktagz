using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using StackTagz.Infrastructure;

namespace StackTagz.Model.Data {

	public class Timeseries : IEnumerable<KeyValuePair<DateTime, TimeseriesPeriod>>, ICollection {

		SortedList<DateTime, TimeseriesPeriod> m_Collection = new SortedList<DateTime, TimeseriesPeriod>();
		private readonly Rollup m_Rollup;

		public Timeseries() {
		}

		public Timeseries(Rollup rollup) {
			m_Rollup = rollup;
		}

		public Timeseries(Rollup rollup, IDictionary<DateTime, TimeseriesPeriod> values) :this(rollup) {
			foreach(var kv in values) {
				m_Collection.Add(kv.Key, kv.Value);
			}
		}
		
		

		public void Add(DateTime date, TimeseriesPoint value) {

			date = GetRolledUpDate(date);

			TimeseriesPeriod existing;
			if (!m_Collection.TryGetValue(date.Date, out existing))
				existing = new TimeseriesPeriod();

			existing += value;
			m_Collection[date] = existing;
		}

		public DateTime GetRolledUpDate(DateTime date) {
			switch(m_Rollup) {
				case Rollup.Daily:
					date = date.Date;
					break;
				case Rollup.Monthly:
					if(date.Day == 1)
						return date.Date;
					date = new DateTime(date.Year, date.Month, 1).AddMonths(1); //monthly rolls up to the first of next month
					break;
				case Rollup.Weekly:
					if(date.DayOfWeek == DayOfWeek.Sunday)
						return date.Date;
					date = date.Date.AddDays(-(int)date.Date.DayOfWeek );
					break;
				default:
					throw new NotSupportedException("Rollup value {0} is not supported.".FormatString(m_Rollup));
			}

			return date;
		}

		public override bool Equals(object obj) {

			var other = obj as Timeseries;
			if(other == null) {
				return false;
			}

			return this.m_Rollup == other.m_Rollup && m_Collection.IsEqual(other.m_Collection);
		}


		public override int GetHashCode() {
			int hash = 17;
			hash = hash * 23 + ((m_Collection != null )? m_Collection.GetHashCode() : 0);
			hash = hash * 23 + m_Rollup.GetHashCode();
			return hash;
		}
    
    

		#region IEnumerable<KeyValuePair<DateTime,TimeseriesPeriod>> Members

		public IEnumerator<KeyValuePair<DateTime, TimeseriesPeriod>> GetEnumerator() {
			return m_Collection.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator() {
			return ((IEnumerable)m_Collection).GetEnumerator();
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index) {
			foreach(var item in m_Collection.Skip(index)) {
				array.SetValue(item, index++);
			}
		}

		public int Count {
			get { return m_Collection.Count; }
		}

		public bool IsSynchronized {
			get { return false; }
		}

		public object SyncRoot {
			get { return null; }
		}

		#endregion
	}
}
