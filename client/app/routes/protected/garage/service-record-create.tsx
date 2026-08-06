import type { Route } from './+types/service-record-create';
import type { ServiceType } from '~/types/serviceType';

import { redirect, useLoaderData, useParams, useActionData } from 'react-router';

import { ServiceTypesApi } from '~/lib/api/service-type.server';
import { ServiceRecordsApi } from '~/lib/api/service-record.server';

import ServiceRecordForm from '~/components/ServiceRecordForm';

export async function loader({ request }: Route.LoaderArgs) {
  const response = await ServiceTypesApi.getAll(request);

  if (!response.ok) {
    throw new Response('Failed to load service types', {
      status: response.status,
    });
  }

  const serviceTypes: ServiceType[] = await response.json();

  return {
    serviceTypes,
  };
}

export async function action({ request, params }: Route.ActionArgs) {
  const vehicleId = Number(params.vehicleId);

  if (!vehicleId) {
    throw new Response('Vehicle not found', {
      status: 404,
    });
  }

  const formData = await request.formData();

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

  const response = await ServiceRecordsApi.create(request, vehicleId, data);

  if (!response.ok) {
    const error = await response.json();

    return {
      error:
        Object.values(error.errors ?? {})
          .flat()
          .join(' ') ||
        error.error ||
        error.message ||
        'Failed to create service record',
    };
  }

  return redirect(`/garage/vehicle/${vehicleId}`);
}

export default function ServiceRecordCreate() {
  const { vehicleId } = useParams();

  const { serviceTypes } = useLoaderData<typeof loader>();

  const actionData = useActionData<typeof action>();

  return (
    <ServiceRecordForm
      vehicleId={vehicleId!}
      serviceTypes={serviceTypes}
      error={actionData?.error}
    />
  );
}
