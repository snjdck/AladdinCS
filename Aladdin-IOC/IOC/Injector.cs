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

		public void mapValue<T>(T value, string id = null, bool needInject = false) where T : class
		{
			var injectionType = new InjectionTypeValue(value, needInject, this);
			mapRule<T>(injectionType, id);
		}

		public void mapValue<T>(T value, string id, Injector realInjector) where T : class
		{
			var injectionType = new InjectionTypeValue(value, true, realInjector ?? this);
			mapRule<T>(injectionType, id);
		}
		public void mapClass<K, V>(string id = null, Injector creator = null) where K : class where V : K, new()
		{
			var injectionType = new InjectionTypeClass(creator ?? this, typeof(V));
			mapRule<K>(injectionType, id);
		}

		public void mapClass<T>(string id = null, Injector creator = null) where T : class, new()
		{
			mapClass<T, T>(id, creator);
		}

		public void mapSingleton<K, V>(string id = null, Injector creator = null) where K : class where V : K, new()
		{
			var injectionType = new InjectionTypeSingleton(creator ?? this, typeof(V));
			mapRule<K>(injectionType, id);
		}

		public void mapSingleton<T>(string id = null, Injector creator = null) where T : class, new()
		{
			mapSingleton<T, T>(id, creator);
		}

		public void mapRule(Type keyType, IInjectionType rule, string id = null)
		{
			dict.Add(getKey(keyType, id), rule);
		}

		public void mapRule<T>(IInjectionType rule, string id = null) where T : class
		{
			mapRule(typeof(T), rule, id);
		}

		public void unmap(Type keyType, string id = null)
		{
			dict.Remove(getKey(keyType, id));
		}

		public void unmap<T>(string id = null) where T : class
		{
			unmap(typeof(T), id);
		}

		public object getInstance(Type type, string id = null)
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

		public T getInstance<T>(string id = null) where T : class
		{
			return getInstance(typeof(T), id) as T;
		}

		public object newInstance(Type type)
		{
			var result = Activator.CreateInstance(type);
			injectInto(result);
			return result;
		}

	    public T newInstance<T>() where T : class, new()
	    {
		    var result = new T();
			injectInto(result);
		    return result;
	    }

		public void injectInto(object target)
		{
			var injectionPoint = target.GetType().GetInjectionPoint();
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
