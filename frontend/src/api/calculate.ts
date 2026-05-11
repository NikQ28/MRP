import { api } from './client'
import type { RequiredItem } from '../types'

export const calculateApi = {
  getAll: (datetime: string) =>
    api.get<RequiredItem[]>(`/Calculate?datetime=${encodeURIComponent(datetime)}`),
}
