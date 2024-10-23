using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyWebClassLibrary;

namespace MyFirstBlazorApp.Components.Pages.ExamplesPages
{
    // This must be a partial class to properly interact with the .razor page
    // The two together are considered to be the entire class definition
    public partial class DataEntry
    {
        private string feedback = ""; 
        // Dictionaries are associative containers, which means a "value" can be accessed by
        // specifying the "Key".  Both the Key and the Value may be any data type.
        private Dictionary<string, string> errorMessages = new Dictionary<string, string>();

        private string employmentTitle = "";
        private DateTime startDate;
        private double employedYears = 0;
        private SupervisoryLevel employmentLevel;
        private Employment employment;

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IWebHostEnvironment WebHostEnvironment { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            //errorMessages.Add("Test", "My more detailed message");  // Add a Key/Value pair to the Dictionary
            ////errorMessages.Add("Test", "A new message!");  // If you attempt to add the same Key again, the 
            //                                                // Dictionary will throw an Exception
            //errorMessages["Test"] = "A new message";    // You may index the Dictionary by the Key to access the Value.
            //                                            // The Value may be changed in this way, but the Key is not editable.

            //errorMessages["AnotherTest"] = "SomeMessage";  // If you attempt to assign to a Key that does not already
            //                                               // exist, the Key will be created and then the supplied Value 
            //                                               // assigned to the Key

            //Console.WriteLine(errorMessages["AnotherTest"]);  // You may access the Value for extraction using the indexer as well

            startDate = DateTime.Today;
        }

        private void CollectEmploymentInfo()
        {
            feedback = "";
            errorMessages.Clear();

            #region Business Rules
            // Title must be present, must have at least one character
            // Start date must be today or in the past
            // Years of employment may not be less than zero

            if (string.IsNullOrWhiteSpace(employmentTitle))
            {
                errorMessages.Add("Title", "Employment title is required");
            }

            if (startDate > DateTime.Today)
            {
                errorMessages.Add("Start Date", "Start Date must not be in the future");
            }

            if (employedYears < 0)
            {
                errorMessages.Add("Years", "Employed years may not be less than 0");
            }

            if (errorMessages.Count > 0)
            {
                return;
            }
            #endregion


            feedback = $"Entered data is {employmentTitle},{startDate},{employedYears},{employmentLevel}";

            string appRootPath = WebHostEnvironment.ContentRootPath;
            string csvFilename = $@"{appRootPath}/Data/Employments.csv";

            try
            {
                employment = new Employment(employmentTitle, employmentLevel, startDate, employedYears);

                string line = $"{employment.ToString()}\n";
                File.AppendAllText(csvFilename, line);


            }
            catch (FormatException ex)
            {
                errorMessages.Add($"Format Error: {errorMessages.Count + 1}", GetInnerException(ex).Message );
            }
            catch (ArgumentNullException ex)
            {
                errorMessages.Add($"Null Error: {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                errorMessages.Add($"Out of Range Error: {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (ArgumentException ex)
            {
                errorMessages.Add($"Argument Error: {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }
            catch (Exception ex)
            {
                errorMessages.Add($"General Error: {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }

        }

        private Exception GetInnerException(Exception ex)
        {
            while(ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }

        private async Task Clear()
        {
            feedback = "";

            object[] messageline = new object[]
                {"Clearing will lose all unsaved data."
                    + " Are you sure you want to clear the form?" };

            if(await JSRuntime.InvokeAsync<bool>("confirm", messageline))
            {
                errorMessages.Clear();
                employmentTitle = "";
                startDate = DateTime.Today;
                employedYears = 0;
                employmentLevel = SupervisoryLevel.Entry;
            }
        }

        private void GoToReport()
        {
            NavigationManager.NavigateTo("report");
        }
    }
}
