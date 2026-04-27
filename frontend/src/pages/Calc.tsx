import { useCalcPage } from '../hooks/useCalcPage'
import './Pages.css'

export function Calc() {
  const p = useCalcPage()

  return (
    <section className="page">
        <h2>Расчёты</h2>
        <div className="form-row">
          <input
            type="date"
            value={p.date}
            onChange={(e) => p.setDate(e.target.value)}
          />
          <button
            type="button"
            onClick={() => void p.execute()}
            disabled={p.loading || !p.date}
          >
            Выполнить расчёт
          </button>
        </div>

      {!p.loading && (
        <table className="data-table">
          <thead>
            <tr>
              <th>Материал</th>
              <th>Требуется для заказов</th>
              <th>Остаток на складе</th>
              <th>Нужно произвести/приобрести</th>
            </tr>
          </thead>
          <tbody>
            {p.list
            .filter(row => !(row.needCount === 0 && row.requiredCount === 0 && row.stockCount === 0))
            .map((row) => (
              <tr key={row.itemId}>
                <td>{p.itemLabel(row.itemId)}</td>
                <td>{row.requiredCount}</td>
                <td>{row.stockCount}</td>
                <td>{row.needCount}</td>
              </tr>
            ))
            }
          </tbody>
        </table>
      )}
    </section>
  )
}