using DAL;

namespace BLL
{
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        
        public AppUnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private StateRepository _stateRepository;
        public IStateRepository States => _stateRepository ??= new StateRepository(_appDbContext);



    }
}