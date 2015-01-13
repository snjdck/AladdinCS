using System;

namespace Aladdin.IOC
{
    class InjectionTypeClass : IInjectionType
    {
        Type cls;
		
		public InjectionTypeClass(Type cls)
		{
			this.cls = cls;
		}

		public object getValue(Injector injector, string id)
		{
			if(id != null){
				return null;
			}
			return injector.newInstance(cls);
		}
    }
}
