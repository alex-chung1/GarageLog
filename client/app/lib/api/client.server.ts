const baseUrl = process.env.API_URL;

if (!baseUrl) {
  throw new Error('API_URL is not set');
}

const API_TIMEOUT_MS = 15000;

export async function apiFetch(endpoint: string, options?: RequestInit): Promise<Response> {
  const start = Date.now();

  try {
    return await fetch(`${baseUrl}${endpoint}`, {
      ...options,
      signal: AbortSignal.timeout(API_TIMEOUT_MS),
      headers: {
        'Content-Type': 'application/json',
        ...options?.headers,
      },
    });
  } catch (error) {
    const elapsed = Date.now() - start;

    console.error(`API request failed: ${endpoint} after ${elapsed}ms`, error);

    if (error instanceof Error && error.name === 'TimeoutError') {
      throw new Error('The server took too long to respond. Please try again.');
    }

    throw error;
  }
}
