namespace MicroDI.Interfaces
{
    public interface IInjector
    {
        object CreateInstance(IBinding binding, object[] args);

        void InjectMembers(object target);
    }
}
