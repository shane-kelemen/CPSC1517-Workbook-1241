using DBInteractionSystem.DAL;
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
    }
}
