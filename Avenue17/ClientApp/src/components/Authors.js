import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'
import '@fortawesome/fontawesome-svg-core/styles.css'
import { MDBRow, MDBCard, MDBCardBody, MDBCardHeader, MDBCardTitle, MDBBtn, MDBInput, MDBModal, MDBModalDialog, MDBModalContent, MDBModalTitle, MDBModalHeader, MDBModalBody } from 'mdb-react-ui-kit';

const searchAuthors = async (search) => (await axios.get(`api/authors?search=${search}`)).data;
const postAuthor = async (author) => (await axios.post('api/authors', author)).data;
const deleteAuthor = async (authorId) => (await axios.delete(`api/authors/${authorId}`).data);

function RegisterAuthorForm({ onPostAuthor }) {
    const [name, setName] = useState("");
    const [lastName, setLastName] = useState("");

    return (<form onSubmit={async (e) => {
        e.preventDefault();
        const author = { name, lastName, books: [] };
        await postAuthor(author);
        onPostAuthor(author);

    }}>
        <MDBInput type="text" wrapperClass='mb-4' name='name' required label="Please enter the author's name" value={name} onChange={({ target }) => setName(target.value)} />
        <MDBInput type="text" wrapperClass='mb-4' name='lastName' required label="Please enter the author's last name" value={lastName} onChange={({ target }) => setLastName(target.value)} />
        <MDBBtn type="submit" block>Create</MDBBtn>
    </form>)
}

export function Authors() {
    const [authors, setAuthors] = useState([]);
    const [loading, setLoading] = useState(true);
    const [createAuthorVisible, setCreateAuthorVisible] = useState(false);
    const [search, setSearch] = useState("");

    useEffect(() => {
        populateAuthors();
    }, [search]);

    const populateAuthors = async () => {
        const data = await searchAuthors(search);
        setAuthors(data);
        setLoading(false);
    }
    return loading ? <p><em>Loading...</em></p> : <>
        <MDBCard>
            <MDBCardHeader><MDBCardTitle>Search and register authors</MDBCardTitle></MDBCardHeader>
            <MDBCardBody>
                <MDBRow>
                    <MDBInput type="text" value={search} label="Search by author name or last name..." onChange={({ target }) => setSearch(target.value)} />
                </MDBRow>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Name</th>
                            <th>Last Name</th>
                            <th>Books</th>
                            <th><MDBBtn onClick={() => setCreateAuthorVisible(true)}><FontAwesomeIcon icon={faPlus} /></MDBBtn></th>
                        </tr>
                    </thead>
                    <tbody>
                        {authors?.map(({ id, name, lastName, books }) =>
                            <tr key={id}>
                                <td>{id}</td>
                                <td>{name}</td>
                                <td>{lastName}</td>
                                <td>
                                    {books?.map(({ title }) => title).join(", ")}
                                </td>
                                <td>
                                    <MDBBtn onClick={async () => {
                                        await deleteAuthor(id);
                                        setTimeout(populateAuthors, 100);
                                    }}><FontAwesomeIcon icon={faMinus} /></MDBBtn></td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </MDBCardBody>
        </MDBCard>
        <MDBModal show={createAuthorVisible} setShow={setCreateAuthorVisible} >
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Please enter the author's data below</MDBModalTitle>
                        <MDBBtn className="btn-close" color="none" aria-label="Close" onClick={() => setCreateAuthorVisible(false)} />
                    </MDBModalHeader>
                    <MDBModalBody>
                        <RegisterAuthorForm onPostAuthor={async (author) => {
                            await populateAuthors();
                            setCreateAuthorVisible(false);
                        }} />
                    </MDBModalBody>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>
    </>
}
