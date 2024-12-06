export interface ResponseGetScore {
    subjectId: number;
    pupilId: number;
    score?: number;
    semesterId: number;
    status: number;
    subjectName?: string;
    pupilName?: string;
    image?: string;
    createdDate?: Date;
}

export interface ResponseClassScoreListDTO {
    subjects: [
        {
            id: number;
            name?: string
        }
    ];
    pupils: [
        {
            pupilId: number;
            pupilName?: string;
            image?: string;
            subjectScores: [
                {
                    subjectId: number;
                    subjectName?: string;
                    scores?: (number | null)[];
                },
            ];
        },
    ];
}

export interface ErrorScore {
    subjectId? : string[];
    pupilId? : string[];
    score? : string[];
    semesterId? : string[];
    status? : string[];
}

export interface IndividualScore {
    SubjectName?: string;
    scores?: (number | null)[];
}
