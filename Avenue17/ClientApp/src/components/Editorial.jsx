import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { MDBRow, MDBCard, MDBCardBody, MDBCardHeader, MDBCardTitle, MDBBtn, MDBInput, MDBModal, MDBModalDialog, MDBModalContent, MDBModalTitle, MDBModalHeader, MDBModalBody } from 'mdb-react-ui-kit';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'
import '@fortawesome/fontawesome-svg-core/styles.css'

const fetchEditorials = async (search) => (await axios.get(`api/editorials?search=${search}`)).data;
const postEditorial = async (editorial) => (await axios.post('api/editorials', editorial)).data;
const deleteEditorial = async (editorial) => (await axios.delete(`api/editorials/${editorial}`)).data;

function CreateEditorialForm({ onPostEditorial }) {
    const [name, setName] = useState("");
    const [location, setLocation] = useState("");

    return (<form onSubmit={async (e) => {
        e.preventDefault();
        const editorial = { name, location, books: [] };
        await postEditorial(editorial);
        onPostEditorial(editorial);
    }}>
        <MDBInput type="text" wrapperClass='mb-4' name='name' label="Please enter the editorial's name" value={name} onChange={({ target }) => setName(target.value)} />
        <MDBInput type="text" wrapperClass='mb-4' value={location} label="Please enter the editorial's location" onChange={({ target }) => setLocation(target.value)} />
        <MDBBtn type="submit" block>Add</MDBBtn>
    </form>)
}

export default function Editorials(props) {
    const [editorials, setEditorials] = useState([]);
    const [loading, setLoading] = useState(true);
    const [createEditorialVisible, setCreateEditorialVisible] = useState(false);
    const [search, setSearch] = useState("");

    useEffect(() => {
        populateEditorials();
    }, [search]);

    const populateEditorials = async () => {
        const editorials = await fetchEditorials(search);
        setEditorials(editorials);
        setLoading(false);
        setCreateEditorialVisible(false);
    }

    return loading ? <p><em>Loading...</em></p> : (<>
        <MDBCard>
            <MDBCardHeader><MDBCardTitle>Editorials</MDBCardTitle></MDBCardHeader>
            <MDBCardBody>
                <MDBRow>
                    <MDBInput type="text" value={search} label="Search by publisher name or location" onChange={({ target }) => setSearch(target.value)} />
                </MDBRow>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Id</th>
                            <th>Name</th>
                            <th>Location</th>
                            <th>Books count</th>
                            <th><MDBBtn onClick={() => setCreateEditorialVisible(true)}><FontAwesomeIcon icon={faPlus} /></MDBBtn></th>
                        </tr>
                    </thead>
                    <tbody>
                        {editorials.map(({ id, name, location, nbooks }) =>
                            <tr key={id}>
                                <td>{id}</td>
                                <td>{name}</td>
                                <td>{location}</td>
                                <td>{nbooks}</td>
                                <td><MDBBtn onClick={async () => {
                                    await deleteEditorial(id);
                                    await populateEditorials();
                                }}><FontAwesomeIcon icon={faMinus} /></MDBBtn> </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </MDBCardBody>
        </MDBCard>
        <MDBModal show={createEditorialVisible} setShow={setCreateEditorialVisible} >
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Please enter the editorial's data below</MDBModalTitle><MDBBtn className="btn-close" color="none" aria-label="Close" onClick={() => setCreateEditorialVisible(false)} />
                    </MDBModalHeader>
                    <MDBModalBody>
                        <CreateEditorialForm onPostEditorial={populateEditorials} />
                    </MDBModalBody>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>
    </>
    )
}
