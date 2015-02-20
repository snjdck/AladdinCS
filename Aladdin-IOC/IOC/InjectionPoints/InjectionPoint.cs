using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aladdin.IOC
{
    class InjectionPoint : IInjectionPoint
    {
	    readonly IInjectionPoint[] injectionPointList;

		public InjectionPoint(Type type)
		{
			MemberInfo[] memberInfoList = type.FindInjectMembers();

			int count = memberInfoList.Length;
			injectionPointList = new IInjectionPoint[count];

			foreach(var memberInfo in memberInfoList) {
				if (memberInfo.MemberType == MemberTypes.Field){
					injectionPointList[--count] = new InjectionPointField(memberInfo as FieldInfo);
				}
			}

			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType == MemberTypes.Property) {
					injectionPointList[--count] = new InjectionPointProperty(memberInfo as PropertyInfo);
				}
			}

			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType != MemberTypes.Method) {
					continue;
				}
				var methodInfo = memberInfo as MethodInfo;
				if(methodInfo.GetParameters().Length > 0) {
					injectionPointList[--count] = new InjectionPointMethod(methodInfo);
				}
			}

			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType != MemberTypes.Method) {
					continue;
				}
				var methodInfo = memberInfo as MethodInfo;
				if(methodInfo.GetParameters().Length <= 0) {
					injectionPointList[--count] = new InjectionPointMethod(methodInfo);
				}
			}
	    }

		public void injectInto(object target, Injector injector)
		{
			for(int i = injectionPointList.Length - 1; i >= 0; --i){
				injectionPointList[i].injectInto(target, injector);
			}
		}
    }

	static class InjectionPointExt
	{
		static readonly Dictionary<Type, InjectionPoint> injectionPointDict;

		static InjectionPointExt()
		{
			injectionPointDict = new Dictionary<Type, InjectionPoint>();
		}

		static internal IInjectionPoint GetInjectionPoint(this Type type)
		{
			if(!injectionPointDict.ContainsKey(type)) {
				injectionPointDict.Add(type, new InjectionPoint(type));
			}
			return injectionPointDict[type];
		}
	}
}
