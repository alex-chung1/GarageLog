import type { ServiceType, SelectedService } from '~/types/serviceType';
import type { ServiceRecordResponse } from '~/types/serviceRecord';

import { Form, Link, useNavigation } from 'react-router';
import { useState } from 'react';

import ServiceSelector from '~/components/ServiceSelector';
import FormSection from '~/components/FormSection';

export default function ServiceRecordForm({
  vehicleId,
  serviceTypes,
  record,
  error,
}: {
  vehicleId: string;
  serviceTypes: ServiceType[];
  record?: ServiceRecordResponse;
  error?: string;
}) {
  const navigation = useNavigation();
  const isSubmitting = navigation.state === 'submitting';
  const isEditing = !!record;

  const [formItems, setFormItems] = useState<SelectedService[]>(
    record?.items.map((item) => ({
      serviceTypeId: item.serviceTypeId,
      name: item.serviceTypeName,
      customName: item.customName ?? undefined,
    })) ?? []
  );

  const [isDIY, setIsDIY] = useState(record?.isSelfService ?? true);
  const [totalCost, setTotalCost] = useState(record?.totalCost?.toFixed(2) ?? '');
  const [mileage, setMileage] = useState(record?.mileage?.toLocaleString() ?? '');

  return (
    <div>
      <div className="mb-6">
        <h1 className="text-3xl font-bold text-primary">
          {isEditing ? 'Edit Service Record' : 'Add Service Record'}
        </h1>

        <p className="mt-1 text-muted">Record maintenance performed on your vehicle.</p>
      </div>

      {error && (
        <div className="mx-auto mb-6 max-w-3xl rounded-lg border border-red-500/30 bg-red-500/10 p-4 text-sm text-red-500">
          {error}
        </div>
      )}

      <Form method="post" className="mx-auto max-w-3xl space-y-6">
        <input
          type="hidden"
          name="items"
          value={JSON.stringify(
            formItems.map((service) => ({
              serviceTypeId: service.serviceTypeId,
              customName: service.customName ?? null,
            }))
          )}
        />

        <FormSection title="Service Details">
          <div className="grid gap-4 md:grid-cols-2">
            <div>
              <label className="mb-1 block text-sm font-medium text-text">Service Date</label>
              <input
                name="serviceDate"
                type="date"
                defaultValue={record?.serviceDate ?? new Date().toISOString().split('T')[0]}
                max={new Date().toISOString().split('T')[0]}
                required
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium text-text">Mileage</label>
              <input
                name="mileage"
                type="text"
                inputMode="numeric"
                required
                value={mileage}
                onChange={(e) => {
                  const value = e.target.value.replace(/,/g, '');
                  if (/^\d*$/.test(value)) {
                    setMileage(value ? Number(value).toLocaleString() : '');
                  }
                }}
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            </div>
          </div>
        </FormSection>

        <FormSection title="Performed By">
          <div className="space-y-3">
            <label className="flex items-center gap-2 text-text">
              <input
                type="radio"
                name="provider"
                value="DIY"
                checked={isDIY}
                onChange={() => setIsDIY(true)}
              />
              DIY
            </label>

            <label className="flex items-center gap-2 text-text">
              <input
                type="radio"
                name="provider"
                value="Repair Shop"
                checked={!isDIY}
                onChange={() => setIsDIY(false)}
              />
              Repair Shop
            </label>

            {!isDIY && (
              <input
                name="shopName"
                defaultValue={record?.shopName ?? ''}
                placeholder="Shop Name"
                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
              />
            )}
          </div>
        </FormSection>

        <ServiceSelector
          serviceTypes={serviceTypes}
          initialServices={formItems}
          onChange={setFormItems}
        />

        <FormSection title="Cost">
          <input
            name="totalCost"
            type="text"
            inputMode="decimal"
            placeholder="0.00"
            value={totalCost}
            onChange={(e) => {
              const value = e.target.value;
              if (/^\d*\.?\d{0,2}$/.test(value)) {
                setTotalCost(value);
              }
            }}
            onBlur={() => {
              if (totalCost) {
                setTotalCost(Number(totalCost).toFixed(2));
              }
            }}
            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
          />
        </FormSection>

        <FormSection title="Notes">
          <textarea
            name="notes"
            rows={4}
            defaultValue={record?.notes ?? ''}
            placeholder="Optional notes..."
            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
          />
        </FormSection>

        <div className="flex justify-end gap-3">
          <Link
            to={`/garage/vehicle/${vehicleId}`}
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
            {isSubmitting ? 'Saving...' : isEditing ? 'Save Changes' : 'Save Record'}
          </button>
        </div>
      </Form>
    </div>
  );
}
