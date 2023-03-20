# Avenue17 - Una librería siempre abierta

Este repositorio implementa mi solución a una prueba técnica de mis habilidades en ASP .Net, C# y Javascript para Browser Travel Solutions.

La aplicación se encuentra desplegada en esta URL: https://avenue1720230320150946.azurewebsites.net/

Avenue17 es una aplicación de gestión para librerías, y tiene funcionalidades de catálogo y registro de libros, autores y editoriales.

# Despliegue e instalación local

Avenue17 está desplegada en Azure, en esta dirección: https://avenue1720230320150946.azurewebsites.net/

Para correrla localmente, descárguela con Visual Studio 2022 y configure la cadena de conexión con la base de datos, _cadenaLibreria_. Usa la versión de .Net Framework 7. 

El script de base de datos contiene las distintas tablas, y una colección de más o menos un millar de libros y autores.

# Arquitectura

Avenue17 es una aplicación de ASP .Net Core, y tiene una arquitectura REST con modelos de Entity Framework 7 y controladores de Web API, y una capa de vista en Node y React.

La base de datos es una instancia SQL Server, igualmente desplegada en Azure.

La capa de vista utiliza varias bibliotecas de componentes adicionales (agradecimientos):

- Select y MultiSelect.
- MDBootstrap React para el look & feel general.
- íconos de Font-Awesome.

# Avenue17 - An always open library

This repository implements mi solution to a .Net technical test of my abilities in .Net, C# and Javascript for Browser Travel Solutions.

The application is currently deployed here: https://avenue1720230320150946.azurewebsites.net/

Avenue17 is aplication for libraries management, and includes functions of listing and registering books, authors and publishers.

# Deployment and local installation

Avenue17 is currently deployed in Azure, at this URL: 

To run it in your local development server, just clone or fork this repo with Visual Studio 2022 and configure the database connection string, _cadenaLibreria_, in your local Secrets repository. It targets .Net Framework 7.

A database script is provided in order to initialize it with tables and a few thousands of book records.

# Architecture

Avenue17 is an ASP .Net Core application, and uses a REST architecture through Entity Framework 7 models and Web API controlers, and a view layer in Node and React.

The SQL Server database is also deployed in Azure.

The view layer uses several third-party Javascript libraries (agradecimientos):

- Select and MultiSelect.
- MDBootstrap React provides the general Look & Feel.
- Font-Awesome icons.
