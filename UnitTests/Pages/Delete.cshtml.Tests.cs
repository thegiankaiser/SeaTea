﻿using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Pages.Delete
{
    //<summary>
    //The purpose of this class is to run a unit test
    //on the Delete method because we want to have a 100%
    //coverage for our unit tests.
    //</summary>
    public class DeleteTests
    {
        #region TestSetup

        //Sets up the testing environment
        public static DeleteModel pageModel;


        //Inititates pagemodel to a Delete Model object
        [SetUp]
        public void TestInitialize()
        {
            //Initilizes the unit test using the TestHelper class.
            pageModel = new DeleteModel(TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        //Applies Unit Tests to the delete method for the website. 
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange



            // Act
            pageModel.OnGet("Boba Up");



            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Boba Up - Self Serve", pageModel.Product.Title);
        }
        #endregion OnGet



        #region OnPostAsync
        [Test]
        public void OnPostAsync_Valid_Should_Return_Products()
        {
            // Arrange



            // First Create the product to delete
            pageModel.Product = TestHelper.ProductService.CreateData();
            pageModel.Product.Title = "Example to Delete";
            TestHelper.ProductService.UpdateData(pageModel.Product);



            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;



            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Listing"));



            // Confirm the item is deleted
            Assert.AreEqual(null, TestHelper.ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(pageModel.Product.Id)));
        }



        [Test]
        public void OnPostAsync_InValid_Model_NotValid_Return_Page()
        {
            // Arrange
            pageModel.Product = new ProductModel
            {
                Id = "bogus",
                Title = "bogus",
                Description = "bogus",
                Url = "bogus",
                Image = "bougs"
            };



            // Force an invalid error state
            pageModel.ModelState.AddModelError("bogus", "bogus error");



            // Act
            var result = pageModel.OnPost() as ActionResult;



            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);
        }
        #endregion OnPostAsync
    }
}