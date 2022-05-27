using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float turnSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator animator;
    [SerializeField] Stack stack;
    [SerializeField] Transform groundCheck;
    Touch touch;
    Rigidbody rb;
    Vector2 startPosition, currentPosition;
    Vector3 targetPosition;
    bool onRamp = false;
    bool onGround;
    int stage = 1;
    public int Stage { get { return stage; } private set { } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!GameManager.Instance.GameEnded)
        {
            onGround = Physics.CheckSphere(groundCheck.position, 0.4f, groundLayer);
            TouchControl();
            if (!onGround)
                GameManager.Instance.Restart();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            onRamp = true;
            rb.useGravity = false;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Bridge"))
        {
            onRamp = false;
            rb.useGravity = true;
        }
    }
    private void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                startPosition = touch.position;
            currentPosition = touch.position;
            targetPosition = new Vector3(currentPosition.x - startPosition.x + transform.position.x, transform.position.y, currentPosition.y - startPosition.y + transform.position.z);
            if (targetPosition.z < transform.position.z)
                stack.Back();
            if (stage == 1 && !onRamp)
            {
                targetPosition.y = 0.5f;
                if (transform.position.z > 24.5f)
                    targetPosition.z = 24.2f;
            }
            if (stage == 2 && !onRamp)
            {
                targetPosition.y = 2.1f;
                if (transform.position.z < 25f)
                    targetPosition.z = 25f;
            }
            if (!stack.GoBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * runSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), Time.deltaTime * turnSpeed);
                if (!animator.GetBool("Run"))
                    animator.SetBool("Run", true);
            }
        }
        else
        {
            animator.SetBool("Run", false);
            startPosition = Vector2.zero;
            currentPosition = Vector2.zero;
            targetPosition = Vector3.zero;
        }
    }
    public void ChangeStage(int number)
    {
        stage = number;
        if (stage == 3)
            GameManager.Instance.EndGame(1);
    }
    public void End(int x)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        stack.ClearStack();
        onRamp = false;
        animator.SetBool("Run", false);
        if (x == 0)
            animator.SetBool("GreenDance", true);
        transform.position = new Vector3(x, 3.5f, 53f);
        transform.eulerAngles = Vector3.up * 180f;
    }
}