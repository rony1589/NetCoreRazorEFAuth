CREATE OR ALTER PROCEDURE sp_CreateAlbumSet
    @Name NVARCHAR(100),
    @Precio DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF EXISTS (SELECT 1 FROM AlbumSet WHERE Name = @Name)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT 
                -1 AS Id, -- Código de error identificar que el Registro duplicado
                -1 AS RowsAffected,
                'Registro duplicado' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        INSERT INTO AlbumSet (Name,Precio)
        VALUES (@Name,@Precio);
        
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
            -99 AS Id, --Indica Error inesperado
            -1 AS RowsAffected,
            ERROR_MESSAGE() AS ErrorMessage,
            ERROR_SEVERITY() AS ErrorSeverity;
    END CATCH
END