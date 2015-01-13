using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointProperty : IInjectionPoint
	{
		readonly PropertyInfo property;
		readonly Inject tag;

		public InjectionPointProperty(PropertyInfo property, Inject tag)
		{
			this.property = property;
			this.tag = tag;
		}

		public void injectInto(object target, Injector injector)
		{
			var val = injector.getInstance(property.PropertyType, tag.id);
			if(val != null){
				property.SetValue(target, val, null);
			}
		}
	}
}
