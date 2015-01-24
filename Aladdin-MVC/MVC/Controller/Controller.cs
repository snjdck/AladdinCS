using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	class Controller : IController
	{
		[Inject]
		private Injector injector;

		private Dictionary<object, Command> cmdRefs;

		public Controller()
		{
			
		}

		public void regCmd()
		{
			throw new NotImplementedException();
		}

		public void delCmd()
		{
			throw new NotImplementedException();
		}

		public bool hasCmd()
		{
			throw new NotImplementedException();
		}

		public void execCmd()
		{
			throw new NotImplementedException();
		}
	}
}
