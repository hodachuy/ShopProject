using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Service
{
    public interface IContactDetailService
    {
        ContactDetail GetDeFaultContact();
    }
    public class ContactDetailService : IContactDetailService
    {
        IContactDetailRepository _contactDetailRepository;
        IUnitOfWork _unitOfWork;
        public ContactDetailService(IContactDetailRepository contactDetailRepository, IUnitOfWork unitOfWork)
        {
            _contactDetailRepository = contactDetailRepository;
            _unitOfWork = unitOfWork;
        }
        public ContactDetail GetDeFaultContact()
        {
            return _contactDetailRepository.GetSingleByCondition(x => x.Status);
        }
    }
}
