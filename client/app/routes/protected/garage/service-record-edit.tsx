import type { Route } from './+types/service-record-edit';
import type { ServiceType } from '~/types/serviceType';
import type { ServiceRecordResponse } from '~/types/serviceRecord';

import { redirect, useLoaderData, useParams, useActionData, Form } from 'react-router';
import { useState } from 'react';

import { ServiceTypesApi } from '~/lib/api/service-type.server';
import { ServiceRecordsApi } from '~/lib/api/service-record.server';

import ServiceRecordForm from '~/components/ServiceRecordForm';

export async function loader({ request, params }: Route.LoaderArgs) {
  const vehicleId = Number(params.vehicleId);
  const serviceRecordId = Number(params.serviceRecordId);

  const [serviceTypesResponse, recordResponse] = await Promise.all([
    ServiceTypesApi.getAll(request),
    ServiceRecordsApi.getById(request, vehicleId, serviceRecordId),
  ]);

  if (!serviceTypesResponse.ok) {
    throw new Response('Failed to load service types', {
      status: serviceTypesResponse.status,
    });
  }

  if (!recordResponse.ok) {
    throw new Response('Failed to load service record', {
      status: recordResponse.status,
    });
  }

  const serviceTypes: ServiceType[] = await serviceTypesResponse.json();
  const record: ServiceRecordResponse = await recordResponse.json();

  return { serviceTypes, record };
}

export async function action({ request, params }: Route.ActionArgs) {
  const vehicleId = Number(params.vehicleId);
  const serviceRecordId = Number(params.serviceRecordId);

  if (!vehicleId || !serviceRecordId) {
    throw new Response('Not found', { status: 404 });
  }

  const formData = await request.formData();
  const intent = formData.get('intent');

  if (intent === 'delete') {
    const response = await ServiceRecordsApi.delete(request, vehicleId, serviceRecordId);

    if (!response.ok) {
      return { error: 'Failed to delete service record' };
    }

    return redirect(`/garage/vehicle/${vehicleId}`);
  }

  const items = JSON.parse(formData.get('items')?.toString() ?? '[]');

  const data = {
    serviceDate: formData.get('serviceDate'),
    mileage: Number(formData.get('mileage')?.toString().replace(/,/g, '')),
    isSelfService: formData.get('provider') === 'DIY',
    shopName: formData.get('shopName')?.toString() || null,
    totalCost: formData.get('totalCost') ? Number(formData.get('totalCost')) : null,
    notes: formData.get('notes')?.toString() || null,
    items,
  };

  const response = await ServiceRecordsApi.update(request, vehicleId, serviceRecordId, data);

  if (!response.ok) {
    const error = await response.json();

    return {
      error:
        Object.values(error.errors ?? {})
          .flat()
          .join(' ') ||
        error.error ||
        error.message ||
        'Failed to update service record',
    };
  }

  return redirect(`/garage/vehicle/${vehicleId}`);
}

export default function ServiceRecordEdit() {
  const { vehicleId } = useParams();
  const { serviceTypes, record } = useLoaderData<typeof loader>();
  const actionData = useActionData<typeof action>();

  const [confirmingDelete, setConfirmingDelete] = useState(false);

  return (
    <div>
      <ServiceRecordForm
        vehicleId={vehicleId!}
        serviceTypes={serviceTypes}
        record={record}
        error={actionData?.error}
      />

      {/* Danger Zone */}
      <div className="mx-auto mt-8 max-w-3xl border-t border-border pt-6">
        {!confirmingDelete ? (
          <button
            type="button"
            onClick={() => setConfirmingDelete(true)}
            className="rounded-lg border border-red-500/50 px-4 py-2 text-sm font-medium text-red-500 transition hover:border-red-500 hover:bg-red-500/10"
          >
            Delete Service Record
          </button>
        ) : (
          <div className="rounded-xl border border-red-500/30 bg-red-500/5 p-4">
            <p className="text-sm text-red-500">
              This will permanently delete this service record. Are you sure?
            </p>

            <div className="mt-4 flex gap-3">
              <Form method="post">
                <input type="hidden" name="intent" value="delete" />

                <button
                  type="submit"
                  className="rounded-lg border border-red-500/50 px-4 py-2 text-sm font-medium text-red-500 transition hover:border-red-500 hover:bg-red-500/10"
                >
                  Yes, Delete
                </button>
              </Form>

              <button
                type="button"
                onClick={() => setConfirmingDelete(false)}
                className="rounded-lg border border-border px-4 py-2 text-sm font-medium text-text transition hover:bg-background"
              >
                Cancel
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
