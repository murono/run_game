using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
　　//動く速度, ジャンプ力
    [SerializeField]
    private float moveSpeed, jumpForce;

    [SerializeField]
    private GaugeController gaugeController;

    [SerializeField]
    private GameManager gameManager;

    private Rigidbody2D rb;
    private Animator animator;
    private int maxJumpCount = 2;
    private int jumpCount = 0;
    private bool isGameOver = false;
    private float playerStartPos;

    public enum MotionType
    {
        Jump,
        Run,
        GameOver,
        OutOfBattery,
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // playerの座標が0からスタートしない可能性があるため初期位置を取っておく
        playerStartPos = this.gameObject.transform.position.x;
    }

    private void FixedUpdate()
    {
        if (isGameOver) return;

        MovePlayer();
        ChangeAnimation(); 
    }

    private void MovePlayer() 
    {    
    　　　//プレイヤーが自動で進んでいく
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void ChangeAnimation()
    {
        if (rb.velocity.y > 0 || rb.velocity.y < 0){
            SetAnimatorTrigger(MotionType.Jump);
        } else {
            SetAnimatorTrigger(MotionType.Run);
        }
    }

    private void SetAnimatorTrigger(MotionType motionType)
    {
        switch (motionType) {
            case MotionType.Run:
                animator.SetTrigger("run_trigger");
                break;
            case MotionType.Jump:
                animator.SetTrigger("jump_trigger");
                break;
            case MotionType.GameOver:
                animator.SetTrigger("gameOver_trigger");
                break;
            case MotionType.OutOfBattery:
                animator.SetTrigger("outOfBattery_trigger");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGameOver) return;

        if (collision.gameObject.tag == "Obstacle"){
            gameManager.ShowGameOver();
        }
        if (collision.gameObject.tag == "Floor"){
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isGameOver) return;

        if (collider.gameObject.tag == "Item")
            gaugeController.IncreaseHp(30f);
        if (collider.gameObject.tag == "SpecialItem")
            gaugeController.IncreaseHp(50f);
    }

    public void Jump()
    {
        if (isGameOver) return;

        if (jumpCount < maxJumpCount)
        {
            // 速度をゼロにして、2回目のジャンプも1回目と同じ挙動にする
            rb.velocity = Vector2.zero;
            // リジッドボディに力を加える（上方向にジャンプ力をかける）
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    public float GameOver(MotionType motionType)
    {
        isGameOver = true;
        rb.velocity = Vector2.zero;
        SetAnimatorTrigger(motionType);

        return this.gameObject.transform.position.x - playerStartPos;
    }
}