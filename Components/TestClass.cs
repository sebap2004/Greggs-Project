namespace SoftwareProject.Components;
/// <summary>
/// Calculates the age of a person on a certain date based on the supplied date of birth.  Takes account of leap years,
/// using the convention that someone born on 29th February in a leap year is not legally one year older until 1st March
/// of a non-leap year.
/// </summary>
/// <param name="dateOfBirth">Individual's date of birth.</param>
/// <param name="date">Date at which to evaluate age at.</param>
/// <returns>Age of the individual in years (as an integer).</returns>
/// <remarks>This code is not guaranteed to be correct for non-UK locales, as some countries have skipped certain dates
/// within living memory.</remarks>
public class TestClass
{
    /// <summary>
    /// Calculates the age of a person on a certain date based on the supplied date of birth.  Takes account of leap years,
    /// using the convention that someone born on 29th February in a leap year is not legally one year older until 1st March
    /// of a non-leap year.
    /// </summary>
    /// <param name="dateOfBirth">Individual's date of birth.</param>
    /// <param name="date">Date at which to evaluate age at.</param>
    /// <returns>Age of the individual in years (as an integer).</returns>
    /// <remarks>This code is not guaranteed to be correct for non-UK locales, as some countries have skipped certain dates
    /// within living memory.</remarks>
    private int TestProperty { get; set; }
    
    
    /// <summary>
    /// Calculates the age of a person on a certain date based on the supplied date of birth.  Takes account of leap years,
    /// using the convention that someone born on 29th February in a leap year is not legally one year older until 1st March
    /// of a non-leap year.
    /// </summary>
    /// <param name="dateOfBirth">Individual's date of birth.</param>
    /// <param name="date">Date at which to evaluate age at.</param>
    /// <returns>Age of the individual in years (as an integer).</returns>
    /// <remarks>This code is not guaranteed to be correct for non-UK locales, as some countries have skipped certain dates
    /// within living memory.</remarks>
    public void TestMethod(DateTime dateOfBirth, DateTime date)
    {
        
    }
}