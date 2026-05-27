// Exercise 1: The First Safety Net (LO 1.1: Environment + Null Safety) 
// ********************************************

    // The situation: The legacy TMS stored student region data as a plain string with no null checks. 
    // When the batch enrollment confirmation system tried to format mailing addresses at 2 AM, it 
    // called .ToUpper() on a region that was never assigned. The app crashed with 
    // NullReferenceException. Three hundred students received no confirmation email. Nobody noticed 
    // until Monday morning. 
    // The fix is not to add null checks everywhere after the fact. The fix is to make the compiler tell 
    // you about the problem before the code ever runs.
    // step 1
    // string Region = null;
    // Console.WriteLine(Region.ToUpper()); // NullReferenceException at runtime
    // the above code contain warring warning CS8600: Converting null literal or possible null value to non-nullable type.

    // step 2 How to fix it?
    // 1. Make the variable nullable
    string? Region = null;
    Console.WriteLine(Region?.ToUpper()); // No exception, but no output either. The null-conditional operator (?.) prevents the method call if Region is null.


    // step 3 declare the first TMS variable
    string StudentName = "Alice";
    string StudentId = "S12345";
    int enrollmentCount = 3;
    decimal courseFee = 199.99m;
    decimal GPA = 3.75m;
    DateTime enrollmentDate = DateTime.UtcNow;
    Console.WriteLine($"Student Name: {StudentName}, Student ID: {StudentId}, Enrollment Count: {enrollmentCount}, Course Fee: {courseFee}, GPA: {GPA}");


// Exercise 2: The Ministry Audit Failure (LO 1.2: Primitives)

    // The situation: The Ministry of Education relies on the TMS to allocate and report on training 
    // grants. During a quarterly audit, an automated script flagged a discrepancy of 0.03 Birr — a 
    // phantom balance distributed across 100,000 student accounts. The lead architect traced it to 
    // the invoicing module’s grant calculation. The legacy code used double for money. That single 
    // type choice created a financial error that survived three years of testing

    // The fix is to use the right type for money: decimal. The decimal type has a higher precision and is designed for financial calculations, which eliminates the rounding errors associated with double.
    // step 1: show the problem with double
    //  Legacy implementation — the bug that caused the audit failure 
     double grantPerStudent = 1999.99; 
    double totalAllocation = grantPerStudent * 100_000; // This may show a value like 970.0000000000001 due to precision issues.
    Console.WriteLine($"Final amount with double: {totalAllocation}");
    // step 2: show the fix with decimal
    decimal grantPerStudentDecimal = 1999.99m;
    decimal discountDecimal = 0.03m;
    decimal finalAmountDecimal = grantPerStudentDecimal - (grantPerStudentDecimal * discountDecimal);
    Console.WriteLine($"Final Amount with decimal: {finalAmountDecimal}"); // This will show the correct value without precision issues.
        // The double type is a base-2 (binary) floating-point primitive. It cannot accurately represent 
        // base-10 fractional numbers like 0.99.




// Exercise 3: Pipeline Data Corruption (LO 1.3 & 1.4: Encapsulation) 

    // The situation: In the TMS, once an Enrollment is processed, it passes through multiple 
    // middleware services logging, telemetry, notification. The database revealed that 12 students 
    // were mysteriously enrolled in NULL. The investigation found that a poorly written logging service 
    // was accidentally mutating the data during processing. Because the DTO used public set 
    // properties, the compiler could not prevent the mutation

// public class Enrollment
// {
//     public string StudentId { get; set; } = string.Empty;
//     public string CourseId { get; set; } = string.Empty;
//     public DateTime EnrollmentDate { get; set; }

// }

// // logging service that mutates the data
// class LoggingService
// {
//     public void LogEnrollment(Enrollment enrollment)
//     {
//         // accidentally mutating the data
//         // enrollment.CourseId = null;
//         Console.WriteLine($"Logging: {enrollment.CourseId}");
//     }

// }
// class Courses
// {
//     static void Main()
//     {
// var enrollment = new Enrollment
//     {
//         StudentId = "S12345",
//         CourseId = "C67890",
//         EnrollmentDate = DateTime.UtcNow
//     };
//     var logger = new LoggingService();
//     logger.LogEnrollment(enrollment);
//       Console.WriteLine($"After Logging: {enrollment.CourseId}"); // This will show the mutated value if the logging service mutates it.  
//     }
// }

// var enrollment1 = new EnrollmentRecord("S12345", "C67890", DateTime.UtcNow);
// // enrollment1.CourseId = "Hacked"; // This will cause a compile-time error because record types are immutable by default, and their properties cannot be modified after initialization.
// Console.WriteLine($"Enrollment Record: {enrollment1.StudentId}, CourseId: {enrollment1.CourseId}, EnrolledAt: {enrollment1.EnrolledAt}");

// // update the record with a new instance
// var correctedEnrollemnt = enrollment1 with { CourseId = "C#" }; // This creates a new instance of EnrollmentRecord with the updated CourseId, while keeping the other properties the same.
// Console.WriteLine($"Corrected Enrollment Record: {correctedEnrollemnt.StudentId}), CourseId: {correctedEnrollemnt.CourseId}, EnrolledAt: {correctedEnrollemnt.EnrolledAt}");

// // Value equality check
// var duplicate = new EnrollmentRecord("S12345", "C#", enrollment1.EnrolledAt);
// Console.WriteLine($"Are enrollment1 and duplicate equal? {enrollment1 == duplicate}");  // this will return bool values false because they are different instances, but they have the same values. This is one of the key features of record types: they provide value-based equality by default, meaning that two record instances are considered equal if their properties have the same values, regardless of whether they are the same instance in memory.



// Exercise 3 — Part 2: Course Capacity with the field Keyword 

    // The situation: The Course entity needs to be mutable — courses can change capacity and 
    // update titles throughout a semester. But the legacy code let anyone set Capacity = -5 with no 
    // complaint. In production, a negative capacity made the enrollment check if (course.EnrolledCount 
    // >= course.Capacity) pass immediately, blocking all students from a 30-seat course. 
    // Before C# 14, enforcing validation on a property required a private backing field and seven lines 
//     // of boilerplate for one simple check.
// public class Course
// {
//     private int _capacity;
//     public int Capacity
//     {
//         get => _capacity;
//         set
//         {
//             if (value < 0)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be negative.");
//             }
//             _capacity = value;
//         }
        
//     }

// }

// class Programs
// {
//     static void Main()
//     {
//         var course = new Course();
//         course.Capacity = 30; // This will work fine.
//         // course.Capacity = -5; // This will throw an ArgumentOutOfRangeException
//         Console.WriteLine($"Course Capacity: {course.Capacity}"); // This will show the current capacity of the course.
//     }
// }



var course1 = new Courses
{
    Code = "C101",
    Title = "Introduction to C#",
    Capacity = 30,
    EnrolledCount = 25
};
Console.WriteLine($"Course Code: {course1.Code}, Title: {course1.Title}, Capacity: {course1.Capacity}, Enrolled Count: {course1.EnrolledCount}");

// invalid capacity show throw an error
try
{
    course1.Capacity = -5;
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}


var s = new Student
{
    Name = "Bob",
    Age = 20,
    GPA = 3.5m
};
Console.WriteLine($"Student Name: {s.Name}, Age: {s.Age}, GPA: {s.GPA}");

// 
void PrintGradeReport (IEnumerable<IGradable>assesment)
{
    Console.WriteLine("--Grade Report--");
    foreach(var items in assesment)
    {
        Console.WriteLine($"{items.Title}: {items.CalculateGrade():F2}%");
    }
}

// test in one array 
IGradable[] chortAssesment = 
[
new Quiz {Title="C#",correctAnswer= 18, TotalQuetions=20},
new  LabAssigment {Title="API Integration",FunctionalitySore = 90m, CodeQuality=85m}
];
PrintGradeReport(chortAssesment);

// Exercise 4: Defeating the “Pyramid of Doom” (LO 1.6: Pattern
    // Matching & Guards)
    // The situation: The enrollment team asked for a simple validation: “Before registering a
    // student, check that the student exists, the course exists, and the course is not full.” The
    // previous developer wrote this
if (s != null)
{
    if (course1 != null)
    {
        if (course1.Capacity > 0);
    }
}

// guard clause approach
if (s is null)
{
    Console.WriteLine("Student does not exist.");
    return;
}
if (course1 is null)
{
    Console.WriteLine("Course does not exist.");
    return;
}
if (course1.Capacity <= 0)
{
    Console.WriteLine("Course is full.");
    return;
}