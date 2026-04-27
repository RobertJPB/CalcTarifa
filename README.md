# CalcTarifa - Sistema de Cálculo de Envíos Internacionales

CalcTarifa es una solución integral diseñada para el cálculo preciso de tarifas de envío internacional. La plataforma permite a los usuarios cotizar el costo de sus paquetes basándose en el peso, las unidades de medida y la región de destino, integrando una lógica de negocio robusta y una arquitectura escalable.

##  Características Principales

- **Cálculo por Región**: Implementación de estrategias de costos específicas para regiones clave (India, Estados Unidos, Reino Unido, etc.).
- **Gestión de Unidades**: Soporte nativo para Kilogramos (Kg) y Libras (Lb) con normalización automática en la capa de dominio.
- **Panel Administrativo**: Gestión centralizada de tarifas vigentes por región.
- **Historial de Consultas**: Registro detallado de cálculos realizados para auditoría y consulta de usuarios.
- **Seguridad**: Autenticación robusta y gestión de roles para proteger las funciones administrativas.
- **Interfaz Profesional**: Diseño moderno y responsivo optimizado para la experiencia del usuario.

##  Arquitectura del Sistema

El proyecto está construido siguiendo los principios de Arquitectura Limpia, lo que garantiza un bajo acoplamiento y una alta testabilidad.

### Capas del Proyecto

1.  **CalcTarifa.Domain**: El corazón del sistema. Contiene las entidades de negocio, objetos de valor (como `PesoKg` y `PesoLb`) y la lógica de cálculo pura. No tiene dependencias externas.
2.  **CalcTarifa.BusinessApplication**: Implementa los casos de uso del sistema. Orquestra el flujo de datos entre el dominio y las capas externas a través de interfaces y DTOs.
3.  **CalcTarifa.Infrastructure**: Detalles de implementación técnica. Incluye la persistencia de datos con **Entity Framework Core**, repositorios SQL Server y servicios externos.
4.  **CalcTarifa.API**: Capa de servicios REST que expone la lógica de negocio a clientes externos, gestionando la autenticación JWT.
5.  **CalcTarifa.Web**: Portal web desarrollado en ASP.NET Core MVC que ofrece la interfaz de usuario para clientes y administradores.

##  Lógica de Dominio y Patrones

El sistema destaca por el uso de patrones de diseño avanzados:

- **Pattern Strategy**: Utilizado para resolver el cálculo de tarifas de forma dinámica según la región, permitiendo extender el sistema a nuevos países sin modificar la lógica existente.
- **Value Objects**: El manejo de pesos se realiza mediante objetos de valor que encapsulan las reglas de validación y conversión, asegurando la integridad de los datos en todo el ciclo de vida de la solicitud.

## Stack Tecnológico
- **Backend**: .NET 8, C#.
- **Persistencia**: Entity Framework Core, SQL Server.
- **Seguridad**: ASP.NET Core Identity, JSON Web Tokens (JWT).
- **Mapeo y Validación**: AutoMapper, FluentValidation.
- **Frontend**: ASP.NET Core MVC, Vanilla CSS (fuente Outfit), JavaScript.


## Principios SOLID Aplicados:
S (Responsabilidad Única): Cada capa y clase tiene una función clara. Los Value Objects (PesoKg) validan, mientras que los Use Cases orquestan el negocio.
O (Abierto/Cerrado): Gracias al Patrón Strategy, puedes añadir nuevas regiones de envío (ej. CalculoJapon) creando una clase nueva, sin modificar el código existente.
L (Sustitución de Liskov): Todas las estrategias de cálculo implementan ICalculoTarifaStrategy, permitiendo que el sistema las use indistintamente sin errores.
I (Segregación de Interfaces): Las interfaces son pequeñas y específicas (ej. ITaxRateResolver), evitando que las clases implementen métodos que no necesitan.
D (Inversión de Dependencias): La lógica de negocio depende de interfaces, no de bases de datos. La infraestructura se inyecta, facilitando cambios tecnológicos futuros.





