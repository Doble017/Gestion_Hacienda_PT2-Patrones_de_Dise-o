---
name: srp-evaluator
user-invocable: true
description: "Evaluate classes for the Single Responsibility Principle (SRP) from SOLID and recommend responsibility separation and refactoring."
---

# SRP Evaluation Skill

Use this skill to assess whether a class follows the Single Responsibility Principle and to generate targeted refactorings.

## What this skill evaluates
- Multiple responsibilities inside the same class
- Excessive cohesion issues and high coupling
- Mixed concerns across business logic, persistence, presentation, and validation
- Dependencies on components from different functional domains
- Overly long methods that should be split into smaller units
- Classes with too many fields or methods as candidates for fragmentation

## Evaluation protocol
1. Each class must have a single reason to change.
2. Each method must fulfill one clearly defined function.
3. Business logic, persistence, presentation, and validation should not be mixed inside the same class.
4. A class must not depend on components from different functional domains.
5. Excessively long methods should be divided into smaller, focused units.
6. Classes with too many attributes or methods should be considered for splitting.
7. Functional cohesion should remain high.
8. Provide concrete refactoring recommendations and responsibility redistribution.

## Recommended usage
- Analyze Java classes for SOLID SRP compliance.
- Identify opportunities to create new classes, services, interfaces, or modules.
- Recommend refactoring plans when a class is doing too much.

> Example: "Review this class for SRP violations and propose a refactor that separates responsibilities into services, interfaces, or modules."
