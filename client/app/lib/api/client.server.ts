const baseUrl = process.env.API_URL

export async function apiFetch(endpoint: string, options?: RequestInit) {
    return fetch(`${baseUrl}${endpoint}`, {
        ...options,
        signal: AbortSignal.timeout(5000),
        headers: {
            'Content-Type': 'application/json',
            ...options?.headers,
        },
    })
}
