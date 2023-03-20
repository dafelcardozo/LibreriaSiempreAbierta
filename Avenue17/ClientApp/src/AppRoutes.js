import { Counter } from "./components/Counter";
import { Authors } from "./components/Authors";
import Editorials from "./components/Editorial";
import BooksSearch from './components/BooksSearch'
import { Home } from "./components/Home";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/authors',
        element: <Authors />
    },
    {
        path: '/editorials',
        element: <Editorials />
    },
    {
        path: '/search',
        element: <BooksSearch />
    }
];

export default AppRoutes;
