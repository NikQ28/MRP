import { useState } from 'react';
import { treeApi } from '../api/tree';
import type { ItemNode } from '../types';
import './Pages.css';

function TreeNode({ node, depth = 0 }: { node: ItemNode; depth?: number }) {
  return (
    <div className="tree-node" style={{ marginLeft: depth * 20 }}>
      <span className="tree-item">
        {node.name} (id: {node.id}, кол-во: {node.count})
      </span>
      {node.children?.length > 0 && (
        <div className="tree-children">
          {node.children.map((child) => (
            <TreeNode key={child.id} node={child} depth={depth + 1} />
          ))}
        </div>
      )}
    </div>
  );
}

export function Tree() {
  const [rootId, setRootId] = useState<string>('1');
  const [tree, setTree] = useState<ItemNode | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadTree = async () => {
    const id = Number(rootId);
    if (Number.isNaN(id) || id < 1) {
      setError('Введите корректный ID корня (целое число ≥ 1)');
      return;
    }
    setLoading(true);
    setError(null);
    setTree(null);
    try {
      const data = await treeApi.getTree(id);
      setTree(data);
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка загрузки дерева');
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="page">
      <h2>Дерево состава</h2>
      <p className="muted">Введите ID корневой позиции и нажмите «Загрузить».</p>
      {error && <div className="error">{error}</div>}

      <div className="form-row">
        <input
          type="number"
          placeholder="ID корня"
          value={rootId}
          onChange={(e) => setRootId(e.target.value)}
          min={1}
        />
        <button type="button" onClick={loadTree} disabled={loading}>
          {loading ? 'Загрузка…' : 'Загрузить дерево'}
        </button>
      </div>

      {tree && (
        <div className="tree-root">
          <TreeNode node={tree} />
        </div>
      )}
    </section>
  );
}
