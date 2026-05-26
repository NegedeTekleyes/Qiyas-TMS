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
Console.WriteLine($"Student Name: {StudentName}, Student ID: {StudentId}, Enrollment Count: {enrollmentCount}, Course Fee: {courseFee}, GPA: {GPA}");