import { api } from './client';
import type { WatchBom, WatchBomCreate } from '../types';

export const watchBomsApi = {
  getAll: () => api.get<WatchBom[]>('/WatchBom'),
  create: (data: WatchBomCreate) => api.post<number>('/WatchBom', data),
  update: (id: number, data: WatchBomCreate) => api.put<number>(`/WatchBom/${id}`, data),
  delete: (id: number) => api.delete<number>(`/WatchBom/${id}`),
};
