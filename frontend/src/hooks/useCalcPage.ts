import { useCallback, useEffect, useState } from 'react'
import { calculateApi } from '../api/calculate'
import { itemsApi } from '../api/items'
import { itemName } from '../lib/items'
import type { Item, RequiredItem } from '../types'

export function useCalcPage() {
  const [list, setList] = useState<RequiredItem[]>([])
  const [items, setItems] = useState<Item[]>([])
  const [loading, setLoading] = useState(true)

  const load = useCallback(async () => {
    setLoading(true)
    try {
      const [calcData, itemsData] = await Promise.all([
        calculateApi.getAll(),
        itemsApi.getAll(),
      ])
      setList(calcData)
      setItems(itemsData)
    } catch {
      setList([])
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  return {
    list,
    loading,
    reload: load,
    itemLabel: (itemId: number) => itemName(items, itemId),
  }
}
