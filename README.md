# LibraryApi

REST API desarrollada con **ASP.NET Core** para la gestión de una biblioteca. El sistema permite administrar libros, miembros, préstamos y autenticación de administradores mediante endpoints REST.

## Tecnologías utilizadas

- ASP.NET Core
- C#
- Entity Framework Core
- SQL
- REST API
- MVC / Controllers
- DTOs
- Migrations

## Funcionalidades principales

- Autenticación de administrador.
- Gestión de libros.
- Gestión de miembros.
- Gestión de préstamos.
- Operaciones CRUD mediante endpoints REST.
- Validación básica de datos.
- Separación por controladores, modelos, DTOs y capa de datos.

## Estructura del proyecto

```txt
LibraryApi/
├── Controllers/     # Endpoints REST
├── Data/            # Contexto de base de datos
├── Dtos/            # Objetos de transferencia de datos
├── Helpers/         # Utilidades como validación de contraseña
├── Migrations/      # Migraciones de base de datos
├── Models/          # Entidades principales
├── Program.cs       # Configuración de la aplicación
└── appsettings.json # Configuración del proyecto
```

## Endpoints principales

| Módulo | Endpoint base | Descripción |
|---|---|---|
| Auth | `/api/Auth` | Inicio de sesión de administrador |
| Books | `/api/Books` | CRUD de libros |
| Members | `/api/Members` | CRUD de miembros |
| Loans | `/api/Loans` | Gestión de préstamos |

## Instalación y ejecución

### 1. Clonar el repositorio

```bash
git clone https://github.com/Hhurtadoabcx/LibraryApi.git
cd LibraryApi
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar la base de datos

Edita el archivo `appsettings.json` y configura tu cadena de conexión.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "TU_CADENA_DE_CONEXION"
  }
}
```

### 4. Aplicar migraciones

```bash
dotnet ef database update
```

### 5. Ejecutar la API

```bash
dotnet run
```

La API quedará disponible en la URL local configurada por ASP.NET Core.

## Frontend relacionado

Este backend puede conectarse con el frontend React del proyecto:

🔗 https://github.com/Hhurtadoabcx/library-frontend

## Estado del proyecto

Proyecto académico orientado a practicar desarrollo backend, APIs REST, Entity Framework Core y separación de responsabilidades en ASP.NET Core.

## Aprendizajes

- Diseño de endpoints REST.
- Uso de controladores en ASP.NET Core.
- Modelado de entidades.
- Manejo de base de datos con Entity Framework Core.
- Organización de un backend por capas.
