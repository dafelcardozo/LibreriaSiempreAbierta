import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'
import '@fortawesome/fontawesome-svg-core/styles.css'
import { MDBBtn, MDBInput, MDBModal, MDBModalDialog, MDBModalContent, MDBModalTitle, MDBModalHeader, MDBModalBody } from 'mdb-react-ui-kit';

const fetchAuthors = async () => (await axios.get('api/authors')).data;
const postAuthor = async (author) => (await axios.post('api/authors', author)).data;
const deleteAuthor = async (authorId) => (await axios.delete(`api/authors/${authorId}`).data);

function CreateAuthorForm({ onPostAuthor}) {
    const [name, setName] = useState("");
    const [lastName, setLastName] = useState("");
    
    return (<form onSubmit={async (e) => {
        e.preventDefault();
        const author = { name, lastName, books:[] };
        await postAuthor(author);
        onPostAuthor(author);

    }}>
        <MDBInput type="text" name='name' required label="Please enter the author's name" value={name} onChange={({ target }) => setName(target.value)} />
        <MDBInput type="text" name='lastName' required label="Please enter the author's last name" value={lastName} onChange={({ target }) => setLastName(target.value)} />
        <MDBBtn type="submit" className='btn-block'>Create</MDBBtn>
    </form>)
}

export function Authors(props) {
    const [authors, setAuthors] = useState([]);
    const [loading, setLoading] = useState(true);
    const [createAuthorVisible, setCreateAuthorVisible] = useState(false);

    useEffect(() => {
        populateAuthors();
    }, [loading]);

    const populateAuthors = async () => {
        const data = await fetchAuthors();
        setAuthors(data);
        setLoading(false);
    }

    return <>
        <h1 id="tabelLabel" >Authors</h1>
        {loading ? <p><em>Loading...</em></p> : (
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
                            <td><MDBBtn onClick={async () => {
                                await deleteAuthor(id);
                                setTimeout(populateAuthors, 100);

                            }}><FontAwesomeIcon icon={faMinus} /></MDBBtn></td>
                        </tr>
                    )}
                </tbody>
            </table>
        )}
        <MDBModal show={createAuthorVisible} setShow={setCreateAuthorVisible} >
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Please enter the author's data below</MDBModalTitle>
                        <MDBBtn className="btn-close" color="none" aria-label="Close" onClick={() => setCreateAuthorVisible(false)} />
                    </MDBModalHeader>
                    <MDBModalBody>
                        <CreateAuthorForm onPostAuthor={async (author) => {
                            await populateAuthors();
                            setCreateAuthorVisible(false);
                        }} />
                    </MDBModalBody>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>
            </>
}
