import type { VehicleResponse } from '~/types/vehicle';

import { Form, Link, useNavigation } from 'react-router';

import FormSection from '~/components/FormSection';

export default function VehicleForm({
  vehicle,
  error,
}: {
  vehicle?: VehicleResponse;
  error?: string;
}) {
  const navigation = useNavigation();
  const isSubmitting = navigation.state === 'submitting';
  const isEditing = !!vehicle;

  const cancelTo = isEditing ? `/garage/vehicle/${vehicle.id}` : '/garage';

  return (
    <div>
      <Link
        to={cancelTo}
        className="mb-2 inline-flex items-center gap-1 text-sm text-muted hover:text-text"
      >
        ← Back to {isEditing ? 'Vehicle Details' : 'Garage'}
      </Link>

      <div className="mb-6">
        <h1 className="text-3xl font-bold text-primary">
          {isEditing ? 'Edit Vehicle' : 'Add Vehicle'}
        </h1>

        <p className="mt-1 text-muted">
          {isEditing ? 'Update your vehicle details.' : 'Add a vehicle to your garage.'}
        </p>
      </div>

      {error && (
        <div className="mx-auto mb-6 max-w-3xl rounded-lg border border-red-500/30 bg-red-500/10 p-4 text-sm text-red-500">
          {error}
        </div>
      )}

      <Form method="post" className="mx-auto max-w-3xl space-y-6">
        <FormSection title="Vehicle Information">
          <div className="space-y-4">
            <div>
              <label htmlFor="type" className="mb-1 block text-sm font-medium text-text">
                Vehicle Type
              </label>

              <select
                id="type"
                name="type"
                defaultValue={vehicle?.type ?? 1}
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              >
                <option value="1">Car</option>
                <option value="2">Truck</option>
                <option value="3">SUV</option>
              </select>
            </div>

            <div>
              <label htmlFor="make" className="mb-1 block text-sm font-medium text-text">
                Make
              </label>

              <input
                id="make"
                name="make"
                required
                defaultValue={vehicle?.make}
                placeholder="Toyota"
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            </div>

            <div>
              <label htmlFor="model" className="mb-1 block text-sm font-medium text-text">
                Model
              </label>

              <input
                id="model"
                name="model"
                required
                defaultValue={vehicle?.model}
                placeholder="Corolla"
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            </div>

            <div>
              <label htmlFor="year" className="mb-1 block text-sm font-medium text-text">
                Year
              </label>

              <input
                id="year"
                name="year"
                type="number"
                required
                min="1886"
                max={new Date().getFullYear() + 1}
                defaultValue={vehicle?.year}
                placeholder="2018"
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            </div>

            <div>
              <label htmlFor="vin" className="mb-1 block text-sm font-medium text-text">
                VIN
              </label>

              <input
                id="vin"
                name="vin"
                maxLength={17}
                defaultValue={vehicle?.vin ?? ''}
                onChange={(e) => {
                  e.currentTarget.value = e.currentTarget.value.toUpperCase();
                }}
                placeholder="Optional"
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            </div>
          </div>
        </FormSection>

        <div className="flex justify-end gap-3">
          <Link
            to={cancelTo}
            aria-disabled={isSubmitting}
            className={`rounded-lg border border-border px-5 py-2 font-medium text-text ${
              isSubmitting ? 'pointer-events-none cursor-not-allowed opacity-50' : ''
            }`}
          >
            Cancel
          </Link>

          <button
            type="submit"
            disabled={isSubmitting}
            className="rounded-lg bg-primary px-5 py-2 font-medium text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {isSubmitting ? 'Saving...' : isEditing ? 'Save Changes' : 'Save Vehicle'}
          </button>
        </div>
      </Form>
    </div>
  );
}
