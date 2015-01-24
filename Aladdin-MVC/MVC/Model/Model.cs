using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class Model : IModel
	{
		[Inject]
		private Module module;

		[Inject]
		private Injector injector;

		private Dictionary<Type, Proxy> proxyRefs;

		public Model()
		{
			proxyRefs = new Dictionary<Type, Proxy>();
		}

		public void regProxy(Type proxyType)
		{
			throw new NotImplementedException();
		}

		public void delProxy(Type proxyType)
		{
			throw new NotImplementedException();
		}

		public bool hasProxy(Type proxyType)
		{
			return proxyRefs.ContainsKey(proxyType);
		}
	}
}
