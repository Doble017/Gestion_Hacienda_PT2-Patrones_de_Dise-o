
Archivos
Sistema de gestión de hacienda ganadera orientado a dominio, con arquitectura en capas y patrones de diseño aplicados sobre un diseño previo alineado a SOLID.

Curso: Arquitectura de Software
Evolución: del diseño correcto (SOLID) al diseño robusto (patrones)
Solicitud de cambio implementada (Reto 2): SC-1 — venta de productos derivados del ganado (carne, piel, lácteos)

Estructura de la solución
Hacienda_TOBE/
├── Hacienda.Domain          # Entidades, factories, strategies, puertos, reglas, eventos
├── Hacienda.Application     # Casos de uso (AppServices) + Fachada
├── Hacienda.Infrastructure  # Repositorios en archivos + DI (composition root)
├── Hacienda.Web             # ASP.NET Core MVC
├── Hacienda.Consola         # Cliente de consola (misma Application/Infrastructure)
├── Hacienda.Tests           # Pruebas unitarias
└── HaciendaTOBE.sln
Regla de dependencia:

Presentation (Web / Consola)
        → Application
            → Domain
Infrastructure → Domain   (adaptadores de persistencia y eventos)
El composition root está en Hacienda.Infrastructure/DependencyInjection.cs (registrado desde Program.cs de Web/Consola).

Requisitos
.NET 8 SDK

Sistema operativo con soporte para dotnet (Windows, Linux o macOS)

dotnet --version   # debe ser 8.x
Cómo ejecutar
Consola
cd Hacienda_TOBE/Hacienda.Consola
dotnet run
Web (MVC)
cd Hacienda_TOBE/Hacienda.Web
dotnet run
Abrir la URL que indique la consola (típicamente https://localhost:5xxx o http://localhost:5xxx).

Login de prueba (si aplica en su entorno): según datos en Hacienda.Web/Datos/Usuarios.txt (ejemplo histórico del proyecto: usuario de demostración cargado en ese archivo).

Pruebas
cd Hacienda_TOBE
dotnet test
Compilar toda la solución
cd Hacienda_TOBE
dotnet build HaciendaTOBE.sln
Persistencia
Almacenamiento en archivos de texto (sin base de datos relacional), bajo la carpeta de datos de la aplicación web:


}
