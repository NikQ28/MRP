import { useState, useEffect } from 'react';
import { watchBomsApi } from '../api/watchBoms';
import type { WatchBom, WatchBomCreate } from '../types';
import './Pages.css';

export function WatchBoms() {
  const [list, setList] = useState<WatchBom[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState<WatchBomCreate>({ parentId: 0, childId: 0, count: 1 });
  const [editingId, setEditingId] = useState<number | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await watchBomsApi.getAll();
      setList(data);
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка загрузки');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    setError(null);
    try {
      await watchBomsApi.create(form);
      setForm({ parentId: 0, childId: 0, count: 1 });
      await load();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка создания');
    } finally {
      setSubmitting(false);
    }
  };

  const handleUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (editingId == null) return;
    setSubmitting(true);
    setError(null);
    try {
      await watchBomsApi.update(editingId, form);
      setEditingId(null);
      setForm({ parentId: 0, childId: 0, count: 1 });
      await load();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка обновления');
    } finally {
      setSubmitting(false);
    }
  };

  const startEdit = (row: WatchBom) => {
    setEditingId(row.id);
    setForm({ parentId: row.parentId, childId: row.childId, count: row.count });
  };

  const cancelEdit = () => {
    setEditingId(null);
    setForm({ parentId: 0, childId: 0, count: 1 });
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Удалить связь?')) return;
    setError(null);
    try {
      await watchBomsApi.delete(id);
      if (editingId === id) cancelEdit();
      await load();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка удаления');
    }
  };

  return (
    <section className="page">
      <h2>Спецификации (Watch BOM)</h2>
      <p className="muted">Родитель → Дочерняя позиция, количество.</p>
      {error && <div className="error">{error}</div>}

      <form
        className="form-row"
        onSubmit={editingId != null ? handleUpdate : handleCreate}
      >
        <input
          type="number"
          placeholder="Parent ID"
          value={form.parentId || ''}
          onChange={(e) => setForm((f) => ({ ...f, parentId: Number(e.target.value) || 0 }))}
          min={0}
        />
        <input
          type="number"
          placeholder="Child ID"
          value={form.childId || ''}
          onChange={(e) => setForm((f) => ({ ...f, childId: Number(e.target.value) || 0 }))}
          min={0}
        />
        <input
          type="number"
          placeholder="Кол-во"
          value={form.count}
          onChange={(e) => setForm((f) => ({ ...f, count: Number(e.target.value) || 0 }))}
          min={1}
        />
        <div className="form-actions">
          {editingId != null ? (
            <>
              <button type="submit" disabled={submitting}>Сохранить</button>
              <button type="button" onClick={cancelEdit}>Отмена</button>
            </>
          ) : (
            <button type="submit" disabled={submitting}>Добавить</button>
          )}
        </div>
      </form>

      {loading ? (
        <p className="muted">Загрузка…</p>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Parent ID</th>
              <th>Child ID</th>
              <th>Кол-во</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {list.map((row) => (
              <tr key={row.id}>
                <td>{row.id}</td>
                <td>{row.parentId}</td>
                <td>{row.childId}</td>
                <td>{row.count}</td>
                <td>
                  <button type="button" className="btn-sm" onClick={() => startEdit(row)}>Изменить</button>
                  {' '}
                  <button type="button" className="btn-sm danger" onClick={() => handleDelete(row.id)}>Удалить</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
