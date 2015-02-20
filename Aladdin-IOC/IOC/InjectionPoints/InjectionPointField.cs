using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointField : IInjectionPoint
	{
		readonly FieldInfo field;

		public InjectionPointField(FieldInfo field)
		{
			this.field = field;
		}

		public void injectInto(object target, Injector injector)
		{
			var val = injector.getInstance(field.FieldType, field.GetInjectTag().id);
			if(val != null) {
				field.SetValue(target, val);
			}
		}
	}
}
