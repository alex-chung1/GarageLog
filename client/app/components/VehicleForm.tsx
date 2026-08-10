import type { VehicleResponse } from '~/types/vehicle';

import { Form, useNavigation } from 'react-router';

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

  return (
    <div className="mx-auto max-w-xl">
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-primary">
          {isEditing ? 'Edit Vehicle' : 'Add Vehicle'}
        </h1>

        <p className="mt-1 text-muted">
          {isEditing ? 'Update your vehicle details' : 'Add a vehicle to your garage'}
        </p>
      </div>

      {error && <div className="mb-4 rounded-lg bg-red-500/10 p-4 text-red-500">{error}</div>}

      <Form method="post" className="space-y-5">
        <FormSection title="Vehicle Information">
          <div className="space-y-4">
            <div>
              <label htmlFor="type" className="text-sm font-medium text-text">
                Vehicle Type
              </label>
              <select
                id="type"
                name="type"
                defaultValue={vehicle?.type ?? 1}
                className="mt-1 w-full rounded-lg border border-border bg-background p-2"
              >
                <option value="1">Car</option>
                <option value="2">Truck</option>
                <option value="3">SUV</option>
              </select>
            </div>

            <div>
              <label htmlFor="make" className="text-sm font-medium text-text">
                Make
              </label>
              <input
                id="make"
                name="make"
                required
                defaultValue={vehicle?.make}
                placeholder="Toyota"
                className="mt-1 w-full rounded-lg border border-border bg-background p-2"
              />
            </div>

            <div>
              <label htmlFor="model" className="text-sm font-medium text-text">
                Model
              </label>
              <input
                id="model"
                name="model"
                required
                defaultValue={vehicle?.model}
                placeholder="Corolla"
                className="mt-1 w-full rounded-lg border border-border bg-background p-2"
              />
            </div>

            <div>
              <label htmlFor="year" className="text-sm font-medium text-text">
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
                className="mt-1 w-full rounded-lg border border-border bg-background p-2"
              />
            </div>

            <div>
              <label htmlFor="vin" className="text-sm font-medium text-text">
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
                className="mt-1 w-full rounded-lg border border-border bg-background p-2"
              />
            </div>
          </div>
        </FormSection>

        <button
          type="submit"
          disabled={isSubmitting}
          className="w-full rounded-lg bg-primary px-4 py-2 font-medium text-white transition hover:opacity-90 disabled:cursor-not-allowed disabled:opacity-50"
        >
          {isSubmitting ? 'Saving...' : isEditing ? 'Save Changes' : 'Add Vehicle'}
        </button>
      </Form>
    </div>
  );
}
