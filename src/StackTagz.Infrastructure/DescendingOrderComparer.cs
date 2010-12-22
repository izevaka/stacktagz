using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Infrastructure {
	public class DescendingOrderComparer : IComparer<int>{
		#region IComparer<int> Members

		public int Compare(int x, int y) {
			return y - x;
		}

		#endregion
	}
}
