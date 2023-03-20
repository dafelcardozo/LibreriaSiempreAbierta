import React, { useState, useEffect } from 'react';
import axios from 'axios';

const postBook = async (book) => {
    const response = await axios.post('api/books', book);
    return response.data;
}

const fetchEditorials = async () => (await axios.get("api/editorials")).data;

function CreateBook({ author, onPostBook }) {
    const [title, setTitle] = useState("");
    const [synopsis, setSynopsis] = useState("");
    const [nPages, setNPages] = useState("");
    const [editorial, setEditorial] = useState(0);
    const [editorials, setEditorials] = useState([]);
    const populateEditorials = async () => {
        const data = await fetchEditorials();
        setEditorials(data);
    };

    useEffect(() => {
        populateEditorials();
    }, [true]);

    return <form onSubmit={async (e) => {
        e.preventDefault();
        const book = { title, synopsis, author, nPages };
        await postBook(book);
        onPostBook(book);
    }}>
        <input type="text" name='title' value={title} onChange={({ target }) => setTitle(target.value)} />
        <input type="text" name='synopsis' value={synopsis} onChange={({ target }) => setSynopsis(target.value)} />
        <input type="text" name='npages' value={nPages} onChange={({ target }) => setNPages(target.value)} />
        <select  value={editorial} onChange={({ target }) => setEditorial(target.value)}>
            {editorials.map(({ id, name, location }) => <option value={ id} key={ id}>{name} - { location }</option>)}
        </select>
        <button type="submit" >Agregar</button>
        <button type="button">Cancelar</button>
    </form>
}

export default function BooksSearch({ }) {
    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [createBookVisible, setCreateBookVisible] = useState(true);

    useEffect(() => {
        populateBooks();
    }, [loading]);

    const populateBooks = async () => {
        const response = await fetch('api/books');
        const books = await response.json();
        setBooks(books);
        setLoading(false);
    }

    return <div>
        <h1 id="tabelLabel" >Search by book title, authors, editorial</h1>
        {loading ? <p><em>Loading...</em></p> : (
            <>
                <input type="text" value={search} onChange={({ target }) => setSearch(target.value)} />
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Title</th>
                        <th>Synopsis</th>
                        <th>Authors</th>
                        <th>Editorials</th>
                    </tr>
                </thead>
                <tbody>
                    {books.map(({ isbn, title, synopsis, npages, authors, editorial }) =>
                        <tr key={isbn}>
                            <td>{title}</td>
                            <td>{synopsis}</td>
                            <td>{npages}</td>
                            <td></td>
                        </tr>
                    )}
                </tbody>
                </table>
            </>)}
        <div>
            {createBookVisible && <CreateBook onPostBook={() => {
                populateBooks();

            }} />}
        </div>
    </div>
}
