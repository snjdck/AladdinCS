using System;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	abstract public class Proxy : INotifier
	{
		[Inject]
		private Module module;

		public bool notify(Enum msgName, object msgData = null)
		{
			module.notifyImpl(new Msg(msgName, msgData, this));
		}

		abstract protected internal void onReg();
		abstract protected internal void onDel();
	}
}
