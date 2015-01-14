using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	public class Application
	{
		public void regModule(Module module)
		{
			
		}

		public void regService<K, V>(Injector moduleInjector=null) where K : class where V : K, new()
		{
			
		}

		public Injector getInjector()
		{
			return null;
		}

		public void startup()
		{
			
		}

		void onStartup()
		{
			
		}
	}
}
