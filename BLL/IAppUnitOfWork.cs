using DAL;

namespace BLL
{
    public interface IAppUnitOfWork
    {
        IStateRepository States { get; }

    }
}