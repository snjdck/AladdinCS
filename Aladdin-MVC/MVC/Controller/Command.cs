using System;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	abstract public class Command : INotifier
	{
		internal bool hasInject;

		[Inject]
		protected Module module;

		[Inject]
		protected object contextView;

		abstract public void exec(Msg msg);

		public bool notify(Enum msgName, object msgData=null)
		{
			return module.notifyImpl(new Msg(msgName, msgData, this));
		}
	}
}
