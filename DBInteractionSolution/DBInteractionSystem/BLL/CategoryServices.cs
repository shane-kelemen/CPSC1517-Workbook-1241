using DBInteractionSystem.DAL;
using DBInteractionSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractionSystem.BLL
{
    public class CategoryServices
    {
        #region Setup of context and class constructor
        // This set up is the same in every system service file in your BLL folder
        // The only thing that will change is the name of the context being used
        private readonly WestWindContext _westWindContext;

        internal CategoryServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }
        #endregion

        /// <summary>
        /// Return a list of all the categories in the database table ordered by name.
        /// Eventually to be used to populate the Categories Select control
        /// </summary>
        /// <returns></returns>
        public List<Category> Categories_GetAll()
        {
            return _westWindContext.Categories
                                   .OrderBy(category => category.CategoryName)
                                   .ToList();
        }
    }
}
