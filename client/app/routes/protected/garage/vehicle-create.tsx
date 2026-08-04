import type { Route } from './+types/vehicle-create'

import { redirect, useActionData } from 'react-router'

import { VehiclesApi } from '~/lib/api/vehicle.server'

import VehicleForm from '~/components/VehicleForm'

export async function action({ request }: Route.ActionArgs) {
    const formData = await request.formData()

    const vehicle = {
        type: Number(formData.get('type')),
        make: formData.get('make'),
        model: formData.get('model'),
        year: Number(formData.get('year')),
        vin: formData.get('vin') || null,
    }

    const response = await VehiclesApi.create(request, vehicle)

    if (!response.ok) {
        return {
            error: 'Failed to create vehicle',
        }
    }

    return redirect('/garage')
}

export default function VehicleCreate() {
    const actionData = useActionData<typeof action>()

    return <VehicleForm error={actionData?.error} />
}
