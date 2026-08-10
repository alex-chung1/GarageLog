import type { Route } from './+types/index';
import type { VehicleResponse } from '~/types/vehicle';

import { Link, useLoaderData } from 'react-router';
import { VehiclesApi } from '~/lib/api/vehicle.server';
import { getErrorMessage } from '~/lib/errors';

import VehicleCard from '~/components/VehicleCard';

export function meta({}: Route.MetaArgs) {
  return [{ title: 'GarageLog' }, { name: 'description', content: 'Manage your vehicles' }];
}

export async function loader({ request }: Route.LoaderArgs) {
  try {
    const response = await VehiclesApi.getAll(request);

    if (!response.ok) {
      throw new Error('Failed to load vehicles');
    }

    const vehicles: VehicleResponse[] = await response.json();

    return { vehicles };
  } catch (error) {
    return {
      vehicles: [],
      error: getErrorMessage(error),
    };
  }
}

export default function Garage() {
  const { vehicles, error } = useLoaderData<typeof loader>();

  return (
    <div>
      {/* Header */}
      <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-3xl font-bold text-primary">My Garage</h1>

          <p className="mt-1 text-muted">Manage your vehicles</p>
        </div>

        <Link
          to="/garage/vehicle/new"
          className=" self-start rounded-lg bg-primary px-4 py-2 text-white font-medium hover:opacity-90"
        >
          + Add Vehicle
        </Link>
      </div>

      {/* Error */}
      {error && <div className="mb-6 rounded-lg bg-red-950 p-3 text-sm text-red-300">{error}</div>}

      {/* VehicleCard */}
      {vehicles.length === 0 ? (
        <div className="rounded-xl border border-border bg-card p-6 text-muted shadow-sm">
          No vehicles found.
        </div>
      ) : (
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {vehicles.map((vehicle) => (
            <Link
              key={vehicle.id}
              to={`/garage/vehicle/${vehicle.id}`}
              className="block transition hover:-translate-y-1"
            >
              <VehicleCard vehicle={vehicle} />
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
