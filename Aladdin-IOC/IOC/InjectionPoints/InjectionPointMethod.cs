using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointMethod : IInjectionPoint
	{
		readonly MethodInfo method;

		public InjectionPointMethod(MethodInfo method)
		{
			this.method = method;
		}

		public void injectInto(object target, Injector injector)
		{
			method.Invoke(target, getInstances(injector));
		}

		object[] getInstances(Injector injector)
		{
			var paramList = method.GetParameters();
			if(paramList.Length <= 0){
				return null;
			}
			var result = new object[paramList.Length];
			foreach(var paramInfo in paramList){
				result[paramInfo.Position] = injector.getInstance(paramInfo.ParameterType);
			}
			return result;
		}
	}
}
