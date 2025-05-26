CREATE OR ALTER PROCEDURE sp_DeleteSongSet
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verificar si existe la canción
        IF NOT EXISTS (SELECT 1 FROM SongSet WHERE Id = @Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT -1 AS Id,
                -1 AS RowsAffected,
                'Canción no encontrada' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Eliminar la canción
        DELETE FROM SongSet WHERE Id = @Id;
        
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