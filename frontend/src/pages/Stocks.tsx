import { useStocksPage } from '../hooks/useStocksPage'
import './Pages.css'

export function Stocks() {
  const p = useStocksPage()
  const editing = p.editingId != null

  return (
    <section className="page">
      <h2>Запасы</h2>

      <div className="stock-snapshot-block">
        <h3 className="stock-snapshot-title">Остатки на дату</h3>
        <div className="form-row stock-snapshot-row">
          <input
            type="date"
            value={p.asOfDate}
            onChange={(e) => p.setAsOfDate(e.target.value)}
            disabled={p.snapshotLoading}
          />
          <button
            type="button"
            disabled={p.snapshotLoading}
            onClick={() => void p.onCalculateSnapshot()}
          >
            {p.snapshotLoading ? 'Расчёт…' : 'Рассчитать'}
          </button>
        </div>
        {p.snapshotError != null && (
          <p className="stock-snapshot-error">{p.snapshotError}</p>
        )}
        {p.snapshotRows != null && (
          <table className="data-table stock-snapshot-table">
            <thead>
              <tr>
                <th>Изделие/Материал</th>
                <th>Количество на складе</th>
              </tr>
            </thead>
            <tbody>
              {p.snapshotRows.length === 0 ? (
                <tr>
                  <td colSpan={2}>Нет данных на выбранную дату</td>
                </tr>
              ) : (
                p.snapshotRows.map((row) => (
                  <tr key={row.itemId}>
                    <td>{row.name}</td>
                    <td>{row.count}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        )}
      </div>

      <h3 className="stock-section-title">Операции с запасами</h3>
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
              <th>Изделие/Материал</th>
              <th>Количество</th>
              <th>Операция</th>
              <th>Дата и время</th>
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
