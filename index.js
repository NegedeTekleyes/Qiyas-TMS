import { Temporal } from "@js-temporal/polyfill";
const student = {
    id: "STU-001",
    name: "Hana Tadesse",
    enrollmentDate: Temporal.Now.instant(),
};
// Try these what does the compiler say?
// student.id = "STU-999";
// console.log(student.gpa.toFixed(2));
console.log(student.gpa?.toFixed(2) ?? "Not yet graded");
