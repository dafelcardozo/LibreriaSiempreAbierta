import React, { useState, useEffect } from 'react';
import axios from 'axios';

const postAuthor = async (author) => {
    const response = await axios.post('api/authors', author);
    return response.data;
}

function CreateAuthor({ onPostAuthor}) {
    const [name, setName] = useState("");
    const [lastName, setLastName] = useState("");
    
    return (<form onSubmit={async (e) => {
        e.preventDefault();
        const author = { name, lastName, books:[] };
        await postAuthor(author);
        onPostAuthor(author);

    }}>
        <input type="text" value={name} onChange={({ target }) => setName(target.value)} />
        <input type="text" value={lastName} onChange={({ target }) => setLastName(target.value)} />
        <button type="submit" >Crear</button>
        <button type="button">Cancelar</button>
    </form>)
}

export function FetchData(props) {
    
    const [authors, setAuthors] = useState([]);
    const [loading, setLoading] = useState(true);
    const [createAuthorVisible, setCreateAuthorVisible] = useState(true);

    useEffect(() => {
        populateAuthors();
    }, [loading]);

    const populateAuthors = async () => {
        const response = await fetch('api/authors');
        const authors = await response.json();
        setAuthors(authors);
        setLoading(false);
    }

    const renderForecastsTable = () => (
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
                {authors.map(({ id, name, lastName, books }) =>
                    <tr key={id}>
                        <td>{id}</td>
                        <td>{name}</td>
                        <td>{lastName}</td>
                        <td>
                            {books?.map(({ title }) => title).join(", ")}
                        </td>
                    </tr>
                )}
            </tbody>
        </table>
    );
    let contents = loading? <p><em>Loading...</em></p>: renderForecastsTable();

    return <div>
                <h1 id="tabelLabel" >Authors</h1>
        {contents}
        <div>
            {createAuthorVisible && <CreateAuthor onPostAuthor={() => {
                populateAuthors();
            }} />}
        </div>
            </div>
}
