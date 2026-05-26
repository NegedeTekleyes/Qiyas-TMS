// public record EnrollmentRecord
// (
//     string StudentName,
//      string StudentId, 
//      DateTime ProcessedAt
// );

// public class Course 
// { 
//     public required string Code { get; init; }  
//     public required string Title 
//     { 
//         get; 
//         set => field = !string.IsNullOrWhiteSpace(value) 
//             ? value 
//             : throw new ArgumentException("Title cannot be empty or whitespace.", nameof(value)); 
//     }  
//     // old way to validate capacity with a private backing field
//     // C# 14 Auto-property validation using 'field' 
//     // public int Capacity 
//     // { 
//     //     get; 
//     //     set => field = value > 0 
//     //         ? value 
//     //         : throw new ArgumentOutOfRangeException(nameof(value), "System constraint: Capacity must be greater than zero."); 
//     // }  
//     // public int EnrolledCount { get; set; } 


// // modern way to validate capacity with a private backing field
//     public int Capacity
// {
//     get;
//     set
//     {
//         if (value <= 0)
       
//             throw new Exception("System constraint: Capacity must be greater than zero.");
//              field = value;
        
//     }
// }

// }



// public class Student
// {
//     public required string Id {get; init; }
//     public required string Name
//     {
//       get;
//       set => field = !string.IsNullOrWhiteSpace(value)
//         ? value
//         : throw new ArgumentException("Name cannot be empty or whitespace.", nameof(value));  
//     }
//     public int Age
//     {
//         get;
//         set => field = value is >=16 and <= 100
//         ? value
//         : throw new ArgumentOutOfRangeException(nameof(value), "Name must be between 16 and 100 characters.");
//     }

//     public decimal GPA
//     {
//         get;
//         set => field = value is >=0.0m and <= 4.0m
//         ? value
//         : throw new ArgumentOutOfRangeException(nameof(value), "GPA must be between 0.0 and 4.0.");
//     }
// }



// public interface IGradable
// {
//     string  Title {get;}
//     decimal calculateGrade();

//     public class Quiz : IGradable
//     {
//       public required string Title {get; init; }
//       public required int correctAnswer{get; init; }
//       public required int totalQuestions{get; init; }
//     }

//     // public decimal calcuLateGrade()
//     {
//         // if (totalQuestions == 0)return 0.0m;
//         // return (decimal)correctAnswer / totalQuestions * 100;
//     }

// }






public class Student
{
    public string Id{get;set;}
    public string Name{get;set;}
    public int Age{get;set;}
    public decimal GPA{get;set;}
}

public class Course
{
    public string Code{get;set;}
    public string Title{get;set;}
    public int Capacity{get;set;}
}

public class ExceptionCapacity : Exception
{
    public ExceptionCapacity(string message) : base(message)
    {
    }
}