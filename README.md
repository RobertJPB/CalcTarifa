# CalcTarifa - Sistema de Cálculo de Envíos Internacionales

CalcTarifa es una solución integral diseñada para el cálculo preciso de tarifas de envío internacional. La plataforma permite a los usuarios cotizar el costo de sus paquetes basándose en el peso, las unidades de medida y la región de destino, integrando una lógica de negocio robusta y una arquitectura escalable.

## 🚀 Características Principales

- **Cálculo Dinámico por Región**: Implementación de estrategias de costos específicas para regiones clave (India, Estados Unidos, Reino Unido, etc.).
- **Gestión Multi-Unidad**: Soporte nativo para Kilogramos (Kg) y Libras (Lb) con normalización automática en la capa de dominio.
- **Panel Administrativo**: Gestión centralizada de tarifas vigentes por región.
- **Historial de Consultas**: Registro detallado de cálculos realizados para auditoría y consulta de usuarios.
- **Seguridad**: Autenticación robusta y gestión de roles para proteger las funciones administrativas.
- **Interfaz Profesional**: Diseño moderno y responsivo optimizado para la experiencia del usuario.

## 🏗️ Arquitectura del Sistema

El proyecto está construido siguiendo los principios de **Clean Architecture** (Arquitectura Limpia), lo que garantiza un bajo acoplamiento y una alta testabilidad.

### Capas del Proyecto

1.  **CalcTarifa.Domain**: El corazón del sistema. Contiene las entidades de negocio, objetos de valor (como `PesoKg` y `PesoLb`) y la lógica de cálculo pura. No tiene dependencias externas.
2.  **CalcTarifa.BusinessApplication**: Implementa los casos de uso del sistema. Orquestra el flujo de datos entre el dominio y las capas externas a través de interfaces y DTOs.
3.  **CalcTarifa.Infrastructure**: Detalles de implementación técnica. Incluye la persistencia de datos con **Entity Framework Core**, repositorios SQL Server y servicios externos.
4.  **CalcTarifa.API**: Capa de servicios REST que expone la lógica de negocio a clientes externos, gestionando la autenticación JWT.
5.  **CalcTarifa.Web**: Portal web desarrollado en ASP.NET Core MVC que ofrece la interfaz de usuario para clientes y administradores.

## 🧠 Lógica de Dominio y Patrones

El sistema destaca por el uso de patrones de diseño avanzados:

- **Pattern Strategy**: Utilizado para resolver el cálculo de tarifas de forma dinámica según la región, permitiendo extender el sistema a nuevos países sin modificar la lógica existente.
- **Value Objects**: El manejo de pesos se realiza mediante objetos de valor que encapsulan las reglas de validación y conversión, asegurando la integridad de los datos en todo el ciclo de vida de la solicitud.

## 🛠️ Stack Tecnológico

- **Backend**: .NET 8, C#.
- **Persistencia**: Entity Framework Core, SQL Server.
- **Seguridad**: ASP.NET Core Identity, JSON Web Tokens (JWT).
- **Mapeo y Validación**: AutoMapper, FluentValidation.
- **Frontend**: ASP.NET Core MVC, Vanilla CSS (fuente Outfit), JavaScript.

## ⚙️ Instalación y Configuración

### Requisitos Previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB o Express)

### Pasos para la ejecución
1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/tu-usuario/calculadora-tarifa.git
   ```
2. **Configurar la base de datos**:
   Actualiza la cadena de conexión en `appsettings.json` dentro de los proyectos `CalcTarifa.API` y `CalcTarifa.Web`.
3. **Aplicar Migraciones**:
   Ejecuta el siguiente comando en la consola del administrador de paquetes o mediante la CLI de dotnet:
   ```bash
   dotnet ef database update --project CalcTarifa.Infrastructure --startup-project CalcTarifa.API
   ```
4. **Ejecutar la aplicación**:
   Puedes iniciar ambos proyectos (`Web` y `API`) simultáneamente desde Visual Studio o usando `dotnet run`.
