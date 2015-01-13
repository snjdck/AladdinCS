using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aladdin.IOC
{
    class InjectionPoint : IInjectionPoint
    {
	    readonly List<IInjectionPoint> injectionPointList;
	    readonly List<Type> typesNeedInject;

		public InjectionPoint(Type type)
		{
			MemberInfo[] memberInfoList = type.FindInjectMembers();

			injectionPointList = new List<IInjectionPoint>(memberInfoList.Length);
			typesNeedInject = new List<Type>(getTypesCount(memberInfoList));

			foreach(var memberInfo in memberInfoList) {
				if (memberInfo.MemberType != MemberTypes.Field){
					continue;
				}
				var fieldInfo = memberInfo as FieldInfo;
				injectionPointList.Add(new InjectionPointField(fieldInfo, fieldInfo.GetInjectTag()));
				typesNeedInject.Add(fieldInfo.FieldType);
			}

			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType != MemberTypes.Property) {
					continue;
				}
				var propInfo = memberInfo as PropertyInfo;
				injectionPointList.Add(new InjectionPointProperty(propInfo, propInfo.GetInjectTag()));
				typesNeedInject.Add(propInfo.PropertyType);
			}

			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType != MemberTypes.Method) {
					continue;
				}
				var methodInfo = memberInfo as MethodInfo;
				var paramInfoList = methodInfo.GetParameters();
				if(paramInfoList.Length <= 0){
					continue;
				}
				injectionPointList.Add(new InjectionPointMethod(methodInfo));
				foreach(var paramInfo in paramInfoList) {
					typesNeedInject.Add(paramInfo.ParameterType);
				}
			}

			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType != MemberTypes.Method) {
					continue;
				}
				var methodInfo = memberInfo as MethodInfo;
				if(methodInfo.GetParameters().Length <= 0) {
					injectionPointList.Add(new InjectionPointMethod(methodInfo));
				}
			}
	    }

		public void injectInto(object target, Injector injector)
		{
			foreach(var injectionPoint in injectionPointList){
				injectionPoint.injectInto(target, injector);
			}
		}

		internal Type[] getTypesNeedInject()
		{
			return typesNeedInject.ToArray();
		}

	    int getTypesCount(MemberInfo[] memberInfoList)
	    {
		    int result = memberInfoList.Length;
			foreach(var memberInfo in memberInfoList) {
				if(memberInfo.MemberType == MemberTypes.Method) {
					var methodInfo = memberInfo as MethodInfo;
					result += methodInfo.GetParameters().Length - 1;
				}
			}
		    return result;
	    }
    }

	static class InjectionPointExt
	{
		static readonly Dictionary<Type, InjectionPoint> injectionPointDict;

		static InjectionPointExt()
		{
			injectionPointDict = new Dictionary<Type, InjectionPoint>();
		}

		static internal InjectionPoint GetInjectionPoint(this Type type)
		{
			if(!injectionPointDict.ContainsKey(type)) {
				injectionPointDict.Add(type, new InjectionPoint(type));
			}
			return injectionPointDict[type];
		}
	}
}
