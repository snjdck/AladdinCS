using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class View : IView
	{
		[Inject]
		private Injector injector;

		private readonly Dictionary<object, Mediator> mediatorRefs;

		public View()
		{
			mediatorRefs = new Dictionary<object, Mediator>();
		}

		public void regMediator(Mediator mediator)
		{
			if(hasMediator(mediator)){
				return;
			}
			injector.injectInto(mediator);
			mediatorRefs[mediator.viewComponent] = mediator;
			mediator.onReg();
		}

		public void delMediator(Mediator mediator)
		{
			if(!hasMediator(mediator)) {
				return;
			}
			mediatorRefs.Remove(mediator.viewComponent);
			mediator.onDel();
		}

		public bool hasMediator(Mediator mediator)
		{
			return mediatorRefs.ContainsValue(mediator);
		}

		internal void notifyMediators(Msg msg)
		{
			foreach(var pair in mediatorRefs){
				if(!msg.isProcessCanceled()){
					pair.Value.handleMsg(msg);
				}
			}
		}
	}
}
