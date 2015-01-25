using System;
using System.Collections.Generic;

namespace Aladdin.MVC
{
	abstract public class Mediator
	{
		private object _viewComponent;
		private Dictionary<string, Action<Msg>> handlerDict;

		public Mediator(object viewComponent)
		{
			this._viewComponent = viewComponent;
			handlerDict = new Dictionary<string, Action<Msg>>();
		}

		internal protected object viewComponent
		{
			get { return _viewComponent; }
		}

		internal void handleMsg(Msg msg)
		{
			string key = msg.name.GetFullName();
			if(handlerDict.ContainsKey(key)){
				handlerDict[key](msg);
			}
		}

		protected void addMsgHandler(Enum msgName, Action<Msg> handler)
		{
			string key = msgName.GetFullName();
			if(!handlerDict.ContainsKey(key)){
				handlerDict[key] = handler;
			}
		}

		protected void removeMsgHandler(Enum msgName)
		{
			handlerDict.Remove(msgName.GetFullName());
		}

		protected internal abstract void onReg();
		protected internal abstract void onDel();
	}
}
