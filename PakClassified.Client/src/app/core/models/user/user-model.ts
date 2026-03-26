export interface UserGet {
  id: number;
  name: string;
  email: string;
  profilePic: string;
  contactNo: number;
  dob: Date;
  secQues?: string;
  secAns?: string;
  createdBy: string;
  lastmodifiedBy?: string;
  roleId: number;
  roleName?: string;
}

export interface UserPost {
  id: number;
  name: string;
  email: string;
  password: string;
  profilePic: File;
  contactNo: number;
  dob: Date;
  secQues?: string;
  secAns?: string;
  createdBy: string;
  lastmodifiedBy?: string;
  roleId: number;
}