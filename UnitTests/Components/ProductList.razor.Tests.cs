﻿using Bunit;
using NUnit.Framework;

using ContosoCrafts.WebSite.Components;
using Microsoft.Extensions.DependencyInjection;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using ContosoCrafts.WebSite.Models;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestContext = Bunit.TestContext;
using Castle.DynamicProxy;

namespace UnitTests.Components
{

    /// <summary>
    /// The purpose of this class file is to unit test
    /// ProductList Razor file because we want to be able
    /// to hit 100% on all of our unit testing in our file.
    /// </summary>
    public class ProductListTests : BunitTestContext
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
            /// Used to initialize ProductList.razor.Tests.cs
        {
        }

        #endregion TestSetup

        [Test]
        public void ProductList_Default_Should_Return_Content()
        ///Ensures that the product list returns default content.
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Get the Cards retrned
            var result = page.Markup;

            // Assert
            Assert.AreEqual(true, result.Contains("Tea King"));
        }


        #region SelectProduct
        [Test]
        public void SelectProduct_Valid_ID_TeaKing_Should_Return_Content()
            ///Test to check that selecting a specific item on the home page
            ///selects the correct product tile
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "Tea King";

            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));

            // Act
            button.Click();

            // Get the markup to use for the assert
            var pageMarkup = page.Markup;

            // Assert
            Assert.AreEqual(true, pageMarkup.Contains("Tea King"));
        }
        #endregion SelectProduct

        #region SubmitRating

        [Test]
        public void SubmitRating_Valid_ID_Click_Unstared_Should_Increment_Count_And_Check_Star()
        {
            ///
            ///This test tests that the SubmitRating will change the vote as well as the Star checked
            ///Because the star check is a calculation of the ratings, using a record that has no stars and checking one makes it clear what was changed
            ///The test needs to open the page
            ///Then open the popup on the card
            ///Then record the state of the count and star check status
            ///Then check a star
            ///Then check again the state of the cound and star check status
            ///

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "MoreInfoButton_Drip Tea";

            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();

            // Get the markup of the page post the Click action
            var buttonMarkup = page.Markup;

            // Get the Star Buttons
            var starButtonList = page.FindAll("span");

            // Get the Vote Count
            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var preVoteCountSpan = starButtonList[1];
            var preVoteCountString = preVoteCountSpan.OuterHtml;

            // Get the First star item from the list, it should not be checked
            var starButton = starButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star"));

            // Save the html for it to compare after the click
            var preStarChange = starButton.OuterHtml;

            // Act

            // Click the star button
            starButton.Click();

            // Get the markup to use for the assert
            buttonMarkup = page.Markup;

            // Get the Star Buttons
            starButtonList = page.FindAll("span");

            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var postVoteCountSpan = starButtonList[1];
            var postVoteCountString = postVoteCountSpan.OuterHtml;

            // Get the Last stared item from the list
            starButton = starButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star checked"));

            // Save the html for it to compare after the click
            var postStarChange = starButton.OuterHtml;

            // Assert

            // Confirm that the record had no votes to start, and 1 vote after
            Assert.AreEqual(true, preVoteCountString.Contains("Be the first to vote!"));
            Assert.AreEqual(true, postVoteCountString.Contains("1 Vote"));
            Assert.AreEqual(false, preVoteCountString.Equals(postVoteCountString));
        }

        [Test]
        public void SubmitRating_Valid_ID_Click_Stared_Should_Increment_Count_And_Leave_Star_Check_Remaining()
        {
            ///
            /// This test tests that the SubmitRating will change the vote as well as the Star checked
            /// Because the star check is a calculation of the ratings, using a record that has no stars and checking one makes it clear what was changed
            /// The test needs to open the page
            /// Then open the popup on the card
            /// Then record the state of the count and star check status
            /// Then check a star
            /// Then check again the state of the cound and star check status
            ///


            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "Tea King";

            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("Button");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();

            // Get the markup of the page post the Click action
            var buttonMarkup = page.Markup;

            // Get the Star Buttons
            var starButtonList = page.FindAll("span");

            // Get the Vote Count
            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var preVoteCountSpan = starButtonList[1];
            var preVoteCountString = preVoteCountSpan.OuterHtml;

            // Get the Last star item from the list, it should one that is checked
            var starButton = starButtonList.Last(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star checked"));

            // Save the html for it to compare after the click
            var preStarChange = starButton.OuterHtml;

            // Act

            // Click the star button
            starButton.Click();

            // Get the markup to use for the assert
            buttonMarkup = page.Markup;

            // Get the Star Buttons
            starButtonList = page.FindAll("span");

            // Get the Vote Count, the List should have 7 elements, element 2 is the string for the count
            var postVoteCountSpan = starButtonList[1];
            var postVoteCountString = postVoteCountSpan.OuterHtml;

            // Get the Last stared item from the list
            starButton = starButtonList.Last(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star checked"));

            // Save the html for it to compare after the click
            var postStarChange = starButton.OuterHtml;

            // Assert

            // Confirm that the record had no votes to start, and 1 vote after
            Assert.AreEqual(true, preVoteCountString.Contains("6 Votes"));
            Assert.AreEqual(true, postVoteCountString.Contains("7 Votes"));
            Assert.AreEqual(false, preVoteCountString.Equals(postVoteCountString));
        }
        #endregion SubmitRating
        [Test]
        public void FilterInput_IsValid()
        {
            ///
            /// This function tests that users can input information into the
            /// filter tab
            /// 


            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var input = "Tea King";

            var page = RenderComponent<ProductList>();

            // Find the Filter Input
            var inputBar = page.Find("input[type='text']");
            inputBar.Change(input);

            // Act

            // Assert
            Assert.AreEqual(input, inputBar.GetAttribute("value"));
        }
        [Test]
        public void Filter_Input_Filter_Button_Page_Is_Valid()
        {
            ///
            /// This function tests the filter button will re-render the page for
            /// whatever input the user given
            ///


            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var input = "Tea King";

            var page = RenderComponent<ProductList>();

            // Find the Filter Input
            var inputBar = page.Find("input[type='text']");
            var filterButton = page.Find("button.btn-success");
            inputBar.Change(input);

            // Act
            filterButton.Click();
            var filterMarkup = page.Markup;



            // Assert
            Assert.IsTrue(filterMarkup.Contains(input));
        }
        [Test]
        public void Clear_Input_Filter_Is_Valid()
        {
            ///
            /// This function tests that the clear button resets the input filter and 
            /// renders all objects to the home page
            ///

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var input = "Tea King";
            var otherStore = "Ding Tea";
            var page = RenderComponent<ProductList>();
            // Find the Filter Input
            var inputBar = page.Find("input[type='text']");
            var filterButton = page.Find("button.btn-success");
            inputBar.Change(input);
            filterButton.Click();
            var filterMarkup = page.Markup;
            var clearButton = page.Find("button.btn-danger");

            // Act
            clearButton.Click();
            var clearMarkup = page.Markup;

            // Assert
            Assert.IsTrue(clearMarkup.Contains(otherStore));

        }
        [Test]
        public void Neighborhood_Filter_Is_Valid()
        {
            ///
            /// This function tests that changing the neighborhood filter gives a 
            /// valid output
            ///

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var page = RenderComponent<ProductList>();
            var storeInInternationalDistrict = "TP Tea";
            var storeNotInNeighborhood = "Tea King";
            var input = "International District";
            var neighborhoodOptions = page.Find("#neighborhoodSelect");
            var selectNeighborhood = neighborhoodOptions.Children[2];

            // Act
            neighborhoodOptions.Change(input);
            var neighborhoodFilter = page.Markup;

            // Assert
            Assert.IsTrue(neighborhoodFilter.Contains(storeInInternationalDistrict));
            Assert.IsFalse(neighborhoodFilter.Contains(storeNotInNeighborhood));


        }
        [Test]
        public void Neighborhood_Filter_With_Input_Filter_Valid()
        {
            ///
            /// This function tests to ensure that the when the user interacts with both the 
            /// neighborhood filter and the input filter renders the products delimited by
            /// both data filters
            ///

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var page = RenderComponent<ProductList>();
            var neighborhoodInput = "International District";
            var input = "TP Tea";
            var neighborhoodOptions = page.Find("#neighborhoodSelect");
            var selectNeighborhood = neighborhoodOptions.Children[2];

            // Act
            neighborhoodOptions.Change(neighborhoodInput);
            var neighborhoodFilter = page.Markup;
            var inputBar = page.Find("input[type='text']");
            var filterButton = page.Find("button.btn-success");
            inputBar.Change(input);
            var inputFilter = page.Markup;

            // Assert
            Assert.IsTrue(inputFilter.Contains(input));
        }
    }
}
