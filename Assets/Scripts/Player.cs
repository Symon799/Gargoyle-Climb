using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float maxJumpHeight = 4f;
    public float minJumpHeight = 1f;
    public float timeToJumpApex = .4f;
    private float accelerationTimeAirborne = .2f;
    private float accelerationTimeGrounded = .1f;
    private float moveSpeed = 4f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public GameObject dashEffect;
    public GameObject auraEffect;
    private GameObject currentAuraEffect;

    private bool canDash = false;
    private bool isFrozen = false;
    private bool isDashing = false;

    public float wallSlideSpeedMax = 3f;
    public float wallStickTime = .25f;
    private float timeToWallUnstick;

    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;
    private Animator animator;

    private Vector2 directionalInput;
    private bool wallSliding;
    private int wallDirX;

    private void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    private void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above)
        {
            velocity.y = 0f;
        }
        if (controller.collisions.below)
        {
            velocity.y = 0f;
            canDash = true;
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
        if (input.x != 0)
        animator.SetBool("Moving", true);
    }

    public void OnJumpInputDown()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            canDash = true;
        }
    }

    public void OnDashInputDown()
    {
        if (canDash && !isDashing)
        {
            isDashing = true;
            isFrozen = true;
            animator.SetBool("Frozen", isFrozen);
            velocity = Vector2.zero;
        }
    }

    public void OnDashInputUp()
    {
        if (isFrozen)
        {
            if (directionalInput.magnitude > 0.01)
            {
                float angle = Vector2.Angle(directionalInput, Vector2.right);
                Quaternion quatAngle = Quaternion.AngleAxis(angle, directionalInput.y < 0 ? Vector3.back : Vector3.forward);
                GameObject currentDash = Instantiate(dashEffect, transform.position, quatAngle);
                Destroy(currentDash, 0.5f);

                velocity = directionalInput.normalized * 10;
                canDash = false;
                StartCoroutine(Dashing());
            }
            else
            {
                isDashing = false;
                isFrozen = false;
                canDash = false;
                animator.SetBool("Frozen", isFrozen);
            }
            
        }
    }

    IEnumerator Dashing()
    {
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        isFrozen = false;
        animator.SetBool("Frozen", isFrozen);
        velocity = velocity / 1.5f;
    }


    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    public void OnAuraInputDown()
    {
        isFrozen = true;
        animator.SetBool("Frozen", isFrozen);
        velocity = Vector2.zero;

        currentAuraEffect = Instantiate(auraEffect, transform.position, Quaternion.identity);
    }

    public void OnAuraInputUp()
    {
        isFrozen = false;
        animator.SetBool("Frozen", isFrozen);

        Destroy(currentAuraEffect);
    }

    private void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0f)
            {
                velocityXSmoothing = 0f;
                velocity.x = 0f;
                if (directionalInput.x != wallDirX && directionalInput.x != 0f)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        if (!isFrozen)
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));

        if (!isDashing)
            velocity.y += gravity * Time.deltaTime;
    }
}
