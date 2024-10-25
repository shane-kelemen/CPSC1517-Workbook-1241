using MyWebClassLibrary;

namespace MyFirstBlazorApp.Components.Pages.ExamplesPages
{
    public partial class EmploymentReport
    {
        // Standard fields for the feedback and error messages sections on the razor page
        private string feedback = "";
        private List<string> errorMessages = new List<string>();

        // The list that will nbe examine to determin what to display on the front page.  Will
        // hold all of the valid Employment instances we are able to Parse from the file.
        private List<Employment> employments = null;


        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ReadEmploymentsFromFile();  // Call the file Parsing method on page load.
        }


        void ReadEmploymentsFromFile()
        {
            // Reset the user messaging fields
            feedback = "";
            errorMessages.Clear();

            // On this page we will specify the working directory (current content directory) differently than the DataEntry page
            // This type of pathing will start at the top of your web application (meaning the root directory).
            string filePath = @"./Data";

            // Files for testing different things.  Note that these are all in the Data folder
            // 0 - The good file.  All data is valid and so will display without errors
            // 1 - An Empty file to have our empty List message display
            // 2 - A file with an incorrect file extension.  Should cause the invalid file message to display
            // 3 - A non-existant file.  Should cause the invalid file message to display
            // 4 - A file with a row that is invalid.  The valid rows will Parse and display correctly.
            //     Some errors for the bad lines will also be displayed.  If all bad line, then the empty list message
            //     would display with the errors instead of a table.
            string[] filenames = { "Employments.csv", "Empty.csv", "TextFile.txt", "", "BadData.csv" };
            string filename = $@"{filePath}/{filenames[4]}";  // Adjust the index to one of the above to see the different results.


            string[] employmentsData = null;   // Array for holding the results of the ReadAllLines() operation if successfully read
            Employment employment = null;      // Object to hold each created employment instance when Parsed
            int recordIndex = 1;               // For tracking which line of the file we are reading.  Wanted to write more complete
                                               // error message when a bad Parse is encountered.

            try
            {
                // Attempt to run the Parse if the file exists and it has the correct file extension (.csv)
                if (File.Exists(filename) && Path.GetExtension(filename) == ".csv")
                {
                    employments = new List<Employment>();  // Create the List so that the bad file message does not display
                                                           // Still allows the No Employments message to display

                    employmentsData = File.ReadAllLines(filename);  // Read all of the data from the file

                    // Attempt to Parse each line in the file to a valid Employment instance
                    foreach (string empFromFile in employmentsData)
                    {
                        try
                        {
                            employment = Employment.Parse(empFromFile);  // If this line throws an exception
                            employments.Add(employment);
                        }
                        catch (Exception ex)
                        {
                            // Add an error message to the user indicating the row that had a problem.
                            errorMessages.Add($"employmentsData {recordIndex} : {ex.Message}");
                        }

                        ++recordIndex;  // Always increase the recordIndex
                    }
                }
                else
                {
                    throw new Exception($"File {filename} does not exist or wrong extension!");
                }
            }
            catch (Exception ex)
            {
                errorMessages.Add(GetInnerException(ex).Message);  // For displaying error messages we cannot predict.
                                                                   // Usually file operation mishaps.
            }
        }

    }
}
