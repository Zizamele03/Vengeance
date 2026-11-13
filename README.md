
---

```markdown
# Vengeance

A Unity 3D action game project combining melee combat and gunplay, built on the Universal Render Pipeline (URP). The game is structured around a scene-based flow: an intro sequence, a main menu, two gameplay scenes, and game-over/victory end states.

## Table of Contents

- [Author and Project Information](#author-and-project-information)
- [Screenshots and Media](#screenshots-and-media)
- [Features](#features)
- [Technologies and Tools](#technologies-and-tools)
- [Gameplay and Controls](#gameplay-and-controls)
- [Project Architecture](#project-architecture)
- [Installation and Setup](#installation-and-setup)
- [Running the Project](#running-the-project)
- [Technical Implementation](#technical-implementation)


## Author and Project Information

- **Authors:** Zizamele Nyawo, Sibongeleni Magudulela, Sokwani Ntombela, Thandokazi Phunzana & Joel
- **Repository:** [Zizamele03/Vengence](https://github.com/Zizamele03/Vengence)
- Third-party assets (Unity Particle Pack, Low Poly Weapons VOL.1, QuickOutline) remain the property of their respective creators.


## Screenshots and Media

<img width="1920" height="1080" alt="Screenshot 2026-11-11 225906" src="https://github.com/user-attachments/assets/710e749d-5d9d-4432-a875-b03a4dd3da58" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225843" src="https://github.com/user-attachments/assets/a0980d9a-a3c8-434c-97c0-c867513a4077" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225823" src="https://github.com/user-attachments/assets/0c4118b0-d81d-4d82-8d68-355a6203a846" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225708" src="https://github.com/user-attachments/assets/31019272-07f7-462f-9e8d-54f4502b9df7" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230501" src="https://github.com/user-attachments/assets/fdf4864e-5d27-4554-85f8-33d670f537dc" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230442" src="https://github.com/user-attachments/assets/be7d91d3-1115-438d-888d-cd3e63a9994e" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230436" src="https://github.com/user-attachments/assets/095e328c-1880-4150-aa31-b0b6efb06b8b" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230420" src="https://github.com/user-attachments/assets/d20d9b46-86e2-414c-9e43-9af9cf7483de" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 230404" src="https://github.com/user-attachments/assets/377334a1-e04d-4030-b645-5691525506bb" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225941" src="https://github.com/user-attachments/assets/9f60cefd-bdcb-43e3-8e36-68269404fc93" />
<img width="1920" height="1080" alt="Screenshot 2026-11-11 225921" src="https://github.com/user-attachments/assets/ffac2158-4082-452d-a1b0-d2d9ae44c359" />




## Features

| Feature | Description |
|---|---|
| Scene-based game flow | Dedicated scenes for intro, main menu, two gameplay levels, game over, and victory (`Assets/Scenes/`). |
| Intro sequence | `IntroWithVengeanceAudio` plays a `VideoPlayer` clip with intro audio, then a second audio track, then loads the main menu scene (`Assets/Intro.cs`). |
| Melee damage system | `DealDamage` uses a trigger collider toggled by animation events (`EnableDamage`/`DisableDamage`), applies damage via `HealthSystem.takeDamage`, prevents double-hits per swing with a `HashSet`, and spawns a blood particle effect at the hit point (`Assets/SCRIPTS/DealDamage.cs`). |
| Weapon system | `Weapon` supports shooting direction calculation with spread, reloading with a timed delay, bullet lifetime management via coroutine, and sound hooks through `SoundManager` (`Assets/Scenes/Scene1/Scrips/Weapon.cs`). |
| Enemy AI | FSM scripts exist under `Assets/Scenes/Scene1/Enemy/Animation/FSMScripts/`; specific behaviors are not fully documented here. |

### Referenced but unverified / incomplete

- A large portion of `Weapon.cs` (input handling, fire modes, burst logic, ammo UI) is commented out; the active implementation retains reload, spread calculation, and bullet cleanup.
- `HealthSystem`, `SoundManager`, and `AmmoManager` are referenced by active code; their implementations were not fully reviewed.
- Victory/game-over trigger conditions are not documented.

## Technologies and Tools

| Category | Technology | Version |
|---|---|---|
| Engine | Unity (URP template `com.unity.template.urp-blank`) | Template 17.0.11; exact editor version not documented |
| Language | C# | — |
| Render pipeline | Universal Render Pipeline | 17.0.4 |
| Shader authoring | Shader Graph | 17.0.4 |
| Camera | Cinemachine | 3.1.4 |
| Input | Unity Input System | 1.14.1 |
| AI navigation | AI Navigation | 2.0.9 |
| Post-processing | Post Processing | 3.5.0 |
| UI | Unity UI (uGUI) | 2.0.0 |
| Timeline | Timeline | 1.8.7 |
| Third-party assets | Unity Particle Pack (`Assets/UnityTechnologies/EffectExamples`), Low Poly Weapons VOL.1, QuickOutline | Not versioned |

Full dependency list: [`Packages/manifest.json`](Packages/manifest.json).

## Gameplay and Controls

**Objective:** Not documented. The scene structure implies level progression ending in victory or game-over states, but win/lose conditions are not described in the repository.

**Controls:** Not fully documented. The commented-out portion of `Weapon.cs` references left mouse button (`Mouse0`) for firing and `R` for reloading, but this code is currently disabled. Active control bindings could not be confirmed.

## Project Architecture

```
Vengence/
├── Assets/
│   ├── Intro.cs                  # Intro video/audio sequence → MainMenu
│   ├── SCRIPTS/                  # Shared gameplay scripts (e.g., DealDamage.cs)
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── IntroScene.unity
│   │   ├── Scene1.unity          # Gameplay level 1
│   │   ├── Scene2.unity          # Gameplay level 2
│   │   ├── GameOverScene.unity
│   │   ├── VictoryScene.unity
│   │   └── Scene1/
│   │       ├── Scrips/           # Level-1 scripts (e.g., Weapon.cs)
│   │       ├── Enemy/            # Enemy scripts, FSM, animations, textures
│   │       ├── Prefabs/
│   │       └── Models/
│   ├── Settings/                 # URP pipeline/render assets
│   ├── Materials/, Textures/, Shaders/, Music/, Buttons/, Resources/
│   ├── Low Poly Weapons VOL.1/   # Third-party weapon assets
│   └── UnityTechnologies/        # Unity Particle Pack (third-party)
├── Packages/
│   └── manifest.json             # Package dependencies
└── ProjectSettings/              # Unity project configuration
```

### Key scripts

- **`IntroWithVengeanceAudio`** (`Assets/Intro.cs`): plays intro video + audio, waits `videoLength` seconds, switches to a second audio track, then calls `SceneManager.LoadScene("MainMenu")`.
- **`DealDamage`** (`Assets/SCRIPTS/DealDamage.cs`): melee hit detection via trigger collider; animation events enable/disable the damage window; raycasts for surface normals to orient blood particles.
- **`Weapon`** (`Assets/Scenes/Scene1/Scrips/Weapon.cs`): gun logic — `CalculateDirectionAndSpread()` raycasts from screen center, applies random spread; `Reload()`/`ReloadCompleted()` manage magazine state; bullets are destroyed after a lifetime via coroutine.

### Component interaction

`DealDamage` → calls `HealthSystem.takeDamage(damage, hitPosition)` on hit targets. `Weapon` → calls `SoundManager.Instance` for shooting/reload sounds and `AmmoManager.Instance` for the ammo display (in the commented-out section).

## Installation and Setup

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Zizamele03/Vengence.git
   ```
2. **Open in Unity Hub:** Add the cloned folder as a project and open it.
3. **Unity version:** Not documented — requires confirmation. The project was created from the URP blank template 17.0.11 with URP 17.0.4, which corresponds to the Unity 6 generation; check `ProjectSettings/ProjectVersion.txt` locally for the exact version.
4. **Dependencies:** All packages in `Packages/manifest.json` resolve automatically via the Unity Package Manager on first open.

## Running the Project

- Open the project in the Unity Editor and press **Play**. The intended entry point appears to be `IntroScene.unity` (which loads `MainMenu`), but the configured startup scene in Build Settings was not verified.
- Standalone build instructions: Not documented.

## Technical Implementation

- **OOP / component pattern:** All gameplay logic is implemented as `MonoBehaviour` components attached to scene objects, following Unity's composition model.
- **Animation events:** `DealDamage.EnableDamage()` / `DisableDamage()` are invoked by animation events to gate the hit window — damage colliders are disabled by default and only active during attack frames.
- **Coroutines:** Used for timed sequences — `PlayVengeanceAfterVideo()` in the intro, and `DestroyBulletAfterTime()` for bullet cleanup.
- **Duplicate-hit prevention:** A `HashSet<HealthSystem>` tracks enemies already hit during a single swing and is cleared per attack.
- **Physics:** Trigger colliders for melee; `Rigidbody.AddForce(..., ForceMode.Impulse)` for projectile bullets; raycasts for hit-point and surface-normal detection.
- **Singletons:** `SoundManager.Instance` and `AmmoManager.Instance` are referenced as singleton-style managers.
- **State management:** Scene transitions via `SceneManager.LoadScene`; weapon state via flags (`readyToShoot`, `isReloading`, `allowReset`).


## Vengeance — Game Flow and Combat Systems
End-to-end map of the Vengeance Unity game: scene flow, gunplay, melee combat, enemy AI, wave progression, and win/lose handling. Notable entry points: intro sequence [1a], weapon firing [2b], melee hit detection [3b], enemy chase FSM [4b], wave-to-Scene2 transition [5c], and death-driven scene loads [6c].
### 1. Scene flow: Intro → MainMenu → Gameplay → Victory/GameOver
How the game boots and moves between scenes: intro video/audio, main menu buttons, gameplay scenes, and end screens. This is the spine connecting all other traces.
### 1a. Intro sequence starts (`Intro.cs:20`)
IntroScene: plays VideoPlayer + intro audio, then starts PlayVengeanceAfterVideo coroutine.
```text
videoPlayer.Play(); introAudio.Play(); StartCoroutine(PlayVengeanceAfterVideo());
```
### 1b. Load MainMenu after intro (`Intro.cs:43`)
After video + vengeance audio finish, loads the MainMenu scene.
```text
SceneManager.LoadScene(mainMenuSceneName);
```
### 1c. PlayGame loads Scene1 (`MainMenu.cs:13`)
MainMenu button loads Scene1 (first gameplay scene).
```text
SceneManager.LoadScene("Scene1");
```
### 1d. Persistent GameManager (`GameManager.cs:18`)
GameManager singleton persists via DontDestroyOnLoad and owns win/lose scene loads.
```text
Instance = this; DontDestroyOnLoad(gameObject);
```
### 1e. Victory path (`GameManager.cs:30`)
PlayerWins stores score/time in PlayerPrefs and loads VictoryScene.
```text
PlayerPrefs.SetInt("FinalScore", score); SceneManager.LoadScene(victoryScene);
```
### 1f. Victory screen after video (`VictorySceneManager.cs:40`)
VictorySceneManager plays victory video then reveals stats/buttons on loopPointReached.
```text
victoryVideo.loopPointReached += OnVideoEnd;
```
### 2. Gunplay: input → FireWeapon → bullet physics → enemy damage
What happens when the player fires a weapon: input polling, fire modes, spread calculation, bullet spawn, and impact damage.
### 2a. Fire input polling (`Weapon.cs:87`)
Weapon.Update polls Mouse0 (hold for Auto, click for Single/Burst) and R for reload while isActiveWeapon.
```text
isShooting = Input.GetKey(KeyCode.Mouse0);
```
### 2b. FireWeapon executes shot (`Weapon.cs:117`)
FireWeapon decrements ammo, plays muzzle particles + RECOIL trigger, and calls SoundManager.
```text
bulletsLeft--; muzzleEffect.GetComponent<ParticleSystem>().Play(); animator.SetTrigger("RECOIL");
```
### 2c. Aim via center-screen raycast (`Weapon.cs:192`)
CalculateDirectionAndSpread raycasts from screen center and adds random spread offset.
```text
Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
```
### 2d. Bullet launched (`Weapon.cs:140`)
Bullet prefab instantiated, damage assigned, launched with Rigidbody impulse.
```text
bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
```
### 2e. Bullet hits enemy (`Bullet.cs:40`)
Bullet.OnCollisionEnter applies damage to Enemy-tagged objects and spawns blood spray.
```text
enemy.TakeDamage(bulletDamage);
```
### 2f. Enemy death handling (`Enemy.cs:52`)
Enemy.TakeDamage reduces HP; on death stops NavMeshAgent and triggers a random DIE animation.
```text
navAgent.isStopped = true; animator.SetTrigger("DIE1");
```
### 3. Melee combat: animation events → DealDamage → HealthSystem reactions
What happens when a melee attack lands: animation events gate a trigger collider, DealDamage applies damage once per swing, and HealthSystem produces directional hit reactions.
### 3a. Animation event enables collider (`PlayerCombat.cs:13`)
PlayerCombat.EnableCollider(index) is invoked by animation events to open the damage window.
```text
damageColliders[index].EnableDamage();
```
### 3b. Damage window opens (`DealDamage.cs:86`)
DealDamage.EnableDamage turns on the trigger collider and clears the per-swing hit set.
```text
col.enabled = true; hitEnemies.Clear();
```
### 3c. Damage applied on trigger (`DealDamage.cs:42`)
OnTriggerEnter looks up HealthSystem on the target and calls takeDamage with attacker position.
```text
enemy.takeDamage(damage, transform.position);
```
### 3d. Blood effect spawn (`DealDamage.cs:54`)
Blood particle spawned at raycast hit point oriented to surface normal.
```text
SpawnBlood(hit.point, hit.normal);
```
### 3e. Directional hit reaction (`HealthSystem.cs:78`)
HealthSystem.takeDamage computes front/back direction and triggers Hit_Front/Hit_Back animator reactions.
```text
StartCoroutine(PlayDirectionalHitReaction(directionLabel));
```
### 4. Enemy AI: NavMesh FSM patrol → chase → attack
How guards perceive and hunt the player: Animator StateMachineBehaviours drive a NavMeshAgent through patrol, chase, and attack states using distance thresholds.
### 4a. Chase state entry (`EnemyChaseState.cs:17`)
EnemyChaseState.OnStateEnter finds the Player-tagged object and sets NavMeshAgent speed.
```text
player = playerObject.transform; agent = animator.GetComponent<NavMeshAgent>(); agent.speed = chaseSpeed;
```
### 4b. Chase pursues player (`EnemyChaseState.cs:30`)
OnStateUpdate continuously sets the agent destination to the player position.
```text
agent.SetDestination(player.position);
```
### 4c. Chase→attack transition (`EnemyChaseState.cs:38`)
Distance checks transition to attack (<=1.8f) or back to patrol (>6f) via SetAnimationState.
```text
animator.GetComponent<Enemy>().SetAnimationState(false, false, true);
```
### 4d. Exclusive animation state (`Enemy.cs:85`)
Enemy.SetAnimationState keeps exactly one of isPatroling/isChasing/isAttacking true.
```text
animator.SetBool("isPatroling", patrol); animator.SetBool("isChasing", chase); animator.SetBool("isAttacking", attack);
```
### 4e. Guard damage setup (`Guard.cs:11`)
Guard wires guardDamage into its GuardHand damage collider at Start.
```text
guardHand.damage = guardDamage;
```
### 5. Wave progression: guard spawning → all dead → Scene2
What happens when all guards in Scene1 die: the spawn controller tracks live enemies and triggers the transition to Scene2.
### 5a. First wave spawned (`GuardSpawnController.cs:33`)
Start initializes the wave and begins spawning guards with a per-guard delay coroutine.
```text
StartNextWave(); StartCoroutine(SpawnWave());
```
### 5b. Guard instantiation (`GuardSpawnController.cs:56`)
SpawnWave instantiates guardPrefab at random offsets and registers each Enemy in currentGuardAlive.
```text
var guard = Instantiate(guardPrefab, spawnPosition, Quaternion.identity); currentGuardAlive.Add(enemyScript);
```
### 5c. All guards dead → Scene2 (`GuardSpawnController.cs:72`)
Update removes dead/null guards; when the list empties, loads Scene2 once.
```text
currentGuardAlive.RemoveAll(g => g == null || g.isDead); SceneManager.LoadScene("Scene2");
```
### 5d. Backup scene switch (`SceneSwitcher.cs:22`)
SceneSwitcher is a parallel/backup mechanism that also loads Scene2 when the guard list empties.
```text
SceneManager.LoadScene("Scene2");
```
### 6. Win/lose: character death → GameOverScene or VictoryScene
What happens when a key character dies: HealthSystem.Die routes to GameOverScene (player 'John') or VictoryScene (boss 'Marco'), and end screens display stored stats.
### 6a. Death threshold reached (`HealthSystem.cs:82`)
takeDamage clamps health, sets alarmTrigger below 50, and calls Die() at zero.
```text
if (currentHealth <= 0f) { alarmTrigger = false; Die(); }
```
### 6b. Boss special reactions (`HealthSystem.cs:127`)
Boss 'Marco' gets threshold-based reactions including sweep+kickup at 20%/10% health.
```text
StartCoroutine(PlayBossSweepSequence());
```
### 6c. Death routes to end scene (`HealthSystem.cs:246`)
Die() loads GameOverScene for 'John' or VictoryScene for 'Marco' by name check.
```text
SceneManager.LoadScene("GameOverScene"); ... SceneManager.LoadScene("VictoryScene");
```
### 6d. Game over screen (`GameOverSceneManager.cs:40`)
GameOverSceneManager plays the game-over video, then shows score/time/cause from PlayerPrefs.
```text
gameOverVideo.loopPointReached += OnVideoEnd; gameOverVideo.Play();
```
### 6e. End screen buttons (`GameOverSceneManager.cs:32`)
Restart/quit buttons return to MainMenu or quit the application.
```text
restartButton.onClick.AddListener(LoadMainMenu);
```
### 7. Weapon pickup and inventory: raycast hover → F pickup → Q/E slots → HUD
What happens when the player looks at a weapon on the ground: center-screen raycast highlights it with QuickOutline and F picks it up into the active slot.
### 7a. Hover raycast (`InteractionManager.cs:25`)
InteractionManager raycasts from screen center each frame looking for inactive Weapon components.
```text
Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
```
### 7b. F to pick up (`InteractionManager.cs:35`)
Hovered weapon gets its Outline enabled; pressing F calls WeaponManager.PickupWeapon.
```text
hoveredWeapon.GetComponent<Outline>().enabled = true; if (Input.GetKeyDown(KeyCode.F))
```
### 7c. Weapon equipped (`WeaponManager.cs:65`)
PickupWeapon parents the weapon to the active slot and applies its spawn transform.
```text
pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false); weapon.isActiveWeapon = true;
```
### 7d. Weapon slot switching (`WeaponManager.cs:45`)
Q/E keys switch activeWeaponSlot between weaponSlots[0] and [1].
```text
if (Input.GetKeyDown(KeyCode.Q)) SwitchActiveSlot(0); if (Input.GetKeyDown(KeyCode.E)) SwitchActiveSlot(1);
```
### 7e. HUD ammo display (`HUDManager.cs:43`)
HUDManager.Update reads the active weapon to display ammo counts and weapon sprites.
```text
magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
```



