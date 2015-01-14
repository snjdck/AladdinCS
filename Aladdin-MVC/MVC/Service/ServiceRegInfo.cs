using System;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class ServiceRegInfo
	{
		internal Type serviceInterface;
		internal Type serviceClass;
		internal Injector moduleInjector;

		internal Type[] typesNeedToBeInjected;

		public ServiceRegInfo(Type serviceInterface, Type serviceClass, Injector moduleInjector)
		{
			this.serviceInterface = serviceInterface;
			this.serviceClass = serviceClass;
			this.moduleInjector = moduleInjector;
			typesNeedToBeInjected = Inject.GetTypesNeedInject(serviceClass);
		}

		public void regService(Injector appInjector)
		{
			var service = moduleInjector.newInstance(serviceClass);
			appInjector.mapValue(serviceInterface, service, null, false);
		}
	}
}
