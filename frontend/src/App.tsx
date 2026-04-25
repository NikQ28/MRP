import { useState } from 'react'
import { Tree } from './pages/Tree'
import { Stocks } from './pages/Stocks'
import { Orders } from './pages/Orders'
import './pages/Pages.css'

type Tab = 'tree' | 'stocks' | 'orders'

function App() {
  const [tab, setTab] = useState<Tab>('tree')

  return (
    <>
      <nav className="nav">
        <a href="#tree" className={tab === 'tree' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setTab('tree') }}>
          Спецификация
        </a>
        <a href="#stocks" className={tab === 'stocks' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setTab('stocks') }}>
          Запасы
        </a>
        <a href="#orders" className={tab === 'orders' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setTab('orders') }}>
          Заказы
        </a>
      </nav>

      <main style={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
        {tab === 'tree' && <Tree />}
        {tab === 'stocks' && <Stocks />}
        {tab === 'orders' && <Orders />}
      </main>
    </>
  )
}

export default App
