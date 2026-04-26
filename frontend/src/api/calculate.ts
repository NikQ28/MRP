import { api } from './client'
import type { RequiredItem } from '../types'

export const calculateApi = {
  getAll: () => api.get<RequiredItem[]>('/Calculate'),
}
