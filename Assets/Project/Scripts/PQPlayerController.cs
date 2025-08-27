using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PixelQuest;

public class PQPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private PlayerControls input;         // your generated input-actions class
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;            // WASD/Stick
    private Camera cam;

    private void Awake()
    {
        input = new PlayerControls();     // requires your .inputactions to exist
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();       // works if Animator is on a child
        sr = GetComponentInChildren<SpriteRenderer>(); // same if SpriteRenderer is on a child
        cam = Camera.main;
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        // 1) Read WASD (Vector2) from your action map
        moveInput = input.Movement.Move.ReadValue<Vector2>();

        // Optional: clamp so diagonal isn’t faster
        if (moveInput.sqrMagnitude > 1f) moveInput = moveInput.normalized;

        // 2) Face the mouse (aim only; no position change)
        FaceMouse();

        // 3) Drive animator floats if you use them
        if (anim != null)
        {
            anim.SetFloat("moveX", moveInput.x);
            anim.SetFloat("moveY", moveInput.y);
            anim.SetFloat("speed", moveInput.sqrMagnitude);
        }
    }

    private void FixedUpdate()
    {
        // Move with physics timestep
        rb.MovePosition(rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
    }

    private void FaceMouse()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        // Works with new Input System; falls back if not present
        Vector2 mouseScreen = (Mouse.current != null)
            ? Mouse.current.position.ReadValue()
            : (Vector2)Input.mousePosition;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
        Vector2 look = mouseWorld - rb.position;

        if (sr != null) sr.flipX = (look.x < 0f);
    }
}
