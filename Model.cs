

public record EnrollmentRecord
(
     string StudentId, 
     string CourseId,
     DateTime EnrolledAt

);
// C#14 introduce a feild keywords
public class Courses
{
    public required string Code { get; init; }
    public required string Title {
        get; 
        set => field = !string.IsNullOrWhiteSpace(value)
        ? value
        : throw new ArgumentNullException(nameof(value), "Title cannot be null or empty.");

        }
        
public int Capacity
    {
        get;
        set => field = value >=0 
        ? value
        : throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be negative.");
    }
public int EnrolledCount { get; set; }

        }


public class Student
{
    public string Id { get; init; }
    public string Name 
    { 
        get; 
        set => field = !string.IsNullOrEmpty(value)
        ? value
        : throw new ArgumentNullException(nameof(value), "Name cannot be null or empty."); 
    }

public int Age
{
    get;
    set => field = value is >=16 and <= 100
        ? value
        : throw new ArgumentOutOfRangeException(nameof(value), "Age must be between 16 and 100.");

}

public decimal GPA
    {
        get;
        set => field = value is >=0m and <= 4.0m
        ? value
        : throw new ArgumentOutOfRangeException(nameof(value), "GPA must be between 0.0 and 4.0.");
    }
}

// Exercise 3B: Interface Contract Wiring (LO 1.4: OOP Contracts) 

    // The situation: The analytics team needs to generate grade reports for the end-of-semester 
    // review. The problem: the TMS has two completely different assessment types quizzes (graded 
    // by correct answers out of total) and lab assignments (graded by a weighted formula of 
    // functionality and code quality). The reporting pipeline needs to process both through the same 
    // method without knowing which type it is dealing with. 
    // This is the problem interfaces solve. An interface is a contract it says “any type that implements 
    // me guarantees it can do these things.” The reporting method accepts IGradable and calls 
    // CalculateGrade(). It does not care whether the object is a quiz, a lab, or something that does not 
    // exist yet. 
    // Step 1 — Define the Contract

    public interface IGradable
{
    string Title{get;}
    decimal CalculateGrade();
}

    // step 2 implemet it in two assesment values
    public class Quiz: IGradable
{
    public required string Title{get; init;}
    public required int correctAnswer{get; init;}
    public required int TotalQuetions{get; init;}

    public decimal CalculateGrade()
    {
        if (TotalQuetions == 0) return 0m;
        return(decimal)correctAnswer/TotalQuetions * 100m;
    }
}

public class LabAssigment: IGradable
{
    public required string Title{get; init;}
    public required decimal FunctionalitySore{get;init;}
    public required decimal CodeQuality{get;init;}

    public decimal CalculateGrade()
    {
        // 70% functionality 30% code quality
        return (FunctionalitySore *0.7m )+ ( CodeQuality*0.3m);
    }
}
// Step 3 — Write the Polymorphic Report 
// Add this to Program.cs
