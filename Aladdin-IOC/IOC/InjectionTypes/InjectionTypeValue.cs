namespace Aladdin.IOC
{
    class InjectionTypeValue : IInjectionType
    {
        bool needInject;
		bool hasInjected;
		object val;
		
		public InjectionTypeValue(object val, bool needInject)
		{
			this.needInject = needInject;
			this.val = val;
		}

		public object getValue(Injector injector, string id)
		{
			if(id != null){
				return null;
			}
			if(needInject && !hasInjected){
				injector.injectInto(val);
				hasInjected = true;
			}
			return val;
		}
    }
}
