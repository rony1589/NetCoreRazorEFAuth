CREATE OR ALTER PROCEDURE sp_CreateSongSet
    @Name NVARCHAR(100),
    @Album_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verificar si existe el álbum
        IF NOT EXISTS (SELECT 1 FROM AlbumSet WHERE Id = @Album_Id)
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT 
                -3 AS Id, -- Código para "álbum no existe"
                -1 AS RowsAffected,
                'Álbum no encontrado' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Verificar duplicado
        IF EXISTS (SELECT 1 FROM SongSet WHERE Name = @Name AND Album_Id = @Album_Id)
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT 
                -1 AS Id,
                -1 AS RowsAffected,
                'Ya existe una canción con ese nombre en el álbum' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Insertar la canción
        INSERT INTO SongSet (Name, Album_Id)
        VALUES (@Name, @Album_Id);
        
        DECLARE @NewId INT = SCOPE_IDENTITY();
        
        COMMIT TRANSACTION;

        SELECT 
            @NewId AS Id,
            1 AS RowsAffected,
            NULL AS ErrorMessage,
            0 AS ErrorSeverity;
            
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        SELECT 
            -99 AS Id,
            -1 AS RowsAffected,
            ERROR_MESSAGE() AS ErrorMessage,
            ERROR_SEVERITY() AS ErrorSeverity;
    END CATCH
END