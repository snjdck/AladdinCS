using System;

namespace Aladdin.MVC
{
	static class ModuleExt
	{
		static public string GetFullName(this Enum msgName)
		{
			return string.Format("{0}::{1}", msgName.GetType().FullName, msgName.ToString());
		}
	}
}
