CREATE OR ALTER PROCEDURE dbo.Sp_UpdateOrder
(
    @JsonData NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @OrderHeaderID UNIQUEIDENTIFIER;

        SET @OrderHeaderID = JSON_VALUE(@JsonData, '$.OrderHeaderID');

        IF NOT EXISTS (
            SELECT 1 
            FROM OrderHeaders
            WHERE OrderHeaderID = @OrderHeaderID
        )
        BEGIN
            ;THROW 50001, 'Order not found', 1;
        END

        UPDATE OrderHeaders
        SET
            OrderDate = JSON_VALUE(@JsonData, '$.OrderDate'),
            ShipCity = JSON_VALUE(@JsonData, '$.ShipCity'),
            ShipAddress = JSON_VALUE(@JsonData, '$.ShipAddress'),
            CustomerID = JSON_VALUE(@JsonData, '$.CustomerID')
        WHERE OrderHeaderID = @OrderHeaderID;

        DELETE FROM OrderDetails
        WHERE OrderHeaderID = @OrderHeaderID;

        INSERT INTO OrderDetails
        (
            OrderDetailID,
            OrderHeaderID,
            ProductID,
            UnitPrice,
            Quantity
        )
        SELECT
            OrderDetailID,
            @OrderHeaderID,
            ProductID,
            UnitPrice,
            Quantity
        FROM OPENJSON(@JsonData, '$.OrderDetails')
        WITH (
            OrderDetailID UNIQUEIDENTIFIER '$.OrderDetailID',
            ProductID UNIQUEIDENTIFIER '$.ProductID',
            UnitPrice DECIMAL(18,2) '$.UnitPrice',
            Quantity INT '$.Quantity'
        );

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END
GO