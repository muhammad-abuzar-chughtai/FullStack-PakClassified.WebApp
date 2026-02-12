export interface UserGet {
  id: number;
  name: string;
  email: string;
  profilePic: string;
  contactNo: number;
  dob: Date;
  secQuestion: string;
  secAnswer: string;
  createdBy: string;
  lastmodifiedBy: string;
  roleId: number;
}

export interface UserPost {
  id: number;
  name: string;
  email: string;
  profilePic: File;
  contactNo: number;
  dob: Date;
  secQuestion: string;
  secAnswer: string;
  createdBy: string;
  lastmodifiedBy: string;
  roleId: number;
}