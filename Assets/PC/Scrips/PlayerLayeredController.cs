using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerLayeredController : MonoBehaviour
{
    // ---------------- Animator / Layer Settings ----------------
    [Header("Layer / Animator settings")]
    public string fightLayerName = "FightStance";
    public float layerFadeSpeed = 6f;
    public bool instantLayerSwitch = false;

    // ---------------- Movement Settings ----------------
    [Header("Movement settings")]
    public float walkSpeed = 15f;        // 1.5x original walking speed
    public float runMultiplier = 2.5f;   // running is 2.5x walking speed
    public float rotationSpeed = 10f;
    public float fightTurnSpeed = 120f;

    // ---------------- Camera Reference ----------------
    [Header("Camera (optional)")]
    public Transform cameraTransform;

    // ---------------- Combat move translation ----------------
    [Header("Combat move translation (script-driven)")]
    public float stepDistance = 0.8f;
    public float stepDuration = 0.28f;
    public float frontKickDistance = 0.6f;
    public float roundKickDistance = 1.6f;
    public float kickDuration = 0.45f;

    // ---------------- NEW Extra Moves ----------------
    [Header("Extra Moves")]
    public float upperCutDistance = 0.4f;
    public float upperCutDuration = 0.35f;
    public float mmaKickDistance = 1.2f;
    public float mmaKickDuration = 0.25f;

    // ---------------- Gravity ----------------
    [Header("Gravity")]
    public float gravity = -9.81f;
    float verticalVelocity = 0f;

    // ---------------- Key Mappings ----------------
    public KeyCode toggleFightKey = KeyCode.F;
    public KeyCode punchKey = KeyCode.J;
    public KeyCode kickKey = KeyCode.K;
    public KeyCode flipKickKey = KeyCode.U;
    public KeyCode comboPunchKey = KeyCode.I;
    public KeyCode upperCutKey = KeyCode.L;
    public KeyCode mmaKickKey = KeyCode.O;
    public KeyCode rotateLeftKey = KeyCode.Q;
    public KeyCode rotateRightKey = KeyCode.E;
    public KeyCode runKey = KeyCode.LeftShift;

    // ---------------- Internal references ----------------
    Animator animator;
    CharacterController controller;
    int fightLayerIndex = -1;
    float currentFightWeight = 0f;
    float targetFightWeight = 0f;
    bool isFighting = false;

    readonly string[] combatTriggers = new string[]
    {
        "stepForward", "stepBack", "punch", "frontKick", "roundKick", "comboPunch",
        "upperCut", "mmaKick"
    };

    float animMoveX = 0f;
    float animMoveY = 0f;
    public float animSmoothing = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

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

        animator.applyRootMotion = false;
    }

    void Update()
    {
        HandleToggle();
        UpdateLayerWeight();

        if (!isFighting) HandleLocomotion();
        else HandleFightStance();
    }

    // ---------------- Handle Fight Toggle ----------------
    void HandleToggle()
    {
        if (Input.GetKeyDown(toggleFightKey))
            ToggleFightMode();
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
        animator.SetBool("isRunning", false);
        foreach (var t in combatTriggers) animator.ResetTrigger(t);
    }

    // ---------------- Locomotion ----------------
    void HandleLocomotion()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (cameraTransform != null)
        {
            camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            camRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;
        }

        Vector3 desiredMove = camForward * inputV + camRight * inputH;
        Vector3 moveDir = desiredMove.sqrMagnitude > 0.001f ? desiredMove.normalized : Vector3.zero;

        bool hasMovementInput = desiredMove.sqrMagnitude > 0.001f;
        bool isRunning = hasMovementInput && Input.GetKey(runKey);

        // Calculate current speed
        float currentSpeed = walkSpeed;
        if (isRunning) currentSpeed *= runMultiplier;

        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -0.5f;
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMove = moveDir * currentSpeed;
        finalMove.y = verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        Vector3 localMove = transform.InverseTransformDirection(desiredMove);
        float targetAnimX = Mathf.Clamp(localMove.x, -1f, 1f);
        float targetAnimY = Mathf.Clamp(localMove.z, -1f, 1f);

        animMoveX = Mathf.Lerp(animMoveX, targetAnimX, Time.deltaTime * animSmoothing);
        animMoveY = Mathf.Lerp(animMoveY, targetAnimY, Time.deltaTime * animSmoothing);

        animator.SetFloat("moveX", animMoveX);
        animator.SetFloat("moveY", animMoveY);
        animator.SetBool("isWalking", hasMovementInput && !isRunning);
        animator.SetBool("isRunning", isRunning);
    }

    // ---------------- Fight Stance ----------------
    void HandleFightStance()
    {
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", 0f);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        float turn = 0f;
        if (Input.GetKey(rotateLeftKey)) turn = -1f;
        if (Input.GetKey(rotateRightKey)) turn = 1f;

        if (turn != 0f)
            transform.Rotate(Vector3.up, turn * fightTurnSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("stepForward");
            StartCoroutine(MoveDuringAnimation(transform.forward, stepDistance, stepDuration));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("stepBack");
            StartCoroutine(MoveDuringAnimation(-transform.forward, stepDistance, stepDuration));
        }
        if (Input.GetKeyDown(punchKey)) animator.SetTrigger("punch");
        if (Input.GetKeyDown(kickKey))
        {
            animator.SetTrigger("frontKick");
            StartCoroutine(MoveDuringAnimation(transform.forward, frontKickDistance, kickDuration));
        }
        if (Input.GetKeyDown(flipKickKey))
        {
            animator.SetTrigger("roundKick");
            StartCoroutine(MoveDuringAnimation(transform.forward, roundKickDistance, kickDuration));
        }
        if (Input.GetKeyDown(comboPunchKey)) animator.SetTrigger("comboPunch");
        if (Input.GetKeyDown(upperCutKey))
        {
            animator.SetTrigger("upperCut");
            StartCoroutine(MoveDuringAnimation(transform.forward, upperCutDistance, upperCutDuration));
        }
        if (Input.GetKeyDown(mmaKickKey))
        {
            animator.SetTrigger("mmaKick");
            Vector3 snapPos = transform.position + transform.forward * mmaKickDistance;
            controller.enabled = false;
            transform.position = snapPos;
            controller.enabled = true;
        }
    }

    IEnumerator MoveDuringAnimation(Vector3 direction, float distance, float duration)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction.normalized * distance;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 targetPos = Vector3.Lerp(startPos, endPos, t);
            Vector3 delta = targetPos - transform.position;
            controller.Move(delta);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 finalDelta = endPos - transform.position;
        if (finalDelta.magnitude > 0.001f) controller.Move(finalDelta);
    }

    public void SetFightMode(bool on)
    {
        if (isFighting != on) ToggleFightMode();
    }
}