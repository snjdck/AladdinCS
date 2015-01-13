using System;
using System.Reflection;

namespace Aladdin.IOC
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class Inject : Attribute
	{
		static public Type[] GetTypesNeedInject(Type type)
		{
			return type.GetInjectionPoint().getTypesNeedInject();
		}

		public string id;
		
		public Inject(string id=null)
		{
			this.id = id;
		}
	}

	static class InjectExt
	{
		const MemberTypes MemberType = MemberTypes.Method | MemberTypes.Property | MemberTypes.Field;
		const BindingFlags BindingFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		static internal MemberInfo[] FindInjectMembers(this Type type)
		{
			return type.FindMembers(MemberType, BindingFlag, __FilterMembers, null);
		}

		static bool __FilterMembers(MemberInfo memberInfo, object filterCriteria)
		{
			if(!Attribute.IsDefined(memberInfo, typeof(Inject))) {
				return false;
			}
			if(memberInfo.MemberType == MemberTypes.Property)
			{
				var propInfo = memberInfo as PropertyInfo;
				return propInfo.CanWrite && propInfo.GetIndexParameters().Length <= 0;
			}
			return true;
		}

		static internal Inject GetInjectTag(this MemberInfo info)
		{
			return Attribute.GetCustomAttribute(info, typeof(Inject)) as Inject;
		}
	}
}
