import { useCallback, useEffect, useState } from 'react'
import { calculateApi } from '../api/calculate'
import { itemsApi } from '../api/items'
import { itemName } from '../lib/items'
import type { Item, RequiredItem } from '../types'

function todayIsoDate(): string {
  return new Date().toISOString().split('T')[0]
}

export function useCalcPage() {
  const [list, setList] = useState<RequiredItem[]>([])
  const [items, setItems] = useState<Item[]>([])
  const [loading, setLoading] = useState(false)
  const [date, setDate] = useState(todayIsoDate())

  const loadItems = useCallback(async () => {
    try {
      const itemsData = await itemsApi.getAll()
      setItems(itemsData)
    } catch {
      setItems([])
    }
  }, [])

  const execute = useCallback(async () => {
    setLoading(true)
    try {
      const datetime = `${date}T00:00:00`
      const calcData = await calculateApi.getAll(datetime)
      setList(calcData)
    } catch {
      setList([])
    } finally {
      setLoading(false)
    }
  }, [date])

  useEffect(() => {
    void loadItems()
  }, [loadItems])

  return {
    date,
    setDate,
    list,
    loading,
    execute,
    itemLabel: (itemId: number) => itemName(items, itemId),
  }
}
