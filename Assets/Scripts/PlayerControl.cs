using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveForce = 400f;
    public float maxSpeed = 5f;
    public float jumpForce = 100f;
    private Animator anim;
    [HideInInspector]
    public bool jump = false;
    public bool faceRight = true;

    public AudioClip[] jumpCilps;
    public UnityEngine.Audio.AudioMixer mixer;

    private AudioSource audio;
    private bool grounded = false;
    private Transform groundCheck;

    private Rigidbody2D heroBody;
    [HideInInspector]
    // Start is called before the first frame update
    void Start()
    {
        heroBody = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();

        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        //获取水平方向输入
        float h = Input.GetAxis("Horizontal");
        //判断是否超过最大速度
        if (h * heroBody.velocity.x < maxSpeed)
            heroBody.velocity += Vector2.right * h * moveForce;

        if (Mathf.Abs(heroBody.velocity.x) > maxSpeed)         
            heroBody.velocity = new Vector2(Mathf.Sign(heroBody.velocity.x) * maxSpeed, heroBody.velocity.y);

        anim.SetFloat("speed", Mathf.Abs(heroBody.velocity.x));
        if (h > 0 && !faceRight)
        {
            flip();
        }
        if (h < 0 && faceRight)
        {
            flip();
        }
        if (jump)
        {
            anim.SetTrigger("jump");
            heroBody.AddForce(new Vector2(0f, jumpForce));
            jump = false;

            if(audio != null)
            {
                if (!audio.isPlaying)
                {
                    int i = Random.RandomRange(0, jumpCilps.Length);
                    audio.clip = jumpCilps[i];
                    audio.Play();
                    mixer.SetFloat("hero", 0); //0是最大值
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 
                                           1 << LayerMask.NameToLayer("Ground"));
        if(Input.GetButtonDown("Jump")&& grounded)
        {
            jump = true;
        }
    }

    void flip()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
