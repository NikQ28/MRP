import { useStocksPage } from '../hooks/useStocksPage'
import './Pages.css'

export function Stocks() {
  const p = useStocksPage()
  const editing = p.editingId != null

  return (
    <section className="page">
      <h2>Запасы</h2>
      <form className="form-row" onSubmit={p.onSubmit}>
        <select
          value={p.form.itemId}
          onChange={(e) =>
            p.setForm((f) => ({ ...f, itemId: Number(e.target.value) || 0 }))
          }
        >
          {p.items.map((item) => (
            <option key={item.id} value={item.id}>
              {item.name}
            </option>
          ))}
        </select>
        <input
          type="number"
          min={1}
          value={p.form.count}
          onChange={(e) =>
            p.setForm((f) => ({ ...f, count: e.target.value }))
          }
        />
        <select
          value={p.form.operation}
          onChange={(e) =>
            p.setForm((f) => ({
              ...f,
              operation: Number(e.target.value) as 0 | 1,
            }))
          }
        >
          <option value={0}>Приход</option>
          <option value={1}>Расход</option>
        </select>
        <input
          type="datetime-local"
          value={p.form.datetime}
          onChange={(e) =>
            p.setForm((f) => ({ ...f, datetime: e.target.value }))
          }
        />
        <div className="actions-right">
          <button type="submit" disabled={p.submitting}>
            {editing ? 'Сохранить' : 'Добавить'}
          </button>
          {editing && (
            <button type="button" onClick={p.cancelEdit}>
              Отмена
            </button>
          )}
        </div>
      </form>

      {!p.loading && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Объект</th>
              <th>Кол-во</th>
              <th>Операция</th>
              <th>Дата/время</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {p.list.map((row) => (
              <tr key={row.id}>
                <td>{p.itemLabel(row.itemId)}</td>
                <td>{row.count}</td>
                <td>{row.operation === 0 ? 'Приход' : 'Расход'}</td>
                <td>{new Date(row.datetime).toLocaleString()}</td>
                <td className="buttons">
                  <button type="button" className="btn-sm" onClick={() => p.startEdit(row)}>
                    Изменить
                  </button>
                  <button type="button" className="btn-sm danger" onClick={() => void p.deleteRow(row.id)}>
                    Удалить
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  )
}
