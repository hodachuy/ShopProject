using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;

namespace ShopProject.Service
{
    public interface IErrorService
    {
        Error Create(Error error);
        void Save();
    }
    public class ErrorService : IErrorService
    {
        IErrorRepository errorRepository;
        IUnitOfWork unitOfWork;
        public ErrorService(IErrorRepository errorRepository,IUnitOfWork unitOfWork)
        {
            this.errorRepository = errorRepository;
            this.unitOfWork = unitOfWork;
        }
        public Error Create(Error error)
        {
            return errorRepository.Add(error);
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
    }
}
