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

		public bool notify()
		{
			//module.notifyImp();
		}

		abstract public void exec(Msg msg);
	}
}
