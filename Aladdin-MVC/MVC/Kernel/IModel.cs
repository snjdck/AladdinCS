using System;

namespace Aladdin.MVC
{
	interface IModel
	{
		void regProxy(Type proxyType);
		void delProxy(Type proxyType);
		bool hasProxy(Type proxyType);
	}
}
