# Vengeance

A 3D Action game built in Unity, combining fast-paced melee combat with tactical gunplay. Developed using the Universal Render Pipeline (URP), the game features a complete scene-based flow (Intro → Main Menu → Gameplay → Victory/GameOver), a custom weapon pickup system, and a wave-based enemy AI framework.

## 🎮 Features

*   **Dual Combat Systems:** Seamlessly switch between melee attacks (driven by animation events and trigger colliders) and ranged gunplay (raycast-based shooting with bullet spread and recoil).
*   **Enemy AI (FSM):** Enemies use a Finite State Machine (FSM) to patrol, chase, and attack the player using Unity's NavMesh and Animator StateMachineBehaviours.
*   **Weapon Pickup System:** A center-screen raycast system detects nearby weapons, highlights them using the QuickOutline asset, and allows the player to pick them up and swap between weapon slots.
*   **Wave Progression:** A Game Manager spawns waves of guards and dynamically tracks live enemies, triggering the next scene transition when the area is cleared.
*   **Directional Hit Reactions:** A custom Health System calculates the angle of incoming damage to play front or back hit animations on enemies and bosses.
*   **Cinematic End States:** Custom managers handle video playback for Game Over and Victory screens, pulling final scores and stats from `PlayerPrefs`.

## 📸 Screenshots

*(See the images below for gameplay, UI, and combat demonstrations)*

<img width="1920" height="1080" alt="Screenshot 2026-11-11 225906" src="https://github.com/user-attachments/assets/710e749d-5d9d-4432-a875-b03a4dd3da58" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225843" src="https://github.com/user-attachments/assets/a0980d9a-a3c8-434c-97c0-c867513a4077" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225823" src="https://github.com/user-attachments/assets/0c4118b0-d81d-4d82-8d68-355a6203a846" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225708" src="https://github.com/user-attachments/assets/31019272-07f7-462f-9e8d-54f4502b9df7" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230501" src="https://github.com/user-attachments/assets/fdf4864e-5d27-4554-85f8-33d670f537dc" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230442" src="https://github.com/user-attachments/assets/be7d91d3-1115-438d-888d-cd3e63a9994e" />

## 🛠️ Technologies & Tools

*   **Engine:** Unity (URP - Universal Render Pipeline)
*   **Language:** C#
*   **Input:** Unity Input System
*   **AI:** Unity AI Navigation (NavMesh)
*   **Camera:** Cinemachine
*   **UI/Visuals:** Unity UI (uGUI), Post Processing, Shader Graph, Timeline.
*   **Third-Party Assets:** Unity Particle Pack, Low Poly Weapons VOL.1, QuickOutline.

## 🧠 Technical Architecture Highlights

*   **Component-Based Design:** Gameplay logic is heavily modularized into `MonoBehaviour` components (e.g., `HealthSystem`, `Weapon`, `DealDamage`) allowing for reusable and attachable behaviors.
*   **Animation Events:** Melee hitboxes are disabled by default and are only toggled on during specific frames of the attack animation using C# Animation Events. A `HashSet` prevents a single swing from hitting the same enemy twice.
*   **Singletons:** Global systems like `GameManager`, `SoundManager`, and `AmmoManager` use the Singleton pattern to manage cross-scene data and audio.
*   **Coroutines:** Used extensively for timed operations, such as bullet self-destruction, weapon reloading delays, and sequential video/audio playback in the intro.

## 🚀 How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/Zizamele03/Vengence.git
