using System;

namespace ShopProject.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        ShopDbContext Init();
    }
}