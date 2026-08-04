import type { LoginRequest, RegisterRequest } from "~/types/auth";

import { apiFetch } from "./client.server";

export const AuthApi = {
    login(data: LoginRequest) {
        return apiFetch("/auth/login", {
            method: "POST",
            body: JSON.stringify(data),
        });
    },

    register(data: RegisterRequest) {
        return apiFetch("/auth/register", {
            method: "POST",
            body: JSON.stringify(data),
        });
    },

    async getCurrentUser(request: Request) {
        const cookie = request.headers.get("Cookie");

        try {
            const response = await apiFetch("/auth/me", {
                headers: {
                    Cookie: cookie ?? "",
                },
            });

            if (!response.ok) {
                return null;
            }

            return response.json();
        } catch (error) {
            console.error("getCurrentUser failed:", error);
            return null;
        }
    },
};
