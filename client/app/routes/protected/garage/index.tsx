import type { Route } from './+types/index';
import type { VehicleResponse } from '~/types/vehicle';

import { Link, useLoaderData } from 'react-router';
import { VehiclesApi } from '~/lib/api/vehicle.server';
import { getErrorMessage } from '~/lib/errors';

import VehicleCard from '~/components/VehicleCard';

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
      <div className="mb-6">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
          <div>
            <h1 className="text-2xl font-bold text-primary sm:text-3xl">My Garage</h1>

            <p className="mt-1 text-sm text-muted sm:text-base">Manage your vehicles</p>
          </div>

          <Link
            to="/garage/vehicle/new"
            className="inline-flex w-full items-center justify-center rounded-lg bg-primary px-4 py-2.5 text-sm font-medium text-white transition hover:opacity-90 sm:w-auto sm:px-4 sm:py-2"
          >
            + Add Vehicle
          </Link>
        </div>
      </div>

      {/* Error */}
      {error && <div className="mb-6 rounded-lg bg-red-950 p-3 text-sm text-red-300">{error}</div>}

      {/* Vehicle Cards */}
      {vehicles.length === 0 ? (
        <div className="rounded-xl border border-border bg-card p-6 text-muted">
          No vehicles found.
        </div>
      ) : (
        <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {vehicles.map((vehicle) => (
            <Link
              key={vehicle.id}
              to={`/garage/vehicle/${vehicle.id}`}
              className="block transition duration-200 hover:-translate-y-1"
            >
              <VehicleCard vehicle={vehicle} />
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}
