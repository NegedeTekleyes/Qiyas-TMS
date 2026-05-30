

export interface Quiz {
    readonly id: string;
    kind: "quiz";
    title: string;
    correctAnswer: number;
    totalQuestions: number;
}

export interface LabAssignment {
    readonly id: string;
    kind: "lab";
    title: string;
    functionalityScore: number;
    codeQualityScore: number;
}

export type AssesmentItem = Quiz | LabAssignment;

// Step 2 Write the Grade Calculator

export function calculateGrade(item: AssesmentItem): number {
    switch(item.kind){
        case "quiz":
            return Math.round((item.correctAnswer / item.totalQuestions) * 100);
            case "lab":
                return Math.round((item.functionalityScore * 0.7) + (item.codeQualityScore * 0.3));
    }
}