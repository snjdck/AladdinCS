namespace Aladdin.MVC
{
	interface IView
	{
		void regMediator();
		void delMediator();
		bool hasMediator();

		void mapView();
	}
}
