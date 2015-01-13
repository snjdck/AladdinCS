using System;
using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointMethod : IInjectionPoint
	{
		readonly MethodInfo method;
		readonly Type[] argTypes;

		public InjectionPointMethod(MethodInfo method)
		{
			this.method = method;
			this.argTypes = getArgTypes();
		}

		public void injectInto(object target, Injector injector)
		{
			method.Invoke(target, getInstances(injector));
		}

		Type[] getArgTypes()
		{
			var paramList = method.GetParameters();
			var result = new Type[paramList.Length];
			foreach(var paramInfo in paramList){
				argTypes[paramInfo.Position] = paramInfo.ParameterType;
			}
			return result;
		}

		object[] getInstances(Injector injector)
		{
			int count = argTypes.Length;
			var result = new object[count];
			for(int i = 0; i < count; ++i){
				result[i] = injector.getInstance(argTypes[i]);
			}
			return result;
		}
	}
}
