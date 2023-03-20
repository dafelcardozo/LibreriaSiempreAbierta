import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { MDBRow, MDBCol, MDBBtn, MDBInput, MDBTextArea, MDBModalFooter, MDBModal, MDBModalDialog, MDBModalContent, MDBModalTitle, MDBModalHeader, MDBModalBody } from 'mdb-react-ui-kit';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'
import '@fortawesome/fontawesome-svg-core/styles.css'
import { MultiSelect } from "react-multi-select-component";
import Select from 'react-select'

const postBook = async (book) => (await axios.post('api/books', {...book, book})).data;
const fetchEditorials = async () => (await axios.get("api/editorials")).data;
const fetchAllAuthors = async () => (await axios.get("api/authors")).data;

function AuthorsList({ authors, onListUpdated }) {
    const [selectedAuthors, setSelectedAuthors] = useState([]);
    const [allAuthors, setAllAuthors] = useState(authors || []);
    
    const populateAuthors = async () => {
        const data = await fetchAllAuthors();
        setAllAuthors(data);
        onListUpdated(selectedAuthors);
    }

    useEffect(() => {
        populateAuthors()
    }, [true]);
    const removeAuthor = (id) => setSelectedAuthors(selectedAuthors.filter(({ value }) => id != value));
    const options = allAuthors.map(({ id, name, lastName }) => ({ value: id, label: `${name} ${lastName}`}) );
    return <>
        <ul>{selectedAuthors.map(({ label, value }) => (<li key={value}>{label} <MDBBtn type='button' color='secondary' onClick={() => removeAuthor(value)}><FontAwesomeIcon icon={faMinus} /></MDBBtn></li>))}</ul>
        <MultiSelect options={options} value={selectedAuthors} onChange={setSelectedAuthors} labelledBy="Select" />
    </>
}

function CreateBookForm({ author, onPostBook }) {
    const [isbn, setIsbn] = useState("0");
    const [Title, setTitle] = useState("");
    const [Synopsis, setSynopsis] = useState("");
    const [NPages, setNPages] = useState("");
    const [editorial, setEditorial] = useState(0);
    const [editorials, setEditorials] = useState([]);
    const [Authors, setAuthors] = useState([]);
    const populateEditorials = async () => {
        const data = await fetchEditorials();
        setEditorials(data);
    };

    useEffect(() => {
        populateEditorials();
    }, [true]);

    return <form onSubmit={async (e) => {
        e.preventDefault();
        const book = {
            isbn: isbn, Title, Synopsis, Authors, NPages, editorial: { id: editorial }
        };
        await postBook(book);
        onPostBook(book);
    }}>
        <MDBInput type="number" name='ISBN' label="Please enter the book's ISBN" required value={isbn} onChange={({ target }) => setIsbn(target.value)} />
        <MDBInput type="text" name='title' label="Please enter the book's title" required value={Title} onChange={({ target }) => setTitle(target.value)} />
        <MDBTextArea type="text" name='synopsis' rows={4} required label="Please enter a synopsis" value={Synopsis} onChange={({ target }) => setSynopsis(target.value)} />
        <MDBInput type="text" name='npages' label="Enter a number of pages" required value={NPages} onChange={({ target }) => setNPages(target.value)} />
        <Select options={editorials.map(({ id, name, location }) => ({ value: id, label: name }))} onChange={({ value, label}) => setEditorial(value)} />
        <AuthorsList authors={Authors} onListUpdated={(authors) => setAuthors(authors)}></AuthorsList>
        <MDBRow>
            <MDBCol><MDBBtn type="submit" className='btn-block'>Agregar</MDBBtn></MDBCol>
        </MDBRow>
    </form>
}

export default function BooksSearch({ }) {
    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const [createBookVisible, setCreateBookVisible] = useState(false);

    useEffect(() => {
        populateBooks();
    }, [loading]);

    const populateBooks = async () => {
        const response = await fetch('api/books');
        const books = await response.json();
        setBooks(books);
        setLoading(false);
    }

    return <>
        <h1 id="tabelLabel" >Search by book title, authors, editorial</h1>
        {loading ? <p><em>Loading...</em></p> : (
            <>
                <MDBInput type="text" value={search} label="Search by book title, author's name, etc" onChange={({ target }) => setSearch(target.value)} className='btn-block'/>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Title</th>
                            <th>Synopsis</th>
                            <th>Authors</th>
                            <th>Editorials</th>
                            <th>
                                <MDBBtn onClick={() => setCreateBookVisible(true)}>
                                    <FontAwesomeIcon icon={faPlus} />
                                </MDBBtn>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {books.map(({ isbn, title, synopsis, npages, authors, editorial }) =>
                            <tr key={isbn}>
                                <td>{title}</td>
                                <td>{synopsis}</td>
                                <td>{npages}</td>
                                <td>{authors.map(({ id, name, lastName }) => `${name} ${lastName}`).join(', ')}</td>
                                <td>{editorial}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </>)}

        <MDBModal show={createBookVisible} setShow={setCreateBookVisible} >
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Please enter the book's data below</MDBModalTitle><MDBBtn className="btn-close" color="none" aria-label="Close" onClick={() => setCreateBookVisible(false)} />
                    </MDBModalHeader>
                    <MDBModalBody>
                        <CreateBookForm onPostBook={populateBooks} />
                    </MDBModalBody>
                    <MDBModalFooter>
                        <MDBBtn>Save changes</MDBBtn>
                    </MDBModalFooter>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>

    </>
}
