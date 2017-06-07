namespace ShopProject.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShopDbContext _dbContext;
        private IDbFactory _dbFactory;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this._dbFactory = dbFactory;
        }

        public ShopDbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = _dbFactory.Init()); }
        }
        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
