namespace MicroDI.Interfaces
{
    public interface IBootstrap
    {
        void SetupBindings(IContainer container);
    }
}
