using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace StackTagz.SqlRepository.Test {
	public class MockObjectSet<T> : IObjectSet<T> where T : class {
		private IList<T> m_List;
		private IQueryable<T> m_Queryable;

		public MockObjectSet(IList<T> list) {
			m_List = list;
			m_Queryable = m_List.AsQueryable();
		}
		
		#region IObjectSet<T> Members

		public void AddObject(T entity) {
			m_List.Add(entity);
		}

		public void Attach(T entity) {
			m_List.Add(entity);
		}

		public void DeleteObject(T entity) {
			m_List.Remove(entity);
		}

		public void Detach(T entity) {
			m_List.Remove(entity);
		}

		#endregion

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator() {
			return m_List.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return m_List.GetEnumerator();
		}

		#endregion

		#region IQueryable Members

		public Type ElementType {
			get { return typeof(T); }
		}

		public System.Linq.Expressions.Expression Expression {
			get { return m_Queryable.Expression; }
		}

		public IQueryProvider Provider {
			get { return m_Queryable.Provider; }
		}

		#endregion
	}
}
