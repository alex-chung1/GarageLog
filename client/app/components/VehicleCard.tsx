import type { VehicleResponse } from '~/types/vehicle';

export default function VehicleCard({ vehicle }: { vehicle: VehicleResponse }) {
  return (
    <div className="rounded-xl border border-border bg-card p-6">
      <div className="mb-3 flex h-10 w-10 items-center justify-center rounded-lg bg-primary/20 text-primary">
        🚗
      </div>

      <h2 className="text-xl font-bold text-text">
        {vehicle.year} {vehicle.make} {vehicle.model}
      </h2>

      <div className="mt-4 space-y-2 text-sm text-muted">
        <p>
          <span className="font-medium text-text">Latest Mileage:</span>{' '}
          {vehicle.latestMileage
            ? `${vehicle.latestMileage.toLocaleString()} miles`
            : 'No mileage recorded'}
        </p>

        <p>{/* <span className="font-medium text-text">VIN:</span> {vehicle.vin} */}</p>
      </div>
    </div>
  );
}
