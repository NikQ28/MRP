import { useState, useEffect } from 'react';
import { itemsApi } from '../api/items';
import type { Item, ItemCreate } from '../types';
import './Pages.css';

export function Items() {
  const [items, setItems] = useState<Item[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState<ItemCreate>({ name: '', description: '' });
  const [editingId, setEditingId] = useState<number | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await itemsApi.getAll();
      setItems(data);
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
    if (!form.name.trim()) return;
    setSubmitting(true);
    setError(null);
    try {
      await itemsApi.create({ name: form.name.trim(), description: form.description || null });
      setForm({ name: '', description: '' });
      await load();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка создания');
    } finally {
      setSubmitting(false);
    }
  };

  const handleUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (editingId == null || !form.name.trim()) return;
    setSubmitting(true);
    setError(null);
    try {
      await itemsApi.update(editingId, { name: form.name.trim(), description: form.description || null });
      setEditingId(null);
      setForm({ name: '', description: '' });
      await load();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка обновления');
    } finally {
      setSubmitting(false);
    }
  };

  const startEdit = (item: Item) => {
    setEditingId(item.id);
    setForm({ name: item.name, description: item.description ?? '' });
  };

  const cancelEdit = () => {
    setEditingId(null);
    setForm({ name: '', description: '' });
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Удалить позицию?')) return;
    setError(null);
    try {
      await itemsApi.delete(id);
      if (editingId === id) cancelEdit();
      await load();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Ошибка удаления');
    }
  };

  return (
    <section className="page">
      <h2>Позиции (Items)</h2>
      {error && <div className="error">{error}</div>}

      <form
        className="form-row"
        onSubmit={editingId != null ? handleUpdate : handleCreate}
      >
        <input
          placeholder="Название"
          value={form.name}
          onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))}
          required
        />
        <input
          placeholder="Описание"
          value={form.description ?? ''}
          onChange={(e) => setForm((f) => ({ ...f, description: e.target.value }))}
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
              <th>Название</th>
              <th>Описание</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.name}</td>
                <td>{item.description ?? '—'}</td>
                <td>
                  <button type="button" className="btn-sm" onClick={() => startEdit(item)}>Изменить</button>
                  {' '}
                  <button type="button" className="btn-sm danger" onClick={() => handleDelete(item.id)}>Удалить</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
}
