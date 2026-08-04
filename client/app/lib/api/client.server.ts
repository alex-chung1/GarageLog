const baseUrl = process.env.API_URL

if (!baseUrl) {
    throw new Error('API_URL is not set')
}

const API_TIMEOUT_MS = 5000

export async function apiFetch(endpoint: string, options?: RequestInit): Promise<Response> {
    return fetch(`${baseUrl}${endpoint}`, {
        ...options,
        signal: AbortSignal.timeout(API_TIMEOUT_MS),
        headers: {
            'Content-Type': 'application/json',
            ...options?.headers,
        },
    })
}
