// import type { Route } from './+types/vehicle-create'
import type { Route } from './+types'

import { Form, redirect, useActionData } from 'react-router'

import { VehiclesApi } from '~/lib/api/vehicle.server'

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

    return (
        <div className="mx-auto max-w-xl">
            <div className="mb-6">
                <h1 className="text-3xl font-bold text-primary">Add Vehicle</h1>

                <p className="mt-1 text-muted">Add a vehicle to your garage</p>
            </div>

            {actionData?.error && (
                <div className="mb-4 rounded-lg bg-red-500/10 p-4 text-red-500">
                    {actionData.error}
                </div>
            )}

            <Form
                method="post"
                className="space-y-5 rounded-xl border border-border bg-card p-6 shadow-sm"
            >
                {/* Type */}
                <div>
                    <label
                        htmlFor="type"
                        className="text-sm font-medium text-text"
                    >
                        Vehicle Type
                    </label>

                    <select
                        id="type"
                        name="type"
                        className="mt-1 w-full rounded-lg border border-border bg-background p-2"
                    >
                        <option value="1">Car</option>

                        <option value="2">Truck</option>

                        <option value="3">SUV</option>
                    </select>
                </div>

                {/* Make */}
                <div>
                    <label
                        htmlFor="make"
                        className="text-sm font-medium text-text"
                    >
                        Make
                    </label>

                    <input
                        id="make"
                        name="make"
                        required
                        className="mt-1 w-full rounded-lg border border-border bg-background p-2"
                        placeholder="Honda"
                    />
                </div>

                {/* Model */}
                <div>
                    <label
                        htmlFor="model"
                        className="text-sm font-medium text-text"
                    >
                        Model
                    </label>

                    <input
                        id="model"
                        name="model"
                        required
                        className="mt-1 w-full rounded-lg border border-border bg-background p-2"
                        placeholder="Accord"
                    />
                </div>

                {/* Year */}
                <div>
                    <label
                        htmlFor="year"
                        className="text-sm font-medium text-text"
                    >
                        Year
                    </label>

                    <input
                        id="year"
                        name="year"
                        type="number"
                        required
                        min="1886"
                        max={new Date().getFullYear() + 1}
                        className="mt-1 w-full rounded-lg border border-border bg-background p-2"
                        placeholder="2021"
                    />
                </div>

                {/* VIN */}
                <div>
                    <label
                        htmlFor="vin"
                        className="text-sm font-medium text-text"
                    >
                        VIN
                    </label>

                    <input
                        id="vin"
                        name="vin"
                        maxLength={17}
                        className="mt-1 w-full rounded-lg border border-border bg-background p-2"
                        placeholder="Optional"
                    />
                </div>

                <button
                    type="submit"
                    className="w-full rounded-lg bg-primary px-4 py-2 font-medium text-white transition hover:opacity-90"
                >
                    Add Vehicle
                </button>
            </Form>
        </div>
    )
}
