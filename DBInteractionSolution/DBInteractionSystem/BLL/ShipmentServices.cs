using DBInteractionSystem.DAL;
using DBInteractionSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBInteractionSystem.BLL
{
    public class ShipmentServices
    {
        #region Setup of context and class constructor
        // This set up is the same in every system service file in your BLL folder
        // The only thing that will change is the name of the context being used
        private readonly WestWindContext _westWindContext;

        internal ShipmentServices(WestWindContext westWindContext)
        {
            _westWindContext = westWindContext;
        }
        #endregion

        public List<Shipment> Shipment_GetByYearAndMonth(int year, int month)
        {
            #region
            List<Exception> errorMessages = new List<Exception>();

            if (year < 1950 || year > DateTime.Today.Year)
            {
                errorMessages.Add(new Exception ("The input year is out of the allowable range!"));
            }

            if (month < 1 || month > 12)
            {
                errorMessages.Add(new Exception("The input month is not a valid value!"));
            }

            if (errorMessages.Count > 0)
            {
                throw new AggregateException("There was an error(s) with your request:", errorMessages);
            }
            #endregion


            //List<Shipment> shipments =
            //                    _westWindContext.Shipments
            //                        .Where(shipment => shipment.ShippedDate.Year == year
            //                                                && shipment.ShippedDate.Month == month)
            //                        .ToList();

            //return shipments;


            return _westWindContext.Shipments
                                    .Include(shipment => shipment.ShipViaNavigation)
                                    .Where(shipment => shipment.ShippedDate.Year == year
                                                            && shipment.ShippedDate.Month == month)
                                    .ToList();
        }
    }
}
