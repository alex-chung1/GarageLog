import { apiFetch } from "./client.server";

export const ServiceRecordsApi = {
    getAll(request: Request, vehicleId: number) {
        const cookie = request.headers.get("Cookie");

        return apiFetch(`/vehicles/${vehicleId}/service-records`, {
            method: "GET",
            headers: { Cookie: cookie ?? "" },
        });
    },
};
