namespace Aladdin.IOC
{
	class InjectionTypeValue : IInjectionType
    {
		readonly Injector realInjector;
		readonly bool needInject;
		readonly object val;
		bool hasInjected;

		public InjectionTypeValue(object val, bool needInject, Injector realInjector)
		{
			this.realInjector = realInjector;
			this.needInject = needInject;
			this.val = val;
		}

		public object getValue(Injector injector, string id)
		{
			if(needInject && !hasInjected){
				realInjector.injectInto(val);
				hasInjected = true;
			}
			return val;
		}
    }
}
