import { Temporal } from "@js-temporal/polyfill";
import type { Student } from "./models/student.model.js";
import  { isStudent } from "./models/student.model.js";
import { parseStudent } from "./models/student.model.js";
import type {AssesmentItem} from "./models/assesment.model.js";
import {calculateGrade} from "./models/assesment.model.js";
const student: Student = {
  id: "STU-001",
  name: "Hana Tadesse",
  enrollmentDate: Temporal.Now.instant()
};

console.log(student.gpa?.toFixed(2) ?? "Not yet graded");

// function processStudent(data: any) {
//     console.log(`GPA: ${data.gpa?.toFixed(2)}`);
// }
function processStudent(raw: unknown) {
    if (isStudent(raw)) {
        const gpaDisplay = raw.gpa?.toFixed(2) ?? "Not yet graded";

    console.log(`Student: ${raw.name}, GPA: ${gpaDisplay}`);
} else {
    console.error("Invalid student data");
}

}
processStudent({
    id: "STU-002",
    name: "Yonas Alemu",
    // enrollmentDate: Temporal.Now.instant(),
    gpa: 3.8
});
// processStudent(42);  // prints Invalid student data

console.log(parseStudent({id:"STU-003",name:"Negede",}))

// console.log(parseStudent({id:42, name: "Negede"}))

const quiz: AssesmentItem = {
    id: "QUIZ-001",
    kind: "quiz",
    title: "SQL Basics",
    correctAnswer: 8,
    totalQuestions: 10
}

const lab: AssesmentItem = {
    id: "LAB-001",
    kind: "lab",
    title: "REST API Project",
    functionalityScore: 85,
    codeQualityScore: 90
}
console.log(`Quiz Grade: ${calculateGrade(quiz)}%`);
console.log(`Lab Grade: ${calculateGrade(lab)}%`);
// quiz.id = "QUIZ-002"; // Error: Cannot assign to 'id' because it is a read-only property.