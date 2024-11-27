using Microsoft.AspNetCore.Components;
using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;


namespace DBInteractionWebApp.Components.Examples
{
    public partial class ProductCRUD
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                    // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();   // For storing error messages to be displayed on the razor page

        private Product CurrentProduct = new Product();            // For storing the current product information
        private List<Category> categories = new List<Category>();  // For storing the list of all available categories
        private List<Supplier> suppliers = new List<Supplier>();   // For storing the list of all available suppliers

        [Inject]
        private ProductServices ProductServices { get; set; }

        [Inject]
        private CategoryServices CategoryServices { get; set; }

        [Inject]
        private SupplierServices SupplierServices { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            categories = CategoryServices.Categories_GetAll();
            suppliers = SupplierServices.Suppliers_GetAll();
        }




        private void OnCreate()
        {

        }
    }
}




// CRUD -   CREATE  RETRIEVE    UPDATE  DELETE
// SQL -    INSERT  SELECT      UPDATE  DELETE
// HTTP -   POST    GET         PUT     DELETE

// RESTful Web Design Philosophy
// Works off the HTTP protocol

// Format of HTTP Request 

// GET thor.cnt.sast.ca/forProcessing.php HTTP/1.1
// headers:
//
// Payload - name/value pairs of information to the server


// Format of HTTP Response

// HTTP/1.1 200 OK
//
// Payload - whatever the resource was that was requested
