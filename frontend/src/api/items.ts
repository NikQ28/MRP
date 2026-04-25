import { api } from './client'
import type { Item, ItemCreate } from '../types'

/** Backend: DELETE /Item/{id} вызывает удаление ветки (TreeService), не простую запись Item. */
export const itemsApi = {
  getAll: () => api.get<Item[]>('/Item'),
  create: (data: ItemCreate) => api.post<number>('/Item', data),
  update: (id: number, data: ItemCreate) =>
    api.put<number>(`/Item/${id}`, data),
  deleteBranch: (id: number) => api.delete<number>(`/Item/${id}`),
}
