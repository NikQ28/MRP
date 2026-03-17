import { useState } from 'react';
import { Items } from './pages/Items';
import { WatchBoms } from './pages/WatchBoms';
import { Tree } from './pages/Tree';
import './pages/Pages.css';

type Tab = 'items' | 'watchboms' | 'tree';

function App() {
  const [tab, setTab] = useState<Tab>('items');

  return (
    <>
      <nav className="nav">
        <a
          href="#items"
          className={tab === 'items' ? 'active' : ''}
          onClick={(e) => { e.preventDefault(); setTab('items'); }}
        >
          Позиции
        </a>
        <a
          href="#watchboms"
          className={tab === 'watchboms' ? 'active' : ''}
          onClick={(e) => { e.preventDefault(); setTab('watchboms'); }}
        >
          Спецификации (BOM)
        </a>
        <a
          href="#tree"
          className={tab === 'tree' ? 'active' : ''}
          onClick={(e) => { e.preventDefault(); setTab('tree'); }}
        >
          Дерево состава
        </a>
      </nav>

      <main style={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
        {tab === 'items' && <Items />}
        {tab === 'watchboms' && <WatchBoms />}
        {tab === 'tree' && <Tree />}
      </main>
    </>
  );
}

export default App;
