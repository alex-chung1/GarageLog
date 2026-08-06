// app/types/vehicle.ts

export type VehicleType = 'Car' | 'Truck' | 'SUV';

export interface VehicleResponse {
  id: number;
  type: VehicleType;
  make: string;
  model: string;
  year: number;
  vin?: string | null;
  latestMileage?: number | null;
  createdAt: string;
}
