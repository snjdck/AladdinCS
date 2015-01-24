using System;

namespace Aladdin.MVC
{
	interface IController
	{
		void regCmd<T>(Enum msgName) where T : Command, new();
		void delCmd(Enum msgName);
		bool hasCmd(Enum msgName);
	}
}
