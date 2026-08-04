import type { Route } from './+types/service-record-create'
import type { ServiceType, SelectedService } from '~/types/serviceType'

import {
    Form,
    Link,
    redirect,
    useLoaderData,
    useParams,
    useNavigation,
    useActionData,
} from 'react-router'
import { useState } from 'react'

import { ServiceTypesApi } from '~/lib/api/service-type.server'
import { ServiceRecordsApi } from '~/lib/api/service-record.server'

import ServiceSelector from '~/components/ServiceSelector'

export async function loader({ request }: Route.LoaderArgs) {
    const response = await ServiceTypesApi.getAll(request)

    if (!response.ok) {
        throw new Response('Failed to load service types', {
            status: response.status,
        })
    }

    const serviceTypes: ServiceType[] = await response.json()

    return {
        serviceTypes,
    }
}

export async function action({ request, params }: Route.ActionArgs) {
    const vehicleId = Number(params.vehicleId)

    if (!vehicleId) {
        throw new Response('Vehicle not found', {
            status: 404,
        })
    }

    const formData = await request.formData()

    const items = JSON.parse(formData.get('items')?.toString() ?? '[]')

    const data = {
        serviceDate: formData.get('serviceDate'),
        mileage: Number(formData.get('mileage')?.toString().replace(/,/g, '')),
        isSelfService: formData.get('provider') === 'DIY',
        shopName: formData.get('shopName')?.toString() || null,
        totalCost: formData.get('totalCost') ? Number(formData.get('totalCost')) : null,
        notes: formData.get('notes')?.toString() || null,
        items,
    }

    const response = await ServiceRecordsApi.create(request, vehicleId, data)

    if (!response.ok) {
        const error = await response.json()

        return {
            error:
                Object.values(error.errors ?? {})
                    .flat()
                    .join(' ') ||
                error.error ||
                error.message ||
                'Failed to create service record',
        }
    }

    return redirect(`/garage/vehicle/${vehicleId}`)
}

export default function ServiceRecordCreate() {
    const { vehicleId } = useParams()

    const { serviceTypes } = useLoaderData<typeof loader>()

    const actionData = useActionData<typeof action>()

    const navigation = useNavigation()

    const isSubmitting = navigation.state === 'submitting'

    const [selectedServices, setSelectedServices] = useState<SelectedService[]>([])

    const [isDIY, setIsDIY] = useState(true)

    const [totalCost, setTotalCost] = useState('')
    const [mileage, setMileage] = useState('')

    return (
        <div>
            {/* Header */}
            <div className="mb-6">
                <h1 className="text-3xl font-bold text-primary">Add Service Record</h1>

                <p className="mt-1 text-muted">Record maintenance performed on your vehicle.</p>
            </div>

            {actionData?.error && (
                <div className="mx-auto mb-6 max-w-3xl rounded-lg border border-red-500/30 bg-red-500/10 p-4 text-sm text-red-500">
                    {actionData.error}
                </div>
            )}

            <Form
                method="post"
                className="mx-auto max-w-3xl space-y-6"
            >
                {/* hidden selected services */}
                <input
                    type="hidden"
                    name="items"
                    value={JSON.stringify(
                        selectedServices.map((service) => ({
                            serviceTypeId: service.serviceTypeId,
                            customName: service.customName ?? null,
                        })),
                    )}
                />

                {/* Service Details */}
                <section className="rounded-xl border border-border bg-card p-6 shadow-sm">
                    <h2 className="mb-4 text-xl font-semibold text-text">Service Details</h2>

                    <div className="grid gap-4 md:grid-cols-2">
                        <div>
                            <label className="mb-1 block text-sm font-medium text-text">
                                Service Date
                            </label>

                            <input
                                name="serviceDate"
                                type="date"
                                defaultValue={new Date().toISOString().split('T')[0]}
                                max={new Date().toISOString().split('T')[0]}
                                required
                                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
                            />
                        </div>

                        <div>
                            <label className="mb-1 block text-sm font-medium text-text">
                                Mileage
                            </label>

                            <input
                                name="mileage"
                                type="text"
                                inputMode="numeric"
                                required
                                value={mileage}
                                onChange={(e) => {
                                    const value = e.target.value.replace(/,/g, '')

                                    if (/^\d*$/.test(value)) {
                                        setMileage(value ? Number(value).toLocaleString() : '')
                                    }
                                }}
                                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
                            />
                        </div>
                    </div>
                </section>

                {/* Provider */}
                <section className="rounded-xl border border-border bg-card p-6 shadow-sm">
                    <h2 className="mb-4 text-xl font-semibold text-text">Performed By</h2>

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
                                placeholder="Shop Name"
                                className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
                            />
                        )}
                    </div>
                </section>

                {/* Services */}
                <ServiceSelector
                    serviceTypes={serviceTypes}
                    onChange={setSelectedServices}
                />

                {/* Cost */}
                <section className="rounded-xl border border-border bg-card p-6 shadow-sm">
                    <h2 className="mb-4 text-xl font-semibold text-text">Cost</h2>

                    <input
                        name="totalCost"
                        type="text"
                        inputMode="decimal"
                        placeholder="0.00"
                        value={totalCost}
                        onChange={(e) => {
                            const value = e.target.value

                            if (/^\d*\.?\d{0,2}$/.test(value)) {
                                setTotalCost(value)
                            }
                        }}
                        onBlur={() => {
                            if (totalCost) {
                                setTotalCost(Number(totalCost).toFixed(2))
                            }
                        }}
                        className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
                    />
                </section>

                {/* Notes */}
                <section className="rounded-xl border border-border bg-card p-6 shadow-sm">
                    <h2 className="mb-4 text-xl font-semibold text-text">Notes</h2>

                    <textarea
                        name="notes"
                        rows={4}
                        placeholder="Optional notes..."
                        className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
                    />
                </section>

                {/* Buttons */}
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
                        {isSubmitting ? 'Saving...' : 'Save Record'}
                    </button>
                </div>
            </Form>
        </div>
    )
}
