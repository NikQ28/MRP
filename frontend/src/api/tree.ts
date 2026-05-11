import { api } from './client'
import type { TreeNode } from '../types'

export const treeApi = {
  getTree: (rootItemId: number) =>
    api.get<TreeNode>(`/Tree/${rootItemId}`),
}
