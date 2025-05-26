CREATE OR ALTER PROCEDURE sp_UpdateAlbumSet
    @Id INT,
    @Name NVARCHAR(100),
    @Precio DECIMAL(18,2)
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
                -2 AS Id,  --Registro no encontrado
                -1 AS RowsAffected,
                'Álbum no encontrado' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Verificar duplicado
        IF EXISTS (SELECT 1 FROM AlbumSet WHERE Id != @Id AND Name = @Name)
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT 
                -1 AS Id, -- Registro duplicado
                -1 AS RowsAffected,
                'Ya existe otro álbum con ese nombre' AS ErrorMessage,
                16 AS ErrorSeverity;
            RETURN;
        END
        
        -- Actualizar el álbum
        UPDATE AlbumSet
        SET Name = @Name
        ,Precio = @Precio
        WHERE Id = @Id;
        
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
            -99 AS Id,
            -1 AS RowsAffected,
            ERROR_MESSAGE() AS ErrorMessage,
            ERROR_SEVERITY() AS ErrorSeverity;
    END CATCH
END