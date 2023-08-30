SELECT * from dbo.libros order by titulo;
SELECT count(*) from dbo.libros ;
select * from autores;
select * from editoriales;
select count(*) from dbo.AuthorBook;

select count(*) from libros 
inner join AuthorBook on AuthorBook.BooksIsbn = libros.Isbn
inner join autores on autores.Id = AuthorBook.AuthorsId
inner join editoriales on editoriales.Id = libros.EditorialId


;
delete from dbo.autores ;
delete from dbo.editoriales;


update editoriales set sede = '';