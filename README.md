# 📚 MusicRadio
**MusicRadio** es una simple aplicación web desarrollada con **ASP.NET Core 8**, **Razor Pages con bootstrap** y **Entity Framework Core**, cuyo principal objetivo es **mostrar algunas formas de cómo construir y organizar CRUDs** usando diferentes enfoques con EF Core: 

- **EF tradicional con LINQ**  
- **Procedimientos almacenados (Stored Procedures)**  
- **Uso de DTOs, mapeo automático, pruebas y principios SOLID**

La solución está diseñada siguiendo una patrón de arquitectura **limpia, desacoplada y distribuida en capas**, altamente mantenible y orientada a buenas prácticas de desarrollo backend moderno.

## 🎯 ¿Qué hace MusicRadio?

Permite dos tipos de experiencia de usuario:
- **Clientes**: pueden registrarse, explorar álbumes y realizar compras.
- **Administradores de Inventario**: pueden ingresar, editar y eliminar álbumes y canciones del sistema.
  
## 🏗️🧱 Arquitectura de capas del Proyecto
La solución está organizada en los siguientes proyectos:

- **MusicRadio.Core**: Contiene las entidades del dominio y las interfaces de los servicios.

- **MusicRadio.Application**: Incluye los DTOs y las configuraciones de mapeo entre entidades y DTOs.

- **MusicRadio.Database**: Incluye algunos .sql que son la estructura de los StoredProcedure  de las entidades AlbumSets y SongSets para Crear, Editar, Eliminar.

- **MusicRadio.DatabaseCli**: Contiene la logica para que desde comando de EF se desplieguen los StoredProcedure.

- **MusicRadio.Shared**: Incluye las clases que se comparten en todos los proyectos.

- **MusicRadio.Infrastructure**: Implementa los servicios definidos en MusicRadio.Core y maneja la interacción con la base de datos mediante Entity Framework Core.

- **MusicRadio.Web**: Proyecto principal de la aplicación web que utiliza Razor Pages para la interfaz de usuario.

- **MusicRadio.Tests**: Contiene las pruebas unitarias para los servicios y controladores.

## 🧠 Algunos Detalles Técnicos y Buenas Prácticas
### 🔐 Autenticación y Seguridad

- Implementación de **JWT (JSON Web Tokens)** para autenticación de usuarios.
- El servicio `ITokenGenerator` encapsula la generación de tokens firmados.
- Autenticación diferenciada por roles (usuarios comunes vs. administradores de inventario).

### 🧰 Inyección de Dependencias

- Toda la lógica de servicios y repositorios está registrada en `DependencyInjection.cs` y configurada mediante `IServiceCollection`.
- Se sigue el patrón de inversión de dependencias para desacoplar la lógica de negocio del framework.

### 💾 Acceso a Datos

- Uso de **Entity Framework Core** con `DbContext` personalizado (`MusicRadioDbContext`).
- Configuración detallada de entidades usando Fluent API (clases en `Configurations/`).
- Soporte para procedimientos almacenados (`StoredProcedure`) para operaciones CRUD directas.

### 🔄 Mapeo de Datos

- Se utiliza **AutoMapper** para transformar entre entidades del dominio y DTOs/ViewModels.
- Archivos `MappingProfile` y `WebMappingProfile` contienen las configuraciones de mapeo.

### 🧪 Pruebas Unitarias

- Pruebas de servicios y controladores usando xUnit.
- Uso de clases base (`BasePruebas.cs`) para simular contexto de pruebas.

## 📁 Estructura de Carpetas
```text
MusicRadio/
├── MusicRadio.Core/
│   ├── Entities/
│   │   ├── AlbumSet.cs
│   │   ├── Client.cs
│   │   └── PurchaseDetail.cs
│   │   └── Role.cs
│   │   └── SongSet.cs
│   └── Interfaces/
│       ├── IAlbumSetService.cs
│       ├── IAuthService.cs
│       └── IClientService.cs
│       └── IPurchaseDetailService.cs
│       └── IRoleService.cs
│       └── ISongSetService.cs
│       └── ITokenGenerator.cs
│
├── MusicRadio.Application/
│   └── DTOs/
│       └── AlbumSetDto.cs
│       └── ClientDto.cs
│       └── LoginDto.cs
│       └── PurchaseDetailDto.cs
│       └── RoleDto.cs
│       └── SongSetDto.cs
│   └── Mapping/
│       └── MappingProfile.cs
│
├── MusicRadio.Database/
│   └── StoredProcedure/
│       └── AlbumSets/
│       	└── sp_CreateAlbumSet.sql
│       	└── sp_DeleteAlbumSet.sql
│       	└── sp_UpdateAlbumSet.sql
│       └── SongSets/
│       	└── sp_CreateSongSets.sql
│       	└── sp_DeleteSongSets.sql
│       	└── sp_UpdateSongSets.sql
│
├── MusicRadio.Infrastructure/
│   ├── Data/
│   │   ├── DatabaseSeeder.cs
│   │   ├── MusicRadioDbContext.cs
│   ├── Configurations/
│   │   ├── AlbumSetConfiguration.cs
│   │   ├── ClientConfiguration.cs
│   │   ├── PurchaseDetailConfiguration.cs
│   │   ├── RoleConfiguration.cs
│   │   ├── SongSetConfiguration.cs
│   ├── Services/
│   │   ├── AlbumSetService.cs
│   │   ├── AuthService.cs
│   │   └── ClientService.cs
│   │   ├── PurchaseDetailService.cs
│   │   ├── RoleService.cs
│   │   └── SongSetService.cs
│   │   └── TokenGeneratorService.cs
│   └── DependencyInjection.cs
│
├── MusicRadio.Infrastructure/
│   ├── Common/
│   │   ├── Enums.cs
│   │   ├── OperationResult.cs
│   │   └── PasswordHasher.cs
│   │   ├── StoredProcedureResults.cs
│
├── MusicRadio.Tests /
│   ├── Controllers/
│   │   ├── AlbumsControllerTests.cs
│   ├── Services/
│   │   ├── ClientServiceTests.cs
│	BasePruebas.cs
│
├── MusicRadio.Web/
│   ├── Mapping/
│   │   ├── WebMappingProfile.cs
│   ├── ViewModels/
│   │   ├── AlbumViewModel.cs
│   │   ├── LoginViewModel.cs
│   │   ├── PurchaseDetailViewModel.cs
│   │   ├── RegisterViewModel.cs
│   │   ├── SongsViewModel.cs
│   ├── Pages/
│   │   ├── Albums/
│   │   │   ├── Index.cshtml + Index.cshtml.cs
│   │   │   ├── Create.cshtml + Create.cshtml.cs
│   │   │   ├── Editar.cshtml + Editar.cshtml.cs
│   │   │   └── Delete.cshtml + Delete.cshtml.cs
│   │   ├── Auth/
│   │   │   ├── Login.cshtml + Login.cshtml.cs
│   │   │   ├── Logout.cshtml + Logout.cshtml.cs
│   │   │   ├── Register.cshtml + Register.cshtml.cs
│   │   └── Songs/
│   │   │   ├── Index.cshtml + Index.cshtml.cs
│   │   │   ├── Create.cshtml + Create.cshtml.cs
│   │   │   ├── Editar.cshtml + Editar.cshtml.cs
│   │   │   └── Delete.cshtml + Delete.cshtml.cs
│   │   └── Purchases/
│   │   │   ├── Index.cshtml + Index.cshtml.cs
│   │   │   ├── Create.cshtml + Create.cshtml.cs
│   ├── wwwroot/
│   │   └── css/
│   │   │   ├── auth.css
│   │   │   ├── site.css
│   └── appsettings.json
│   └── Program.cs
│
└── MusicRadio.sln
```

## 🚀 Requisitos Previos
Antes de ejecutar el proyecto, asegúrate de tener instalado lo siguiente:
- Visual Studio 2022 con el paquete de desarrollo de ASP.NET y desarrollo de bases de datos.
- SQL Server 2019 o superior.
- .NET 6 SDK o superior.

## 🚀 Instalación y Ejecución

## 1. 📥 Clonación del Repositorio
Para clonar el repositorio, ejecuta el siguiente comando en tu terminal:
```bash
git clone https://github.com/rony1589/NetCoreRazorEFAuth.git
cd NetCoreRazorEFAuth
```

## 2. Restaurar los paquetes NuGet
dotnet restore


## 3. ⚙️ Configuración de la Conexión a la Base de Datos
La cadena de conexión a la base de datos se encuentra en el archivo appsettings.json dentro del proyecto MusicRadio.Web.

📁 appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=SchoolDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

Reemplaza TU_SERVIDOR con el nombre de tu instancia de SQL Server. Si estás utilizando SQL Server Express, podría ser localhost\\SQLEXPRESS.

## 🧪 Migraciones y Creación de la Base de Datos
Para aplicar las migraciones y crear la base de datos, utiliza la Consola del Administrador de Paquetes en Visual Studio:

Add-Migration InitialCreate
Update-Database

o

dotnet ef database update --project MusicRadio.Infrastructure

## Desde powershell ingresando a la raiz del proyecto:
1. Esto generará las tablas necesarias en la base de datos especificada.
dotnet ef migrations add InitialCreate --project MusicRadio.Infrastructure --startup-project MusicRadio.Web


2. Aplicar migración:
dotnet ef database update --project MusicRadio.Infrastructure --startup-project MusicRadio.Web

3. Aplicar cambios en sp
dotnet run --project MusicRadio.DatabaseCli

## ▶️ Usuarios Login
1. Para comprar álbumes se puede registrar.
2. Para ingresar con un usuario Administrador Inventario usar el siguiente:
   Email: admininventario@music.com
   Password: 1234

## ▶️ Ejecución del Proyecto
Para ejecutar la aplicación:

1. Abre la solución MusicRadio.sln en Visual Studio.

2. Establece MusicRadio.Web como proyecto de inicio.

3. Presiona F5 o haz clic en "Iniciar" para ejecutar la aplicación.

4. La aplicación se abrirá en tu navegador predeterminado mostrando la lista de estudiantes.

## ✅ Pruebas Unitarias
El proyecto MusicRadio.Tests contiene algunas pruebas unitarias para validar la lógica de negocio. Para ejecutarlas:

1. Abre el Explorador de Pruebas en Visual Studio (Prueba > Ventanas > Explorador de pruebas).

2. Haz clic en "Ejecutar todas" para ejecutar las pruebas.

o

dotnet test

## 📦 Publicación
Para publicar la aplicación en un servidor o servicio en la nube:

1. Haz clic derecho en el proyecto MusicRadio.Web y selecciona Publicar.

2. Sigue el asistente para seleccionar el destino de publicación (por ejemplo, Azure, carpeta local, IIS, etc.).

## 📄 Licencia
Este proyecto está bajo la Licencia MIT. Consulta el archivo LICENSE para más detalles.
