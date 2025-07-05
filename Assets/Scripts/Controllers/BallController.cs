using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float airControl = 5f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 0.5f;

    [Header("Jelly Effect")]
    [SerializeField] private Transform visual;
    [SerializeField] private float maxStretch = 0.15f;
    [SerializeField] private float effectSpeed = 8f; // Немного уменьшил для плавности
    [SerializeField] private float returnToNormalSpeed = 4f; // Отдельная скорость возврата
    [SerializeField] private float landSquash = 0.25f;
    [SerializeField] private float minVelocityForEffect = 0.2f; // Минимальная скорость для эффекта

    private Rigidbody rb;
    private Vector3 originalScale;
    private bool isGrounded;
    private float lastLandTime;
    private bool wasGroundedLastFrame;
    public bool CanMove = true;
    private GameManager GameManager;
    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameManager = GameObject.FindFirstObjectByType<GameManager>();
        originalScale = visual.localScale;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        Application.targetFrameRate = 60;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        wasGroundedLastFrame = isGrounded;
        isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);

        HandleJump();
        UpdateJellyEffect();
        ControlDrag();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        LimitSpeed();
    }

    private void HandleMovement()
    {
        if (!CanMove)
            return;
        float moveInput = Input.GetAxis("Horizontal");

        if (isGrounded)
        {
            Vector3 moveForce = new Vector3(moveInput * moveSpeed * 2f, 0, 0);
            rb.AddForce(moveForce, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 airForce = new Vector3(moveInput * airControl, 0, 0);
            rb.AddForce(airForce, ForceMode.Acceleration);
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, 0);
    }

    private void LimitSpeed()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = horizontalVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void ControlDrag()
    {
        rb.linearDamping = isGrounded ? groundDrag : airDrag;
    }

    private void HandleJump()
    {
        if (!CanMove)
            return;
        if (isGrounded && (Input.GetButtonDown("Jump") || Input.touchCount > 0))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            audioSource.pitch = Random.Range(0.7f,1.35f);
            audioSource.PlayOneShot(jumpClip);
        }
    }

    private void UpdateJellyEffect()
    {
        bool shouldApplyEffects = Mathf.Abs(rb.linearVelocity.y) > minVelocityForEffect ||
                                !isGrounded ||
                                Time.time - lastLandTime < 0.3f;

        if (!shouldApplyEffects)
        {
            visual.localScale = Vector3.Lerp(
                visual.localScale,
                originalScale,
                Time.deltaTime * returnToNormalSpeed
            );
            return;
        }
        float velocityFactor = Mathf.Clamp(rb.linearVelocity.y * 0.1f, -maxStretch, maxStretch);
        Vector3 targetScale = originalScale;

        if (!isGrounded)
        {
            targetScale += new Vector3(-velocityFactor, velocityFactor, -velocityFactor);
        }
        else if (Time.time - lastLandTime < 0.3f)
        {
            float squashAmount = landSquash * Mathf.Clamp01(1 - (Time.time - lastLandTime) * 4f);
            targetScale = new Vector3(
                originalScale.x + squashAmount,
                originalScale.y - squashAmount,
                originalScale.z + squashAmount
            );
        }
        float currentEffectSpeed = isGrounded ? effectSpeed * 0.5f : effectSpeed;
        visual.localScale = Vector3.Lerp(
            visual.localScale,
            targetScale,
            Time.deltaTime * currentEffectSpeed
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.contacts[0].normal.y > 0.7f)
            {
                isGrounded = true;
                lastLandTime = Time.time;
            }
            if (collision.gameObject.CompareTag("Danger"))
            {
                GameManager.ReloadLevel();
                return;
            }
            if (collision.gameObject.CompareTag("Finish"))
            {
                GameManager.OnFinish();
                return;
            }
            if (collision.gameObject.CompareTag("Destroy"))
            {
                collision.gameObject.SetActive(false);
                return;
            }
        }
    }

    public void Freeze()
    {
        rb.isKinematic = true;
    }
}