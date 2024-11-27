using DBInteractionSystem.DAL;
using DBInteractionSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractionSystem.BLL
{
    public class ProductServices
    {
        #region Setup of context and class constructor
        // This set up is the same in every system service file in your BLL folder
        // The only thing that will change is the name of the context being used
        private readonly WestWindContext _westWindContext;

        internal ProductServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }
        #endregion

        public int Product_Add(Product inputProduct)
        {
            #region Data Validation
            List<Exception> errors = new List<Exception>();

            // If no information is provided, then we may as well exit the whole operation with a throw
            // to let the user know what they did incorrectly.
            if (inputProduct == null)
            {
                throw new ArgumentNullException("You must supply product information to be saved!");
            }

            if (string.IsNullOrWhiteSpace(inputProduct.ProductName))
            {
                errors.Add(new Exception("The product name must be provided!"));
            }

            #endregion

            #region Business Rules Enforcement
            // In addition to simple data validation, we may have a more complex business rule.  In this 
            // case, we will allow a supplier to give us only one product with the same name and description.
            // For example:  Supplier 25, may provide Milk in a QuantityPerUnit of 4L, but they may not
            //                have a second product with the same two features.  However, Supplier 30 may 
            //                also supply Milk in a quantity of 4L.
            bool exists = false;

            exists = _westWindContext.Products
                                    .Any(product => product.SupplierID == inputProduct.SupplierID
                                                    && product.ProductName == inputProduct.ProductName
                                                    && product.QuantityPerUnit == inputProduct.QuantityPerUnit);

            if (exists)
            {
                errors.Add(new Exception($"Product {inputProduct.ProductName} "
                                            + $"from {inputProduct.Supplier.CompanyName} of size "
                                            + $"{inputProduct.QuantityPerUnit} aleady exists!"));
            }

            #endregion

            // If any data validation or business rule violation is detected, then we should inform the user
            // of the problems and exit without performing any database manipulation.
            if(errors.Count > 0)
            {
                throw new AggregateException("Server operation detected errors: ", errors);
            }

            // Once all data validation and business rules adherence has been cleared without problems,
            // then we may proceed with database operations.
            
            // Accessing a database may always possibly result in unexpected errors.  For instance, a
            // communication breakdown with the database engine.
            try
            {
                // This is where the insert operation is being attempted.
                _westWindContext.Products.Add(inputProduct);
            }
            catch (Exception ex)
            {
                errors.Add(ex);  // Add any error that may be caught into our collection of errors to be
                                 // thrown back to the called if a database interaction problem occurs
            }

            // If any errors whatsoever are encountered, we must clear any changes that may have occurred
            // successfully before throwing the exception that ends the method.
            if(errors.Count > 0)
            {
                // The ChangeTracker stores all changes to be made to the database when a SaveChanges() call
                // is used.  This would include any changes left over in the CHangeTracker during a failed 
                // database operation.  Thus, we must Clear() the ChangeTracker on error detection.
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("Server operation detected errors: ", errors);
            }
            // If no errors are encountered, then we may save all changes to the database.
            else
            {
                _westWindContext.SaveChanges();
            }

            return inputProduct.ProductID;
        }
    }
}
