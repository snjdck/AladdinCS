using System.Reflection;

namespace Aladdin.IOC
{
	class InjectionPointField : IInjectionPoint
	{
		readonly FieldInfo field;
		readonly Inject tag;

		public InjectionPointField(FieldInfo field, Inject tag)
		{
			this.field = field;
			this.tag = tag;
		}

		public void injectInto(object target, Injector injector)
		{
			var val = injector.getInstance(field.FieldType, tag.id);
			if(val != null) {
				field.SetValue(target, val);
			}
		}
	}
}
