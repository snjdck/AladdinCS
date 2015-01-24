using System;

namespace Aladdin.MVC
{
	sealed public class Msg
	{
		public readonly Enum name;
		public readonly object data;

		private bool processCanceledFlag;
		private bool defaultPreventedFlag;

		public Msg(Enum name, object data)
		{
			this.name = name;
			this.data = data;
		}

		public void cancelProcess()
		{
			processCanceledFlag = true;
		}

		public bool isProcessCanceled()
		{
			return processCanceledFlag;
		}
		
		public void preventDefault()
		{
			defaultPreventedFlag = true;
		}

		public bool isDefaultPrevented()
		{
			return defaultPreventedFlag;
		}
	}
}
