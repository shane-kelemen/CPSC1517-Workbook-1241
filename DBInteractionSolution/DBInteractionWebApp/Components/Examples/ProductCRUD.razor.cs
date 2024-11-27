using Microsoft.AspNetCore.Components;
using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;
using Microsoft.AspNetCore.Components.Forms;


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
        
        private EditContext editContext;                           // To support the EditForm on the razor page
        private ValidationMessageStore validationMessageStore;     // For storing the messages to be displayed beside the controls


        [Inject]
        private ProductServices ProductServices { get; set; }

        [Inject]
        private CategoryServices CategoryServices { get; set; }

        [Inject]
        private SupplierServices SupplierServices { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();

            editContext = new EditContext(CurrentProduct);
            validationMessageStore = new ValidationMessageStore(editContext);

            categories = CategoryServices.Categories_GetAll();
            suppliers = SupplierServices.Suppliers_GetAll();
        }


        private void OnCreate()
        {
            feedback = "";
            errorMessages.Clear();
            validationMessageStore.Clear();
            CurrentProduct.ProductID = 0;

            try
            {
                if (editContext.Validate())
                {
                    if (CurrentProduct.CategoryID == 0)
                    {
                        validationMessageStore.Add(editContext.Field(nameof(CurrentProduct.CategoryID)), 
                                                        "You must select a category");
                    }

                    if (CurrentProduct.SupplierID == 0)
                    {
                        validationMessageStore.Add(editContext.Field(nameof(CurrentProduct.SupplierID)),
                                                        "You must select a supplier");
                    }

                    if (CurrentProduct.UnitPrice <= 0)
                    {
                        validationMessageStore.Add(editContext.Field(nameof(CurrentProduct.UnitPrice)),
                                                        "The unit price must be greater than 0");
                    }

                    if(editContext.GetValidationMessages().Any())
                    {
                        editContext.NotifyValidationStateChanged();
                    }
                    else
                    {
                        int newProductID = ProductServices.Product_Add(CurrentProduct);

                        feedback = $"Product: {CurrentProduct.ProductName} (ID: {newProductID} has been added!";
                    }                 
                }
            }
            catch (ArgumentNullException ex)
            {
                errorMessages.Add(ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach(Exception inner in ex.InnerExceptions)
                { 
                    errorMessages.Add(inner.Message);
                }
            }
            catch (Exception ex)
            {
                errorMessages.Add(HelperClass.GetInnerException(ex).Message);
            }
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
