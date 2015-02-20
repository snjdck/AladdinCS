using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointProperty : IInjectionPoint
	{
		readonly PropertyInfo property;

		public InjectionPointProperty(PropertyInfo property)
		{
			this.property = property;
		}

		public void injectInto(object target, Injector injector)
		{
			var val = injector.getInstance(property.PropertyType, property.GetInjectTag().id);
			if(val != null){
				property.SetValue(target, val, null);
			}
		}
	}
}
