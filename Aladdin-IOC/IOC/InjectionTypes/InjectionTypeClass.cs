using System;

namespace Aladdin.IOC
{
    class InjectionTypeClass : IInjectionType
    {
		readonly Injector creator;
		readonly Type cls;
		
		public InjectionTypeClass(Injector creator, Type cls)
		{
			this.creator = creator;
			this.cls = cls;
		}

		public object getValue(Injector injector, string id)
		{
			return creator.newInstance(cls);
		}
    }
}
