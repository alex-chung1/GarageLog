export interface ServiceRecordItem {
    id: number
    serviceTypeId: number
    serviceTypeName: string
    quantity: number
    customName?: string | null
}

export interface ServiceRecord {
    id: number
    serviceDate: string
    mileage: number
    totalCost?: number | null
    isSelfService: boolean
    shopName?: string | null
    notes?: string | null
    createdAt: string
    items: ServiceRecordItem[]
}
