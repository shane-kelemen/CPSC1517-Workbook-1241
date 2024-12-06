using Microsoft.AspNetCore.Components;
using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;

namespace DBInteractionWebApp.Components.Examples
{
    public partial class CategoryProducts
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                    // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();   // For storing error messages to be displayed on the razor 

        private int categoryID = 0;                                // For storing the current categoryID from the select control
        private List<Category> categories = new List<Category>();  // For storing the categories retrieved via the CategoryServices

        private List<Product> products = new List<Product>();      // For storing the products retrieved via the ProductServices
        private int productPage = 0;                               // Multiplier for how many pages to skip forward
        private int productsPerPage = 5;                           // For storing how many products to display on each page
        private int totalProductCountForCategory = 0;              // For storing the number of products in the current select categroy

        [Inject]
        public CategoryServices CategoryServices { get; set; }     // Make the Category Services methods available

        [Inject]
        public ProductServices ProductServices { get; set; }       // Make the Product Services methods available


        protected override void OnInitialized()
        {
            base.OnInitialized();

            categories = CategoryServices.Categories_GetAll();     // Retrieve all of the categories for population of the select control
        }


        /// <summary>
        /// This method retrieves products for the selected category from the ProductServices, 
        /// limited by the productsPerPage, and offset by the productPage
        /// and offset
        /// </summary>
        private void Fetch()
        {
            // Empty out the old information
            feedback = string.Empty;
            errorMessages.Clear();
            products.Clear();

            // Ensure a proper category has been selected
            if(categoryID <= 0)
            {
                feedback = "You must select a valid category to view products";
            }
            else
            {
                try
                {
                    // Retrieve the total number of possible results, for use with pagination
                    totalProductCountForCategory = ProductServices.Product_GetCountForCategoryID(categoryID);

                    // Get the products that match the paginator limits and the selected category.
                    products = ProductServices.Product_GetByCategoryID(categoryID, productPage, productsPerPage);

                }
                catch (ArgumentNullException ex)  // Triggered if no Product instance is provided to the service method
                {
                    errorMessages.Add(ex.Message);
                }
                catch (AggregateException ex)   // Triggered if data validation, business rules, or temporary data modifications encountered
                {
                    foreach (Exception inner in ex.InnerExceptions)
                    {
                        errorMessages.Add(inner.Message);
                    }
                }
                catch (Exception ex)    // Triggered if the final SaveChanges() operation throws an unexpected exception.
                {
                    errorMessages.Add(HelperClass.GetInnerException(ex).Message);
                }
            }
        }

        /// <summary>
        /// Increase the page count and then refresh the table results, leveraging the Fetch method
        /// </summary>
        void Next()
        {
            // Ensure that the page is only increased if that increase will not go past the last possible results
            if ((productPage + 1) * productsPerPage < totalProductCountForCategory)
            {
                ++productPage;
                Fetch();
            }        
        }

        /// <summary>
        /// Decrease the page count and then refresh the table results, leveraging the Fetch method
        /// </summary>
        void Previous()
        {
            // Make sure you do not go into negative page results
            if (productPage > 0)
            {
                --productPage;
                Fetch();
            }
        }
    }
}
