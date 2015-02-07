using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class Controller : IController
	{
		[Inject]
		private Injector injector;

		private readonly Dictionary<string, Command> cmdRefs;

		public Controller()
		{
			cmdRefs = new Dictionary<string, Command>();
		}

		public void regCmd<T>(Enum msgName) where T : Command, new()
		{
			cmdRefs[msgName.GetFullName()] = injector.newInstance<T>();
		}

		public void delCmd(Enum msgName)
		{
			cmdRefs.Remove(msgName.GetFullName());
		}

		public bool hasCmd(Enum msgName)
		{
			return cmdRefs.ContainsKey(msgName.GetFullName());
		}

		internal void execCmd(Msg msg)
		{
			if(msg.isProcessCanceled()){
				return;
			}
			var cmd = cmdRefs[msg.name.GetFullName()];
			cmd.exec(msg);
		}
	}
}
