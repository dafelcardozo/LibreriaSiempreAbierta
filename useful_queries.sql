SELECT * from dbo.libros order by titulo;
SELECT count(*) from dbo.libros ;
select count(*) from editoriales;
select count(*), nombre, sede from editoriales
group by nombre, sede having count(*) > 1;
select count(*) from dbo.AuthorBook;

select count(*) from libros 
inner join AuthorBook on AuthorBook.BooksIsbn = libros.Isbn
inner join autores on autores.Id = AuthorBook.AuthorsId
inner join editoriales on editoriales.Id = libros.EditorialId


;
delete from dbo.autores ;--where id <= 3;
delete from dbo.editoriales;-- where id <=3;


update editoriales set sede = '';