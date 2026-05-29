// // Exercise 1: The First Safety Net (LO 1.1: Environment + Null Safety) 
// // ********************************************

// // The situation: The legacy TMS stored student region data as a plain string with no null checks. 
// // When the batch enrollment confirmation system tried to format mailing addresses at 2 AM, it 
// // called .ToUpper() on a region that was never assigned. The app crashed with 
// // NullReferenceException. Three hundred students received no confirmation email. Nobody noticed 
// // until Monday morning. 
// // The fix is not to add null checks everywhere after the fact. The fix is to make the compiler tell 
// // you about the problem before the code ever runs.
// // step 1
// // string Region = null;
// // Console.WriteLine(Region.ToUpper()); // NullReferenceException at runtime
// // the above code contain warring warning CS8600: Converting null literal or possible null value to non-nullable type.

// // step 2 How to fix it?
// // 1. Make the variable nullable
// string? Region = null;
// Console.WriteLine(Region?.ToUpper()); // No exception, but no output either. The null-conditional operator (?.) prevents the method call if Region is null.


// // step 3 declare the first TMS variable
// string StudentName = "Alice";
// string StudentId = "S12345";
// int enrollmentCount = 3;
// decimal courseFee = 199.99m;
// decimal GPA = 3.75m;
// DateTime enrollmentDate = DateTime.UtcNow;
// Console.WriteLine($"Student Name: {StudentName}, Student ID: {StudentId}, Enrollment Count: {enrollmentCount}, Course Fee: {courseFee}, GPA: {GPA}");


// // Exercise 2: The Ministry Audit Failure (LO 1.2: Primitives)

// // The situation: The Ministry of Education relies on the TMS to allocate and report on training 
// // grants. During a quarterly audit, an automated script flagged a discrepancy of 0.03 Birr — a 
// // phantom balance distributed across 100,000 student accounts. The lead architect traced it to 
// // the invoicing module’s grant calculation. The legacy code used double for money. That single 
// // type choice created a financial error that survived three years of testing

// // The fix is to use the right type for money: decimal. The decimal type has a higher precision and is designed for financial calculations, which eliminates the rounding errors associated with double.
// // step 1: show the problem with double
// //  Legacy implementation — the bug that caused the audit failure 
// double grantPerStudent = 1999.99;
// double totalAllocation = grantPerStudent * 100_000; // This may show a value like 970.0000000000001 due to precision issues.
// Console.WriteLine($"Final amount with double: {totalAllocation}");
// // step 2: show the fix with decimal
// decimal grantPerStudentDecimal = 1999.99m;
// decimal discountDecimal = 0.03m;
// decimal finalAmountDecimal = grantPerStudentDecimal - (grantPerStudentDecimal * discountDecimal);
// Console.WriteLine($"Final Amount with decimal: {finalAmountDecimal}"); // This will show the correct value without precision issues.
//                                                                        // The double type is a base-2 (binary) floating-point primitive. It cannot accurately represent 
//                                                                        // base-10 fractional numbers like 0.99.




// // Exercise 3: Pipeline Data Corruption (LO 1.3 & 1.4: Encapsulation) 

// // The situation: In the TMS, once an Enrollment is processed, it passes through multiple 
// // middleware services logging, telemetry, notification. The database revealed that 12 students 
// // were mysteriously enrolled in NULL. The investigation found that a poorly written logging service 
// // was accidentally mutating the data during processing. Because the DTO used public set 
// // properties, the compiler could not prevent the mutation

// // public class Enrollment
// // {
// //     public string StudentId { get; set; } = string.Empty;
// //     public string CourseId { get; set; } = string.Empty;
// //     public DateTime EnrollmentDate { get; set; }

// // }

// // // logging service that mutates the data
// // class LoggingService
// // {
// //     public void LogEnrollment(Enrollment enrollment)
// //     {
// //         // accidentally mutating the data
// //         // enrollment.CourseId = null;
// //         Console.WriteLine($"Logging: {enrollment.CourseId}");
// //     }

// // }
// // class Courses
// // {
// //     static void Main()
// //     {
// // var enrollment = new Enrollment
// //     {
// //         StudentId = "S12345",
// //         CourseId = "C67890",
// //         EnrollmentDate = DateTime.UtcNow
// //     };
// //     var logger = new LoggingService();
// //     logger.LogEnrollment(enrollment);
// //       Console.WriteLine($"After Logging: {enrollment.CourseId}"); // This will show the mutated value if the logging service mutates it.  
// //     }
// // }

// // var enrollment1 = new EnrollmentRecord("S12345", "C67890", DateTime.UtcNow);
// // // enrollment1.CourseId = "Hacked"; // This will cause a compile-time error because record types are immutable by default, and their properties cannot be modified after initialization.
// // Console.WriteLine($"Enrollment Record: {enrollment1.StudentId}, CourseId: {enrollment1.CourseId}, EnrolledAt: {enrollment1.EnrolledAt}");

// // // update the record with a new instance
// // var correctedEnrollemnt = enrollment1 with { CourseId = "C#" }; // This creates a new instance of EnrollmentRecord with the updated CourseId, while keeping the other properties the same.
// // Console.WriteLine($"Corrected Enrollment Record: {correctedEnrollemnt.StudentId}), CourseId: {correctedEnrollemnt.CourseId}, EnrolledAt: {correctedEnrollemnt.EnrolledAt}");

// // // Value equality check
// // var duplicate = new EnrollmentRecord("S12345", "C#", enrollment1.EnrolledAt);
// // Console.WriteLine($"Are enrollment1 and duplicate equal? {enrollment1 == duplicate}");  // this will return bool values false because they are different instances, but they have the same values. This is one of the key features of record types: they provide value-based equality by default, meaning that two record instances are considered equal if their properties have the same values, regardless of whether they are the same instance in memory.



// // Exercise 3 — Part 2: Course Capacity with the field Keyword 

// // The situation: The Course entity needs to be mutable — courses can change capacity and 
// // update titles throughout a semester. But the legacy code let anyone set Capacity = -5 with no 
// // complaint. In production, a negative capacity made the enrollment check if (course.EnrolledCount 
// // >= course.Capacity) pass immediately, blocking all students from a 30-seat course. 

// // Before C# 14, enforcing validation on a property required a private backing field and seven lines 
// //     // of boilerplate for one simple check.
// // public class Course
// // {
// //     private int _capacity;
// //     public int Capacity
// //     {
// //         get => _capacity;
// //         set
// //         {
// //             if (value < 0)
// //             {
// //                 throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be negative.");
// //             }
// //             _capacity = value;
// //         }

// //     }

// // }

// // class Programs
// // {
// //     static void Main()
// //     {
// //         var course = new Course();
// //         course.Capacity = 30; // This will work fine.
// //         // course.Capacity = -5; // This will throw an ArgumentOutOfRangeException
// //         Console.WriteLine($"Course Capacity: {course.Capacity}"); // This will show the current capacity of the course.
// //     }
// // }



// var course1 = new Courses
// {
//     Code = "C101",
//     Title = "Introduction to C#",
//     Capacity = 30,
//     EnrolledCount = 25
// };
// Console.WriteLine($"Course Code: {course1.Code}, Title: {course1.Title}, Capacity: {course1.Capacity}, Enrolled Count: {course1.EnrolledCount}");

// // invalid capacity show throw an error
// try
// {
//     course1.Capacity = -5;
// }
// catch (ArgumentOutOfRangeException ex)
// {
//     Console.WriteLine($"Error: {ex.Message}");
// }


// var s = new Student
// {
//     Name = "Bob",
//     Age = 20,
//     GPA = 3.5m
// };
// Console.WriteLine($"Student Name: {s.Name}, Age: {s.Age}, GPA: {s.GPA}");

// // 
// void PrintGradeReport(IEnumerable<IGradable> assesment)
// {
//     Console.WriteLine("--Grade Report--");
//     foreach (var items in assesment)
//     {
//         Console.WriteLine($"{items.Title}: {items.CalculateGrade():F2}%");
//     }
// }

// // test in one array 
// IGradable[] chortAssesment =
// [
// new Quiz {Title="C#",correctAnswer= 18, TotalQuetions=20},
// new  LabAssigment {Title="API Integration",FunctionalitySore = 90m, CodeQuality=85m}
// ];
// PrintGradeReport(chortAssesment);

// // Exercise 4: Defeating the “Pyramid of Doom” (LO 1.6: Pattern
// // Matching & Guards)
// // The situation: The enrollment team asked for a simple validation: “Before registering a
// // student, check that the student exists, the course exists, and the course is not full.” The
// // previous developer wrote this
// if (s != null)
// {
//     if (course1 != null)
//     {
//         if (course1.Capacity > 0) ;
//     }
// }

// // guard clause approach 
// if (s is null)
// {
//     Console.WriteLine("Student does not exist.");
//     return;
// }
// if (course1 is null)
// {
//     Console.WriteLine("Course does not exist.");
//     return;
// }
// if (course1.Capacity <= 0)
// {
//     Console.WriteLine("Course is full.");
//     return;
// }


// var service = new EnrollmentService();
// // valid registration
// var validateStudent = new Student
// {
//     Id = "S1",
//     Name = "Charlie",
//     Age = 17,
//     GPA = 3.0m
// };
// var validateCourse = new Courses
// {
//     Code = "C102",
//     Title = "Intermediate C#",
//     Capacity = 25,
//     EnrolledCount = 20
// };
// var result = service.ProcessRegistration(validateStudent, validateCourse);
// Console.WriteLine($"Enrolled: {result.StudentId},in {result.CourseId}");

// // test 2 null student throws an error
// try
// {
//     service.ProcessRegistration(null, validateCourse);
// }
// catch (ArgumentException ex)
// {

//     Console.WriteLine($"Guard catch: {ex.ParamName}");
// }

// // test 3 full course throws an error
// var fullCourse = new Courses
// {
//     Code = "C103",
//     Title = "Full Course",
//     Capacity = 1, // This will trigger the full course validation
// };
// fullCourse.EnrolledCount = 1;
// try
// {
//     service.ProcessRegistration(validateStudent, fullCourse);
// }
// catch (InvalidOperationException ex)
// {
//     Console.WriteLine($"Business rule: {ex.Message}");
// }



// // Exercise 5: The Analytics Dashboard (LO 1.5: Collections & LINQ)
// // LINQ means: Language Integrated Query
// // SIMPLE DEFINITION
// // LINQ allows you to:
// // Query/filter/transform collections using readable syntax.
// // // REAL LIFE ANALOGY

// // Think of database queries.
// // Example:
// // SELECT Name
// // FROM Students
// // WHERE GPA >= 3.5
// // ORDER BY GPA DESC

// // LINQ brings similar thinking into C#.
// // The situation: The Head of Faculty needs a leaderboard report by end of day. She wants:
// // all Honors students sorted by GPA, the class average, and a breakdown of students by
// // academic standing. The data is in a list. You have LINQ.
// // From this exercise forward, the solution code is not fully provided for every step. You
// // will implement logic using the TODO comments as guidance the same way a senior
// // developer leaves code review comments for a junior. If you get stuck, each TODO has a
// // “Stuck?” hint. 
// // Step 1 Create the Student Data
// // In Program.cs, add:
// // C# 12+ Collection Expressions the modern way to initialize lists

// // List<Student> means “A dynamic collection that stores Student objects.”
// List<Student> students = [
// new Student { Id = "S1", Name= "Abeba", Age = 22, GPA= 3.8m },
// new Student { Id = "S2", Name= "Kidane", Age = 21, GPA = 2.4m},
// new Student { Id = "S3", Name= "Dawit", Age = 20, GPA= 3.1m },
// new Student { Id = "S4", Name= "Sara", Age = 23, GPA = 3.9m },
// new Student { Id = "S5", Name= "Frehiwot", Age = 19, GPA = 2.0m},
// new Student { Id = "S6", Name= "Yonas", Age = 24, GPA= 3.5m },
// new Student { Id = "S7", Name= "Meron", Age = 22, GPA =1.8m},
// new Student { Id = "S8", Name= "Tesfaye", Age = 21, GPA = 2.9m}
// ];

// // step 2 build the honor lead board
// var leadboard = students;
// // TODO 1 Extract students where GPA>=3.5m
// var honorsStudents = leadboard.Where(s =>s.GPA >= 3.5m);
// // TODO 2 Sort the honors students by GPA in descending order
// var sortedHonorsStudents = honorsStudents.OrderByDescending(s => s.GPA);
// // TODO 3 Print the sorted honors students with their names and GPAs
// Console.WriteLine("Honors Students:");
// foreach (var student in sortedHonorsStudents)
// {
//     Console.WriteLine($"{student.Name} - GPA: {student.GPA}");
// }
// // TODO 4 Materialize the lazy query into a concrete list using ToList()
// var honorsStudentList = sortedHonorsStudents.ToList();
// Console.WriteLine($"Total Honors Students: {honorsStudentList.Count}");

// // Step 3 Calculate the class average
// // TODO 5: Use LINQ to calculate the average GPA across all students
// var averageGPA = students.Average(s => s.GPA);
// Console.WriteLine($"Class Average GPA: {averageGPA:F2}");
// decimal averageGpa = students.Average(s => s.GPA);
// // Stuck? Pattern: students.Average(s => s.SomeProperty
// // Step 4 Group students by academic standing
// // TODO 6: Use LINQ to group students by academic standing based on their GPA
// var standingGroups = students.GroupBy(s =>s.GPA switch
// {
//     >= 3.5m => "Honors",
//     >= 2.5m => "Good Standing",
//     _ => "Academic Warning"
// });
// // Console.WriteLine("\n---Academic Standing Report---:");
// foreach (var group in standingGroups)
// {
//     Console.WriteLine($"\n{group.Key}: {group.Count()}");
//     foreach (var student in group)
//     {
//         // Console.WriteLine($"{student.Name} GPA:{student.GPA}");
//     }
// }

// // step 5 collection expressions with spread operator
// // TODO 7: Use the spread operator(...) to merge two arrays and append a value.
// string[] backendCourses = ["C#", "ASP.NET Core"];
// string[] frontendCourses = ["Typescript", "Angular"];
// string[] allCourses = [..backendCourses, ..frontendCourses, "React"];
// // Console.WriteLine("\nFull Curriculum:{string.Join(", ", allCourses)}");


// Exercise 6: Connection Dropping Under Load (LO 1.7: Async/Await)
// The situation: On the first day of registration, 200 students hit the enrollment endpoint
// simultaneously. The server froze. Not from CPU or memory pressure from thread
// starvation. The previous developer called .Result on every database query, blocking a
// thread pool thread for the entire duration of each 300ms database call. With 200
// concurrent requests and only ~20 available threads, every thread was locked waiting for
// I/O. New requests queued. Timeouts cascaded. The registrar rebooted the server twice
// before calling engineering.
// The fix is async/await. Instead of blocking a thread while waiting for the database, you
// release it back to the pool. When the database responds, the runtime picks up where
// you left off on any available thread. Same work, same result, but the thread pool never
// starves


// step 1 See threread starvation in numbers

using System.Diagnostics;

// Simulate 5 database calls each taking 300ms

// The wrong way - blocking with Thread.Sleep and .Result

var sw = Stopwatch.StartNew();for (int i =0; i<5; i++)
{
    Thread.Sleep(300); // Thread is held for 300ms, simulating a blocking database call
}
Console.WriteLine($"Blocking sequential:{sw.ElapsedMilliseconds}ms");

// ASYNC BUT STILL SEQUENTIAL: Thread realesed but calls one at a time
sw.Restart();
for(int i =0; i<5; i++)
{
    await Task.Delay(300); // Thread is released while waiting, but calls are still sequential
}
Console.WriteLine($"Async sequential: {sw.ElapsedMilliseconds}ms");

// The Right way : Aync parallel all 5 starts simuntaneously
sw.Restart();
var tasks = Enumerable.Range(0,5).Select(__ => Task.Delay(300));
await Task.WhenAll(tasks);
Console.WriteLine($"Async parallel: {sw.ElapsedMilliseconds}ms");