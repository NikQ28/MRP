export interface Item {
  id: number
  name: string
  description: string | null
}

export interface ItemCreate {
  name: string
  description?: string | null
}

/** Совпадает с BomResponse: parentId, componentId, count */
export interface Bom {
  id: number
  parentId: number
  componentId: number
  count: number
}

/** Совпадает с backend.Domain.DTO.TreeNode (JSON camelCase) */
export interface TreeNode {
  id: number
  name: string
  count: number
  children: TreeNode[]
}

/** Совпадает с backend.Domain.Entity.OperationType: Приход = 0, Расход = 1 */
export type OperationType = 0 | 1

export interface Stock {
  id: number
  itemId: number
  count: number
  operation: OperationType
  datetime: string
}

export interface StockCreate {
  itemId: number
  count: number
  operation: OperationType
  datetime: string
}

export type OrderStatus = 0 | 1

export interface OrderLine {
  itemId: number
  count: number
}

export interface Order {
  orderId: number
  creation: string
  execution: string
  status: OrderStatus
  orderStrings: OrderLine[]
}

export interface OrderRequest {
  creation: string
  execution: string
  status: OrderStatus
  orderStrings: OrderLine[]
}

export interface RequiredItem {
  itemId: number
  requiredCount: number
  stockCount: number
  needCount: number
}
