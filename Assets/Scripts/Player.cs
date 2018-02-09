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
    private float timeCharged = 0;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public GameObject dashEffect;
    public GameObject auraEffect;
    private GameObject currentAuraEffect;

    private bool canDash = false;
    private bool canEnableAura = false;
    private bool isFrozen = false;
    private bool isDashing = false;
    private bool wasAuraEnabled = false;
    private bool wasDashEnabled = false;

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
    private bool moving = false;
    private Respawn game;

    AudioSource music;
    AudioSource step;
    AudioSource jump;
    AudioSource jumpland;
    AudioSource dash;

    float scaleStep = Mathf.Pow(2f, 1.0f / 12f);

    private void Start()
    {
        music = GameObject.FindGameObjectWithTag("Game").GetComponent<AudioSource>();
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        AudioSource[] audios = GetComponents<AudioSource>();
        step = audios[0];
        jump = audios[1];
        jumpland = audios[2];
        dash = audios[3];
        StartCoroutine(PlayStep());
    }

    IEnumerator PlayStep()
    {
        while (true)
        {
            if (moving)
            {
                step.pitch = Random.Range(1.1f, 0.9f);
                step.Play();
            }
            yield return new WaitForSeconds(0.3f);
        }
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
            canEnableAura = true;
        }
        moving = animator.GetBool("Moving");
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
            jump.Play();
        }
        if (controller.collisions.below)
        {
            jump.Play();
            velocity.y = maxJumpVelocity;
            canDash = true;
            canEnableAura = true;
            StartCoroutine(PlayLand());
        }
    }

    IEnumerator PlayLand()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => controller.collisions.below == true);
        jumpland.Play();
    }

        public void OnDashInputDown()
    {
        if (canDash && !isDashing)
        {
            wasAuraEnabled = canEnableAura;
            canEnableAura = false;
            isDashing = true;
            isFrozen = true;
            animator.SetBool("Frozen", isFrozen);
            velocity = Vector2.zero;
            StartCoroutine(timeFreeze());
        }
    }

    IEnumerator timeFreeze()
    {
        timeCharged = Time.time;
        yield return new WaitForSeconds(0.8f);
        
        OnDashInputUp();
    }

    public void OnDashInputUp()
    {
        if (canDash && isFrozen && isDashing)
        {
            timeCharged = Time.time - timeCharged + 0.4f;
            timeCharged = (timeCharged > 1) ? 1 : timeCharged;

            GameObject currentDash;
            if (directionalInput.magnitude > 0.01)
            {
                dash.Play();
                float angle = Vector2.Angle(directionalInput, Vector2.right);
                Quaternion quatAngle = Quaternion.AngleAxis(angle, directionalInput.y < 0 ? Vector3.back : Vector3.forward);
                currentDash = Instantiate(dashEffect, transform.position, quatAngle);
                velocity = directionalInput.normalized * 10 * timeCharged;
            }
            else
            {
                currentDash = Instantiate(dashEffect, transform.position, Quaternion.AngleAxis(90, Vector3.forward));
                Destroy(currentDash, 0.5f);
                velocity = Vector2.up * 10 * timeCharged;
            }

            Destroy(currentDash, 0.5f);
            canDash = false;
            canEnableAura = wasAuraEnabled;
            StartCoroutine(Dashing());
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
        if (canEnableAura && !isFrozen)
        {
            isFrozen = true;
            wasDashEnabled = canDash;
            canDash = false;
            animator.SetBool("Frozen", isFrozen);
            velocity = Vector2.zero;

            currentAuraEffect = Instantiate(auraEffect, transform.position, Quaternion.identity);
            music.pitch = 0.6f;
        }
    }

    public void OnAuraInputUp()
    {
        if (isFrozen && canEnableAura)
        {
            isFrozen = false;
            canEnableAura = false;
            canDash = wasDashEnabled;
            animator.SetBool("Frozen", isFrozen);

            AuraSizeController auraScript = currentAuraEffect.GetComponent<AuraSizeController>() as AuraSizeController;
            auraScript.launchFadeOut();
            music.pitch = 1f;

        }
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
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));
            if (!isDashing)
                velocity.y += gravity * Time.deltaTime;
        }
    }
}
