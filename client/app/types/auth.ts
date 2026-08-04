// app/types/auth.ts

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
}

export interface AuthResponse {
    token: string;
    userId: string;
    email: string;
    firstName: string;
    lastName: string;
}
