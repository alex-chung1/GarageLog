// vehicle.server.ts
import { apiFetch } from './client.server';

export const VehiclesApi = {
  getAll(request: Request) {
    const cookie = request.headers.get('Cookie');
    return apiFetch('/vehicles', {
      method: 'GET',
      headers: { Cookie: cookie ?? '' },
    });
  },

  getById(request: Request, id: number) {
    const cookie = request.headers.get('Cookie');
    return apiFetch(`/vehicles/${id}`, {
      method: 'GET',
      headers: { Cookie: cookie ?? '' },
    });
  },

  create(request: Request, data: unknown) {
    const cookie = request.headers.get('Cookie');
    return apiFetch('/vehicles', {
      method: 'POST',
      headers: {
        Cookie: cookie ?? '',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
  },

  update(request: Request, id: number, data: unknown) {
    const cookie = request.headers.get('Cookie');
    return apiFetch(`/vehicles/${id}`, {
      method: 'PUT',
      headers: {
        Cookie: cookie ?? '',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
  },

  delete(request: Request, id: number) {
    const cookie = request.headers.get('Cookie');
    return apiFetch(`/vehicles/${id}`, {
      method: 'DELETE',
      headers: { Cookie: cookie ?? '' },
    });
  },
};
