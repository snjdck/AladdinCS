namespace Aladdin.IOC
{
    interface IInjectionPoint
    {
        void injectInto(object target, Injector injector);
    }
}
