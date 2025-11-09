IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250712041307_InitialSync', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Datos]', N'DatoSastreria';
GO

EXEC sp_rename N'[TipoDocumento].[Nombre]', N'name', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[IdSastre]', N'idsastre', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[IdModelo]', N'idmodelo', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[IdEstado]', N'idestado', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[IdCliente]', N'idcliente', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[FechaEntrega]', N'fechaentrega', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[IdPedido]', N'idpedido', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[Detalle]', N'details', N'COLUMN';
GO

EXEC sp_rename N'[Pedido].[IX_Pedido_IdSastre]', N'IX_Pedido_idsastre', N'INDEX';
GO

EXEC sp_rename N'[Pedido].[IX_Pedido_IdModelo]', N'IX_Pedido_idmodelo', N'INDEX';
GO

EXEC sp_rename N'[Pedido].[IX_Pedido_IdEstado]', N'IX_Pedido_idestado', N'INDEX';
GO

EXEC sp_rename N'[Pedido].[IX_Pedido_IdCliente]', N'IX_Pedido_idcliente', N'INDEX';
GO

EXEC sp_rename N'[Horario].[HoraInicio]', N'horainicio', N'COLUMN';
GO

EXEC sp_rename N'[Horario].[HoraFin]', N'horafin', N'COLUMN';
GO

EXEC sp_rename N'[Horario].[IdHorario]', N'idhorario', N'COLUMN';
GO

EXEC sp_rename N'[Horario].[Estado]', N'state', N'COLUMN';
GO

EXEC sp_rename N'[Horario].[Dia]', N'day', N'COLUMN';
GO

EXEC sp_rename N'[Estado].[Nombre]', N'name', N'COLUMN';
GO

EXEC sp_rename N'[Cliente].[IdTipoDocumento]', N'idtipodocumento', N'COLUMN';
GO

EXEC sp_rename N'[Cliente].[Telefono]', N'phonenumber', N'COLUMN';
GO

EXEC sp_rename N'[Cliente].[Nombre]', N'name', N'COLUMN';
GO

EXEC sp_rename N'[Cliente].[Correo]', N'email', N'COLUMN';
GO

EXEC sp_rename N'[Cliente].[Apellido]', N'lastname', N'COLUMN';
GO

EXEC sp_rename N'[Cliente].[IX_Cliente_IdTipoDocumento]', N'IX_Cliente_idtipodocumento', N'INDEX';
GO

EXEC sp_rename N'[Cita].[IdCliente]', N'idcliente', N'COLUMN';
GO

EXEC sp_rename N'[Cita].[IdCita]', N'idcita', N'COLUMN';
GO

EXEC sp_rename N'[Cita].[Notas]', N'notes', N'COLUMN';
GO

EXEC sp_rename N'[Cita].[FechaCita]', N'citafecha', N'COLUMN';
GO

EXEC sp_rename N'[Cita].[Estado]', N'state', N'COLUMN';
GO

EXEC sp_rename N'[Cita].[IX_Cita_IdCliente]', N'IX_Cita_idcliente', N'INDEX';
GO

EXEC sp_rename N'[DatoSastreria].[Telefono]', N'phonenumber', N'COLUMN';
GO

EXEC sp_rename N'[DatoSastreria].[Nombre]', N'name', N'COLUMN';
GO

EXEC sp_rename N'[DatoSastreria].[LogoSastreria]', N'picture', N'COLUMN';
GO

EXEC sp_rename N'[DatoSastreria].[Direccion]', N'address', N'COLUMN';
GO

EXEC sp_rename N'[DatoSastreria].[Descripcion]', N'description', N'COLUMN';
GO

EXEC sp_rename N'[DatoSastreria].[IdDatos]', N'iddatosastreria', N'COLUMN';
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Pedido]') AND [c].[name] = N'fechaentrega');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Pedido] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Pedido] ALTER COLUMN [fechaentrega] datetime2 NOT NULL;
GO

ALTER TABLE [Cliente] ADD [numerodocumento] varchar(20) NOT NULL DEFAULT '';
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cita]') AND [c].[name] = N'citafecha');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Cita] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Cita] ALTER COLUMN [citafecha] datetime2 NOT NULL;
GO

CREATE TABLE [CitaImagen] (
    [IdCitaImagen] int NOT NULL IDENTITY,
    [IdCita] int NOT NULL,
    [ImageUrl] varchar(max) NOT NULL,
    CONSTRAINT [PK_CitaImagen] PRIMARY KEY ([IdCitaImagen])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250901181930_AddPedidoToCita', N'8.0.10');
GO

COMMIT;
GO

