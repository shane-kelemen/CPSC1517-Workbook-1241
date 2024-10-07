namespace EmploymentSystem
{
    public class Employment
    {
        #region Fields
        SupervisoryLevel _level;
        string _title;
        double _years;
        #endregion

        #region Properties
        public SupervisoryLevel Level
        {
            get
            {
                return _level;
            }

            set
            {
                // Testing an enumeration for validity by converting the supplied enumeration
                // type into an interger,mthen using integers to test the range
                //if (value < 0 || (int)value > 5)
                //    throw new ArgumentException("The provided supervisory level is undefined!");

                // Testing an enumeration values by checking that the supplied value is defined
                // in the enumeration type
                //if (!Enum.GetNames(typeof(SupervisoryLevel)).Contains(value.ToString()))
                //    throw new ArgumentException("The provided supervisory level is undefined!");

                // An alternate way to extract all the enumeration names into an array of strings
                // but otherwise equivalent to the second option.
                //if (!Enum.GetNames<SupervisoryLevel>().Contains(value.ToString()))
                //    throw new ArgumentException("The provided supervisory level is undefined!");

                // A less compact, or more explicit set of steps for ac complishing the same as
                // the second and third options
                // First, extract all the enumeration defined values into an array of strings
                string[] names = Enum.GetNames(typeof(SupervisoryLevel));
                // Second, convert the provided enumeration value into a string
                string input = value.ToString();

                // Third, if the converted string does exist in the array of defined values
                if (!names.Contains(input))
                    // throw the exception
                    throw new ArgumentException("The provided supervisory level is undefined!");

                // Assign the provided enumeration value to the data field
                _level = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                // Ensure that the provided string is not null, empty string, or just whitespace.
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Title is required!");
                }

                _title = value.Trim();  // Remove the leading and trailing whitespace during
                                        // assignment to the class field.
            }
        }

        public double Years
        {
            get
            {
                return _years;
            }

            set
            {
                // The following test will limit the acceptable values to the inclusive range 
                // of 0 to 40.
                if (value < 0 || value > 40)
                {
                    throw new ArgumentOutOfRangeException("Years value supplied is out of range!");
                }
               
                _years = value;
            }
        }

        public DateTime StartDate { get; set; }  // Only a Fact test will be required when 
                                                 // automatic properties are involved.

        public List<WriteUp> WriteUps { get; set; }
        #endregion

        #region Constructors
        // The following is a default constructor.  You get one of these for free if no other
        // constructors are defined for the class.  Generally, a default constructor is required for
        // testing the properties before testing anything else.  After the properties are completed,
        // explicit constructors may be build leveraging the properties for assignment.
        public Employment() 
            // using "this" after the colon allows you to call one of the other constructors in the class.
            // This is one form of leveraging, where we use the algorithm in one constructor to simplify
            // the code in another constructor.  Basically, so you do not write the code twice.
            : this("Hello World!", SupervisoryLevel.TeamMember, DateTime.Today, null, 10)
        { 
        
        }





        public Employment(string title, SupervisoryLevel level, DateTime startDate, List<WriteUp> writeUps, double years = 0.0)
        {
            Level = level;
            Title = title;
            Years = years;
            StartDate = startDate;
            WriteUps = writeUps;
        }
        #endregion


        #region Methods
        public override string ToString()
        {
            return $"{Title},{Level},{StartDate.ToShortDateString()},{Years}";
        }
        #endregion
    }
}
