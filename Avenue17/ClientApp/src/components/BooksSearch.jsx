import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { MDBCard, MDBCardBody, MDBCardText, MDBCardHeader, MDBCardTitle, MDBRow, MDBCol, MDBBtn, MDBInput, MDBTextArea, MDBModalFooter, MDBModal, MDBModalDialog, MDBModalContent, MDBModalTitle, MDBModalHeader, MDBModalBody } from 'mdb-react-ui-kit';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'
import '@fortawesome/fontawesome-svg-core/styles.css'
import { MultiSelect } from "react-multi-select-component";
import Select from 'react-select'

const postBook = async (book) => (await axios.post('api/books', { ...book })).data;
const deleteBook = async (id) => (await axios.delete(`api/books/${id}`)).data;
const fetchEditorials = async () => (await axios.get("api/editorials")).data;
const fetchAllAuthors = async () => (await axios.get("api/authors")).data;

function AuthorsList({ authors, onListUpdated }) {
    const [selectedAuthors, setSelectedAuthors] = useState([]);
    const [allAuthors, setAllAuthors] = useState(authors || []);
    const [loading, setLoading] = useState(true);

    const populateAuthors = async () => {
        const data = await fetchAllAuthors();
        setAllAuthors(data);
        setLoading(false);
    }
    useEffect(() => {
        populateAuthors()
    }, [loading]);

    const options = allAuthors.map(({ id, name, lastName }) => ({ value: id, label: `${name} ${lastName}` }));
    return <MultiSelect options={options} value={selectedAuthors} name="author"
        valueRenderer={(selected, _options) => (selected.length ? selected.map(({ label }) => label) : "Please select the book's authors")}
        onChange={(authors) => {
            setSelectedAuthors(authors)
            onListUpdated(authors);
        }} />

}

function RegisterBookForm({ onPostBook }) {
    const [isbn, setIsbn] = useState("");
    const [Title, setTitle] = useState("");
    const [Synopsis, setSynopsis] = useState("");
    const [nPages, setNPages] = useState("");
    const [editorial, setEditorial] = useState(0);
    const [editorials, setEditorials] = useState([]);
    const [authors, setAuthors] = useState([]);
    const [loading, setLoading] = useState(true);

    const populateEditorials = async () => {
        const data = await fetchEditorials();
        setEditorials(data);
        setLoading(false);
    };
    useEffect(() => {
        populateEditorials();
    }, [loading]);

    return <form onSubmit={async (e) => {
        e.preventDefault();
        const book = {
            isbn: isbn, Title, Synopsis, authors: authors.map(({ value }) => value), nPages, editorial: editorial,
        };
        await postBook(book);
        onPostBook(book);
    }}>
        <MDBInput type="number" wrapperClass='mb-4' name='ISBN' label="Please enter the book's ISBN" required value={isbn} onChange={({ target }) => setIsbn(target.value)} />
        <MDBInput type="text" wrapperClass='mb-4' name='title' label="Please enter the book's title" required value={Title} onChange={({ target }) => setTitle(target.value)} />
        <MDBTextArea type="text" wrapperClass='mb-4' name='synopsis' rows={4} required label="Please enter a synopsis" value={Synopsis} onChange={({ target }) => setSynopsis(target.value)} />
        <MDBInput type="text" wrapperClass='mb-4' name='npages' label="Please enter a number of pages" required value={nPages} onChange={({ target }) => setNPages(target.value)} />
        <Select wrapperClass='mb-4' options={editorials.map(({ id, name, location }) => ({ value: id, label: `${name} at ${location}` }))}
            onChange={({ value }) => setEditorial(value)} placeholder="Select an editorial..." />
        <br />
        <MDBRow className='mb-4'>
            <AuthorsList authors={authors} onListUpdated={(authors) => setAuthors(authors)}></AuthorsList>
        </MDBRow>
        <MDBBtn type="submit" block>Add</MDBBtn>
    </form>
}

export default function BooksSearch() {
    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [createBookVisible, setCreateBookVisible] = useState(false);

    useEffect(() => {
        populateBooks();
    }, [search]);

    const populateBooks = async () => {
        let books;
        if (search.trim()) {
            const response = await fetch(`api/search?search=${search.trim()}&pageSize=50`);
            books = await response.json();
        } else {
            const response = await fetch('api/books?pageSize=50');
            books = await response.json();
        }
        setBooks(books);
        setLoading(false);
    }

    return loading ? <p>< em > Loading...</em ></p > : (
        <>
            <MDBCard>
                <MDBCardHeader><MDBCardTitle>Search a book by it's title, author or editorial name, ISBN, synopsis text</MDBCardTitle></MDBCardHeader>
                <MDBCardBody>
                    <MDBRow>
                        <MDBInput type="text" value={search} label="Please enter your search string..." onChange={({ target }) => setSearch(target.value)} />
                    </MDBRow>
                    <table className='table table-striped' aria-labelledby="tabelLabel">
                        <thead>
                            <tr>
                                <th>Id</th>
                                <th>Title</th>
                                <th>Synopsis</th>
                                <th>Number of pages</th>
                                <th>Authors</th>
                                <th>Editorial</th>
                                <th>
                                    <MDBBtn onClick={() => setCreateBookVisible(true)}>
                                        <FontAwesomeIcon icon={faPlus} />
                                    </MDBBtn>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {books.map(({ isbn, title, synopsis, nPages, authors, editorial }) =>
                                <tr key={isbn}>
                                    <td>{isbn}</td>
                                    <td>{title}</td>
                                    <td>{synopsis}</td>
                                    <td>{nPages}</td>
                                    <td>{authors?.map(({ name, lastName }) => `${name} ${lastName}`).join(', ')}</td>
                                    <td>{editorial.name} ({editorial.location})</td>
                                    <td><MDBBtn onClick={async () => {
                                        await deleteBook(isbn);
                                        setTimeout(populateBooks, 100);
                                    }}><FontAwesomeIcon icon={faMinus} /></MDBBtn></td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </MDBCardBody>
            </MDBCard>
            <MDBModal show={createBookVisible} setShow={setCreateBookVisible} staticBackdrop  >
                <MDBModalDialog data-mdb-backdrop="static">
                    <MDBModalContent>
                        <MDBModalHeader>
                            <MDBModalTitle>Please enter the book's data below</MDBModalTitle>
                            <MDBBtn className="btn-close" color="none" aria-label="Close" onClick={() => setCreateBookVisible(false)} />
                        </MDBModalHeader>
                        <MDBModalBody>
                            <RegisterBookForm onPostBook={async () => {
                                await populateBooks();
                                setCreateBookVisible(false);
                            }} />
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
        </>
    );

}
