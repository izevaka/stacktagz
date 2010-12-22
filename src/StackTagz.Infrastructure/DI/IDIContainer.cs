using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackTagz.Infrastructure.DI {
	public interface IDIContainer {
		void RegisterType<TFrom>() where TFrom : class;
		void RegisterType<TFrom, TTo>() where TTo : TFrom;
		void RegisterType<TFrom, TTo>(string name) where TTo : TFrom;
		void RegisterType(Type from, Type to);
		void RegisterType(Type from, Type to, string name);
		
		void RegisterInstance<TFrom>(TFrom instance) where TFrom : class;
		
		T Resolve<T>();
		T Resolve<T>(string name);
		IEnumerable<T> ResolveAll<T>();

		bool IsRegistered<TFrom>(string name) where TFrom : class;
		bool IsRegistered<TFrom>() where TFrom : class;
	}
}
