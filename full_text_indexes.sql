CREATE FULLTEXT CATALOG FTIAutoresNombreApellidos;  
CREATE FULLTEXT INDEX ON dbo.autores
(  
    nombre   Language 2057        
	, apellidos Language 2057 
)  
KEY INDEX PK_autores ON FTIAutoresNombreApellidos 
WITH CHANGE_TRACKING AUTO     
GO  
;
--drop fulltext index on dbo.autores;
--drop fulltext catalog FTIAutoresNombreApellidos;

CREATE FULLTEXT CATALOG FTIEditorialesNombreSede;  
CREATE FULLTEXT INDEX ON dbo.editoriales
(  
    nombre   Language 2057        
	, sede Language 2057 
)  
KEY INDEX PK_editoriales ON FTIEditorialesNombreSede 
WITH CHANGE_TRACKING AUTO     
GO  
;

--drop fulltext index on dbo.editoriales;
--drop fulltext catalog FTIEditorialesNombreSede;

CREATE FULLTEXT CATALOG FTILibrosTituloSinopsis;  
CREATE FULLTEXT INDEX ON dbo.libros
(  
    titulo   Language 2057        
	, sinopsis Language 2057 
)  
KEY INDEX PK_libros ON FTILibrosTituloSinopsis 
WITH CHANGE_TRACKING AUTO     
GO  
;

--drop fulltext index on dbo.libros;
--drop fulltext catalog FTILibrosTituloSinopsis;
