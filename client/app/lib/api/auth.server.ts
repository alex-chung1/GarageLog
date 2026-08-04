import type { LoginRequest, RegisterRequest } from '~/types/auth'

import { apiFetch } from './client.server'

export const AuthApi = {
    login(data: LoginRequest): Promise<Response> {
        return apiFetch('/auth/login', {
            method: 'POST',
            body: JSON.stringify(data),
        })
    },

    register(data: RegisterRequest): Promise<Response> {
        return apiFetch('/auth/register', {
            method: 'POST',
            body: JSON.stringify(data),
        })
    },

    getCurrentUser(request: Request): Promise<Response> {
        const cookie = request.headers.get('Cookie')

        return apiFetch('/auth/me', {
            headers: { Cookie: cookie ?? '' },
        })
    },
}
