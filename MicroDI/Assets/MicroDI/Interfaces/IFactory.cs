using MicroDI.Interfaces;

namespace Assets.MicroDI.Interfaces
{
    public interface IFactory
    {
        object Create(IContainer container, IBinding binding, params object[] args);
    }
}
