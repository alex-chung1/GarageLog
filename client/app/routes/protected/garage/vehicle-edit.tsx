import type { Route } from './+types/vehicle-edit'
import type { VehicleResponse } from '~/types/vehicle'

import { redirect, useLoaderData, useActionData, Form } from 'react-router'
import { useState } from 'react'

import { VehiclesApi } from '~/lib/api/vehicle.server'

import VehicleForm from '~/components/VehicleForm'

export async function loader({ request, params }: Route.LoaderArgs) {
    const vehicleId = Number(params.vehicleId)

    if (!vehicleId) {
        throw new Response('Vehicle not found', { status: 404 })
    }

    const response = await VehiclesApi.getById(request, vehicleId)

    if (!response.ok) {
        throw new Response('Failed to load vehicle', { status: response.status })
    }

    const vehicle: VehicleResponse = await response.json()

    return { vehicle }
}

export async function action({ request, params }: Route.ActionArgs) {
    const vehicleId = Number(params.vehicleId)

    if (!vehicleId) {
        throw new Response('Vehicle not found', { status: 404 })
    }

    const formData = await request.formData()
    const intent = formData.get('intent')

    if (intent === 'delete') {
        const response = await VehiclesApi.delete(request, vehicleId)

        if (!response.ok) {
            return { error: 'Failed to delete vehicle' }
        }

        return redirect('/garage')
    }

    const vehicle = {
        type: Number(formData.get('type')),
        make: formData.get('make'),
        model: formData.get('model'),
        year: Number(formData.get('year')),
        vin: formData.get('vin') || null,
    }

    const response = await VehiclesApi.update(request, vehicleId, vehicle)

    if (!response.ok) {
        const error = await response.json()

        return {
            error:
                Object.values(error.errors ?? {})
                    .flat()
                    .join(' ') ||
                error.error ||
                error.message ||
                'Failed to update vehicle',
        }
    }

    return redirect(`/garage/vehicle/${vehicleId}`)
}

export default function VehicleEdit() {
    const { vehicle } = useLoaderData<typeof loader>()
    const actionData = useActionData<typeof action>()

    const [confirmingDelete, setConfirmingDelete] = useState(false)

    return (
        <div className="mx-auto max-w-xl">
            <VehicleForm
                vehicle={vehicle}
                error={actionData?.error}
            />

            <div className="mt-4 text-center">
                {!confirmingDelete ? (
                    <button
                        type="button"
                        onClick={() => setConfirmingDelete(true)}
                        className="text-sm font-medium text-red-500 hover:underline"
                    >
                        Delete Vehicle
                    </button>
                ) : (
                    <div className="space-y-3">
                        <p className="text-sm text-red-500">
                            This will permanently delete this vehicle and all its service records.
                            Are you sure?
                        </p>

                        <div className="flex justify-center gap-3">
                            <Form method="post">
                                <input
                                    type="hidden"
                                    name="intent"
                                    value="delete"
                                />
                                <button
                                    type="submit"
                                    className="rounded-lg bg-red-500 px-4 py-2 text-sm font-medium text-white transition hover:opacity-90"
                                >
                                    Yes, Delete
                                </button>
                            </Form>

                            <button
                                type="button"
                                onClick={() => setConfirmingDelete(false)}
                                className="rounded-lg border border-border px-4 py-2 text-sm font-medium text-text"
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    )
}
