using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class Model
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

		public void regProxy<T>() where T : Proxy, new()
		{
			Type proxyType = typeof(T);
			if(hasProxy(proxyType)){
				return;
			}
			Proxy proxy = injector.getInstance(proxyType) as Proxy;
			if (null == proxy){
				injector.mapSingleton<T>();
				regProxy<T>();
			}else{
				proxyRefs[proxyType] = proxy;
				proxy.onReg();
			}
		}

		public void delProxy(Type proxyType)
		{
			if(!hasProxy(proxyType)){
				return;
			}
			Proxy proxy = proxyRefs[proxyType];
			proxyRefs.Remove(proxyType);
			proxy.onDel();
		}

		public bool hasProxy(Type proxyType)
		{
			return proxyRefs.ContainsKey(proxyType);
		}
	}
}
