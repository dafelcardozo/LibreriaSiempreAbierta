import React from 'react';
import { MDBCard, MDBCardBody, MDBCardText, MDBCardHeader, MDBCardTitle } from 'mdb-react-ui-kit';

export function Home() {
    return (
        <MDBCard>
            <MDBCardHeader><MDBCardTitle>Welcome to Avenue 17 - a demo test in ASP.Net, C# and React.js!</MDBCardTitle></MDBCardHeader>
            <MDBCardBody>
                <p><span style={{ fontWeight: '500' }}>Avenue17</span> is a demo application of my programming abilities in ASP.Net, C#, and Javascript/React plus Azure and SQL Server. </p>

                <h4>Deployment and local installation</h4>
                <p><span style={{ fontWeight: '500' }}>Avenue17</span> is currently deployed in Azure, at this very same URL you're using.</p>
                <p> To run it in your local development server, just clone or fork my code <a href="https://github.com/dafelcardozo/LibreriaSiempreAbierta">repository</a> with Visual Studio 2022
                    and configure the database connection string, <span style={{ fontWeight: '500' }}>cadenaLibreria</span>, in your local Secrets repository.</p>

                <p>You can run EF migrations to create the local database, but a database creation script is also provided and may be easier to use in some cases.
                    A data insertion script with a few thousands of book records is also provided.
                </p>
                <h4>Architecture</h4>
                <p><span style={{ fontWeight: '500' }}>Avenue17</span> is an ASP .Net Core application, and uses a REST architecture through Entity Framework 7 models and Web API controlers, and a view layer in Node and React. It targets .Net Framework 7.</p>

                <p>The SQL Server database is also deployed in Azure.</p>

                <p>The view layer uses several third-party Javascript libraries (many thanks to):</p>
                <ul>
                    <li>React-Select and React-MultiSelect for two very specific but extremely important components</li>
                    <li>MDBootstrap React provides the general Look & Feel and it's React based components.</li>
                    <li>the Font-Awesome icons and their React mappings</li>
                </ul>
                <p><span style={{ fontWeight: '500' }}>Avenue17</span> is an aplication for libraries management, and includes functions of listing and registering books, authors and publishers.</p>
            </MDBCardBody>
        </MDBCard>
    );

}
