import React, { useState, useEffect } from 'react';
import axios from 'axios';

const postBook = async (book) => {
    const response = await axios.post('api/books', book);
    return response.data;
}

function CreateBook({ author, onPostBook }) {
    const [title, setTitle] = useState("");
    const [synopsis, setSynopsis] = useState("");
    const [nPages, setNPages] = useState("");

    return  <form onSubmit={async (e) => {
        e.preventDefault();
        const book = { title, synopsis, author, nPages};
        await postBook(book);
        onPostBook(book);
        setVisible(false);
    }}>
        <input type="text" value={title} onChange={({ target }) => setTitle(target.value)} />
        <input type="text" value={location} onChange={({ target }) => setSynopsis(target.value)} />
        <input type="text" value={nPages} onChange={({ target }) => setNPages(target.value)} />

        <button type="submit" >Agregar</button>
        <button type="button">Cancelar</button>
    </form>
}

export default function AuthorBooks({ author}) {

    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
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

    return loading ? <p><em>Loading...</em></p> : (
        <>
            <MDBCard>
                <MDBCardHeader><MDBCardTitle>All of the books we know of...</MDBCardTitle></MDBCardHeader>
                <MDBCardBody>
                    <MDBCardText >
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Last Name</th>
                        <th>Books</th>
                    </tr>
                </thead>
                <tbody>
                    {books.map(({ isbn, title, synopsis, npages}) =>
                        <tr key={isbn}>
                            <td>{title}</td>
                            <td>{synopsis}</td>
                            <td>{npages}</td>

                        </tr>
                    )}
                </tbody>
                        </table>
                    </MDBCardText>
                </MDBCardBody>
                        </MDBCard>

            {createBookVisible && <CreateBook onPostBook={populateBooks} />}
  
        </>
    )
        
}
