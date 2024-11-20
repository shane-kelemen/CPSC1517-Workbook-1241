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
