import { useOrdersPage } from '../hooks/useOrdersPage'
import './Pages.css'

export function Orders() {
  const p = useOrdersPage()
  const editing = p.editingOrderId != null

  return (
    <section className="page">
      <h2>Заказы</h2>
      <form onSubmit={p.onSubmit}>
        <div className="form-row">
          <input
            type="datetime-local"
            value={p.form.creation}
            onChange={(e) =>
              p.setForm((prev) => ({ ...prev, creation: e.target.value }))
            }
          />
          <input
            type="datetime-local"
            value={p.form.execution}
            onChange={(e) =>
              p.setForm((prev) => ({ ...prev, execution: e.target.value }))
            }
          />
          <select
            value={p.form.status}
            onChange={(e) =>
              p.setForm((prev) => ({
                ...prev,
                status: Number(e.target.value) as 0 | 1,
              }))
            }
          >
            <option value={0}>Открыт</option>
            <option value={1}>Закрыт</option>
          </select>
          <div className="actions-right">
            <button type="button" onClick={p.addLine} className="btn-sm">
              Добавить строку
            </button>
            <button type="submit" disabled={p.submitting}>
              {editing ? 'Сохранить' : 'Добавить'}
            </button>
            {editing && (
              <button type="button" onClick={p.resetForm}>
                Отмена
              </button>
            )}
          </div>
        </div>

        {p.form.orderStrings.map((line, index) => (
          <div className="form-row" key={`${line.itemId}-${index}`}>
            <select
              value={line.itemId}
              onChange={(e) =>
                p.updateLine(index, {
                  ...line,
                  itemId: Number(e.target.value),
                })
              }
            >
              {p.allowedItems.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>
            <input
              type="number"
              min={1}
              value={line.count}
              onChange={(e) =>
                p.updateLine(index, {
                  ...line,
                  count: e.target.value,
                })
              }
            />
            <div className="actions-right">
              <button type="button" className="btn-sm danger" onClick={() => p.removeLine(index)}>
                Удалить строку
              </button>
            </div>
          </div>
        ))}
      </form>

      {!p.loading && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Дата создания</th>
              <th>Дата исполнения</th>
              <th>Статус</th>
              <th>Состав заказа</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {p.orders.map((order) => (
              <tr key={order.orderId}>
                <td>{new Date(order.creation).toLocaleString()}</td>
                <td>{new Date(order.execution).toLocaleString()}</td>
                <td>{order.status === 0 ? 'Открыт' : 'Закрыт'}</td>
                <td>
                  {order.orderStrings
                    .map((row) => `${p.itemLabel(row.itemId)} x ${row.count}`)
                    .join(', ')}
                </td>
                <td className="buttons">
                  <button type="button" className="btn-sm" onClick={() => p.startEdit(order)}>
                    Изменить
                  </button>
                  <button type="button" className="btn-sm danger" onClick={() => void p.deleteOrder(order.orderId)}>
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
