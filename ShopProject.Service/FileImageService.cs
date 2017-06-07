using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;
using System.Collections.Generic;
using System;

namespace ShopProject.Service
{
    public interface IFileImageService
    {
        FileImage Add(FileImage file);
        FileImage Delete(int id);
        void Update(FileImage file);
        void DeleteMutiImage(int productID);
        IEnumerable<FileImage> GetByProductID(int productID);
        void Save();
    }
    public class FileImageService : IFileImageService
    {
        IFileImageRepository _fileImageRepository;
        IUnitOfWork _unitOfWork;
        public FileImageService(IFileImageRepository fileImageRepository, IUnitOfWork unitOfWork)
        {
            _fileImageRepository = fileImageRepository;
            _unitOfWork = unitOfWork;

        }
        public FileImage Add(FileImage file)
        {
            return _fileImageRepository.Add(file);
        }

        public FileImage Delete(int id)
        {
            return _fileImageRepository.Delete(id);
        }

        public void DeleteMutiImage(int productID)
        {
            _fileImageRepository.DeleteMulti(x=>x.ProductImageID == productID);
            //_unitOfWork.Commit();
        }

        public IEnumerable<FileImage> GetByProductID(int productID)
        {
            return _fileImageRepository.GetMulti(x=>x.ProductImageID == productID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(FileImage file)
        {
            _fileImageRepository.Update(file);
        }
    }
}
