import type { Route } from './+types/vehicle-detail';
import type { VehicleResponse } from '~/types/vehicle';
import type { ServiceRecordResponse } from '~/types/serviceRecord';

import { Link, useLoaderData } from 'react-router';
import { VehiclesApi } from '~/lib/api/vehicle.server';
import { ServiceRecordsApi } from '~/lib/api/service-record.server';

import VehicleCard from '~/components/VehicleCard';
import ServiceRecordCard from '~/components/ServiceRecordCard';

export async function loader({ request, params }: Route.LoaderArgs) {
  const vehicleId = params.vehicleId;

  if (!vehicleId) {
    throw new Response('Not Found', { status: 404 });
  }

  const id = Number(vehicleId);

  const [vehicleResponse, recordsResponse] = await Promise.all([
    VehiclesApi.getById(request, id),
    ServiceRecordsApi.getAll(request, id),
  ]);

  if (!vehicleResponse.ok) {
    throw new Response('Failed to load vehicle', {
      status: vehicleResponse.status,
    });
  }

  if (!recordsResponse.ok) {
    throw new Response('Failed to load service records', {
      status: recordsResponse.status,
    });
  }

  const vehicle: VehicleResponse = await vehicleResponse.json();
  const records: ServiceRecordResponse[] = await recordsResponse.json();

  return {
    vehicle,
    records,
  };
}

export default function VehicleDetail() {
  const { vehicle, records } = useLoaderData<typeof loader>();

  return (
    <div>
      {/* Header */}
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-primary">Service History</h1>

        <p className="mt-1 text-muted">Track maintenance and repairs for this vehicle</p>
      </div>

      <div className="grid gap-6 lg:grid-cols-[320px_1fr]">
        {/* Vehicle */}
        <aside>
          <VehicleCard vehicle={vehicle} />

          <div className="mt-4 flex gap-3">
            <Link
              to={`/garage/vehicle/${vehicle.id}/edit`}
              className="flex flex-1 items-center justify-center rounded-lg border border-border px-4 py-2 text-center font-medium text-text transition hover:bg-background"
            >
              Edit Vehicle
            </Link>

            <Link
              to={`/garage/vehicle/${vehicle.id}/service-record/new`}
              className="flex-1 rounded-lg bg-primary px-4 py-2 text-center font-medium text-white transition hover:opacity-90"
            >
              + Add Service Record
            </Link>
          </div>
        </aside>

        {/* Service Records */}
        <section className="space-y-6">
          {records.length === 0 ? (
            <div className="rounded-xl border border-border bg-card p-6 text-muted shadow-sm">
              No service records found.
            </div>
          ) : (
            records.map((record) => (
              <ServiceRecordCard record={record} vehicleId={vehicle.id} key={record.id} />
            ))
          )}
        </section>
      </div>
    </div>
  );
}
