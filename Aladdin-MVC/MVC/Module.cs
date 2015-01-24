using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aladdin.MVC
{
	public class Module
	{
		internal bool notifyImpl(Msg msg)
		{
			
		}
	}

	static class ModuleExt
	{
		static public string GetFullName(this Enum msgName)
		{
			return string.Format("{0}::{1}", msgName.GetType().FullName, msgName.ToString());
		}
	}
}
