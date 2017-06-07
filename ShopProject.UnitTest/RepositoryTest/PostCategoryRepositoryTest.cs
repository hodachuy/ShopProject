using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopProject.Data.Infrastructure;
using ShopProject.Data.Repositories;
using ShopProject.Model.Models;

namespace ShopProject.UnitTest.RepositoryTest
{
    /// <summary>
    /// Summary description for PostCategoryRepositoryTest
    /// </summary>
    [TestClass]
    public class PostCategoryRepositoryTest
    {
        IDbFactory dbFactory;
        IPostCategoryRepository iPostCategoryRepository;
        IUnitOfWork iUnitOfWork;
        [TestInitialize]
        public void Initialize()
        {
            dbFactory = new DbFactory();
            iPostCategoryRepository = new PostCategoryRepository(dbFactory);
            iUnitOfWork = new UnitOfWork(dbFactory);
        }

        [TestMethod]
        public void PostCategory_Repository_Create()
        {
            PostCategory postCategory = new PostCategory();
            postCategory.Name = "A";
            postCategory.Alias = "B";
            postCategory.Status = true;

            var result = iPostCategoryRepository.Add(postCategory);
            iUnitOfWork.Commit();

            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.ID);

        }
       
    }
}
