using DBInteractionSystem.BLL;
using DBInteractionSystem.Entities;
using Microsoft.AspNetCore.Components; // This allows us to use the Inject tag for connecting to our Transients
                                       // in the WestWindExtensions class.

namespace DBInteractionWebApp.Components.Examples
{
    public partial class RegionQuery
    {
        // Data Members / Fields
        private string feedback = string.Empty;
        List<string> errorMessages = new List<string>();
        private int regionArg = 0;
        private int regionSelect = 0;
        List<Region> regions = null;
        private Region regionInfo = null;


        // Properties
        [Inject]
        public RegionServices regionServices { get; set; }

        
        // Similar to OnLoad / Constructor for the page
        protected override void OnInitialized()
        {
            base.OnInitialized();

            regions = regionServices.Region_GetAll();
        }

        void GetByID()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            regionInfo = null;

            if (regionArg > 0)
            {
                regionInfo = regionServices.Region_GetByID(regionArg);
            }
            else
            {
                errorMessages.Add("Region ID must be greater than 0!");
            }
        }

        void GetBySelect()
        {
            feedback = string.Empty;
            errorMessages.Clear();
            regionInfo = null;

            if (regionSelect > 0)
            {
                regionInfo = regionServices.Region_GetByID(regionSelect);
            }
            else
            {
                errorMessages.Add("Region ID must be greater than 0!");
            }
        }
    }
}
