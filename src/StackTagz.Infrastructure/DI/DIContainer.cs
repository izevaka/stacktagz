using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace StackTagz.Infrastructure.DI {
	/// <summary>
	/// This is a thin wrapper around unity container to improve unit testing for components that use DI
	/// Becuase some methods on IUnityContainer are extension methods (like IsRegistered&lt;T&gt;),
	/// it is impossible to mock them. It is also the reason there are no unit tests for this class.
	/// It's practically impossible verify that the right methods get called.
	/// Considering these are all one line methods with exact same parameters,
	/// I trust myself to have done it properly :)
	/// </summary>
	public class DIContainer : IDIContainer {
		IUnityContainer m_Container = new UnityContainer();
		
		#region IDIContainer Members

		public void RegisterType<TFrom>() where TFrom : class {
			m_Container.RegisterType<TFrom>();
		}

		public void RegisterType<TFrom, TTo>() where TTo : TFrom {
			m_Container.RegisterType<TFrom, TTo>();
		}

		public void RegisterType<TFrom, TTo>(string name) where TTo : TFrom {
			m_Container.RegisterType<TFrom, TTo>(name);
		}

		public void RegisterType(Type from, Type to) {
			m_Container.RegisterType(from, to);
		}

		public void RegisterType(Type from, Type to, string name) {
			m_Container.RegisterType(from, to, name);
		}

		public void RegisterInstance<TFrom>(TFrom instance) where TFrom : class {
			m_Container.RegisterInstance<TFrom>(instance);
		}

		public T Resolve<T>() {
			return m_Container.Resolve<T>();
		}

		public IEnumerable<T> ResolveAll<T>() {
			return m_Container.ResolveAll<T>();
		}

		public T Resolve<T>(string name) {
			return m_Container.Resolve<T>(name);
		}

		public bool IsRegistered<TFrom>(string name) where TFrom : class {
			return m_Container.IsRegistered<TFrom>(name);
		}

		public bool IsRegistered<TFrom>() where TFrom : class {
			return m_Container.IsRegistered<TFrom>();
		}

		#endregion
	}
}
