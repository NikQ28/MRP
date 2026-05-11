import { api } from './client'
import type { Stock, StockCreate, StockStateRow } from '../types'

export const stocksApi = {
  getAll: () => api.get<Stock[]>('/Stock'),
  getCurrentStocks: (dateTimeIso: string) =>
    api.get<StockStateRow[]>(
      `/Stock/current?dateTime=${encodeURIComponent(dateTimeIso)}`,
    ),
  create: (data: StockCreate) => api.post<number>('/Stock', data),
  update: (id: number, data: StockCreate) =>
    api.put<number>(`/Stock/${id}`, data),
  delete: (id: number) => api.delete<number>(`/Stock/${id}`),
}
