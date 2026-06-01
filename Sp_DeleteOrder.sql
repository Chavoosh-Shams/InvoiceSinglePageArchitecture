CREATE OR ALTER PROCEDURE dbo.Sp_DeleteOrder
(
    @OrderHeaderID UNIQUEIDENTIFIER
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;

        DELETE FROM OrderDetails
        WHERE OrderHeaderID = @OrderHeaderID;

        DELETE FROM OrderHeaders
        WHERE OrderHeaderID = @OrderHeaderID;

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(MAX);
        SET @ErrorMessage = ERROR_MESSAGE();

        RAISERROR(@ErrorMessage, 16, 1);

    END CATCH
END
GO