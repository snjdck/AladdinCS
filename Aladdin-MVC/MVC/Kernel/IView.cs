namespace Aladdin.MVC
{
	interface IView
	{
		void regMediator(Mediator mediator);
		void delMediator(Mediator mediator);
		bool hasMediator(Mediator mediator);

		void mapView();
	}
}
