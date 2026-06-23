-- =====================================================================
-- 02_StoredProcedures.sql
--
-- Ejecutar este script en DBVIAJE despues de 01_CreateDatabase.sql
-- =====================================================================

USE DBVIAJE;
GO

-- =====================================================================
-- sp_ValidarPasajero
-- =====================================================================
IF OBJECT_ID('dbo.sp_ValidarPasajero', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ValidarPasajero;
GO

CREATE PROCEDURE dbo.sp_ValidarPasajero
    @Documento VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Documento, NombreCompleto, Estado
    FROM dbo.Pasajeros
    WHERE Documento = @Documento;
END
GO

-- =====================================================================
-- sp_ObtenerRuta
-- =====================================================================
IF OBJECT_ID('dbo.sp_ObtenerRuta', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ObtenerRuta;
GO

CREATE PROCEDURE dbo.sp_ObtenerRuta
    @Origen VARCHAR(50),
    @Destino VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RutaId, Origen, Destino, Estado
    FROM dbo.Rutas
    WHERE Origen = @Origen AND Destino = @Destino;
END
GO

-- =====================================================================
-- sp_ConsultarDisponibilidad
-- =====================================================================
IF OBJECT_ID('dbo.sp_ConsultarDisponibilidad', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ConsultarDisponibilidad;
GO

CREATE PROCEDURE dbo.sp_ConsultarDisponibilidad
    @RutaId INT,
    @FechaViaje DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RutaId, FechaViaje, CuposDisponibles
    FROM dbo.Disponibilidad
    WHERE RutaId = @RutaId AND FechaViaje = @FechaViaje;
END
GO

-- =====================================================================
-- sp_ExisteReserva
-- =====================================================================
IF OBJECT_ID('dbo.sp_ExisteReserva', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ExisteReserva;
GO

CREATE PROCEDURE dbo.sp_ExisteReserva
    @Documento VARCHAR(20),
    @RutaId INT,
    @FechaViaje DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN COUNT(1) > 0 THEN 1 ELSE 0 END AS Existe
    FROM dbo.Reservas
    WHERE Documento = @Documento AND RutaId = @RutaId AND FechaViaje = @FechaViaje;
END
GO

-- =====================================================================
-- sp_ReservarCupo
-- =====================================================================
IF OBJECT_ID('dbo.sp_ReservarCupo', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ReservarCupo;
GO

CREATE PROCEDURE dbo.sp_ReservarCupo
    @Documento VARCHAR(20),
    @RutaId INT,
    @FechaViaje DATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @CuposActuales INT;
    DECLARE @YaReservado INT;

    BEGIN TRANSACTION;

    SELECT @YaReservado = COUNT(1)
    FROM dbo.Reservas
    WHERE Documento = @Documento AND RutaId = @RutaId AND FechaViaje = @FechaViaje;

    IF @YaReservado > 0
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT
            CAST(0 AS BIT) AS Exito,
            'Este pasajero ya tiene una reserva para esa ruta y fecha.' AS Mensaje,
            0 AS CuposRestantes;
        RETURN;
    END

    SELECT @CuposActuales = CuposDisponibles
    FROM dbo.Disponibilidad WITH (UPDLOCK, ROWLOCK)
    WHERE RutaId = @RutaId AND FechaViaje = @FechaViaje;

    IF @CuposActuales IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT
            CAST(0 AS BIT) AS Exito,
            'No existe registro de disponibilidad para esa ruta y fecha.' AS Mensaje,
            0 AS CuposRestantes;
        RETURN;
    END

    IF @CuposActuales <= 0
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT
            CAST(0 AS BIT) AS Exito,
            'No hay cupos disponibles para reservar.' AS Mensaje,
            0 AS CuposRestantes;
        RETURN;
    END

    UPDATE dbo.Disponibilidad
    SET CuposDisponibles = CuposDisponibles - 1
    WHERE RutaId = @RutaId AND FechaViaje = @FechaViaje;

    INSERT INTO dbo.Reservas (Documento, RutaId, FechaViaje)
    VALUES (@Documento, @RutaId, @FechaViaje);

    COMMIT TRANSACTION;

    SELECT
        CAST(1 AS BIT) AS Exito,
        'Cupo reservado correctamente.' AS Mensaje,
        @CuposActuales - 1 AS CuposRestantes;
END
GO

-- =====================================================================
-- sp_CancelarReserva
-- =====================================================================
IF OBJECT_ID('dbo.sp_CancelarReserva', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CancelarReserva;
GO

CREATE PROCEDURE dbo.sp_CancelarReserva
    @Documento VARCHAR(20),
    @RutaId INT,
    @FechaViaje DATE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @FilasEliminadas INT;
    DECLARE @CuposActuales INT;

    BEGIN TRANSACTION;

    DELETE FROM dbo.Reservas
    WHERE Documento = @Documento AND RutaId = @RutaId AND FechaViaje = @FechaViaje;

    SET @FilasEliminadas = @@ROWCOUNT;

    IF @FilasEliminadas = 0
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT
            CAST(0 AS BIT) AS Exito,
            'No existe una reserva de este pasajero para esa ruta y fecha.' AS Mensaje,
            0 AS CuposRestantes;
        RETURN;
    END

    SELECT @CuposActuales = CuposDisponibles
    FROM dbo.Disponibilidad WITH (UPDLOCK, ROWLOCK)
    WHERE RutaId = @RutaId AND FechaViaje = @FechaViaje;

    IF @CuposActuales IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT
            CAST(0 AS BIT) AS Exito,
            'No existe registro de disponibilidad para esa ruta y fecha.' AS Mensaje,
            0 AS CuposRestantes;
        RETURN;
    END

    UPDATE dbo.Disponibilidad
    SET CuposDisponibles = CuposDisponibles + 1
    WHERE RutaId = @RutaId AND FechaViaje = @FechaViaje;

    COMMIT TRANSACTION;

    SELECT
        CAST(1 AS BIT) AS Exito,
        'Reserva cancelada correctamente.' AS Mensaje,
        @CuposActuales + 1 AS CuposRestantes;
END
GO

PRINT 'Stored Procedures creados correctamente.';
GO
