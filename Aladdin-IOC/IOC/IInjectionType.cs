namespace Aladdin.IOC
{
    public interface IInjectionType
    {
		object getValue(Injector injector, string id);
    }
}
