using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSastreria.Migrations
{
    /// <inheritdoc />
    public partial class AddModeloCategoriaRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__A3C02A1032B13906", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CitaImagen",
                columns: table => new
                {
                    IdCitaImagen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCita = table.Column<int>(type: "int", unicode: false, nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitaImagen", x => x.IdCitaImagen);
                });

            migrationBuilder.CreateTable(
                name: "DatoSastreria",
                columns: table => new
                {
                    iddatosastreria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    phonenumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    picture = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Datos__A4BC7BC551DEEEC5", x => x.iddatosastreria);
                });

            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado__FBB0EDC135F48D85", x => x.IdEstado);
                });

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    idhorario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    day = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    horainicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    horafin = table.Column<TimeOnly>(type: "time", nullable: false),
                    state = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Horario__1539229B8DE62D0D", x => x.idhorario);
                });

            migrationBuilder.CreateTable(
                name: "Modelo",
                columns: table => new
                {
                    IdModelo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    creationdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modelo__CC30D30CD4075261", x => x.IdModelo);
                });

            migrationBuilder.CreateTable(
                name: "Sastre",
                columns: table => new
                {
                    IdSastre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sastre__06563E870BA6D5DE", x => x.IdSastre);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    IdTipoDocumento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TipoDocu__3AB3332FE8CECEF4", x => x.IdTipoDocumento);
                });

            migrationBuilder.CreateTable(
                name: "ModeloCategoria",
                columns: table => new
                {
                    CategoriasIdCategoria = table.Column<int>(type: "int", nullable: false),
                    ModelosIdModelo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloCategoria", x => new { x.CategoriasIdCategoria, x.ModelosIdModelo });
                    table.ForeignKey(
                        name: "FK_ModeloCategoria_Categoria_CategoriasIdCategoria",
                        column: x => x.CategoriasIdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeloCategoria_Modelo_ModelosIdModelo",
                        column: x => x.ModelosIdModelo,
                        principalTable: "Modelo",
                        principalColumn: "IdModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModeloImagen",
                columns: table => new
                {
                    IdModeloImagen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    idmodelo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ModeloIm__75E9782729A4E057", x => x.IdModeloImagen);
                    table.ForeignKey(
                        name: "FK_ModeloImagen_Modelo_idmodelo",
                        column: x => x.idmodelo,
                        principalTable: "Modelo",
                        principalColumn: "IdModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idtipodocumento = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    lastname = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    phonenumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    numerodocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cliente__D5946642D6F2BE01", x => x.IdCliente);
                    table.ForeignKey(
                        name: "FK__Cliente__IdTipoD__45F365D3",
                        column: x => x.idtipodocumento,
                        principalTable: "TipoDocumento",
                        principalColumn: "IdTipoDocumento");
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    idpedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fechaentrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    details = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    idsastre = table.Column<int>(type: "int", nullable: true),
                    idcliente = table.Column<int>(type: "int", nullable: true),
                    idestado = table.Column<int>(type: "int", nullable: true),
                    idmodelo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pedido__9D335DC383EAADC3", x => x.idpedido);
                    table.ForeignKey(
                        name: "FK__Pedido__IdClient__49C3F6B7",
                        column: x => x.idcliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK__Pedido__IdEstado__4AB81AF0",
                        column: x => x.idestado,
                        principalTable: "Estado",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Pedido__IdModelo__4BAC3F29",
                        column: x => x.idmodelo,
                        principalTable: "Modelo",
                        principalColumn: "IdModelo");
                    table.ForeignKey(
                        name: "FK__Pedido__IdSastre__48CFD27E",
                        column: x => x.idsastre,
                        principalTable: "Sastre",
                        principalColumn: "IdSastre");
                });

            migrationBuilder.CreateTable(
                name: "Cita",
                columns: table => new
                {
                    idcita = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idcliente = table.Column<int>(type: "int", nullable: true),
                    citafecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    state = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    notes = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    PedidoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cita__394B02026E23F138", x => x.idcita);
                    table.ForeignKey(
                        name: "FK_Cita_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "idpedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Cita__IdCliente__5441852A",
                        column: x => x.idcliente,
                        principalTable: "Cliente",
                        principalColumn: "IdCliente");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cita_idcliente",
                table: "Cita",
                column: "idcliente");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_PedidoId",
                table: "Cita",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_idtipodocumento",
                table: "Cliente",
                column: "idtipodocumento");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloCategoria_ModelosIdModelo",
                table: "ModeloCategoria",
                column: "ModelosIdModelo");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloImagen_idmodelo",
                table: "ModeloImagen",
                column: "idmodelo");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_idcliente",
                table: "Pedido",
                column: "idcliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_idestado",
                table: "Pedido",
                column: "idestado");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_idmodelo",
                table: "Pedido",
                column: "idmodelo");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_idsastre",
                table: "Pedido",
                column: "idsastre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cita");

            migrationBuilder.DropTable(
                name: "CitaImagen");

            migrationBuilder.DropTable(
                name: "DatoSastreria");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropTable(
                name: "ModeloCategoria");

            migrationBuilder.DropTable(
                name: "ModeloImagen");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Modelo");

            migrationBuilder.DropTable(
                name: "Sastre");

            migrationBuilder.DropTable(
                name: "TipoDocumento");
        }
    }
}
