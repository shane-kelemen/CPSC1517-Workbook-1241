﻿using Microsoft.AspNetCore.Components;
using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;


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
        //private bool activateText = false;
        //private string supplierText = string.Empty;

        private EditContext editContext;                           // To support the EditForm on the razor page
        private ValidationMessageStore validationMessageStore;     // For storing the messages to be displayed with the controls

        [Parameter]
        public int? productID { get; set; }                        // This is how we get a data piece that was passed into the
                                                                   // page via the URL, specified in the @page directive.  Make 
                                                                   // sure the data types match.  In this case we want to be able
                                                                   // to access the page even if no information was available, 
                                                                   // so we used a nullable data type, indicated by the ? after 
                                                                   // the data type name.


        [Inject]
        private ProductServices ProductServices { get; set; }      // Grants access to the methods in the ProductServices class of
                                                                   // the system library.  Remember the [Inject].

        [Inject]
        private CategoryServices CategoryServices { get; set; }    // Grants access to the methods in the CategoryServices class of
                                                                   // the system library.  Remember the [Inject].

        [Inject]
        private SupplierServices SupplierServices { get; set; }    // Grants access to the methods in the SupplierServices class of
                                                                   // the system library.  Remember the [Inject].

        [Inject]
        private IJSRuntime JSRuntime { get; set; }                 // Making available a dialog that will wait for a user's response

        [Inject]
        private NavigationManager NavigationManager { get; set; }  // Allows us to jump to another page in the application


        /// <summary>
        /// Time to Initialize the Blazor page controls
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            

            // The following are required to make validation of data on the razor page possible
            editContext = new EditContext(CurrentProduct);  // Create a context that will be applied to the CurrentProduct
            validationMessageStore = new ValidationMessageStore(editContext);  // Create a ValidationStore for the EditContext just created

            // Retrieve a List of all the Categories and Suppliers to poulate the select controls
            categories = CategoryServices.Categories_GetAll();
            suppliers = SupplierServices.Suppliers_GetAll();

            // We only want to retrieve product information if we were given a product ID.  If it was not provided, then
            // the productID Parameter we defined above will be null and HasValue will be false.
            if (productID.HasValue)
                CurrentProduct = ProductServices.Product_GetByID((int)productID);   // Get the information for the product
        }


        /// <summary>
        /// This method will collect the information from the razor page and send it to the ProductService
        /// class so that a database addition may be attempted.
        /// </summary>
        private void OnCreate()
        {
            // Reset all of the fields for feedback, error messaging, previous displayed validator errors,
            // and the ProductID in case a previous submit was already successful which would cause the 
            // ProductID to be something other than zero.
            feedback = "";
            errorMessages.Clear();
            validationMessageStore.Clear();
            CurrentProduct.ProductID = 0;

            try
            {
                // When a submit is attempted
                if (editContext.Validate())
                {
                    // Check for illegal or missing data in any of the fields that you wish to check on the
                    // client side
                    if (CurrentProduct.CategoryID == 0)
                    {
                        // For any item where the data is unacceptable, add an error message to the message store
                        // using the following pattern.  Notice it is consistent for all three checks included here
                        // aside from the item being accessed in the CurrentProduct and the message text.
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

                    // If there are any messages for the editContext
                    if(editContext.GetValidationMessages().Any())
                    {
                        // Have those messages displayed
                        editContext.NotifyValidationStateChanged();
                    }
                    // If there are no messages, proceed with a call to the ProductServices method for adding
                    // a new Product to the database.  Display success feedback if no Exceptions are thrown.
                    else
                    {
                        int newProductID = ProductServices.Product_Add(CurrentProduct);

                        feedback = $"Product: {CurrentProduct.ProductName} (ID: {newProductID} has been added!";
                    }                 
                }
            }
            catch (ArgumentNullException ex)  // Triggered if no Product instance is provided to the service method
            {
                errorMessages.Add(ex.Message);
            }
            catch (AggregateException ex)   // Triggered if data validation, business rules, or temporary data modifications encountered
            {
                foreach(Exception inner in ex.InnerExceptions)
                { 
                    errorMessages.Add(inner.Message);
                }
            }
            catch (Exception ex)    // Triggered if the final SaveChanges() operation throws an unexpected exception.
            {
                errorMessages.Add(HelperClass.GetInnerException(ex).Message);
            }
        }


        /// <summary>
        /// The following method will collect data from the razor page and send the values to the service
        /// for the purpose of the updating the database table record.        
        /// </summary>
        void OnUpdate()
        {
            feedback = "";
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                // When a submit is attempted
                if (editContext.Validate())
                {
                    // Check for illegal or missing data in any of the fields that you wish to check on the
                    // client side
                    if (CurrentProduct.CategoryID == 0)
                    {
                        // For any item where the data is unacceptable, add an error message to the message store
                        // using the following pattern.  Notice it is consistent for all three checks included here
                        // aside from the item being accessed in the CurrentProduct and the message text.
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

                    // If there are any messages for the editContext
                    if (editContext.GetValidationMessages().Any())
                    {
                        // Have those messages displayed
                        editContext.NotifyValidationStateChanged();
                    }
                    // If there are no messages, proceed with a call to the ProductServices method for adding
                    // a new Product to the database.  Display success feedback if no Exceptions are thrown.
                    else
                    { 
                        int rowsAffected = ProductServices.Product_Update(CurrentProduct);

                        if(rowsAffected != 0)
                        {
                            feedback = $"Product: {CurrentProduct.ProductName} (ID: {CurrentProduct.ProductID} has been updated!";
                        }
                        else
                        {
                            feedback = $"Product: {CurrentProduct.ProductName} (ID: {CurrentProduct.ProductID} has NOT been updated!  "
                                        + "Check that the product still exists on file!";
                        }
                        
                    }
                }
            }
            catch (ArgumentNullException ex)  // Triggered if no Product instance is provided to the service method
            {
                errorMessages.Add(ex.Message);
            }
            catch (ArgumentException ex)
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

        /// <summary>
        /// This method will call the update method in the service method in order to perform a "SOFT DELETE" of the 
        /// database record by updating the Discontinued flag of the product to true.
        /// </summary>
       void OnDiscontinue()
        {
            feedback = "";
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                // When a submit is attempted
                if (editContext.Validate())
                {
                    // Check for illegal or missing data in any of the fields that you wish to check on the
                    // client side
                    if (CurrentProduct.CategoryID == 0)
                    {
                        // For any item where the data is unacceptable, add an error message to the message store
                        // using the following pattern.  Notice it is consistent for all three checks included here
                        // aside from the item being accessed in the CurrentProduct and the message text.
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

                    // If there are any messages for the editContext
                    if (editContext.GetValidationMessages().Any())
                    {
                        // Have those messages displayed
                        editContext.NotifyValidationStateChanged();
                    }
                    // If there are no messages, proceed with a call to the ProductServices method for adding
                    // a new Product to the database.  Display success feedback if no Exceptions are thrown.
                    else
                    {
                        int rowsAffected = ProductServices.Product_LogicalDelete(CurrentProduct);

                        if (rowsAffected != 0)
                        {
                            feedback = $"Product: {CurrentProduct.ProductName} (ID: {CurrentProduct.ProductID} has been Discontinued!";
                        }
                        else
                        {
                            feedback = $"Product: {CurrentProduct.ProductName} (ID: {CurrentProduct.ProductID} has NOT been Discontinued!  "
                                        + "Check that the product still exists on file!";
                        }

                    }
                }
            }
            catch (ArgumentNullException ex)  // Triggered if no Product instance is provided to the service method
            {
                errorMessages.Add(ex.Message);
            }
            catch (ArgumentException ex)
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

        /// <summary>
        /// This method will perform a PHYSICAL DELETE of a record in the database.  This should not be taken
        /// lightly, and generally is only performed after the data to be deleted has been archived.
        /// </summary>
        void OnDelete()
        {
            feedback = "";
            errorMessages.Clear();
            validationMessageStore.Clear();

            try
            {
                // When a submit is attempted
                if (editContext.Validate())
                {
                    // Check for illegal or missing data in any of the fields that you wish to check on the
                    // client side
                    if (CurrentProduct.CategoryID == 0)
                    {
                        // For any item where the data is unacceptable, add an error message to the message store
                        // using the following pattern.  Notice it is consistent for all three checks included here
                        // aside from the item being accessed in the CurrentProduct and the message text.
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

                    // If there are any messages for the editContext
                    if (editContext.GetValidationMessages().Any())
                    {
                        // Have those messages displayed
                        editContext.NotifyValidationStateChanged();
                    }
                    // If there are no messages, proceed with a call to the ProductServices method for adding
                    // a new Product to the database.  Display success feedback if no Exceptions are thrown.
                    else
                    {
                        int rowsAffected = ProductServices.Product_PhysicalDelete(CurrentProduct);

                        if (rowsAffected != 0)
                        {
                            feedback = $"Product: {CurrentProduct.ProductName} (ID: {CurrentProduct.ProductID} has been Deleted!";
                        }
                        else
                        {
                            feedback = $"Product: {CurrentProduct.ProductName} (ID: {CurrentProduct.ProductID} has NOT been Deleted!  "
                                        + "Check that the product still exists on file!";
                        }

                    }
                }
            }
            catch (ArgumentNullException ex)  // Triggered if no Product instance is provided to the service method
            {
                errorMessages.Add(ex.Message);
            }
            catch (ArgumentException ex)
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

        /// <summary>
        /// This method will pop up a dialog that will seek input from the user.  In this case, 
        /// we are seeking verification that the user wishes to clear the data from the form.
        /// </summary>
        /// <returns></returns>
        async Task OnClear()
        { 
            // Set up the message to be displayed to the user.
            object[] messageLine = new object[] {"Clearing will lose all unsaved data."
                                                    + " Are you sure you want to clear the form?"};

            // Cause the dialog to appear.  If the user presses OK, then "confirm" has taken place and the 
            // if statement will run.
            if(await JSRuntime.InvokeAsync<bool>("confirm", messageLine))
            {
                // Reset all of the important values for the page.
                feedback = "";
                errorMessages.Clear();
                validationMessageStore.Clear();
                CurrentProduct = new Product();
                editContext = new EditContext(CurrentProduct);
            }
        }

        /// <summary>
        /// This method will pop up a dialog that will seek input from the user.  In this case, 
        /// we are seeking verification that the user wishes to leave the current page.
        /// </summary>
        /// <returns></returns>
        async Task GoToCategoryProducts()
        {
            // Set up the message to be displayed to the user.
            object[] messageLine = new object[] {"Leaving the page will lose all unsaved data."
                                                    + " Are you sure you want to clear the form?"};

            // Cause the dialog to appear.  If the user presses OK, then "confirm" has taken place and the 
            // if statement will run.
            if (await JSRuntime.InvokeAsync<bool>("confirm", messageLine))
            {
                NavigationManager.NavigateTo("/categoryProducts");  // Jump to the Category Products page
                                                                    // (same as the @page directive for the page we want to go to)
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
