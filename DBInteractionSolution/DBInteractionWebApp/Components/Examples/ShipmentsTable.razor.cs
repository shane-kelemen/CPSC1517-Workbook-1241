using DBInteractionSystem.Entities;
using DBInteractionSystem.BLL;
using Microsoft.AspNetCore.Components;

namespace DBInteractionWebApp.Components.Examples
{
    public partial class ShipmentsTable
    {
        // Data Members (Fields)
        private string feedback = string.Empty;                    // For feedback variable on the razor page
        private List<string> errorMessages = new List<string>();   // For storing error messages to be displayed on the razor page

        private int yearArg = 0;        // For storing the user selected year
        private int monthArg = 0;       // For storing the user selected month

        private List<Shipment> shipments = new List<Shipment>();    // For storing the results returned from the Shipments Service

        [Inject]
        public ShipmentServices ShipmentServices { get; set; }

        private void GetShipmentsByYearAndMonth()
        {
            // Reset the feedback and error mechanisms
            feedback = string.Empty;
            errorMessages.Clear();

            // Clear out the results from the last shipment query
            shipments = null;

            if (yearArg < 1950 || yearArg > DateTime.Today.Year)
            {
                errorMessages.Add("The input year is out of the allowable range!");
            }

            if (monthArg < 1 || monthArg > 12)
            {
                errorMessages.Add("The input month is not a valid value!");
            }

            if (errorMessages.Count == 0)
            {
                try
                {
                    shipments = ShipmentServices.Shipment_GetByYearAndMonth(yearArg, monthArg);
                }
                catch(Exception ex)
                {
                    errorMessages.Add("Unexpected System Error : " + ex.Message);
                }
            }
        }
    }
}
