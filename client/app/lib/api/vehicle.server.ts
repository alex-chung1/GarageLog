import { apiFetch } from './client.server'

export const VehiclesApi = {
    getAll(request: Request) {
        const cookie = request.headers.get('Cookie')

        return apiFetch('/vehicles', {
            method: 'GET',
            headers: { Cookie: cookie ?? '' },
        })
    },
}
