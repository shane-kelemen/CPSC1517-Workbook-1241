using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Threading.Tasks.Dataflow;

namespace IntroToClasses
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The following are examples of using Enumerations
            // Notice how we may have variables of enumeration types, and how we assign values to them, and
            // how this may be done just after declaration, making enumerations value types.
            DaysOfTheWeek myDay = DaysOfTheWeek.Friday;
            DaysOfTheWeek anotherDay = (DaysOfTheWeek)5;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of Enumeration Operations");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("myDay : " + myDay);                     
            Console.WriteLine("myDay integer value : " + (int)myDay);
            Console.WriteLine("anotherDay : " + anotherDay);
            Console.WriteLine();
            Console.WriteLine("Showing cast enum value in a math statement : " + 6 * (int)PowersOfTwo.Eight);
            Console.WriteLine();

            // There are Enumerations built into the system assemblies too.  Take a look at the use below
            // of an Enumeration called StringSplitOptions to indicate user choices without the use of
            // "magic numbers"  In this case RemoveEmptyEntries is used instead of the number 2.
            Console.WriteLine("The following is a list of words obtained from a string split operation:");
            string greeting = "Hello World!  How are you today?";
            string[] words = greeting.Split(new char[] {' ', '?', '!'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in words)
            {
                Console.WriteLine(word);
            }
            Console.WriteLine();


            // Parallel Arrays
            // Collections of data that are not the same type are often needed to help us fully describe
            // items or situations.  Without complex data types, we would be limited to parallel arrays
            // and shown by the following:

            // Key pieces of information needed to describe a golfer are a firstName, lastName, and a handicap
            string firstName = "John";
            string lastName = "Smith";
            int handicap = 15;          // The average score over par a golfer scores in the last 10 rounds

            // To store many golfers' information, we will need three arrays, called parallel arrays.
            // The following will store 100 golfers' information.  The first spot in each array has been populated
            // with the values above.
            string[] firstNames = new string[100];
            string[] lastNames = new string[100];
            int[] handicaps = new int[100];
            firstNames[0] = firstName;
            lastNames[0] = lastName;
            handicaps[0] = handicap;


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of information in Parallel Arrays");
            Console.ForegroundColor = ConsoleColor.Gray;
            // We may display this information by indexing each of the arrays.
            Console.WriteLine("Directly accessing the parallel arrays : ");
            Console.WriteLine(firstNames[0] + " " + lastNames[0] + " : handicap = " + handicaps[0]);

            // What becomes more inconvenient and prone to error is when we must pass these arrays to other
            // methods.  We must be sure to keep the arrays in the correct order.
            Console.WriteLine("First passing the parallel arrays to a method : ");
            DisplayGolfers(firstNames, lastNames, handicaps);
            Console.WriteLine();


            // The following section is dedicated to working with structures
            //
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of information in Structures");
            Console.ForegroundColor = ConsoleColor.Gray;
            //
            // Declaration and intialization of a single Structure of type SGolfer, defined after the 
            // enumerations below.  Note how the structure can be used immediately after declaration,
            // making structures values types just like enumerations.
            SGolfer golfer;
            golfer.firstName = "Tony";
            golfer.lastName = "Danza";
            golfer.handicap = 34;

            // Display of a single structure SGolfer.  Note that this is not a lot different from displaying
            // the parallel array values for complexity, except that you do not need to worry about indexing.
            Console.WriteLine("Display of information in a single structure SGolfer");
            Console.WriteLine(golfer.firstName + " " + golfer.lastName + " : handicap = " + golfer.handicap);
            Console.WriteLine();

            // Declaration of an array of SGolfer structures.  The first structure in the array has been 
            // inititalized to the values of the test golfer above. Note that the index is still needed, but
            // now you index a single array and just modify the data members of that structure instead of
            // working with three separate arrays.
            SGolfer[] golfers = new SGolfer[100];
            golfers[0].firstName = firstName;
            golfers[0].lastName = lastName;
            golfers[0].handicap = handicap;

            // Display for the array of Structure objects
            Console.WriteLine("Directly accessing the first SGolfer structure in the array :");
            Console.WriteLine(golfers[0].firstName + " " + golfers[0].lastName + " : handicap = " + golfers[0].handicap);
            Console.WriteLine("First passing the SGolfer array to a method : ");
            // Note the simplicity of the method call when passing an array of structures compared to parallel 
            // arrays
            DisplayGolfers(golfers);
            Console.WriteLine();


            // The following section deals with display of information stored in classes
            // 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Display of information in Classes");
            Console.ForegroundColor = ConsoleColor.Gray;
            // Set up of a single Class golfer
            try
            {
                // Notice that we are not assigning any values to the data members inside the class.
                // As a result, when we display the class items similar to the way we displayed a 
                // single structure, the values will display as the default values assigned in the constructor.
                CGolfer myGolfer = new CGolfer();
                Console.WriteLine("Display of information in a single class instance of CGolfer");
                // Note how the access through the class properties allows the class designer to restrict how the
                // contained data will be returned / displayed.
                Console.WriteLine(myGolfer.FirstName + " " + myGolfer.LastName + " : handicap = " + myGolfer.Handicap);
                Console.WriteLine("Display of information in a single class instance of CGolfer using ToString()");
                // The following implicitly calls the ToString() override implemented in the class.  Using this
                // vehicle for display also allows the user to restrict how data will be accessed.
                Console.WriteLine(myGolfer);

                // The following were commented out because they will cause an exception and end program
                // executeion.  Uncomment if you wish to see the exception occur.
                //myGolfer.FirstName = "Sally";
                //myGolfer.LastName = "Jane";
                //myGolfer.Handicap = -11;
                //
                //List<int> tempGolfRounds = myGolfer.GetRounds();
                //myGolfer.FirstName = "Henry";

                // The following calls the more explicit constructor, overriding all the default values.
                Console.WriteLine("Display of overriden information in a single class instance of CGolfer");
                CGolfer otherGolfer = new CGolfer("Enzo", "Ferrari", 5);
                Console.WriteLine(otherGolfer.FirstName + " " + otherGolfer.LastName + " : handicap = " + otherGolfer.Handicap);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            


            
        }


        // Using parallel arrays to display golfers to the screen
        static private void DisplayGolfers(string[] firsts, string[] lasts, int[] caps)
        {
            for (int index = 0; index < firsts.Length; ++index)
            {
                if (firsts[index] != null)
                {
                    Console.WriteLine(firsts[index] + " " + lasts[index] + " : handicap = " + caps[index]);
                }
            }
        }

        // Using an array of structure objects to display golfers to the screen
        static private void DisplayGolfers(SGolfer[] golfers)
        {
            foreach(SGolfer golfer in golfers)
            {
                if (golfer.firstName != null)
                {
                    Console.WriteLine(golfer.firstName + " " + golfer.lastName + " : handicap = " + golfer.handicap);
                }
            }
        }

        // Using an array of class objects to display golfers to the screen
        static private void DisplayGolfers(CGolfer[] golfers)
        {
            foreach (CGolfer golfer in golfers)
            {
                if (golfer.FirstName != null)
                {
                    Console.WriteLine(golfer.FirstName + " " + golfer.LastName + " : handicap = " + golfer.Handicap);
                }
            }
        }
    }

    // Data Types
    // To this point, you have used different data types that are all part of the assemblies that make up .Net
    // They are divided into two general categories:  value types and reference types
    //
    // Value Types - These types include all of the fundamental types like integers, doubles, characters, etc.
    //               These types are are stored entirely in stack memory as discussed in class.  Once they have
    //               been declared they are immediately usable, meaning values may be assigned to them.
    //               For instance:   int number = 45;
    //                               Console.WriteLine(number);
    //               Will work just fine without any further preparation.
    //
    // Reference Types - These types are special in that they are created in heap memory, and only their address
    //                   in the heap memory is stored in stack memory.  Declaring the variable does nothing more
    //                   than reserve space in stack memory for eventually storing the address of the object
    //                   once it has been created in heap memory via the new keyword.
    //                   For instance:  Random rng;
    //                                  rng.Next(256);      This will cause a NullReferenceException because the 
    //                                                      object has not been created yet.
    //
    //                                  rng = new Random(); Now the object has been created in heap memory and
    //                                                      the address has been stored in the stack variable rng.
    //                                  rng.Next(256);      This call will now work properly and provide a number 
    //                                                      between 0 and 255.
    //
    // We talked about this extensively in class, and while you will not be tested on the specifics of addressing,
    // it does well for you to be aware of these details.


    // User-defined Data Types - These are types that are not included in the .NET assemblies, but rather types 
    //                           that you design.  They are also categorized as value types or reference types.
    //                           Best practice is for user-defined types to be defined at the namespace level. 
    //
    // Enumerations - These are the simplest user-defined type, essentially being named integers.
    //                If no number is assigned, then the first will be 0, the second 1, etc.
    //                If a number is assigned, then the next will automatically be one more, and so on.
    //                Be careful that you do not accidentally cause the same number to be used twice or 
    //                both will be interpretted as the first one.
    //
    // The following are example definitions:
    enum DaysOfTheWeek { Sunday, Monday, Tuesday = 3, Wednesday, Thursday, Friday = 36, Saturday }
    enum PowersOfTwo { Zero = 1, One = 2, Two = 4, Three = 8, Four = 16, Five = 32, Six = 64, 
                            Seven = 128, Eight = 256, Nine = 512, Ten = 1024 }


    // Structures - These are composite types that are really only meant to allow the movement of related 
    //              information of different data types.  Structures are values types, and thus may be used
    //              immediately after declaration, with no need of the "new" keyword during declaration.
    //
    // Note that the data members of structures in C# are by default provate, and so you must mark them public
    // if you wish users of your structure to be able to use them.
    struct SGolfer
    {
        public string firstName;
        public string lastName;
        public int handicap;
        public List<int> values;       // NEVER DO THIS!
                                       // Meaning including a reference type inside a value type.
                                       // The danger is that you will have objects that are partly temporary
                                       // copies and partly accessors to original objects in heap memory.  This
                                       // means you run the risk of persisting part of your changes if you
                                       // pass a structure with a reference type in it to a method.
    }


    // Classes - These are composite data types meant to be more complete objects that "encapsulate" data and
    //           behaviour.  Classes are reference types, meaning they are constructed in heap memory, with only
    //           their addresses being stored in stack memory.  The "new" keyword is required to cause a class
    //           instance to be created in heap memory.
    //           
    // As with structure, all things included in a class are by default private.  This is desirable when creating 
    // data members, but properties and methods will often be used to interact with the user of the class.
    class CGolfer
    {
        // Data Members - All data members should be commented even if it is trivial.  These should be left
        //                private, with the only access to them from the users outside the class granted
        //                through the functionality of properties and methods.
        private string _firstName;  // golfer's first name
        private string _lastName;   // golfer's last name
        private int _handicap;      // golfer's average whole number of strokes over par in their last 10 games
        private List<int> _last10RoundScores = new List<int>();  // golfer's last ten scores
        //private int _lastScore;


        // Properties - These are special methods that take the place of accessors and mutators in other languages.
        //              They come in several varieties as described below, generally from a significant portion
        //              of the user interface for the class, and may be used like variable outside the class
        //              instead of having to resort to emthod syntax.


        // Manual Properties - These are bound to data members, meaning their job is to retrieve
        //                     or save information to the data member, often with safeguards on the 
        //                     information in at least one of the set or get, sometimes both.

        // The following manual property was commented out when an equivalent automatic property was created
        // below.
        //public int LastScore
        //{
        //    get
        //    {
        //        return _lastScore;
        //    }

        //    set
        //    {
        //        _lastScore = value;
        //    }
        //}

        public string FirstName
        {
            // We may choose to restrict how our internal data is accessed.  In this case we only allow
            // first names to be accessed as fully capitalized.
            get 
            { 
                return _firstName.ToUpper(); 
            }

            // When the user tries to set infromation in the class, we have the opportunity to place
            // restrictions on the input data. Generally, we throw exceptions when illegal data is supplied,
            // but we could also choose to correct the data, such as setting a supplied value below a tested
            // limit to be equal to the limit boundary.
            //
            // The input data will always be accessible through the keyword "value", whether it is a simple 
            // fundamental type, or a complex composite data type.
            set
            {
                if (value == null)
                    throw new ArgumentNullException("The first name was null!");

                _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName.ToLower();
            }

            set
            {
                _lastName = value;
            }
        }

        // Calculated property
        // A calculated property is one that is not directly linked to setting and retrieving data from within the class.
        // Typically an output calculation is performed on one or more pieces of information in the class.
        public string FullName
        {
            get 
            { 
                return _lastName + ", " + _firstName; 
            }
        }

        public int Handicap
        {
            get
            {
                return _handicap;
            }

            set
            {
                if (value < -10)
                    throw new ArgumentException("A handicap of less than -10 makes not sense.  Try again!");

                _handicap = value;
            }
        }

        // Complex types may be used to define properties.
        public List<int> LastRounds
        {
            get
            {
                // This line returns not the encapsulated List to the the user which would allow unrestricted
                // access to the values contained within, but rather provides a copy of the requested data
                // using the constructor of the List
                List<int> temp = new List<int>(_last10RoundScores);
                return temp;
            }

            // Note that more than one data member may be affected by a single property, even though only a
            // single item of information is provided by the user.
            set
            {
                foreach(int score in value)
                {
                    _last10RoundScores.Add(score);
                }

                LastScore = value[value.Count - 1];
            }
        }


        // Automatic Property - used when all we want to do is get and/or set values 
        //                      without any manipulation or validation.

        // Note:  The lastScore data member and manual property were commented out when
        //        this autmatic property was created.
        //
        //        Notice the optional "private" in front of the set.  This allows us to choose only to 
        //        allow use of the set inside the class.  This may be used on manual properties as well.
        public int LastScore { get; private set; }


        // Constructors - where instances are created
        //
        // Note that constructors have no return type, not even void.  You may also choose to assign
        // default values to the parameters so that if no values are assigned, these with kick in.
        public CGolfer (string firstName = "John", string lastName = "Doe", int handicap = 0)
        { 
            // Once properties have been defined within a class, it is best practice to use those properties
            // everywhere else in the class where they are attached to saving values in the proivate data
            // members. 
            FirstName = firstName;
            LastName = lastName;
            Handicap = handicap;
        }

        // The following is the defualt constructor.  You receive a free defualt constructor from the compiler 
        // if you have not defined one.  But as soon as you define any other constructor, the provided one is 
        // hidden, and so if you do want to have a default you must create your own.
        //
        // Note that this constructor is "leveraging" the more explicit constructor above using the "this" keyword.
        // Also note the use of a named parameter which allows us to choose which of the other constructor's 
        // default values we will override.
        // 
        // Finaly, realize that the body of thist constructor will still be run after the leveraged
        // constructor has finished execution.
        public CGolfer() : this(lastName: "Smith")
        {
            LastScore = 100;

           
        }

        // Override Methods - Inherited from higher up in the framework hierarchy
        // 
        // This particular method is being used in the test code to show how the ToString()
        // is implicitly called in some situations.
        public override string ToString()
        {
            return FirstName + " " + LastName + " : handicap = " + Handicap;
        }














        // the following are Accessor Methods - In languages that do not have properties,
        // these methods are used to get information from the class
        public string GetFirstName()
        {
            return _firstName.ToUpper();
        }

        public string GetLastName()
        {
            return _lastName.ToLower();
        }

        public int GetHandicap()
        {
            return _handicap;
        }

        /// <summary>
        /// Provides a new temporary copy of the internal List of golf rounds
        /// </summary>
        /// <returns></returns>
        public List<int> GetRounds()
        {
            List<int> temp = new List<int>(_last10RoundScores);
            return temp;
        }


        // The following type of method is called a mutator, again used only when
        // the langauge does nto have properties.  Mutatrors are used to provide 
        // information to the class.
        public void SetHandicap(int handicap)
        {
            _handicap = handicap;
        }
    }
}
