import { useCalcPage } from '../hooks/useCalcPage'
import './Pages.css'

export function Calc() {
  const p = useCalcPage()

  return (
    <section className="page">
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h2>Расчёты</h2>
        <div className="form-row">
          <div className="actions-right">
            <button type="button" onClick={() => void p.reload()}>
              Обновить
            </button>
          </div>
        </div>
      </div>

      {!p.loading && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Материал</th>
              <th>Требуется для заказов</th>
              <th>Остаток на складе</th>
              <th>Нужно произвести</th>
            </tr>
          </thead>
          <tbody>
            {p.list.map((row) => (
              <tr key={row.itemId}>
                <td>{p.itemLabel(row.itemId)}</td>
                <td>{row.requiredCount}</td>
                <td>{row.storedCount}</td>
                <td>{row.needToProduce}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  )
}