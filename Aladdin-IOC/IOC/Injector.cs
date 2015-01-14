using System;
using System.Collections.Generic;

namespace Aladdin.IOC
{
    sealed public class Injector
    {
		Dictionary<string, IInjectionType> dict;
		public Injector parent;

	    public Injector()
	    {
		    dict = new Dictionary<string, IInjectionType>();
	    }

		public void mapValue<T>(T value, string id=null, bool needInject=true) where T : class
		{
			mapRule<T>(new InjectionTypeValue(value, needInject), id);
		}

		public void mapClass<K, V>(string id=null) where K : class where V : K, new()
		{
			mapRule<K>(new InjectionTypeClass(typeof(V)), id);
		}

		public void mapClass<T>(string id = null) where T : class, new()
		{
			mapClass<T, T>(id);
		}

		public void mapSingleton<K, V>(string id=null) where K : class where V : K, new()
		{
			mapRule<K>(new InjectionTypeSingleton(typeof(V)), id);
		}

		public void mapSingleton<T>(string id=null) where T : class, new()
		{
			mapSingleton<T, T>(id);
		}

		public void mapRule<T>(IInjectionType rule, string id=null) where T : class
		{
			dict.Add(getKey(typeof(T), id), rule);
		}

		public void unmap<T>(string id=null) where T : class
		{
			dict.Remove(getKey(typeof(T), id));
		}

		public object getInstance(Type type, string id=null)
		{
			IInjectionType injectionType = getInjectionType(getKey(type, id));

			if(injectionType != null){
				return injectionType.getValue(this, null);
			}
			if(!string.IsNullOrEmpty(id)){
				injectionType = getInjectionType(getKey(type));
				if(injectionType != null){
					return injectionType.getValue(this, id);
				}
			}
			return null;
		}

		public object newInstance(Type type)
		{
			var result = Activator.CreateInstance(type);
			injectInto(result);
			return result;
		}

		public void injectInto(object target)
		{
			IInjectionPoint injectionPoint = target.GetType().GetInjectionPoint();
			injectionPoint.injectInto(target, this);
		}

		IInjectionType getInjectionType(string key)
		{
			IInjectionType injectionType;
			Injector injector = this;
			do{
				injectionType = injector.getMapping(key);
				injector = injector.parent;
			}while(injectionType == null && injector != null);
			return injectionType;
		}

		string getKey(Type type, string id = null)
		{
			var key = type.FullName;
			return string.IsNullOrEmpty(id) ? key : (key + "@" + id);
		}

		IInjectionType getMapping(string key)
		{
			return dict.ContainsKey(key) ? dict[key] : null;
		}
	}
}
