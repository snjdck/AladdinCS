using System;
using System.Collections.Generic;
using Aladdin.IOC;
using UnityEngine;

namespace Aladdin.MVC
{
	sealed class View : IView
	{
		[Inject]
		private Module module;

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
			mediatorRefs[mediator.go] = mediator;
			mediator.onReg();
		}

		public void delMediator(Mediator mediator)
		{
			if(!hasMediator(mediator)) {
				return;
			}
			mediatorRefs.Remove(mediator.go);
			mediator.onDel();
		}

		public bool hasMediator(Mediator mediator)
		{
			return mediatorRefs.ContainsValue(mediator);
		}

		public void mapView(GameObject go, Type mediatorType)
		{
			var mediator = Activator.CreateInstance(mediatorType, go) as Mediator;
			injector.injectInto(mediator);
			regMediator(mediator);
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
