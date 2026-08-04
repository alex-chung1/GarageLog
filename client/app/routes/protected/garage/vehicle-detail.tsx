import type { Route } from './+types/vehicle-detail'
import type { ServiceRecord } from '~/types/serviceRecord'

import { useLoaderData } from 'react-router'
import { ServiceRecordsApi } from '~/lib/api/service-record.server'

export async function loader({ request, params }: Route.LoaderArgs) {
    const vehicleId = params.vehicleId

    if (!vehicleId) {
        throw new Response('Not Found', { status: 404 })
    }

    const response = await ServiceRecordsApi.getAll(request, Number(vehicleId))

    if (!response.ok) {
        throw new Response('Failed to load service records', {
            status: response.status,
        })
    }

    const records: ServiceRecord[] = await response.json()

    return { records }
}

export default function VehicleDetail() {
    const { records } = useLoaderData<typeof loader>()

    return (
        <div>
            {/* Header */}
            <div className="mb-6">
                <h1 className="text-3xl font-bold text-primary">
                    Service History
                </h1>

                <p className="mt-1 text-muted">
                    Track maintenance and repairs for this vehicle
                </p>
            </div>

            {/* Content */}
            {records.length === 0 ? (
                <div className="rounded-xl border border-border bg-card p-6 text-muted shadow-sm">
                    No service records found.
                </div>
            ) : (
                <div className="mx-auto max-w-4xl space-y-6">
                    {records.map((record) => (
                        <div
                            key={record.id}
                            className="rounded-xl bg-linear-to-r from-blue-500/30 via-purple-500/30 to-pink-500/30 p-px transition hover:shadow-md"
                        >
                            <div className="rounded-xl bg-card p-5 md:p-6">
                                {/* Top Section */}
                                <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
                                    <div>
                                        <div className="mb-3 flex h-10 w-10 items-center justify-center rounded-lg bg-primary/20 text-primary">
                                            🔧
                                        </div>

                                        <h2 className="text-xl font-bold text-text">
                                            {new Date(
                                                record.serviceDate,
                                            ).toLocaleDateString()}
                                        </h2>

                                        <p className="mt-1 text-sm text-muted">
                                            {record.mileage.toLocaleString()}{' '}
                                            miles
                                        </p>
                                    </div>

                                    {record.totalCost != null && (
                                        <div className="rounded-lg bg-primary/10 px-3 py-2 text-sm font-semibold text-primary">
                                            ${record.totalCost.toFixed(2)}
                                        </div>
                                    )}
                                </div>

                                {/* Provider */}
                                <div className="mt-5 space-y-2 text-sm text-muted">
                                    <p>
                                        <span className="font-medium text-text">
                                            Performed By:
                                        </span>{' '}
                                        {record.isSelfService
                                            ? 'Self Service'
                                            : (record.shopName ??
                                              'Unknown Shop')}
                                    </p>
                                </div>

                                {/* Services */}
                                <div className="mt-5">
                                    <h3 className="font-semibold text-text">
                                        Services
                                    </h3>

                                    <ul className="mt-2 space-y-1 text-sm text-muted">
                                        {record.items.map((item) => (
                                            <li
                                                key={item.id}
                                                className="flex items-center gap-2"
                                            >
                                                <span className="text-primary">
                                                    •
                                                </span>

                                                {item.serviceTypeName}

                                                {item.quantity > 1 &&
                                                    ` x${item.quantity}`}
                                            </li>
                                        ))}
                                    </ul>
                                </div>

                                {/* Notes */}
                                {record.notes && (
                                    <div className="mt-5 rounded-lg border border-border bg-background p-3 text-sm text-muted">
                                        {record.notes}
                                    </div>
                                )}
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    )
}
