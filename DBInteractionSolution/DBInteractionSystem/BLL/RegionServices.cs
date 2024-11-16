using DBInteractionSystem.DAL;
using DBInteractionSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractionSystem.BLL
{
    public class RegionServices
    {
        #region Setup of context and class constructor
        private readonly WestWindContext _westWindContext;

        internal RegionServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }
        #endregion

        #region Services
        public List<Region> Region_GetAll()
        {
            // Separate commands to accomplish the end result, but not really needed
            //List<Region> regions = _westWindContext.Regions.ToList();
            //return regions.OrderBy(region => region.RegionDescription).ToList();

            // Using chaining of method calls, we can accomplish the same task a lot cleaner.
            // Remember that each method is going to return a new collection
            return _westWindContext.Regions
                                    //.Where(region => region.RegionDescription.Contains("New")) // We could filter using a Where call, but we don't want to in this case
                                    .OrderBy(region => region.RegionDescription)
                                    .ToList();
        }

        public Region Region_GetByID(int regionID)
        {
            //Region region = null;
            //region = _westWindContext.Regions
            //                            .Where(region => region.RegionID == regionID)
            //                            .FirstOrDefault();
            //return region;

            return _westWindContext.Regions
                                    .Where(region => region.RegionID == regionID)
                                    .FirstOrDefault();
        }
        #endregion
    }
}
