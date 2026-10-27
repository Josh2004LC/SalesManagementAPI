# SalesManagementAPI — API REST de Gestión de Ventas

API REST desarrollada con **ASP.NET Core Web API y C#** para gestionar clientes, productos, usuarios y operaciones de venta.

El proyecto utiliza Entity Framework Core y SQL Server, con una estructura que separa los controladores, servicios, modelos y acceso a datos.

## Tecnologías utilizadas

- C# y .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI

## Funcionalidades

- Gestión de clientes.
- Administración de productos.
- Gestión de usuarios.
- Validación de credenciales.
- Registro de ventas con detalles.
- Consulta de ventas registradas.
- Actualización y eliminación de ventas.
- Comunicación con una aplicación web mediante endpoints REST.

## Endpoints de ventas

| Método | Endpoint | Función |
|---|---|---|
| GET | `/api/Venta` | Consultar ventas |
| GET | `/api/Venta/{id}` | Consultar una venta |
| POST | `/api/Venta` | Registrar una venta |
| PUT | `/api/Venta/{id}` | Actualizar una venta |
| DELETE | `/api/Venta/{id}` | Eliminar una venta |
| POST | `/api/Login` | Validar credenciales |

Existen controladores adicionales para clientes, productos y usuarios.

## Estructura del proyecto

- `Controllers`: endpoints REST.
- `Services`: operaciones y lógica de acceso a datos.
- `Models`: entidades y modelos.
- `Data`: contexto de Entity Framework Core.
- `Script_Database`: script SQL del proyecto.

## Instalación

1. Clonar el repositorio.
2. Instalar .NET 8 y SQL Server.
3. Revisar y ejecutar `Script_Database/Script_Database.sql`.
4. Configurar `ConnectionStrings:Conexion` para la base de datos local, utilizando una configuración segura.
5. Ejecutar:

```bash
dotnet restore
dotnet run
```

6. Acceder a `/swagger` en el entorno de desarrollo.

## Mejoras futuras

- Incorporar autenticación y autorización para proteger los endpoints.
- Fortalecer la protección de información sensible de usuarios.
- Garantizar que las ventas y sus detalles se registren mediante transacciones.
- Agregar pruebas unitarias y de integración.
- Mejorar las validaciones y respuestas de error.

## Frontend

[WebAppSalesManagement](https://github.com/Josh2004LC/WebAppSalesManagement)

## Estado del proyecto

API desarrollada como proyecto académico, con enfoque en la gestión de operaciones empresariales y la integración entre aplicaciones web y bases de datos.
