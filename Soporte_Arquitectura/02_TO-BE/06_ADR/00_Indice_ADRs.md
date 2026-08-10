# Índice de Registros de Decisión Arquitectónica (ADR)

Mínimo exigido: **cinco** decisiones estructurales. Cada ADR incluye:

1. Contexto y evidencia (hallazgo del inventario)
2. Al menos **dos** alternativas evaluadas (una descartada de forma explícita)
3. Decisión tomada
4. Costo / consecuencia negativa aceptada
5. Principio(s) SOLID involucrado(s)

| ID | Título | Principios | Hallazgo(s) |
|----|--------|------------|-------------|
| ADR-01 | Partición de la God Class `Hacienda` | SRP, ISP, DIP | H-02, H-03 |
| ADR-02 | Puertos de persistencia por agregado | DIP, SRP | H-05 |
| ADR-03 | Segregación de interfaces de aplicación | ISP, DIP | H-02 |
| ADR-04 | Extracción de publishers a adaptadores | DIP, OCP | H-01, H-04 |
| ADR-05 | Conservar jerarquías `Res` y `Vacuna` | LSP, OCP | H-06 (refutación) |
