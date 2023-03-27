SELECT * from dbo.libros order by titulo;
select * from autores;
select count(*) from editoriales;
select count(*), nombre, sede from editoriales
group by nombre, sede having count(*) = 0;
select count(*) from dbo.AuthorBook;

select count(*) from libros 
inner join AuthorBook on AuthorBook.BooksIsbn = libros.Isbn
inner join autores on autores.Id = AuthorBook.AuthorsId
inner join editoriales on editoriales.Id = libros.EditorialId

;

/****** Script for SelectTopNRows command from SSMS  ******/
delete from dbo.autores ;--where id <= 3;
delete from dbo.editoriales;-- where id <=3;


select nombre, sede, count(*) 
from dbo.editoriales group by nombre, sede
having count(*) > 1
;
SELECT collation_name,name AS COLLATION
FROM sys.databases

;

SELECT
COLUMN_NAME,
COLLATION_NAME
FROM
INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'editoriales';

insert into editoriales (nombre, sede) values ('hola', 'mundo');

SELECT DATABASEPROPERTYEX('IX_editoriales_nombre_sede', 'Collation') SQLCollation;