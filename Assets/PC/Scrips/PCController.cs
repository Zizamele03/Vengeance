/*using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerLayeredController : MonoBehaviour
{
    [Header("Layer / Animator settings")]
    public string fightLayerName = "FightStance";
    public float layerFadeSpeed = 6f;
    public bool instantLayerSwitch = false;

    [Header("Movement settings")]
    public float moveSpeed = 3f;
    public float stepDistance = 1f;       // how far a step moves
    public float flipKickDistance = 3f;   // how far a flip kick moves
    public float moveDuration = 0.4f;     // how long steps take
    public float flipKickDuration = 0.6f; // how long flip kick takes

    // key mappings
    public KeyCode toggleFightKey = KeyCode.F;
    public KeyCode punchKey = KeyCode.J;
    public KeyCode kickKey = KeyCode.K;
    public KeyCode flipKickKey = KeyCode.U;
    public KeyCode comboPunchKey = KeyCode.I;

    Animator animator;
    CharacterController controller;
    int fightLayerIndex = -1;
    float currentFightWeight = 0f;
    float targetFightWeight = 0f;
    bool isFighting = false;

    readonly string[] combatTriggers = new string[]
    {
        "stepForward", "stepBack", "punch", "frontKick", "roundKick", "comboPunch"
    };

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        fightLayerIndex = animator.GetLayerIndex(fightLayerName);
        if (fightLayerIndex == -1)
        {
            if (animator.layerCount > 1) fightLayerIndex = 1;
            else fightLayerIndex = 0;
        }

        currentFightWeight = 0f;
        targetFightWeight = 0f;
        animator.SetLayerWeight(fightLayerIndex, 0f);

        isFighting = false;
        animator.SetBool("inFightStance", false);
    }

    void Update()
    {
        HandleToggle();
        UpdateLayerWeight();

        if (!isFighting) HandleLocomotion();
        else HandleFightStance();
    }

    void HandleToggle()
    {
        if (Input.GetKeyDown(toggleFightKey))
        {
            ToggleFightMode();
        }
    }

    void ToggleFightMode()
    {
        isFighting = !isFighting;
        animator.SetBool("inFightStance", isFighting);

        targetFightWeight = isFighting ? 1f : 0f;
        ClearMovementAndTriggers();

        if (instantLayerSwitch)
        {
            currentFightWeight = targetFightWeight;
            animator.SetLayerWeight(fightLayerIndex, currentFightWeight);
        }
    }

    void UpdateLayerWeight()
    {
        if (instantLayerSwitch) return;
        currentFightWeight = Mathf.MoveTowards(currentFightWeight, targetFightWeight, layerFadeSpeed * Time.deltaTime);
        animator.SetLayerWeight(fightLayerIndex, currentFightWeight);
    }

    void ClearMovementAndTriggers()
    {
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", 0f);
        animator.SetBool("isWalking", false);

        foreach (var t in combatTriggers) animator.ResetTrigger(t);
    }

    void HandleLocomotion()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        // update animator
        animator.SetFloat("moveX", h);
        animator.SetFloat("moveY", v);

        bool walking = (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f);
        animator.SetBool("isWalking", walking);

        // move player in world space
        Vector3 move = new Vector3(h, 0, v).normalized;
        if (move.magnitude > 0.1f)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
            transform.forward = move; // rotate to face move direction
        }
    }

    void HandleFightStance()
    {
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", 0f);
        animator.SetBool("isWalking", false);

        // Steps (W/S)
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("stepForward");
            StartCoroutine(MoveDuringAnimation(transform.forward, stepDistance, moveDuration));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("stepBack");
            StartCoroutine(MoveDuringAnimation(-transform.forward, stepDistance, moveDuration));
        }

        // Combat triggers
        if (Input.GetKeyDown(punchKey)) animator.SetTrigger("punch");
        if (Input.GetKeyDown(kickKey)) animator.SetTrigger("frontKick");
        if (Input.GetKeyDown(flipKickKey))
        {
            animator.SetTrigger("roundKick");
            StartCoroutine(MoveDuringAnimation(transform.forward, flipKickDistance, flipKickDuration));
        }
        if (Input.GetKeyDown(comboPunchKey)) animator.SetTrigger("comboPunch");
    }

    IEnumerator MoveDuringAnimation(Vector3 direction, float distance, float duration)
    {
        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 end = start + direction.normalized * distance;

        while (elapsed < duration)
        {
            Vector3 newPos = Vector3.Lerp(start, end, elapsed / duration);
            controller.Move(newPos - transform.position); // move relative
            elapsed += Time.deltaTime;
            yield return null;
        }

        controller.Move(end - transform.position); // snap to end pos
    }

    public void SetFightMode(bool on)
    {
        if (isFighting != on) ToggleFightMode();
    }
}*/


/*using UnityEngine;

// Makes sure the GameObject has Animator and CharacterController
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerLayeredController : MonoBehaviour
{
    [SerializeField] private Health_Bar Healthbar; // Reference to health bar script

    [Header("Components")]
    [Header("Layer / Animator settings")]
    public string fightLayerName = "FightStance"; // Animator layer name
    public float layerFadeSpeed = 6f;             // Speed of layer blending
    public bool instantLayerSwitch = false;       // Switch instantly or fade
    private float maxHealth = 100;                // Max health
    private float currentHealth = 100;            // Current health

    [Header("Movement settings")]
    public float moveSpeed = 3f; // Movement speed

    // Key mappings
    public KeyCode toggleFightKey = KeyCode.F;
    public KeyCode punchKey = KeyCode.J;
    public KeyCode kickKey = KeyCode.K;
    public KeyCode flipKickKey = KeyCode.U;
    public KeyCode comboPunchKey = KeyCode.I;

    Animator animator;             // Animator reference
    CharacterController controller; // Character controller reference
    int fightLayerIndex = -1;       // Index of fight layer
    float currentFightWeight = 0f;  // Current layer weight
    float targetFightWeight = 0f;   // Target layer weight
    bool isFighting = false;        // Is player in fight mode

    // List of combat animation triggers
    readonly string[] combatTriggers = new string[]
    {
        "stepForward", "stepBack", "punch", "frontKick", "roundKick", "comboPunch"
    };

    void Start()
    {
        // Get components
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // Get fight layer index
        fightLayerIndex = animator.GetLayerIndex(fightLayerName);
        if (fightLayerIndex == -1)
        {
            if (animator.layerCount > 1) fightLayerIndex = 1;
            else fightLayerIndex = 0;
        }

        // Set starting values
        currentFightWeight = 0f;
        targetFightWeight = 0f;
        animator.SetLayerWeight(fightLayerIndex, 0f);

        isFighting = false;
        animator.SetBool("inFightStance", false);
    }

    void Update()
    {
        HandleToggle();        // Check toggle key
        UpdateLayerWeight();   // Smoothly change layer weight

        // Handle movement or combat
        if (!isFighting) HandleLocomotion();
        else HandleFightStance();

        // Left-click raycast to damage player
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log("Player clicked via raycast");
                    currentHealth -= Random.Range(4f, 8f);  // Take random damage
                    currentHealth = Mathf.Max(currentHealth, 0); // Clamp at 0
                    Healthbar.updateHealthBar(maxHealth, currentHealth); // Update bar
                }
            }
        }
    }

    void HandleToggle()
    {
        // Toggle fight mode when key pressed
        if (Input.GetKeyDown(toggleFightKey))
        {
            ToggleFightMode();
        }
    }

    void ToggleFightMode()
    {
        // Switch fight stance
        isFighting = !isFighting;
        animator.SetBool("inFightStance", isFighting);

        // Update target weight
        targetFightWeight = isFighting ? 1f : 0f;
        ClearMovementAndTriggers();

        // If instant switch, set weight directly
        if (instantLayerSwitch)
        {
            currentFightWeight = targetFightWeight;
            animator.SetLayerWeight(fightLayerIndex, currentFightWeight);
        }
    }

    void UpdateLayerWeight()
    {
        // Smooth fade between layers
        if (instantLayerSwitch) return;
        currentFightWeight = Mathf.MoveTowards(currentFightWeight, targetFightWeight, layerFadeSpeed * Time.deltaTime);
        animator.SetLayerWeight(fightLayerIndex, currentFightWeight);
    }

    void ClearMovementAndTriggers()
    {
        // Reset movement and triggers
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", 0f);
        animator.SetBool("isWalking", false);

        foreach (var t in combatTriggers) animator.ResetTrigger(t);
    }

    void HandleLocomotion()
    {
        // Get input
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        // Update animator
        animator.SetFloat("moveX", h);
        animator.SetFloat("moveY", v);

        // Walking check
        bool walking = (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f);
        animator.SetBool("isWalking", walking);

        // Move player
        Vector3 move = new Vector3(h, 0, v).normalized;
        if (move.magnitude > 0.1f)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
            transform.forward = move; // Rotate player
        }
    }

    void HandleFightStance()
    {
        // Reset movement params
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", 0f);
        animator.SetBool("isWalking", false);

        // Step forward/back
        if (Input.GetKeyDown(KeyCode.W)) animator.SetTrigger("stepForward");
        if (Input.GetKeyDown(KeyCode.S)) animator.SetTrigger("stepBack");

        // Combat actions
        if (Input.GetKeyDown(punchKey)) animator.SetTrigger("punch");
        if (Input.GetKeyDown(kickKey)) animator.SetTrigger("frontKick");
        if (Input.GetKeyDown(flipKickKey)) animator.SetTrigger("roundKick");
        if (Input.GetKeyDown(comboPunchKey)) animator.SetTrigger("comboPunch");
    }

    public void SetFightMode(bool on)
    {
        // Toggle fight mode externally
        if (isFighting != on) ToggleFightMode();
    }
}*/



//using UnityEngine;

//public class PCController : MonoBehaviour
//{
//    public float walkSpeed = 2f;
//    public float fightMoveSpeed = 1f;

//    private Animator animator;
//    private CharacterController controller;

//    private bool isInFightStance = false;

//    // Combat triggers
//    private readonly string[] combatTriggers = { "punch", "kick", "flipKick", "comboPunch" };

//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        controller = GetComponent<CharacterController>();

//        // Start in locomotion
//        animator.SetBool("isWalking", false);
//    }

//    void Update()
//    {
//        HandleFightToggle();

//        if (isInFightStance)
//            HandleFightMovement();
//        else
//            HandleWalkLocomotion();

//        HandleCombatInputs();
//    }

//    // Toggle stance with F
//    void HandleFightToggle()
//    {
//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            isInFightStance = !isInFightStance;
//            animator.SetBool("isInFightStance", isInFightStance);
//        }
//    }

//    // Normal walking locomotion
//    void HandleWalkLocomotion()
//    {
//        float h = Input.GetAxisRaw("Horizontal"); // A/D
//        float v = Input.GetAxisRaw("Vertical");   // W/S

//        Vector3 move = new Vector3(h, 0, v).normalized;

//        animator.SetFloat("moveX", h);
//        animator.SetFloat("moveY", v);
//        animator.SetBool("isWalking", move.magnitude > 0.01f);

//        // Move the character in world space
//        if (move.magnitude > 0.01f)
//            controller.Move(move * walkSpeed * Time.deltaTime);
//    }

//    // Movement inside fight stance
//    void HandleFightMovement()
//    {
//        float h = Input.GetAxisRaw("Horizontal");
//        float v = Input.GetAxisRaw("Vertical");

//        // Fight stance uses step triggers
//        if (v > 0.1f) animator.SetTrigger("stepForward");
//        else if (v < -0.1f) animator.SetTrigger("stepBack");

//        // Small movement in fight stance (optional)
//        Vector3 move = new Vector3(h, 0, v).normalized;
//        if (move.magnitude > 0.01f)
//            controller.Move(move * fightMoveSpeed * Time.deltaTime);
//    }

//    // Combat input (active only in fight stance)
//    void HandleCombatInputs()
//    {
//        if (!isInFightStance) return;

//        if (Input.GetKeyDown(KeyCode.J)) animator.SetTrigger("punch");
//        if (Input.GetKeyDown(KeyCode.K)) animator.SetTrigger("kick");
//        if (Input.GetKeyDown(KeyCode.L)) animator.SetTrigger("flipKick");
//        if (Input.GetKeyDown(KeyCode.I)) animator.SetTrigger("comboPunch");
//    }
//}
