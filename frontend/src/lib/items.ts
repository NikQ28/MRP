import type { Item } from '../types'

export function itemName(items: Item[], id: number): string {
  return items.find((i) => i.id === id)?.name ?? `Неизвестный объект (${id})`
}
