# Design Patterns and Prototyping in Games – Winter 2026

**Course Project Starter Repository**

> **Professor:** Dr. Rogerio de Leon Pereira
> 
> **Course:** Design Patterns and Prototyping in Games
>
> **Institution:** Sheridan College
>
> **Term:** Winter 2026

This repository contains a Unity project that demonstrates several movement patterns and a simple shooting mechanic. The scripts are organized under `Assets/Scripts` and will be the base for the study and implementation of design patterns such as **Singleton**, **Strategy**, **Component**, **Interface** and others.

## Scripts Overview

| File | Description |
|------|-------------|
| `IDoDamage.cs` | Interface for objects that can deal damage (`Damage`, `ImpactEffect`, `ApplyImpactEffect`). |
| `ITakeDamage.cs` | Interface for objects that can receive damage (`Health`, `NormalizedHealth`, `TakeDamage`). |
| `Bullet.cs` | Implements a projectile that implements `IDoDamage`. Handles lifespan, speed, impact effect, and plays a firing sound. |
| `Brick.cs` | A destructible object implementing `ITakeDamage`. Tracks health, normalised health, and destroys itself when health reaches zero. |
| `MouseUtil.cs` | Utility for converting mouse screen position to world space, visual pointer handling, and toggling tracking with the middle mouse button. |
| `MovementMotorBase.cs` | Base class for movement motors, providing common functionality for movement and rotation. |
| `PlayerController.cs` | Central controller that selects the active movement motor, forwards input, and draws debug gizmos reflecting movement direction (including reverse). |
| `Shooter.cs` | Handles player input for firing bullets, instantiates the bullet prefab at the muzzle, enforces a fire cooldown, and triggers the shooting animation. |
| `LocomotionAnimator.cs` | Syncs animator parameters with the active `MovementMotorBase` (speed, movement state, etc.). |
| `PointAndClickMotor.cs` | Point‑and‑click movement motor. Sets a target on right‑click, moves toward it using capsule‑cast collision detection with sliding, and includes a `StopMovement` helper. |
| `FollowTargetMotor.cs` | Moves the character toward a target `Transform` or position, using the shared `CanMove` capsule‑cast logic with sliding on X/Z axes. |
| `TankMotor.cs` | Tank‑style movement motor (forward/backward + rotation) with capsule‑cast collision detection and sliding logic. |
| `FreeMovementMotor.cs` | WASD‑style movement motor with world‑space input, capsule‑cast collision detection and sliding, and rotation handling. |
| `CameraController.cs` | Controls camera animation based on the selected `CameraMode` (PointAndClick, FollowTarget, Tank, FreeMovement). |

The codebase also implements a minimap and reander objects to show the player and enemy positions when occluded by other objects.

## Getting Started
1. Open the project in Unity (6.3 LTS or later recommended).
2. All the prefabs and references are already assigned in the inspector in the `Assets/Scenes/SampleScene` scene.
3. Press Play – you can test the different movement styles by attaching the corresponding motor component to the player object.

## Design Patterns Highlighted
- **Interface Segregation** – `ITakeDamage` and `IDoDamage` separate damage‑receiving and damage‑dealing responsibilities.
- **Component Pattern** – Unity’s component system is used to compose behaviours (e.g., `Shooter`, `CameraController`).
- **Other patterns:** 
    - Singleton Pattern
    - Observer Pattern
    - Command Pattern
    - Object Pool Pattern
    - Visitor Pattern
    - Strategy Pattern
    - Decorator Pattern
    - State Machine
    - Event Bus