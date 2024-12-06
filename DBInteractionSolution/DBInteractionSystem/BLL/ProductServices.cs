using DBInteractionSystem.DAL;
using DBInteractionSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        /// <summary>
        /// This method will attempt to add a new product into the Products table of the Westwind database,
        /// or more accurately, the database that is being accessed through the DbContext (WestWindContext) 
        /// provided to the constructor of the ProductServices class above from the associated Transient in
        /// the WestWindExtensions class.
        /// </summary>
        /// <param name="inputProduct">An instance of the Product entity.  Definition found in the Entities 
        ///                             folder of the system library.</param>
        /// <returns>The ProductID automatically assigned to the product by the database.  Identity column.</returns>
        /// <exception cref="ArgumentNullException">Thrown if a Product instance is not provided.</exception>
        /// <exception cref="AggregateException">Throws a collection of Exceptions encountered during data validation
        ///                                         and business rules enforcement.</exception>
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

            // All data should be checked to ensure it is legal.  An example check for the required ProductName
            // is shown here.  Other data checks may include valid ranges for values such as >= 0 for UnitPrice.
            // Appropriate messages should be added to the Exception collection as shown in the ProductName check.
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
            //                also supply Milk in a quantity of 4L, or Supplier 25 may also provide Milk in a
            //                QuantityPerUnit of 2L.
            bool exists = false;

            // Check to see if the product with the provided features already exists 
            exists = _westWindContext.Products
                                    .Any(product => product.SupplierID == inputProduct.SupplierID
                                                    && product.ProductName == inputProduct.ProductName
                                                    && product.QuantityPerUnit == inputProduct.QuantityPerUnit);

            // If the product with the provided features does exist, an an Exception with an appropriate and descriptive
            // message to the Exception collection.
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
                // This Clear() of the ChangeTracker is not required unless a database data change has occurred,
                // but it does not hurt anything to be double sure.
                _westWindContext.ChangeTracker.Clear();

                // This throws the Exception collection back to the calling scope so that the contained errors
                // may be used.
                throw new AggregateException("Server operation detected errors: ", errors);
            }

            // Once all data validation and business rules adherence has been cleared without problems,
            // then we may proceed with database operations.
            
            // Accessing a database may always possibly result in unexpected errors.  For instance, a
            // communication breakdown with the database engine, so it is advisable to use a try/catch
            // to perform the operation.  Add any errors to the Exception collection.
            try
            {
                // This is where the insert operation is being attempted, but the operation is only being tested in memory,
                // not saved to the database in SQL Server yet.  This is similar to how a transaction works in SQL.  Above,
                // when we Clear() the ChangeTracker, we are essentially performing a rollback on the transaction.  Later, we
                // will attempt to perform a commit when we use the SaveChanges() method of the context.
                _westWindContext.Products.Add(inputProduct);
            }
            catch (Exception ex)
            {
                errors.Add(ex);  // Add any error that may be caught into our collection of errors to be
                                 // thrown back to the caller if a database interaction problem occurs
            }

            // If any errors whatsoever are encountered, we must clear any changes that may have occurred
            // successfully before throwing the exception that ends the method.
            if(errors.Count > 0)
            {
                // The ChangeTracker stores all changes to be made to the database when a SaveChanges() call
                // is used.  This would include any changes left over in the ChangeTracker during a failed 
                // database operation.  Thus, we must Clear() the ChangeTracker on error detection.
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("Server operation detected errors: ", errors);
            }

            // If no errors are encountered, then we may attempt to save all changes to the database.
            else
            {
                // Once again, a try/catch should be used here, as the action of pushing the data changes to the
                // database may encounter problems that cannot be or have not been predicted, such as a communications
                // error, or the truncation of data.
                try
                {
                    _westWindContext.SaveChanges();
                }
                catch(Exception ex)
                {
                    // Once again, be sure to Clear() the database changes before throwing the received Exception back
                    // to the user.  The error collection is not required here as there can only be one Exception thrown
                    // if you have gotten to this point.
                    _westWindContext.ChangeTracker.Clear();
                    throw ex;
                }
            }

            // If the SaveChanges() method is successful, a ProductID will have been assigned to the product that we tried
            // to save.  We have decided to send it back to the user.
            return inputProduct.ProductID;
        }


        public int Product_Update(Product inputProduct)
        {
            List<Exception> errors = new List<Exception>();

            if (inputProduct == null)
            {
                throw new ArgumentNullException("You must supply product information to be updated!");
            }

            bool exists = false;

            exists = _westWindContext.Products.Any(product => product.ProductID == inputProduct.ProductID);

            if(!exists)
            {
                throw new ArgumentException($"Product {inputProduct.ProductName} from {inputProduct.Supplier.CompanyName}"
                                                + $"of size {inputProduct.QuantityPerUnit} does not exist on file!");
            }


            // All data should be checked to ensure it is legal.  An example check for the required ProductName
            // is shown here.  Other data checks may include valid ranges for values such as >= 0 for UnitPrice.
            // Appropriate messages should be added to the Exception collection as shown in the ProductName check.
            if (string.IsNullOrWhiteSpace(inputProduct.ProductName))
            {
                errors.Add(new Exception("The product name must be provided!"));
            }




            // Check to see if the product with the provided features already exists 
            exists = _westWindContext.Products
                                    .Any(product => product.SupplierID == inputProduct.SupplierID
                                                    && product.ProductName == inputProduct.ProductName
                                                    && product.QuantityPerUnit == inputProduct.QuantityPerUnit
                                                    && product.ProductID != inputProduct.ProductID);

            // If the product with the provided features does exist, an an Exception with an appropriate and descriptive
            // message to the Exception collection.
            if (exists)
            {
                errors.Add(new Exception($"Product {inputProduct.ProductName} "
                                            + $"from {inputProduct.Supplier.CompanyName} of size "
                                            + $"{inputProduct.QuantityPerUnit} aleady exists on a different product!"));
            }

            // If any errors whatsoever are encountered, we must clear any changes that may have occurred
            // successfully before throwing the exception that ends the method.
            if (errors.Count > 0)
            {
                // The ChangeTracker stores all changes to be made to the database when a SaveChanges() call
                // is used.  This would include any changes left over in the ChangeTracker during a failed 
                // database operation.  Thus, we must Clear() the ChangeTracker on error detection.
                _westWindContext.ChangeTracker.Clear();

                throw new AggregateException("Server operation detected errors: ", errors);
            }

            // This line tells the Entity Framework that you wish use the data in the inputProduct to 
            // perform an action
            EntityEntry<Product> updating = _westWindContext.Entry(inputProduct);
            updating.State = EntityState.Modified;      // This line sets the action as an update to the database

            // The following block is where the actions against the database are actually performed.  In particular,
            // SaveChanges().  It is possible an action we are trying to perform is illegal in the database, so we
            // put this block in a try/catch, and if an error occurs, then we clear the ChangeTracker and then throw
            // the Exception we caught back to the calling method.
            int rowsAffected = 0;
            try
            {
                rowsAffected = _westWindContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _westWindContext.ChangeTracker.Clear();
                throw ex;
            }

            return rowsAffected;    // If the operation was successful, return the number of rows affected.
        }


        /// <summary>
        /// Note that a Logical delete is the same as an update where the only thing changed is setting the 
        /// Discontinued flag to true.
        /// </summary>
        /// <param name="inputProduct"></param>
        public int Product_LogicalDelete(Product inputProduct)
        {
            List<Exception> errors = new List<Exception>();

            if (inputProduct == null)
            {
                throw new ArgumentNullException("You must supply product information to be updated!");
            }

            Product existingProduct = _westWindContext.Products.FirstOrDefault(product => product.ProductID == inputProduct.ProductID);

            if (existingProduct == null)
            {
                throw new ArgumentException($"Product {inputProduct.ProductName} from {inputProduct.Supplier.CompanyName}"
                                               + $"of size {inputProduct.QuantityPerUnit} does not exist on file!");
            }

            existingProduct.Discontinued = true;

            EntityEntry<Product> discontinuing = _westWindContext.Entry(existingProduct);
            discontinuing.State = EntityState.Modified;

            int rowsAffected = 0;
            try
            {
                rowsAffected = _westWindContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _westWindContext.ChangeTracker.Clear();
                throw ex;
            }

            return rowsAffected;
        }


        /// <summary>
        /// This method will permanently delete a record from the database.  This should not be done in 
        /// regular business practices unless the data has been archived.
        /// </summary>
        /// <returns></returns>
        public int Product_PhysicalDelete(Product inputProduct)
        {
            List<Exception> errors = new List<Exception>();

            if (inputProduct == null)
            {
                throw new ArgumentNullException("You must supply product information to be updated!");
            }

            bool exists = false;

            // The Any() method returns true if at least one item in the collection satisfies the condition
            // In this case, is there a product that has the same ProductID as the inputProduct?
            exists = _westWindContext.Products.Any(product => product.ProductID == inputProduct.ProductID);

            if (!exists)
            {
                throw new ArgumentException($"Product {inputProduct.ProductName} from {inputProduct.Supplier.CompanyName}"
                                                + $"of size {inputProduct.QuantityPerUnit} does not exist on file!");
            }

            // Is there a product that matches the supplied productID that is being referred to by a record in
            // the ManifestItems table?
            exists = _westWindContext.Products.Any(product => product.ManifestItems.Count > 0
                                                                && product.ProductID == inputProduct.ProductID);

            if (exists)
            {
                errors.Add(new Exception($"Product {inputProduct.ProductName} from {inputProduct.Supplier.CompanyName}"
                                                + $"of size {inputProduct.QuantityPerUnit} has associated manifest records on file!"
                                                + "  Unable to remove!"));
            }

            // Is there a product that matches the supplied productID that is being referred to by a record in
            // the OrderDetails table?
            exists = _westWindContext.Products.Any(product => product.OrderDetails.Count > 0
                                                                && product.ProductID == inputProduct.ProductID);

            if (exists)
            {
                errors.Add(new Exception($"Product {inputProduct.ProductName} from {inputProduct.Supplier.CompanyName}"
                                                + $"of size {inputProduct.QuantityPerUnit} has associated order details records on file!"
                                                + "  Unable to remove!"));
            }


            if (errors.Count > 0)
            {
                throw new AggregateException("There was a problem with the delete operation: ", errors);
            }


            // This time we are indicating that a delete operation is to be performed, but otherwise the format
            // is the same as for updating.
            EntityEntry<Product> deleting = _westWindContext.Entry(inputProduct);
            deleting.State = EntityState.Deleted;  // Indicate a delete...

            int rowsAffected = 0;
            try
            {
                rowsAffected = _westWindContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _westWindContext.ChangeTracker.Clear();
                throw ex;
            }

            return rowsAffected;
        }


        /// <summary>
        /// This method was created to support the paginator on the CategoryProducts page.  It returns
        /// the number of total products in the supplied category.
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public int Product_GetCountForCategoryID(int categoryID)
        {
            return _westWindContext.Products
                                        .Where(product => product.CategoryID == categoryID).Count();
        }


        /// <summary>
        /// This method will return products from the supplied Category.  They will be limited 
        /// in number to the value of productsPerPage, and offset by the productPage value. 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="productPage">The zero-index based page offset of results to return</param>
        /// <param name="productsPerPage">The maximum number of results to return</param>
        /// <returns></returns>
        public List<Product> Product_GetByCategoryID(int categoryID, int productPage, int productsPerPage)
        {
            List<Product> products = _westWindContext.Products
                                        .Include(product => product.Supplier)
                                        .Where(product => product.CategoryID == categoryID)
                                        .OrderBy(product => product.ProductName)
                                        .Skip(productPage * productsPerPage)    // Skip() jumps past the numerical number of results
                                                                                // If on the "first" page, product page will be zero,
                                                                                // so the results will begin at the start of the set.
                                                                                // The second page will begin after jumping past
                                                                                // productPerPage of results.
                                        .Take(productsPerPage)  // Only keep productsPerPage number of results beginning after
                                                                // the skipped results.
                                        .ToList();




            return products;
        }


        /// <summary>
        /// This method will return the details of a single product matching the provided productID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public Product Product_GetByID(int productID)
        {
            return _westWindContext.Products
                                    .Where(product => product.ProductID == productID)
                                    .FirstOrDefault();  // Remember that this method returns either the 
                                                        // first item from the results of the Where method
                                                        // if there are results, and null otherwise.
        }
    }
}
