import { apiFetch } from './client.server';

export const ServiceTypesApi = {
  getAll(request: Request) {
    const cookie = request.headers.get('Cookie');

    return apiFetch('/ServiceType', {
      method: 'GET',
      headers: {
        Cookie: cookie ?? '',
      },
    });
  },
};
