export interface SignUp {
    id: number;
    name: string;
    email: string;
    password: string;
    profilePic: File;
    contactNo: number;
    dob: Date;
    secQuestion: string;
    secAnswer: string;
    createdBy: string;
    lastmodifiedBy: string;
}
