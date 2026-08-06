import { Link } from 'react-router';
import { formatServiceDate } from '~/lib/format';
import type { ServiceRecordResponse } from '~/types/serviceRecord';

export default function ServiceRecordCard({
  record,
  vehicleId,
}: {
  record: ServiceRecordResponse;
  vehicleId: number;
}) {
  return (
    <div className="rounded-xl bg-linear-to-r from-blue-500/30 via-purple-500/30 to-pink-500/30 p-px transition hover:shadow-md">
      <div className="rounded-xl bg-card p-5 md:p-6">
        {/* Top Section */}
        <div className="flex items-start justify-between gap-4">
          <div>
            <div className="mb-3 flex h-10 w-10 items-center justify-center rounded-lg bg-primary/20 text-primary">
              🔧
            </div>

            <h2 className="text-xl font-bold text-text">{formatServiceDate(record.serviceDate)}</h2>

            <p className="mt-1 text-sm text-muted">{record.mileage.toLocaleString()} miles</p>

            {record.totalCost != null && (
              <div className="mt-2 inline-block rounded-lg bg-primary/10 px-3 py-1 text-sm font-semibold text-primary">
                ${record.totalCost.toFixed(2)}
              </div>
            )}
          </div>

          <Link
            to={`/garage/vehicle/${vehicleId}/service-record/${record.id}/edit`}
            className="shrink-0 rounded-lg border border-border px-3 py-2 text-sm font-medium text-text transition hover:bg-background"
          >
            Edit
          </Link>
        </div>

        {/* Provider */}
        <div className="mt-5 space-y-2 text-sm text-muted">
          <p>
            <span className="font-medium text-text">Performed By:</span>{' '}
            {record.isSelfService ? 'Self Service' : (record.shopName ?? 'Unknown Shop')}
          </p>
        </div>

        {/* Services */}
        <div className="mt-5">
          <h3 className="font-semibold text-text">Services</h3>

          <ul className="mt-2 space-y-1 text-sm text-muted">
            {[...record.items]
              .sort((a, b) => {
                const aIsOther = a.serviceTypeName === 'Other' ? 1 : 0;
                const bIsOther = b.serviceTypeName === 'Other' ? 1 : 0;
                return aIsOther - bIsOther;
              })
              .map((item) => (
                <li key={item.id} className="flex items-center gap-2">
                  <span className="text-primary">•</span>
                  {item.serviceTypeName === 'Other' && item.customName
                    ? `${item.serviceTypeName}: ${item.customName}`
                    : item.serviceTypeName}
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
  );
}
