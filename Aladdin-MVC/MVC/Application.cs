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
		}

		public void regModule(Type moduleType)
		{
			if(hasStartup) {
				throw new Exception("module should register before startup!");
			}
			if(moduleDict.ContainsKey(moduleType)){
				throw new Exception(string.Format("{0} has registered yet!", moduleType.FullName));
			}
			Module module = Activator.CreateInstance(moduleType) as Module;
			module.onReg(injector);
			moduleDict[moduleType] = module;
		}

		public void regModule<T>() where T : Module, new()
		{
			regModule(typeof(T));
		}

		internal void regService<K, V>(Injector moduleInjector=null) where K : class where V : K, new()
		{
			serviceInitializer.regService(typeof(K), typeof(V), moduleInjector);
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
			serviceInitializer.initialize();
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
