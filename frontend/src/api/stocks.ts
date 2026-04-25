import { api } from './client'
import type { Stock, StockCreate } from '../types'

export const stocksApi = {
  getAll: () => api.get<Stock[]>('/Stock'),
  create: (data: StockCreate) => api.post<number>('/Stock', data),
  update: (id: number, data: StockCreate) =>
    api.put<number>(`/Stock/${id}`, data),
  delete: (id: number) => api.delete<number>(`/Stock/${id}`),
}
