CREATE OR ALTER PROCEDURE sp_DeleteAlbumSet
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verificar si existe el álbum
        IF NOT EXISTS (SELECT 1 FROM AlbumSet WHERE Id = @Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT 
                -2 as Id, -- Indica Registro no encontrado
                -1 AS RowsAffected, 
                'Álbum no encontrado' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Verificar si tiene canciones asociadas
        IF EXISTS (SELECT 1 FROM SongSet WHERE Album_Id = @Id)
        BEGIN
             ROLLBACK TRANSACTION;

            SELECT 
                -4 AS Id, -- Indica que Tiene registros dependientes
                -1 AS RowsAffected,
                'No se puede eliminar, tiene canciones asociadas' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Eliminar el álbum
        DELETE FROM AlbumSet WHERE Id = @Id;
        
        SELECT 
            @Id AS Id,
            @@ROWCOUNT AS RowsAffected,
            NULL AS ErrorMessage,
            0 AS ErrorSeverity;
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        SELECT 
            -99 AS Id, --Error inesperado
            -1 AS RowsAffected,
            ERROR_MESSAGE() AS ErrorMessage,
            ERROR_SEVERITY() AS ErrorSeverity;
    END CATCH
END