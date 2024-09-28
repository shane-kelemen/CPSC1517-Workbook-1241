//using Xunit;
using FluentAssertions;
using EmploymentSystem;

namespace TDDExampleTests
{
    public class Employment_Should
    {


        // The test for properties are probably the best ones to build and verify so that the 
        // properties may be used when building the class constructors and other methods.
        #region Property Tests
        // A Fact is used to construct a single test, generally the successful operation.
        // A theory could be used, but they are generally reserved for running multiple tests with
        // different sets of data, most often used for testing failure conditions.
        [Fact]
        public void Allow_Change_To_Title()
        {
            // Arrange - Setting up the testing scenario
            // In this case we need a class object, using the default constructor is sufficient as 
            // we are just looking to assign new values to the contained data members through the
            // properties.
            Employment actual = new Employment();
            string expectedTitle = "Hello World!";  // Tradition says use this for your first string test!

            // Act - The action that we are trying to test
            // To show that the test will first fail with incorrect values, use the commented version
            // before using the version that should cause the test to pass.
            //actual.Title = "Hello World";  // Missing the exclamation mark.
            actual.Title = "Hello World!";  // Comment out when testing the fail path.

            // Assert - The verification that the action is working correctly (or not)
            actual.Title.Should().Be(expectedTitle);  // If the actual and expected values match, this 
                                                      // test will pass.
        }

        // Theory is used to test multiple combinations of input data using a sinlge method set up to
        // run the test.
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Throw_Exception_For_Changing_Title(string title)  // The parameter will accept the 
                                                                      // data contained in the InLineData
                                                                      // definitions under the Theory tag
        {
            // Arrange
            Employment actual = new Employment();  // Create the default object

            // Act
            // The Action will save the definition of the operation to be performed by each run of the 
            // test.  In this case, the "action"  is the assignment of the input title value to the 
            // Title property of the actual object created above.
            Action action = () => actual.Title = title;

            // Assert
            // A test passes if the expected exception is thrown with a message that matches the 
            // pattern provided in the WithMessage() call.  The * represents 0 or more characters.
            // Any other characters must match exactly.  In this case, the exception message must end
            // with " is required!" because there is no * after the provided text.
            action.Should().Throw<ArgumentException>().WithMessage("* is required!");

        }

        [Fact]
        public void Allow_Change_To_Years()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            double expectedYears = 34.5;

            // Act - The action that we are trying to test
            actual.Years = 34.5;

            // Assert - The verification that the action is working correctly (or not)
            actual.Years.Should().Be(expectedYears);
        }

        [Theory]
        [InlineData(-0.0001)]  
        [InlineData(40.0001)]
        public void Throw_Exception_For_Changing_Years(double years)
        {
            // Arrange
            Employment actual = new Employment();

            // Act
            Action action = () => actual.Years = years;  // A straight assignment is satisfactory here 
                                                         // because the property is testing against a 
                                                         // range.  If equality was being looked for as
                                                         // the test, then rounding both values to the
                                                         // same number of decimal places is required
                                                         // to eliminate epsilon errors.

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("* out of range*");
        }

        [Fact]
        public void Allow_Change_To_SupervisoryLevel()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            SupervisoryLevel expectedLevel = SupervisoryLevel.Supervisor;
            
            // Act - The action that we are trying to test
            actual.Level = (SupervisoryLevel)3;

            // Assert - The verification that the action is working correctly (or not)
            actual.Level.Should().Be(expectedLevel);
        }

        [Theory]
        [InlineData((SupervisoryLevel)6)]  // Note that this value is not defined for the 
                                           // SupervisoryLevel enumeration.  More explanation in the 
                                           // property for options for how to conduct the tests.
        public void Throw_Exception_For_Changing_SupervisoryLevel(SupervisoryLevel level)
        {
            // Arrange
            Employment actual = new Employment();

            // Act
            Action action = () => actual.Level = level;

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("* is undefined*");
        }

        [Fact]
        public void Allow_Change_To_StartDate()
        {
            // Arrange - Setting up the testing scenario
            Employment actual = new Employment();
            DateTime expectedLevel = DateTime.Today;  // Using today's date makes the test dynamic so
                                                      // that you do not have to manually change the
                                                      // test over time.

            // Act - The action that we are trying to test
            // Using DateTime.Now as the date from which to extract the year, month, and day values
            // provides an alternate path for testing dynamic values.
            actual.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Assert - The verification that the action is working correctly (or not)
            actual.StartDate.Should().Be(expectedLevel);
        }
        #endregion


    }
}