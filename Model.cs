

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
