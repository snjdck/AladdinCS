using System;
using UnityEngine;

namespace Aladdin.MVC
{
	interface IView
	{
		void regMediator(Mediator mediator);
		void delMediator(Mediator mediator);
		bool hasMediator(Mediator mediator);

		void mapView(GameObject go, Type mediatorType);
	}
}
