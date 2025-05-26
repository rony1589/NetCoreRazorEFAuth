CREATE OR ALTER PROCEDURE sp_UpdateSongSet
    @Id INT,
    @Name NVARCHAR(100),
    @Album_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verificar si existe la canción
        IF NOT EXISTS (SELECT 1 FROM SongSet WHERE Id = @Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT -2 AS Id,
                -1 AS RowsAffected,
                'Canción no encontrada' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Verificar si existe el álbum
        IF NOT EXISTS (SELECT 1 FROM AlbumSet WHERE Id = @Album_Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT -3 AS Id,
                -1 AS RowsAffected,
                'Álbum no encontrado' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Verificar duplicado
        IF EXISTS (SELECT 1 FROM SongSet WHERE Id != @Id AND Name = @Name AND Album_Id = @Album_Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT -1 AS Id,
                -1 AS RowsAffected,
                'Ya existe otra canción con ese nombre en el álbum' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Actualizar la canción
        UPDATE SongSet SET 
            Name = @Name,
            Album_Id = @Album_Id
        WHERE Id = @Id;
        
        SELECT @Id AS Id,
            @@ROWCOUNT AS RowsAffected,
            NULL AS ErrorMessage,
            0 AS ErrorSeverity;
            
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        SELECT -99 AS Id,
            -1 AS RowsAffected,
            ERROR_MESSAGE() AS ErrorMessage,
            ERROR_SEVERITY() AS ErrorSeverity;
    END CATCH
END