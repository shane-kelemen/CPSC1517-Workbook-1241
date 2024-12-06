using DBInteractionSystem.DAL;
using DBInteractionSystem.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractionSystem.BLL
{
    public class SupplierServices
    {
        #region Setup of context and class constructor
        // This set up is the same in every system service file in your BLL folder
        // The only thing that will change is the name of the context being used
        private readonly WestWindContext _westWindContext;

        internal SupplierServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }
        #endregion

        /// <summary>
        /// Return a list of all the suppliers in the database table ordered by name.
        /// Eventually to be used to populate the Supplier Select control
        /// </summary>
        /// <returns></returns>
        public List<Supplier> Suppliers_GetAll()
        {
            return _westWindContext.Suppliers
                                   .OrderBy(supplier => supplier.CompanyName)
                                   .ToList();
        }
    }
}
