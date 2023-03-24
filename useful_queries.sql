/****** Script for SelectTopNRows command from SSMS  ******/
SELECT count(*) from dbo.libros;
select count(*) from autores;
select count(distinct editoriales.nombre) from editoriales;
select count(*) from dbo.AuthorBook;

select count(*) from libros 
inner join AuthorBook on AuthorBook.BooksIsbn = libros.Isbn
inner join autores on autores.Id = AuthorBook.AuthorsId
inner join editoriales on editoriales.Id = libros.EditorialId

;

/****** Script for SelectTopNRows command from SSMS  ******/
delete from dbo.autores ;--where id <= 3;
delete from dbo.editoriales;-- where id <=3;
