using System;

namespace Aladdin.IOC
{
	class InjectionTypeSingleton : IInjectionType
    {
		readonly Injector creator;
		readonly Type cls;
		object val;
		
		public InjectionTypeSingleton(Injector creator, Type cls)
		{
			this.creator = creator;
			this.cls = cls;
		}

		public object getValue(Injector injector, string id)
		{
		    if(null == val){
				val = creator.newInstance(cls);
		    }
		    return val;
		}
    }
}
