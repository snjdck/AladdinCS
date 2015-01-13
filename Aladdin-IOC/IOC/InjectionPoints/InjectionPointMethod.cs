using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointMethod : IInjectionPoint
	{
		readonly MethodInfo method;
		readonly ParameterInfo[] paramList;

		public InjectionPointMethod(MethodInfo method)
		{
			this.method = method;
			this.paramList = method.GetParameters();
		}

		public void injectInto(object target, Injector injector)
		{
			method.Invoke(target, getInstances(injector));
		}

		object[] getInstances(Injector injector)
		{
			var result = new object[paramList.Length];
			foreach(var paramInfo in paramList){
				result[paramInfo.Position] = injector.getInstance(paramInfo.ParameterType);
			}
			return result;
		}
	}
}
