namespace MyFirstBlazorApp.Components.Pages.ExamplesPages
{
    // This must be a partial class to properly interact with the .razor page
    // The two together are considered to be the entire class definition
    public partial class DataEntry
    {
        private string feedback; 

        // Dictionaries are associative containers, which means a "value" can be accessed by
        // specifying the "Key".  Both the Key and the Value may be any data type.
        private Dictionary<string, string> errorMessages = new Dictionary<string, string>();


        protected override void OnInitialized()
        {
            base.OnInitialized();

            errorMessages.Add("Test", "My more detailed message");  // Add a Key/Value pair to the Dictionary
            //errorMessages.Add("Test", "A new message!");  // If you attempt to add the same Key again, the 
                                                            // Dictionary will throw an Exception
            errorMessages["Test"] = "A new message";    // You may index the Dictionary by the Key to access the Value.
                                                        // The Value may be changed in this way, but the Key is not editable.

            errorMessages["AnotherTest"] = "SomeMessage";  // If you attempt to assign to a Key that does not already
                                                           // exist, the Key will be created and then the supplied Value 
                                                           // assigned to the Key

            Console.WriteLine(errorMessages["AnotherTest"]);  // You may access the Value for extraction using the indexer as well
        }

    }
}
