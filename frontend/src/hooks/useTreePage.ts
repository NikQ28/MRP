import { useCallback, useEffect, useState } from 'react'
import { itemsApi } from '../api/items'
import { treeApi } from '../api/tree'
import type { Item, TreeNode } from '../types'

export function useTreePage() {
  const [items, setItems] = useState<Item[]>([])
  const [rootId, setRootId] = useState('')
  const [tree, setTree] = useState<TreeNode | null>(null)
  const [loading, setLoading] = useState(false)

  const reloadItems = useCallback(async () => {
    try {
      const data = await itemsApi.getAll()
      setItems(data)
      if (data.length === 0) {
        setRootId('')
        return
      }

      setRootId((prev) => {
        if (!prev) return String(data[0].id)
        const prevId = Number(prev)
        return data.some((item) => item.id === prevId) ? prev : String(data[0].id)
      })
    } catch {}
  }, [])

  useEffect(() => {
    void reloadItems()
  }, [reloadItems])

  const loadTree = useCallback(async () => {
    const id = Number(rootId)
    if (Number.isNaN(id) || id < 1) return
    setLoading(true)
    setTree(null)
    try {
      setTree(await treeApi.getTree(id))
    } catch {
      setTree(null)
    } finally {
      setLoading(false)
    }
  }, [rootId])

  const updateTreeNode = (nodeId: number, updates: { name?: string; count?: number }) => {
    setTree((prev) => {
      if (!prev) return prev
      const update = (node: TreeNode): TreeNode => {
        if (node.id === nodeId) {
          return { ...node, ...(updates.name != null && { name: updates.name }), ...(updates.count != null && { count: updates.count }) }
        }
        return { ...node, children: node.children.map(update) }
      }
      return update(prev)
    })
  }

  const addTreeNode = (parentId: number, child: TreeNode) => {
    setTree((prev) => {
      if (!prev) return prev
      const add = (node: TreeNode): TreeNode => {
        if (node.id === parentId) {
          return { ...node, children: [...node.children, child] }
        }
        return { ...node, children: node.children.map(add) }
      }
      return add(prev)
    })
  }

  const deleteTreeNode = (nodeId: number) => {
    setTree((prev) => {
      if (!prev) return prev
      if (prev.id === nodeId) return null
      const remove = (node: TreeNode): TreeNode => ({
        ...node,
        children: node.children.filter((c) => c.id !== nodeId).map(remove),
      })
      return remove(prev)
    })
  }

  return {
    items,
    rootId,
    setRootId,
    tree,
    loading,
    loadTree,
    reloadItems,
    updateTreeNode,
    addTreeNode,
    deleteTreeNode,
  }
}
