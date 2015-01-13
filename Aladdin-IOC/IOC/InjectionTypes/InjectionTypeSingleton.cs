using System;

namespace Aladdin.IOC
{
    class InjectionTypeSingleton : IInjectionType
    {
        Type cls;
		object val;
		
		public InjectionTypeSingleton(Type cls)
		{
			this.cls = cls;
		}

		public object getValue(Injector injector, string id)
		{
			if(id != null){
				return null;
			}
		    if(null == val){
                val = injector.newInstance(cls);
		    }
		    return val;
		}
    }
}
