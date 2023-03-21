import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { MDBRow, MDBCol, MDBBtn, MDBInput, MDBTextArea, MDBModalFooter, MDBModal, MDBModalDialog, MDBModalContent, MDBModalTitle, MDBModalHeader, MDBModalBody } from 'mdb-react-ui-kit';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons'
import '@fortawesome/fontawesome-svg-core/styles.css'

const fetchEditorials = async () => (await axios.get('api/editorials')).data;
const postEditorial = async (editorial) => (await axios.post('api/editorials', editorial)).data;
const deleteEditorial = async (editorial) => (await axios.delete(`api/editorials/${editorial}`)).data;

function CreateEditorial({ onPostEditorial }) {
    const [name, setName] = useState("");
    const [location, setLocation] = useState("");

    return (<form onSubmit={async (e) => {
        e.preventDefault();
        const editorial = { name, location, books: [] };
        await postEditorial(editorial);
        onPostEditorial(editorial);
    }}>
        <MDBInput type="text" name='name' label="Please enter the editorial's name" value={name} onChange={({ target }) => setName(target.value)} />
        <MDBInput type="text" value={location} label="Please enter the editorial's location" onChange={({ target }) => setLocation(target.value)} />
        <MDBBtn type="submit" className='btn-block'>Crear</MDBBtn>
    </form>)
}

export default function Editorials(props) {
    const [editorials, setEditorials] = useState([]);
    const [loading, setLoading] = useState(true);
    const [createEditorialVisible, setCreateEditorialVisible] = useState(false);

    useEffect(() => {
        populateEditorials();
    }, [loading]);

    const populateEditorials = async () => {
        const editorials = await fetchEditorials();
        setEditorials(editorials);
        setLoading(false);
        setCreateEditorialVisible(false);
    }
	
    return <>
        <h1 id="tabelLabel" >Editorials</h1>
        {loading ? <p><em>Loading...</em></p> : (
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Location</th>
                    <th><MDBBtn onClick={() => setCreateEditorialVisible(true)}><FontAwesomeIcon icon={faPlus} /></MDBBtn></th>
                </tr>
            </thead>
            <tbody>
                {editorials.map(({ id, name, location}) =>
                    <tr key={id}>
                        <td>{id}</td>
                        <td>{name}</td>
                        <td>{location}</td>
                        <td><MDBBtn onClick={async () => {
                            await deleteEditorial(id);
                            await populateEditorials();
                        }}><FontAwesomeIcon icon={faMinus} /></MDBBtn> </td>
                    </tr>
                )}
            </tbody>
        </table>)}
            
        <MDBModal show={createEditorialVisible} setShow={setCreateEditorialVisible} >
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Please enter the book's data below</MDBModalTitle><MDBBtn className="btn-close" color="none" aria-label="Close" onClick={() => setCreateEditorialVisible(false)} />
                    </MDBModalHeader>
                    <MDBModalBody>
                        <CreateEditorial onPostEditorial={populateEditorials} />
                    </MDBModalBody>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>
    </>
}
