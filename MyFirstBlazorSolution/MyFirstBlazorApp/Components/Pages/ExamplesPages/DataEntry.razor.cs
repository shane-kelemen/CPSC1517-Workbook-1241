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

        // The following fields have been included to back up the controls on the Data Entry page 
        private string employmentTitle = "";
        private DateTime startDate;
        private double employedYears = 0;
        private SupervisoryLevel employmentLevel;
        //private Employment employment;            // Used to store an Employmnet class instance.  In this particular page
                                                    // it is actually not needed at the class level because it is being used 
                                                    // in only one place, just before saving the instance to the file in the 
                                                    // CollectEmploymentInfo method.  Thus it could have been created in that 
                                                    // method and then allowed to go out of scope after the instance was no 
                                                    // longer needed.

        // Inject blocks are needed when we wish to use services from other Assemblies in our code behind files
        // Make sure there is an appropriate using block placed at the top of the page if an error is indicated. 
        // Often no error will occur and/or the using statement will be included automatically if required.

        // IJSRuntime is in Microsoft.JSInterop
        // Used to employ some javascript functionality into the page.  In this case a popup that we wish to use to 
        // extract a confirmation from the user.
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        // NavigationManager is in Microsoft.AspNetCore.Components 
        // Used to move from one page to another through C# code rather than an HTML anchor
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        // IWebHostEnvironment is in Microsoft.AspNetCore.Hosting
        // Used to extract information about the current execution environment.  We are using it 
        // to extract the path of our current working directory.
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


        // Method for gathering the information from the page controls, validating the data, and storing it to the file 
        // if all data is valid.
        private void CollectEmploymentInfo()
        {
            // Standard clearing of user messaging fields
            feedback = "";
            errorMessages.Clear();

            #region Business Rules - Similar to the data validation we do in class properties
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
                return;  // An example of a valid early return.  We have detected illegal data.  No reason to continue.
            }
            #endregion


            // If we get to this point, we have all legal data.  Show this in the feedback
            feedback = $"Entered data is {employmentTitle},{startDate},{employedYears},{employmentLevel}";

            // We need to create and/or append to a file
            string appRootPath = WebHostEnvironment.ContentRootPath;  // Find out where the working directory is
            string csvFilename = $@"{appRootPath}/Data/Employments.csv"; // Add the rest of the pathing to where we want the 
                                                                         // file or where it already exists

            try
            {
                // Because this instance is transient (temporarily needed), it is better to declare it in the method
                // and then let it be destroyed (go out of scope) when we are finished with it.
                Employment employment = new Employment(employmentTitle, employmentLevel, startDate, employedYears);

                string line = $"{employment.ToString()}\n";  // This line arguably is not needed.  Could combine with the next line.
                                                             // The ToString() may also be eliminate because it will be called automagically
                                                             // by the runtime.
                File.AppendAllText(csvFilename, line);

                //File.AppendAllText(csvFilename, $"{employment}\n");  // A one-line version of the previous two lines of code
            }
            // The following catch blocks have been included so we may display different message dependent on the type 
            // of Exception encountered
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
            catch (Exception ex)  // Always have one block that can catch all Exceptions
            {
                errorMessages.Add($"General Error: {errorMessages.Count + 1}", GetInnerException(ex).Message);
            }

        }

        /// <summary>
        /// Method for getting to the root cause of an Exception
        /// </summary>
        /// <param name="ex">Provide the Highest Level exception to be mined for the root cause</param>
        /// <returns></returns>
        private Exception GetInnerException(Exception ex)
        {
            while(ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }

        // The following require the IJSRuntime [Inject] that you find above
        /// <summary>
        /// This method will reset all of the web controls to their default values after first seeking 
        /// confirmation from the user that they wish to perform the action
        /// </summary>
        /// <returns></returns>
        private async Task Clear()  // The Task is marked async because we must wait for a user response and we
        {                           // do not know how long that will take
           
            feedback = "";  // Reset the feedback messaging only

            // Initialize the array that will display the confirmation message to the user
            object[] messageline = new object[]
                {"Clearing will lose all unsaved data."
                    + " Are you sure you want to clear the form?" };

            // The await holds the processing of them page until the user either confirms or cancels the action.
            if(await JSRuntime.InvokeAsync<bool>("confirm", messageline))
            {
                errorMessages.Clear();  // Only clear the error messages on user confirmation
                
                // Reset all other controls to their starting values on confirmation
                employmentTitle = "";
                startDate = DateTime.Today;
                employedYears = 0;
                employmentLevel = SupervisoryLevel.Entry;
            }
        }

        // Go to the reporting page using the C# NavigationManager rather than an HTML anchor
        // This require the matching [Inject] that you will find near the top of the page
        private void GoToReport()
        {
            NavigationManager.NavigateTo("report");
        }
    }
}
