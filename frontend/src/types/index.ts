export interface Item {
  id: number;
  name: string;
  description: string | null;
}

export interface ItemCreate {
  name: string;
  description?: string | null;
}

export interface WatchBom {
  id: number;
  parentId: number;
  childId: number;
  count: number;
}

export interface WatchBomCreate {
  parentId: number;
  childId: number;
  count: number;
}

export interface ItemNode {
  id: number;
  name: string;
  count: number;
  children: ItemNode[];
}
