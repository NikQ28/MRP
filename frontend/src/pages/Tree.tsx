import { TreeBranch } from '../components/tree/TreeBranch'
import { bomsApi } from '../api/boms'
import { itemsApi } from '../api/items'
import { useTreePage } from '../hooks/useTreePage'
import './Pages.css'

export function Tree() {
  const p = useTreePage()

  const handleAddChild = async (
    parentId: number,
    payload: { name: string; description: string; count: number },
  ) => {
    const created = await itemsApi.create({
      name: payload.name,
      description: payload.description || null,
    })
    const childId = typeof created === 'number' ? created : (created as { id?: number }).id ?? 0
    if (!childId) throw new Error('Не удалось определить ID нового узла')
    await bomsApi.create({ parentId, componentId: childId, count: payload.count })
    p.addTreeNode(parentId, { id: childId, name: payload.name, count: payload.count, children: [] })
    await p.reloadItems()
  }

  const handleEditNode = async (
    nodeId: number,
    parentId: number | undefined,
    payload: { name: string; description: string; count: number },
  ) => {
    await itemsApi.update(nodeId, { name: payload.name, description: payload.description || null })
    if (parentId != null) {
      const allBoms = await bomsApi.getAll()
      const relation = allBoms.find(
        (row) => row.parentId === parentId && row.componentId === nodeId,
      )
      if (relation) {
        await bomsApi.update(relation.id, { parentId, componentId: nodeId, count: payload.count })
      }
    }
    p.updateTreeNode(nodeId, { name: payload.name, count: payload.count })
    await p.reloadItems()
  }

  const handleDeleteNode = async (nodeId: number) => {
    await itemsApi.deleteBranch(nodeId)
    await p.reloadItems()
    await p.loadTree()
  }

  return (
    <section className="page">
      <h2>Дерево</h2>
      <div className="form-row">
        <select value={p.rootId} onChange={(e) => p.setRootId(e.target.value)}>
          {p.items.map((item) => (
            <option key={item.id} value={item.id}>
              {item.name}
            </option>
          ))}
        </select>
        <button type="button" onClick={() => void p.loadTree()} disabled={p.loading}>
          Показать
        </button>
      </div>

      {p.tree && (
        <div className="tree-root">
          <TreeBranch
            node={p.tree}
            onAddChild={handleAddChild}
            onEditNode={handleEditNode}
            onDeleteNode={handleDeleteNode}
            getItemDraft={(nodeId) => {
              const item = p.items.find((x) => x.id === nodeId)
              return { name: item?.name ?? p.tree?.name ?? '', description: item?.description ?? '' }
            }}
          />
        </div>
      )}
    </section>
  )
}
