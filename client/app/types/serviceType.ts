// app/types/serviceType.ts

export interface ServiceType {
  id: number;
  name: string;
}

export interface SelectedService {
  serviceTypeId: number;
  name: string;
  customName?: string;
}
