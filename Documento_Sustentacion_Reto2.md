# Reto 2 — Documento de sustentación
**HaciendaSoft TO-BE · SC-1 Productos derivados**  
Patrones: Factory Method (reses y productos) · Strategy (cobro) · Facade

---

## 1. Puntos de dolor

| ID | Dónde | Qué lo hace rígido | Costo | Pri. |
|----|-------|-------------------|-------|------|
| P-01 | Venta + VentaAppService | Solo venta de res | ~6 archivos | Alta |
| P-02 | VentaAppService | Ítems no extensibles | servicio + UI | Alta |
| P-03 | DefaultResFactory | Switch por tipo | ~3 archivos | Media |
| P-04 | DefaultResPolicy | Switches pesos/vacunas | 1 clase | Media |
| P-05 | Cobro en ventas | Sin régimen intercambiable | servicio + UI | Alta |
| P-06 | Controllers | Varios AppServices | 3–4 controllers | Media |
| P-07 | Inventario productos | Stock completo (no intervenido) | alto | No intervenir |

## 2. Decisión de patrones

**Adoptados:** Factory Method productos · Factory Method reses · Strategy cobro · Facade  

**Descartados:** Simple Factory (switch) · Abstract Factory · Strategy sesión · Builder  

|       Patrón evaluado      |   Familia  | Punto de dolor que que ataca |      que gana / que cuesta | Decisión | Por qué |
|----------------------------|------------|------------------------------|----------------------------------------|----------|---------|
| Factory Method (Productos) | Creacional |   P-01, P-02     | Extensión sin abrir servicio / +clases | Adoptado | SC-1 exige crear tipos de mercancía sin modificar el consumidor |
| Factory Method (Reses) | Creacional |   P-03    | EOCP real en alta de res /+3 factories + provider | Adoptado | Elimina switch de DefaultResFactory; simétrico a productos|
| Strategy (Cobro) | Comportamiento |   P-05     | Cambiar régimen IVA/dto sin tocar venta / +indirección | Adoptado |Migración a otras reglas de cobro; UI elige estrategia|
| Facade | Estructural |   P-06     | UI con un solo punto de acceso / riesgo SRP si engorda | Adoptado |Solo delega a AppServices; límite declarado|
| Abstract Factory | Creacional |   P-01     | Familias de productos / abstracción de más | Descartado |Los productos manejados no son familias |
| Strategy (sesion) | Comportamiento |   -     | Algoritmos de sesión / sobre-ingeniería | Descartado | No ataca un punto de dolor establecido |
| Builder | Creacional |   -     | No aporta un valor real a la implementacion | Descartado | La creacion de productos no sigue una secuencia de pasos |

### Bitácora (extracto)

### Tabla : Decisiones sobre derivados, fábricas y ventas

| ID    | Consulta                          | Propuesta IA                                   | Acción           | Argumento / evidencia                                                   |
|-------|-----------------------------------|------------------------------------------------|------------------|-------------------------------------------------------------------------|
| B-01  | Cómo vender derivados             | Meter tipos en jerarquía Res                   | Rechazado        | Carne/piel/lácteo no son animales (P-01, dominio)                      |
| B-02  | Fábrica de productos              | Simple Factory con switch                      | Idea nuestra/ Corregido     | OCP: se usó Factory Method + Provider por diccionario                  |
| B-03  | ¿Una sola fábrica para res y producto? | Jerarquía unificada                         | Rechazado        | Motivos de cambio distintos; se separaron                              |
| B-04  | DefaultResFactory                 | Dejar el switch                                | Corregido        | P-03: Termero/Cebon/NovilloFactory + ResFactoryProvider                |
| B-05  | Ampliar Venta vs VentaProducto    | Entidad paralela                               | Idea nuestra | Se amplió Venta con discriminador; menos ruptura                       |
| B-06  | Strategy para sesión              | Aplicar Strategy en auth                       | Idea nuestra /Rechazado | Sin punto de dolor; sobre-ingeniería                                   |
| B-07  | Cálculo de cobro                  | Strategy IVA/descuento                         | Idea Nuestra/ aceptado | P-05: ICalculadora + Nacional/Internacional/...                        |
| B-08  | IVA fijo vs parámetro             | Constante en clase                             | Aceptado/ajustado | Nacional 19%; ConDescuento % por ctor (0–50)                           |
| B-09  | Facade en controllers             | Fachada con lógica                             | Corregido        | IHaciendaFachada solo delega (SRP)                                     |
| B-10  | Abstract Factory productos        | Usar Abstract Factory                          | Rechazado        | Una familia de productos; Anexo A                                      |



## 3. TO-BE

**Entra:** productos + factories; factories res + providers; ICalculadora + 4 estrategias; IHaciendaFachada; VenderProductoAsync.  

**Fichas:** Factory productos (SC-1) · Factory reses (OCP) · Strategy (P-05) · Facade (P-06, solo delega).

### Tabla: tabla de cambio estructural.

| ID    | Elemento                                    | Estado          | Antes → Ahora                                                                 | Reconexión                                                                 |
|-------|---------------------------------------------|-----------------|-------------------------------------------------------------------------------|----------------------------------------------------------------------------|
| E-01  | ProductoVendible + 3 clases                 | Entra           | —→ mercancía vendible                                                         | Factories de producto; Venta ctor producto                                 |
| E-02  | IProductoFactory + 3 Provider               | Entra           | —→ Factory Method productos                                                   | VentaAppService / Fachada                                                  |
| E-03  | IResFactory concreto + Provider             | Entra / transforma | DefaultResFactory switch → factories por tipo                               | Potrero, PotreroAppService, FilePotreroRepository                          |
| E-04  | Venta                                       | Se transforma   | Solo res → res o producto (EsVentaDeProducto)                                 | VentaAppService, FileVentaRepository                                       |
| E-05  | ICalculadora + estrategias + Provider       | Entra           | Monto crudo → régimen intercambiables                                         | VentaAppService (estrategiaCobro)                                          |
| E-06  | IHaciendaFachada                            | Entra           | UI multi-servicio → un puerto                                                 | Controllers Web                                                            |
| E-07  | VentaAppService / IVentaAppService          | Se transforma   | + VenderProductoAsync + cobro                                                 | Fachada / Controller                                                       |
| E-08  | DependencyInjection                         | Se transforma   | Registro factores, strategies, fachada                                        | Composition root                                                           |


### Fichas de patrones adoptados

#### Ficha A — Factory Method (productos derivados)

| Campo         | Contenido                                                                                                                                      |
|---------------|------------------------------------------------------------------------------------------------------------------------------------------------|
| **Patrón y dolor** | Factory Method. P-01/P-02. Venta y VentaAppService no permitían extender mercancía sin modificar el servicio.                                   |
| **Alternativas**   | 1) Simple Factory con switch (descartada: OCP). 2) No hacer nada (no cumple SC-1). 3) Abstract Factory (una familia, no justifica).            |
| **Sale / entra**   | Entra ProductoVendible, ProductoCarne/Piel/Lacteo, IProductoFactory, 3 factories, ProductoFactoryProvider.                                      |
| **Relaciones**     | VentaAppService → IProductoFactoryProvider → IProductoFactory → ProductoVendible → Venta. Independiente de IResFactory.                         |
| **Impacto**        | ~10 clases nuevas de producto/factory. SC-1 habilitada. Venta de res intacta.                                                                  |
| **Costo**          | Más clases e indirección Provider→Factory→Producto.                                                                                            |
| **Origen**         | Equipo + contraste con código; IA propuso fábrica, se corrigió a Method + diccionario (B-02).                                                  |

#### Ficha B — Factory Method (reses)

| Campo         | Contenido                                                                                                                                      |
|---------------|------------------------------------------------------------------------------------------------------------------------------------------------|
| **Patrón y dolor** | Factory Method. P-03. DefaultResFactory con switch; nuevo tipo de res abría la fábrica.                                                         |
| **Alternativas**   | 1) Dejar Simple Factory (descartada). 2) No intervenir (deuda OCP). 3) Mezclar con factory de productos (rechazado: SRP).                      |
| **Sale / entra**   | Sale DefaultResFactory. Entra TemeroFactory, CebonFactory, NovilloFactory, IResFactoryProvider, ResFactoryProvider.                             |
| **Relaciones**     | PotreroAppService → Provider → IResFactory → Res. Potrero recibe IResFactory en constructor.                                                    |
| **Impacto**        | Alta de res OCP. Persistencia rehidrata con provider. Tests actualizados.                                                                      |
| **Costo**          | Más tipos; DI y repositorio deben conocer el provider.                                                                                         |
| **Origen**         | Decisión de equipo al auditar P-03 (B-04).                                                                                                     |

#### Ficha C — Strategy (cálculo de cobro)

| Campo         | Contenido                                                                                                                                      |
|---------------|------------------------------------------------------------------------------------------------------------------------------------------------|
| **Patrón y dolor** | Strategy. P-05. Cobro rígido (monto ingresado o cantidad*precio) sin régimen intercambiable.                                                    |
| **Alternativas**   | 1) if/switch de régimen en VentaAppService (OCP). 2) No hacer nada. 3) Strategy en sesión (sin dolor, descartado).                             |
| **Sale / entra**   | Entra ICalculadora, ResultadoCobro, SinDescuento, ConDescuento, Nacional (IVA 19%), Internacional (0% IVA), ICalculadoraProvider, CalculadoraProvider. |
| **Relaciones**    | VentaAppService obtiene ICalculadora vía provider según nombre; Calcular(subtotal) → Total a Venta. UI pasa estrategiaCobro.                   |
| **Impacto**       | Default equivalente a cobro simple. Nuevos regímenes = nueva clase + registro DI.                                                              |
| **Costo**         | Indirección; el cliente debe elegir o aceptar default.                                                                                         |
| **Origen**        | Monitor del curso + P-05; equipo acotó a cobro de ventas (B-07, B-08).                                                                         |

#### Ficha D — Facade (aplicación)

| Campo         | Contenido                                                                                                                                      |
|---------------|------------------------------------------------------------------------------------------------------------------------------------------------|
| **Patrón y dolor** | Facade. P-06. Controllers dependían de varios I*AppService.                                                                                    |
| **Alternativas**   | 1) Dejar multi-inyección. 2) Facade con lógica de negocio (rechazada: rompe SRP, aviso Anexo A).                                               |
| **Sale / entra**   | Entra IHaciendaFachada y HaciendaFachada. Controllers (p. ej. VentaController) dependen de la fachada.                                         |
| **Relaciones**     | Controller → IHaciendaFachada → IPotrero/IRes/IVenta/IVacunacion/IUsuario AppService. Cero reglas en la fachada.                               |
| **Impacto**        | UI desacoplada del grafo interno. Un solo puerto de aplicación.                                                                               |
| **Costo**          | Capa extra; riesgo de engorde si no se respeta el límite.                                                                                     |
| **Origen**         | Sugerencia de monitor; equipo impuso delegación pura (B-09).                                                                                  |


## 4. Garantía de SOLID y del comportamiento


### 4.1 Matriz de verificación

| Patrón                        | SRP         | OCP         | LSP         | ISP         | DIP         |
|-------------------------------|-------------|-------------|-------------|-------------|-------------|
| Factory Method productos      | Refuerza    | Refuerza    | Neutro      | Neutro      | Refuerza    |
| Factory Method reses          | Refuerza    | Refuerza    | Neutro      | Neutro      | Refuerza    |
| Strategy cobro                | Refuerza    | Refuerza    | Neutro      | Neutro      | Refuerza    |
| Facade                        | Tensionado pero compensado | Neutro | Neutro | Neutro | Refuerza |

**Evidencias**:
• SRP factories/strategy: cada clase concreta una responsabilidad de creación o de régimen de cobro.
• OCP: nuevo producto/res/régimen = nueva clase + registro en DI; no se modifica VentaAppService ni providers.
• DIP: servicios dependen de IProductoFactoryProvider, IResFactoryProvider, ICalculadoraProvider, IHaciendaFachada.
• Facade / SRP: tensionado por ser punto único; compensado porque solo delega (sin reglas de dominio). Declarado en ficha D.
• LSP Strategy (*): todas implementan Calcular(subtotal) y devuelven ResultadoCobro válido; sustituibles.

### 4.2 Comportamiento observable
Con estrategia SinDescuento (o equivalente a total = subtotal):
• Venta de res: mismo flujo de remoción de potrero y registro; monto final = monto base si no hay IVA/dto.
• Venta de producto: subtotal = cantidad × precioUnitario; con SinDescuento el total coincide con el cálculo previo.
• Casos previos de chips, vacunas, potreros y reses no se alteran por los patrones de venta/cobro.
• Casos nuevos: venta Carne/Piel/Lácteo; cobro Nacional (IVA 19%); cobro Internacional (0%); ConDescuento.
En la sustentación: ejecutar al menos los casos del Reto 1 más cuatro caminos que toquen factories, strategy y fachada; mostrar salidas lado a lado.


## 5. Riesgos

R-01 Fachada con lógica · R-02 Producto sin DI · R-03 Cambio default cobro · R-04 Formato ventas.

### 5. Análisis de riesgos

| ID    | Riesgo                                                                                                                                 | P   | I   | Exp | Mitigación                                                                                                                           | Señal de alerta                                                                                                                      |
|-------|----------------------------------------------------------------------------------------------------------------------------------------|-----|-----|-----|--------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------|
| R-01  | Si la fachada absorbe reglas de negocio, entonces se rompe SRP y se vuelve inmantenible                                                | 3   | 4   | 12  | Regla de equipo: solo delegación; revisión en PR                                                                                    | Métodos nuevos en HaciendaFachada sin pasar por AppService                                                                           |
| R-02  | Si se agrega producto solo en UI sin factory/registro DI, entonces falla en runtime                                                    | 2   | 4   | 8   | Guía de dónde tocar; checklist de registro en DI                                                                                    | NotSupportedException del ProductoFactoryProvider                                                                                    |
| R-03  | Si la estrategia de cobro por defecto cambia sin avisar, entonces montos históricos no cuadran                                         | 2   | 5   | 10  | Default SinDescuento; documentar régimen en cada venta si aplica                                                                    | Diferencias en reportes de monto total vs subtotal esperado                                                                          |
| R-04  | Si FileVentaRepository no distingue PROD vs res, entonces se corrompe el archivo de ventas                                             | 2   | 4   | 8   | Formato PROD]... y lectura dual ya implementada; tests de persistencia                                                              | Líneas ilegibles o ventas de producto como res vacía                                                                                |

## 6. Dos vistas

### 6.1 Vista para el negocio (Líder Técnica / presupuesto)

**Qué se le hace al sistema** y qué no cambia. Se habilita la venta de productos derivados del ganado (carne, piel, lácteos) además de la venta
de animales completos. No se cambia de plataforma, ni de base de datos, ni se parte el sistema en microservicios. Las operaciones de
potreros, vacunación y chips siguen igual.

**Dónde se iba el tiempo y el dinero**. Cada vez que el negocio pedía un tipo nuevo de lo que se vende, había que abrir y modificar el mismo
núcleo de ventas. Eso alargaba el tiempo de respuesta a solicitudes y elevaba el riesgo de romper la venta de animales, que ya funcionaba.

**Qué gana el negocio**. Respuesta más rápida a un nuevo producto derivado: se agrega el tipo sin reescribir el módulo de ventas. Se puede
elegir cómo se cobra (sin ajuste, con descuento, régimen nacional con impuesto, régimen de exportación sin impuesto local) sin reprogramar el
flujo de venta. Un solo frente de uso para las operaciones del día a día reduce confusión en soporte.

**Qué cuesta**. Más piezas en el diseño (más archivos que conocer) y un poco más de indirección al depurar. Capacitación breve al equipo de
desarrollo sobre “dónde tocar” al agregar un producto o un régimen de cobro.

**Riesgos en lenguaje de operación**. Si alguien mete reglas de negocio en el punto único de entrada de la aplicación, ese punto se vuelve
cuello de botella. Si se cambia el régimen de cobro por defecto sin avisar, los totales pueden no cuadrar con lo esperado. Señales: errores al
vender un tipo nuevo no registrado; diferencias inexplicables en montos.

**Qué se necesita del negocio**. Confirmar los regímenes de cobro vigentes (porcentajes) y priorizar el próximo producto derivado si no es carne,
piel o lácteo. Validar en piloto que una venta de res y una de producto generan el mismo tipo de comprobante operativo que hoy.

**Qué pasa si no se hace**. Cada solicitud de nuevo producto o de nueva forma de cobro sigue abriendo el núcleo de ventas, con mayor tiempo
de entrega y mayor probabilidad de regresión en la venta de animales.

### 6.2 Vista para el equipo de desarrollo
**Patrones y ubicación**

• Factory Method productos: Domain/Factories (IProductoFactory, Carne/Piel/LacteoFactory, ProductoFactoryProvider) + Entities/Producto*.
• Factory Method reses: Domain/Factories (IResFactory, Ternero/Cebon/NovilloFactory, ResFactoryProvider); Potrero recibe IResFactory.
• Strategy cobro: Domain/Strategies (ICalculadora, SinDescuento, ConDescuento, Nacional, Internacional, CalculadoraProvider).
• Facade: Application IHaciendaFachada / HaciendaFachada; Web Controllers dependen de la fachada.

**Dónde se ensambla**: Infrastructure/DependencyInjection.cs (registro de factories, strategies, providers, AppServices, fachada).
**Reglas que no se deben romper**: no meter lógica de dominio en la fachada; no agregar producto solo en la UI sin factory + registro DI; no reintroducir switch
de creación en providers; no mezclar factory de res con factory de producto.
**Deuda declarada**: DefaultResPolicy aún con switches (P-04); sin inventario/stock de productos; formato archivo de ventas dual (res / PROD).


---

