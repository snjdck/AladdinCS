using System;
using System.Collections.Generic;
using Aladdin.IOC;

namespace Aladdin.MVC
{
	sealed class ServiceInitializer
	{
		private List<ServiceRegInfo> serviceDefList;

		public ServiceInitializer()
		{
			serviceDefList = new List<ServiceRegInfo>();
		}

		public void regService(Type serviceInterface, Type serviceClass, Injector moduleInjector)
		{
			serviceDefList.Add(new ServiceRegInfo(serviceInterface, serviceClass, moduleInjector));
		}

		public void initialize()
		{
			serviceDefList = resortList(serviceDefList);
			foreach(var serviceRegInfo in serviceDefList){
				serviceRegInfo.regService();
			}
		}

		List<ServiceRegInfo> resortList(List<ServiceRegInfo> list)
		{
			var serviceInFinding = new List<ServiceRegInfo>();
			var result = new List<ServiceRegInfo>();
			foreach (var serviceRegInfo in list){
				insertServiceRegInfo(serviceRegInfo, list, result, serviceInFinding);
			}
			return result;
		}

		void insertServiceRegInfo(ServiceRegInfo serviceRegInfo, List<ServiceRegInfo> list, List<ServiceRegInfo> result, List<ServiceRegInfo> serviceInFinding)
		{
			bool isServiceInFinding = serviceInFinding.Contains(serviceRegInfo);
			serviceInFinding.Add(serviceRegInfo);
			if(isServiceInFinding){
				throwRecursionError(serviceInFinding);
			}
			foreach (var serviceType in serviceRegInfo.typesNeedToBeInjected){
				var dependent = findServiceRegInfo(list, serviceType);
				if (dependent != null){
					insertServiceRegInfo(dependent, list, result, serviceInFinding);
				}
			}
			serviceInFinding.Remove(serviceRegInfo);
			if(!result.Contains(serviceRegInfo)){
				result.Add(serviceRegInfo);
			}
		}

		ServiceRegInfo findServiceRegInfo(List<ServiceRegInfo> list, Type serviceType)
		{
			foreach (var serviceRegInfo in list){
				if (ReferenceEquals(serviceRegInfo.serviceInterface, serviceType)){
					return serviceRegInfo;
				}
			}
			return null;
		}

		void throwRecursionError(List<ServiceRegInfo> serviceInFinding)
		{
			List<string> printInfo = new List<string>();
			foreach (var serviceRegInfo in serviceInFinding){
				printInfo.Add(serviceRegInfo.serviceInterface.FullName);
			}
			throw new Exception(
				string.Format(
					"service is recursively used:[{0}]",
					string.Join(" -> ", printInfo.ToArray())
				)
			);
		}
	}
}
