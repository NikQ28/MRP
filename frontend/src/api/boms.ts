import { api } from './client'
import type { Bom } from '../types'

export const bomsApi = {
  getAll: () => api.get<Bom[]>('/Bom'),
  create: (data: { parentId: number; componentId: number; count: number }) => api.post<number>('/Bom', data),
  update: (id: number, data: { parentId: number; componentId: number; count: number }) =>
    api.put<number>(`/Bom/${id}`, data),
  delete: (id: number) => api.delete<number>(`/Bom/${id}`),
}
