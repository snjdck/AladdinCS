using System;
using Aladdin.IOC;
using UnityEngine;

namespace Aladdin.MVC
{
	abstract public class Module : INotifier, IModel, IView, IController
	{
		private Application application;

		private readonly Injector injector;

		private readonly Model model;
		private readonly View view;
		private readonly Controller controller;

		public Module()
		{
			injector = new Injector();
			injector.mapValue(injector, null, false);
			injector.mapValue(this, null, false);

			model = injector.newInstance<Model>();
			view = injector.newInstance<View>();
			controller = injector.newInstance<Controller>();
		}

		internal void onReg(Injector appInjector)
		{
			application = appInjector.getInstance<Application>();
			injector.parent = appInjector;
		}

		protected void regService<K, V>(bool asLocal=false) where K : class where V : K, new()
		{
			if(asLocal){
				injector.mapSingleton<K, V>();
			}else{
				application.regService<K, V>(injector);
			}
		}

		public bool notify(Enum msgName, object msgData=null)
		{
			return notifyImpl(new Msg(msgName, msgData, this));
		}

		internal bool notifyImpl(Msg msg)
		{
			view.notifyMediators(msg);
			controller.execCmd(msg);
			return !msg.isDefaultPrevented();
		}

		protected internal abstract void initAllModels();
		protected internal abstract void initAllServices();
		protected internal abstract void initAllViews();
		protected internal abstract void initAllControllers();
		protected internal abstract void onStartup();

		public void regProxy<T>() where T : Proxy, new()
		{
			model.regProxy<T>();
		}

		public void delProxy(Type proxyType)
		{
			model.delProxy(proxyType);
		}

		public bool hasProxy(Type proxyType)
		{
			return model.hasProxy(proxyType);
		}

		public void regCmd<T>(Enum msgName) where T : Command, new()
		{
			controller.regCmd<T>(msgName);
		}

		public void delCmd(Enum msgName)
		{
			controller.delCmd(msgName);
		}

		public bool hasCmd(Enum msgName)
		{
			return controller.hasCmd(msgName);
		}

		public void regMediator(Mediator mediator)
		{
			view.regMediator(mediator);
		}

		public void delMediator(Mediator mediator)
		{
			view.delMediator(mediator);
		}

		public bool hasMediator(Mediator mediator)
		{
			return view.hasMediator(mediator);
		}

		public void mapView(GameObject go, Type mediatorType)
		{
			view.mapView(go, mediatorType);
		}
	}
}
