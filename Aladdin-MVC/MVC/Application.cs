using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed public class Application
	{
		private readonly Injector injector;
		private readonly Dictionary<Type, Module> moduleDict;
		private ServiceInitializer serviceInitializer;

		public Application(Injector appInjector=null)
		{
			moduleDict = new Dictionary<Type, Module>();
			serviceInitializer = new ServiceInitializer();
			injector = appInjector ?? new Injector();
			injector.mapValue(this, null, false);
		}

		public void regModule(Module module)
		{
			if(hasStartup) {
				throw new Exception("module should register before startup!");
			}
			Type moduleType = module.GetType();
			if(moduleDict.ContainsKey(moduleType)){
				throw new Exception(string.Format("{0} has registered yet!", moduleType.FullName));
			}
			module.onReg(injector);
			moduleDict[moduleType] = module;
		}

		internal void regService<K, V>(Injector moduleInjector=null) where K : class where V : K, new()
		{
			var serviceRegInfo = new ServiceRegInfo(typeof(K), typeof(V), moduleInjector);
			serviceInitializer.regService(serviceRegInfo);
		}

		public void startup()
		{
			if(hasStartup){
				return;
			}
			var moduleList = moduleDict.Values;
			foreach(var module in moduleList){
				module.initAllModels();
			}
			foreach(var module in moduleList) {
				module.initAllServices();
			}
			serviceInitializer.initialize(injector);
			foreach(var module in moduleList) {
				module.initAllViews();
			}
			foreach(var module in moduleList) {
				module.initAllControllers();
			}
			serviceInitializer = null;
			foreach(var module in moduleList) {
				module.onStartup();
			}
		}

		bool hasStartup
		{
			get { return null == serviceInitializer; }
		}
	}
}
