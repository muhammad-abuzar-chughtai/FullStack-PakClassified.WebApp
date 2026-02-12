import { UserGet } from "../user/user-model";

export interface AuthResponse {
    token: string;
    payload: UserGet;
}