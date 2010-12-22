using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Model {
	public interface IDataRepository<T> {
		List<T> Get(string site, int userId);
	}
}
