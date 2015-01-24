using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class View : IView
	{
		[Inject]
		private Module module;

		[Inject]
		private Injector injector;

		private Dictionary<object, Mediator> mediatorRefs;

		public void regMediator(Mediator mediator)
		{
			throw new NotImplementedException();
		}

		public void delMediator(Mediator mediator)
		{
			throw new NotImplementedException();
		}

		public bool hasMediator(Mediator mediator)
		{
			throw new NotImplementedException();
		}

		public void mapView()
		{
			throw new NotImplementedException();
		}

		internal void notifyMediators(Msg msg)
		{
			foreach (var pair in mediatorRefs)
			{
				if (!msg.isProcessCanceled())
				{
					pair.Value.handleMsg(msg);
				}
			}
		}
	}
}
