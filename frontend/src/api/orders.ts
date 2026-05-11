import { api } from './client'
import type { Order, OrderRequest } from '../types'

export const ordersApi = {
  getAll: () => api.get<Order[]>('/Order'),
  create: (data: OrderRequest) => api.post<number>('/Order', data),
  update: (id: number, data: OrderRequest) => api.put<number>(`/Order/${id}`, data),
  delete: (id: number) => api.delete<number>(`/Order/${id}`),
}
