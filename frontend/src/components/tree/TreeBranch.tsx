import type { TreeNode } from '../../types'
import { useState } from 'react'

type Props = {
  node: TreeNode
  parentId?: number
  onAddChild: (parentId: number, payload: { name: string; description: string; count: number }) => Promise<void>
  onEditNode: (nodeId: number, parentId: number | undefined, payload: { name: string; description: string; count: number }) => Promise<void>
  onDeleteNode: (nodeId: number) => Promise<void>
  getItemDraft: (nodeId: number) => { name: string; description: string }
  depth?: number
}

export function TreeBranch({ node, parentId, onAddChild, onEditNode, onDeleteNode, getItemDraft, depth = 0 }: Props) {
  const [showAddForm, setShowAddForm] = useState(false)
  const [showEditForm, setShowEditForm] = useState(false)
  const [addName, setAddName] = useState('')
  const [addCount, setAddCount] = useState('1')
  const [editName, setEditName] = useState('')
  const [editCount, setEditCount] = useState('1')

  const openAdd = () => {
    setAddName('')
    setAddCount('1')
    setShowAddForm(true)
    setShowEditForm(false)
  }

  const openEdit = () => {
    const draft = getItemDraft(node.id)
    setEditName(draft.name)
    setEditCount(String(node.count))
    setShowEditForm(true)
    setShowAddForm(false)
  }

  return (
    <div className="tree-node" style={{ marginLeft: depth * 20 }}>
      <div className="tree-node-row">
        <span className="tree-item">{node.name}: {node.count}</span>
        <span className="tree-actions">
          <button type="button" className="btn-sm" onClick={openAdd}>
            Добавить
          </button>
          <button type="button" className="btn-sm" onClick={openEdit}>
            Изменить
          </button>
          <button type="button" className="btn-sm danger" onClick={() => void onDeleteNode(node.id)}>
            Удалить
          </button>
        </span>
      </div>

      {showAddForm && (
        <form
          className="form-row"
          onSubmit={(e) => {
            e.preventDefault()
            void onAddChild(node.id, { name: addName.trim(), description: '', count: Math.max(1, Number(addCount) || 1) }).then(() => {
              setShowAddForm(false)
            })
          }}
        >
          <input placeholder="Название дочернего" value={addName} onChange={(e) => setAddName(e.target.value)} required />
          <input type="number" min={1} value={addCount} onChange={(e) => setAddCount(e.target.value)} />
          <div className="actions-right">
            <button type="submit" className="btn-sm">Сохранить</button>
            <button type="button" className="btn-sm" onClick={() => setShowAddForm(false)}>Отмена</button>
          </div>
        </form>
      )}

      {showEditForm && (
        <form
          className="form-row"
          onSubmit={(e) => {
            e.preventDefault()
            void onEditNode(node.id, parentId, { name: editName.trim(), description: '', count: Math.max(1, Number(editCount) || 1) }).then(() => {
              setShowEditForm(false)
            })
          }}
        >
          <input placeholder="Название" value={editName} onChange={(e) => setEditName(e.target.value)} required />
          <input
            type="number"
            min={1}
            value={editCount}
            onChange={(e) => setEditCount(e.target.value)}
            disabled={parentId == null}
          />
          <div className="actions-right">
            <button type="submit" className="btn-sm">Сохранить</button>
            <button type="button" className="btn-sm" onClick={() => setShowEditForm(false)}>Отмена</button>
          </div>
        </form>
      )}

      {node.children?.length > 0 && (
        <div className="tree-children">
          {node.children.map((child) => (
            <TreeBranch
              key={child.id}
              node={child}
              parentId={node.id}
              onAddChild={onAddChild}
              onEditNode={onEditNode}
              onDeleteNode={onDeleteNode}
              getItemDraft={getItemDraft}
              depth={depth + 1}
            />
          ))}
        </div>
      )}
    </div>
  )
}
