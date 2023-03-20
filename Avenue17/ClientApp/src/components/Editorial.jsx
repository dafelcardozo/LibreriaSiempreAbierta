import React, { useState, useEffect } from 'react';
import axios from 'axios';

const postEditorial = async (editorial) => (await axios.post('api/editorials', editorial)).data;

function CreateEditorial({ onPostEditorial }) {
    const [name, setName] = useState("");
    const [location, setLocation] = useState("");

    return (<form onSubmit={async (e) => {
        e.preventDefault();
        const editorial = { name, location, books: [] };
        await postEditorial(editorial);
        onPostEditorial(editorial);

    }}>
        <input type="text" value={name} onChange={({ target }) => setName(target.value)} />
        <input type="text" value={location} onChange={({ target }) => setLocation(target.value)} />
        <button type="submit" >Crear</button>
        <button type="button">Cancelar</button>
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
        const response = await fetch('api/editorials');
        const editorials = await response.json();
        setEditorials(editorials);
        setLoading(false);
    }


    return <div>
        <h1 id="tabelLabel" >Editorials</h1>
        {loading ? <p><em>Loading...</em></p> : (
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
                    {editorials.map(({ id, name, location}) =>
                        <tr key={id}>
                            <td>{id}</td>
                            <td>{name}</td>
                            <td>{location}</td>
                            
                        </tr>
                    )}
                </tbody>
            </table>)}
        <div>
            <button onClick={() => setCreateEditorialVisible(true)}>Add</button>
            {createEditorialVisible && <CreateEditorial onPostEditorial={() => {
                populateEditorials();
            }} />}
        </div>
    </div>
}
