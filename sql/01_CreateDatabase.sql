IF DB_ID('DBVIAJE') IS NULL
BEGIN
    CREATE DATABASE DBVIAJE;
END
GO

USE DBVIAJE;
GO

-- =====================================================================
-- Tabla: Pasajeros  (propiedad exclusiva de ServicioPasajeros)
-- =====================================================================
IF OBJECT_ID('dbo.Pasajeros', 'U') IS NOT NULL
    DROP TABLE dbo.Pasajeros;
GO

CREATE TABLE dbo.Pasajeros (
    PasajeroId      INT IDENTITY(1,1) PRIMARY KEY,
    Documento       VARCHAR(20) NOT NULL UNIQUE,
    NombreCompleto  VARCHAR(150) NOT NULL,
    Estado          VARCHAR(20) NOT NULL  -- 'Valido' / 'Invalido' / 'Bloqueado'
);
GO

INSERT INTO dbo.Pasajeros (Documento, NombreCompleto, Estado) VALUES
('70001111', 'Juan Perez Rojas',        'Valido'),
('70002222', 'Maria Lopez Castro',      'Valido'),
('70003333', 'Carlos Fernandez Diaz',   'Valido'),
('70004444', 'Ana Torres Mendoza',      'Valido'),
('70005555', 'Luis Ramirez Vega',       'Bloqueado'),
('70006666', 'Sofia Gutierrez Paredes', 'Valido'),
('70007777', 'Pedro Sanchez Rios',      'Invalido'),
('70008888', 'Lucia Vargas Quispe',     'Valido');
GO

-- =====================================================================
-- Tabla: Rutas  (propiedad exclusiva de ServicioRutas)
-- =====================================================================
IF OBJECT_ID('dbo.Rutas', 'U') IS NOT NULL
    DROP TABLE dbo.Rutas;
GO

CREATE TABLE dbo.Rutas (
    RutaId      INT IDENTITY(1,1) PRIMARY KEY,
    Origen      VARCHAR(50) NOT NULL,
    Destino     VARCHAR(50) NOT NULL,
    Estado      VARCHAR(20) NOT NULL  -- 'Activa' / 'Inactiva'
);
GO

INSERT INTO dbo.Rutas (Origen, Destino, Estado) VALUES
('Lima',     'Cusco',     'Activa'),
('Lima',     'Arequipa',  'Activa'),
('Lima',     'Trujillo',  'Activa'),
('Lima',     'Piura',     'Activa'),
('Cusco',    'Puno',      'Activa'),
('Arequipa', 'Tacna',     'Activa'),
('Lima',     'Iquitos',   'Inactiva'),
('Trujillo', 'Chiclayo',  'Activa');
GO

-- =====================================================================
-- Tabla: Disponibilidad  (propiedad exclusiva de ServicioDisponibilidad)
-- =====================================================================
IF OBJECT_ID('dbo.Disponibilidad', 'U') IS NOT NULL
    DROP TABLE dbo.Disponibilidad;
GO

CREATE TABLE dbo.Disponibilidad (
    DisponibilidadId INT IDENTITY(1,1) PRIMARY KEY,
    RutaId            INT NOT NULL,
    FechaViaje        DATE NOT NULL,
    CuposDisponibles  INT NOT NULL
    -- Nota: sin FOREIGN KEY hacia Rutas a proposito.
    -- En SOA real, cada servicio NO comparte FKs fisicas con otro
    -- servicio; la relacion se resuelve a nivel de orquestacion (ESB),
    -- no a nivel de base de datos.
);
GO

INSERT INTO dbo.Disponibilidad (RutaId, FechaViaje, CuposDisponibles) VALUES
(1, '2026-07-20', 12),  -- Lima - Cusco
(1, '2026-07-21', 0),   -- Lima - Cusco (sin cupos)
(2, '2026-07-20', 8),   -- Lima - Arequipa
(3, '2026-07-22', 20),  -- Lima - Trujillo
(4, '2026-07-20', 5),   -- Lima - Piura
(5, '2026-07-25', 15),  -- Cusco - Puno
(6, '2026-07-20', 3),   -- Arequipa - Tacna
(8, '2026-07-23', 10);  -- Trujillo - Chiclayo
GO

IF OBJECT_ID('dbo.Reservas', 'U') IS NOT NULL
    DROP TABLE dbo.Reservas;
GO

CREATE TABLE dbo.Reservas (
    ReservaId   INT IDENTITY(1,1) PRIMARY KEY,
    Documento   VARCHAR(20) NOT NULL,
    RutaId      INT NOT NULL,
    FechaViaje  DATE NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Reserva UNIQUE (Documento, RutaId, FechaViaje)
);
GO

PRINT 'Base de datos DBVIAJE creada correctamente con datos de prueba.';
GO
