# CalcTarifa - Sistema de Calculo de Envios

Este proyecto es una aplicacion web diseñada para el calculo preciso de tarifas de envio internacional, permitiendo a los usuarios cotizar el costo de sus paquetes basandose en el peso y la region de destino.

## Arquitectura del Sistema

La solucion sigue los principios de Clean Architecture, separando las responsabilidades en capas bien definidas para asegurar la escalabilidad y mantenibilidad del codigo.

### Capas del Proyecto

1. CalcTarifa.Domain: El nucleo del sistema. Contiene las entidades de negocio, objetos de valor (Value Objects) como PesoKg, y las interfaces de los servicios de dominio. Aqui se implementa la logica de calculo mediante polimorfismo y estrategias por region.
2. CalcTarifa.BusinessApplication: Contiene los casos de uso (Use Cases) que orquestan el flujo de datos. Define los DTOs para la comunicacion entre capas y las interfaces de persistencia.
3. CalcTarifa.Infrastructure: Implementacion de los detalles tecnicos. Incluye el acceso a datos mediante Entity Framework Core, la gestion de la base de datos SQL Server y la implementacion del patron Repository y Unit of Work.
4. CalcTarifa.API: Expone la funcionalidad del negocio a traves de endpoints REST. Gestiona la autenticacion JWT y la validacion de solicitudes.
5. CalcTarifa.Web: Interfaz de usuario desarrollada en ASP.NET Core MVC. Ofrece una experiencia moderna y responsiva, permitiendo el calculo de envios en tiempo real y la gestion administrativa de tarifas.

## Caracteristicas Principales

- Calculo dinamico por region (India, Estados Unidos, Reino Unido).
- Soporte para multiples unidades de peso (Kilogramos y Libras) con conversion automatica en el dominio.
- Panel de administracion para la gestion de tarifas vigentes.
- Historial de consultas para usuarios registrados.
- Diseño minimalista y profesional utilizando la fuente Outfit.

## Tecnologias Utilizadas

- Backend: .NET 8, Entity Framework Core, SQL Server.
- Frontend: ASP.NET Core MVC, CSS3 (Vanilla), JavaScript (Vanilla).
- Seguridad: ASP.NET Core Identity, JWT (JSON Web Tokens).
- Arquitectura: Clean Architecture, patron Strategy, Repository y Unit of Work.
