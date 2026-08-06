import type { ServiceType, SelectedService } from '~/types/serviceType';

import { useState } from 'react';

type ServiceSelectorProps = {
  serviceTypes: ServiceType[];
  initialServices?: SelectedService[];
  onChange: (services: SelectedService[]) => void;
};

export default function ServiceSelector({
  serviceTypes,
  initialServices = [],
  onChange,
}: ServiceSelectorProps) {
  const [selectedServices, setSelectedServices] = useState<SelectedService[]>(initialServices);

  const [search, setSearch] = useState('');
  const [showSearch, setShowSearch] = useState(false);

  const [showCustom, setShowCustom] = useState(false);
  const [customName, setCustomName] = useState('');

  const popularServices = serviceTypes.slice(0, 3);

  const searchableServices = serviceTypes
    .filter((service) => service.id !== 9999)
    .filter((service) => !popularServices.some((popular) => popular.id === service.id))
    .filter(
      (service) => !selectedServices.some((selected) => selected.serviceTypeId === service.id)
    )
    .filter((service) => service.name.toLowerCase().includes(search.toLowerCase()));

  function updateSelected(services: SelectedService[]) {
    setSelectedServices(services);
    onChange(services);
  }

  function addService(service: ServiceType) {
    updateSelected([
      ...selectedServices,
      {
        serviceTypeId: service.id,
        name: service.name,
      },
    ]);

    setSearch('');
    setShowSearch(false);
  }

  function removeService(target: SelectedService) {
    updateSelected(
      selectedServices.filter(
        (service) =>
          !(
            service.serviceTypeId === target.serviceTypeId &&
            service.customName === target.customName
          )
      )
    );
  }

  function addCustomService() {
    if (!customName.trim()) {
      return;
    }

    updateSelected([
      ...selectedServices,
      {
        serviceTypeId: 9999,
        name: customName,
        customName,
      },
    ]);

    setCustomName('');
    setShowCustom(false);
  }

  return (
    <section className="rounded-xl border border-border bg-card p-6 shadow-sm">
      <h2 className="mb-4 text-xl font-semibold text-text">Services Performed</h2>

      {/* Search */}
      <div className="relative">
        <input
          type="text"
          placeholder="Search services..."
          value={search}
          onFocus={() => setShowSearch(true)}
          onChange={(e) => {
            setSearch(e.target.value);
            setShowSearch(true);
          }}
          className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
        />

        {showSearch && search && (
          <div className="absolute z-10 mt-2 w-full rounded-lg border border-border bg-card shadow-lg">
            {searchableServices.length === 0 ? (
              <div className="p-3 text-sm text-muted">No services found.</div>
            ) : (
              searchableServices.map((service) => (
                <button
                  key={service.id}
                  type="button"
                  onClick={() => addService(service)}
                  className="block w-full px-3 py-2 text-left text-text hover:bg-background"
                >
                  {service.name}
                </button>
              ))
            )}
          </div>
        )}
      </div>

      {/* Popular */}
      <div className="mt-5">
        <h3 className="mb-2 font-medium text-text">Popular Services</h3>

        <div className="space-y-2">
          {popularServices.map((service) => (
            <label key={service.id} className="flex items-center gap-2 text-text">
              <input
                type="checkbox"
                checked={selectedServices.some((selected) => selected.serviceTypeId === service.id)}
                onChange={() => {
                  const existing = selectedServices.find(
                    (selected) => selected.serviceTypeId === service.id
                  );

                  if (existing) {
                    removeService(existing);
                  } else {
                    addService(service);
                  }
                }}
              />

              {service.name}
            </label>
          ))}
        </div>
      </div>

      {/* Selected */}
      {selectedServices.length > 0 && (
        <div className="mt-5">
          <h3 className="mb-2 font-medium text-text">Selected Services</h3>

          {/* System Services */}
          {selectedServices.some((service) => service.serviceTypeId !== 9999) && (
            <div className="space-y-2">
              {selectedServices
                .filter((service) => service.serviceTypeId !== 9999)
                .map((service) => (
                  <div
                    key={service.serviceTypeId}
                    className="flex items-center justify-between rounded-lg bg-background px-3 py-2 text-sm"
                  >
                    <span className="text-text">{service.name}</span>

                    <button
                      type="button"
                      onClick={() => removeService(service)}
                      className="text-muted hover:text-text"
                    >
                      ✕
                    </button>
                  </div>
                ))}
            </div>
          )}

          {/* Custom Services */}
          {selectedServices.some((service) => service.serviceTypeId === 9999) && (
            <div className="mt-4">
              <h4 className="mb-2 text-sm font-semibold text-primary">Custom Services</h4>

              <div className="space-y-2">
                {selectedServices
                  .filter((service) => service.serviceTypeId === 9999)
                  .map((service, index) => (
                    <div
                      key={`${service.serviceTypeId}-${service.customName}-${index}`}
                      className="flex items-center justify-between rounded-lg border border-primary/30 bg-primary/10 px-3 py-2 text-sm"
                    >
                      <span className="font-medium text-primary">{service.customName}</span>

                      <button
                        type="button"
                        onClick={() => removeService(service)}
                        className="text-muted hover:text-text"
                      >
                        ✕
                      </button>
                    </div>
                  ))}
              </div>
            </div>
          )}
        </div>
      )}

      {/* Custom */}
      {!showCustom ? (
        <button
          type="button"
          onClick={() => setShowCustom(true)}
          className="mt-5 rounded-lg border border-border px-4 py-2 text-sm font-medium text-text hover:bg-background"
        >
          + Add Custom Service
        </button>
      ) : (
        <div className="mt-5 space-y-3">
          <input
            type="text"
            placeholder="Custom service name..."
            value={customName}
            onChange={(e) => setCustomName(e.target.value)}
            className="w-full rounded-lg border border-border bg-background px-3 py-2 text-text"
          />

          <div className="flex gap-3">
            <button
              type="button"
              onClick={addCustomService}
              className="rounded-lg bg-primary px-4 py-2 text-sm font-medium text-white"
            >
              Add
            </button>

            <button
              type="button"
              onClick={() => {
                setShowCustom(false);
                setCustomName('');
              }}
              className="rounded-lg border border-border px-4 py-2 text-sm font-medium text-text"
            >
              Cancel
            </button>
          </div>
        </div>
      )}
    </section>
  );
}
