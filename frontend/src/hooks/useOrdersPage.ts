import { useCallback, useEffect, useMemo, useState } from 'react'
import { bomsApi } from '../api/boms'
import { itemsApi } from '../api/items'
import { ordersApi } from '../api/orders'
import { itemName } from '../lib/items'
import type { Item, Order, OrderRequest, OrderStatus } from '../types'

interface FormLine {
  itemId: number
  count: string
}

interface OrderForm {
  creation: string
  execution: string
  status: OrderStatus
  orderStrings: FormLine[]
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

function toForm(order: Order): OrderForm {
  return {
    creation: toLocalDateTimeValue(new Date(order.creation)),
    execution: toLocalDateTimeValue(new Date(order.execution)),
    status: order.status,
    orderStrings: order.orderStrings.map((line) => ({ itemId: line.itemId, count: String(line.count) })),
  }
}

function createEmptyForm(defaultItemId: number): OrderForm {
  return {
    creation: nowLocalDateTime(),
    execution: nowLocalDateTime(),
    status: 0,
    orderStrings: defaultItemId > 0 ? [{ itemId: defaultItemId, count: '1' }] : [],
  }
}

export function useOrdersPage() {
  const [orders, setOrders] = useState<Order[]>([])
  const [items, setItems] = useState<Item[]>([])
  const [allowedItemIds, setAllowedItemIds] = useState<number[]>([])
  const [loading, setLoading] = useState(true)
  const [editingOrderId, setEditingOrderId] = useState<number | null>(null)
  const [submitting, setSubmitting] = useState(false)
  const [form, setForm] = useState<OrderForm>(createEmptyForm(0))

  const load = useCallback(async () => {
    setLoading(true)
    try {
      const [ordersData, itemsData, bomData] = await Promise.all([
        ordersApi.getAll(),
        itemsApi.getAll(),
        bomsApi.getAll(),
      ])
      const allowedIds = Array.from(new Set(bomData.map((row) => row.parentId)))
      setOrders(ordersData)
      setItems(itemsData)
      setAllowedItemIds(allowedIds)
      setForm((prev) => {
        if (prev.orderStrings.length > 0) return prev
        return createEmptyForm(allowedIds[0] ?? 0)
      })
    } catch {
      setOrders([])
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    void load()
  }, [load])

  const allowedItems = useMemo(
    () => items.filter((item) => allowedItemIds.includes(item.id)),
    [allowedItemIds, items],
  )

  const resetForm = useCallback(() => {
    setEditingOrderId(null)
    setForm(createEmptyForm(allowedItemIds[0] ?? 0))
  }, [allowedItemIds])

  const addLine = () => {
    const fallbackId = allowedItemIds[0]
    if (!fallbackId) return
    setForm((prev) => ({
      ...prev,
      orderStrings: [...prev.orderStrings, { itemId: fallbackId, count: '1' }],
    }))
  }

  const updateLine = (index: number, line: FormLine) => {
    setForm((prev) => ({
      ...prev,
      orderStrings: prev.orderStrings.map((row, i) => (i === index ? line : row)),
    }))
  }

  const removeLine = (index: number) => {
    setForm((prev) => ({
      ...prev,
      orderStrings: prev.orderStrings.filter((_, i) => i !== index),
    }))
  }

  const startEdit = (order: Order) => {
    setEditingOrderId(order.orderId)
    setForm(toForm(order))
  }

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (form.execution && form.creation && form.execution < form.creation) {
      alert('Дата выполнения не может быть раньше даты создания');
      return;
    }

    if (form.orderStrings.length === 0) return

    const hasForbidden = form.orderStrings.some(
      (line) => !allowedItemIds.includes(line.itemId),
    )
    if (hasForbidden) return

    setSubmitting(true)
    try {
      const payload: OrderRequest = {
        creation: toIsoString(form.creation),
        execution: toIsoString(form.execution),
        status: form.status as OrderStatus,
        orderStrings: form.orderStrings.map((line) => ({
          itemId: line.itemId,
          count: Math.max(1, Number(line.count) || 1),
        })),
      }
      if (editingOrderId == null) {
        await ordersApi.create(payload)
        await load()
      } else {
        await ordersApi.update(editingOrderId, payload)
        setOrders((prev) =>
          prev.map((order) =>
            order.orderId === editingOrderId
              ? {
                  ...order,
                  creation: payload.creation,
                  execution: payload.execution,
                  status: payload.status,
                  orderStrings: payload.orderStrings,
                }
              : order,
          ),
        )
      }
      resetForm()
    } catch {} finally {
      setSubmitting(false)
    }
  }

  const deleteOrder = async (orderId: number) => {
    try {
      await ordersApi.delete(orderId)
      if (editingOrderId === orderId) resetForm()
      await load()
    } catch {}
  }

  return {
    orders,
    items,
    allowedItems,
    loading,
    form,
    setForm,
    editingOrderId,
    submitting,
    addLine,
    updateLine,
    removeLine,
    startEdit,
    resetForm,
    onSubmit,
    deleteOrder,
    itemLabel: (itemId: number) => itemName(items, itemId),
  }
}
