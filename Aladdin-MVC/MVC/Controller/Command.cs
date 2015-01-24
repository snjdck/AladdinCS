using System;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	public class Command : INotifier
	{
		[Inject]
		protected Module module;

		[Inject]
		protected object msg;

		[Inject]
		protected object contextView;

		public bool notify()
		{
			//module.notifyImp();
		}

		public void exec()
		{
			
		}
	}
}
