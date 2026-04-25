import { useCallback, useEffect, useState } from 'react'
import { itemsApi } from '../api/items'
import { stocksApi } from '../api/stocks'
import { itemName } from '../lib/items'
import type { Item, Stock, OperationType } from '../types'
import type { StockCreate } from '../types'

interface StockForm {
  itemId: number
  count: string
  operation: OperationType
  datetime: string
}

function toLocalDateTimeValue(date: Date): string {
  const offsetMs = date.getTimezoneOffset() * 60_000
  return new Date(date.getTime() - offsetMs).toISOString().slice(0, 16)
}

function nowLocalDateTime(): string {
  return toLocalDateTimeValue(new Date())
}

function toIsoString(localValue: string): string {
  return new Date(localValue).toISOString()
}

function emptyForm(firstItemId: number): StockForm {
  return {
    itemId: firstItemId,
    count: '1',
    operation: 0,
    datetime: nowLocalDateTime(),
  }
}

function stockToForm(row: Stock): StockForm {
  return {
    itemId: row.itemId,
    count: String(row.count),
    operation: row.operation,
    datetime: toLocalDateTimeValue(new Date(row.datetime)),
  }
}

export function useStocksPage() {
  const [list, setList] = useState<Stock[]>([])
  const [items, setItems] = useState<Item[]>([])
  const [loading, setLoading] = useState(true)
  const [form, setForm] = useState<StockForm>(() => emptyForm(0))
  const [editingId, setEditingId] = useState<number | null>(null)
  const [submitting, setSubmitting] = useState(false)

  const load = useCallback(async () => {
    setLoading(true)
    try {
      const [stocksData, itemsData] = await Promise.all([
        stocksApi.getAll(),
        itemsApi.getAll(),
      ])
      setList(stocksData)
      setItems(itemsData)
      setForm((prev) => {
        if (prev.itemId > 0 || itemsData.length === 0) return prev
        return { ...prev, itemId: itemsData[0].id }
      })
    } catch {
      setList([])
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  const resetForm = useCallback(() => {
    setForm(emptyForm(items[0]?.id ?? 0))
  }, [items])

  const mutation = editingId == null ? 'create' : 'update'

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (mutation === 'update' && editingId == null) return
    const count = Math.max(1, Number(form.count) || 1)
    if (count < 1) return

    setSubmitting(true)
    try {
      const payload: StockCreate = {
        itemId: form.itemId,
        count,
        operation: form.operation,
        datetime: toIsoString(form.datetime),
      }
      if (mutation === 'create') {
        await stocksApi.create(payload)
        await load()
      } else {
        await stocksApi.update(editingId!, payload)
        setList((prev) =>
          prev.map((row) =>
            row.id === editingId
              ? { ...row, itemId: payload.itemId, count: payload.count, operation: payload.operation, datetime: payload.datetime }
              : row,
          ),
        )
        setEditingId(null)
      }
      resetForm()
    } catch {} finally {
      setSubmitting(false)
    }
  }

  const startEdit = (row: Stock) => {
    setEditingId(row.id)
    setForm(stockToForm(row))
  }

  const cancelEdit = () => {
    setEditingId(null)
    resetForm()
  }

  const deleteRow = async (id: number) => {
    try {
      await stocksApi.delete(id)
      if (editingId === id) cancelEdit()
      await load()
    } catch {}
  }

  const label = (id: number) => itemName(items, id)

  return {
    list,
    items,
    loading,
    form,
    setForm,
    editingId,
    submitting,
    onSubmit,
    startEdit,
    cancelEdit,
    deleteRow,
    itemLabel: label,
  }
}
