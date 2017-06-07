using ShopProject.Data.Infrastructure;
using ShopProject.Model.Models;

namespace ShopProject.Data.Repositories
{
    public interface IFileImageRepository : IRepository<FileImage>
    {

    }
    public class FileImageRepository : RepositoryBase<FileImage>, IFileImageRepository
    {
        public FileImageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
