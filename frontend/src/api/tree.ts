import { api } from './client';
import type { ItemNode } from '../types';

export const treeApi = {
  getTree: (rootId: number) => api.get<ItemNode>(`/Tree/${rootId}`),
};
