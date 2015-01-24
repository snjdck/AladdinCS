using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aladdin.MVC
{
	abstract public class Mediator
	{
		private GameObject _go;
		private Dictionary<string, Action<Msg>> handlerDict;

		public Mediator(GameObject go)
		{
			this._go = go;
			handlerDict = new Dictionary<string, Action<Msg>>();
		}

		internal protected GameObject go
		{
			get{ return _go; }
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
