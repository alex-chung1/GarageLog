// app/types/serviceRecord.ts

export interface ServiceRecordItemResponse {
    id: number
    serviceTypeId: number
    serviceTypeName: string
    customName?: string | null
}

export interface ServiceRecordResponse {
    id: number
    serviceDate: string
    mileage: number
    totalCost?: number | null
    isSelfService: boolean
    shopName?: string | null
    notes?: string | null
    createdAt: string
    items: ServiceRecordItemResponse[]
}

export interface CreateServiceRecordItemRequest {
    serviceTypeId: number
    customName?: string | null
}

export interface CreateServiceRecordRequest {
    serviceDate: string
    mileage: number
    isSelfService: boolean
    shopName?: string | null
    totalCost?: number | null
    notes?: string | null
    items: CreateServiceRecordItemRequest[]
}
