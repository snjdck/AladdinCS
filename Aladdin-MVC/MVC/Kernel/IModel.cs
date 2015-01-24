using System;

namespace Aladdin.MVC
{
	interface IModel
	{
		void regProxy<T>() where T : Proxy, new();
		void delProxy(Type proxyType);
		bool hasProxy(Type proxyType);
	}
}
