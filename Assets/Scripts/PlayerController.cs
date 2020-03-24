using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //LoadSceneを使うために必要

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 680.0f;
    public float maxWalkForce = 2.0f;
    public float walkForce = 30.0f;
    public LayerMask layerMask;

    Rigidbody2D rigid2D;
    Animator animator;
    bool isGrounded;    //接地判定用


    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //ジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && 
            this.rigid2D.velocity.y == 0)
        {
            this.animator.SetTrigger("JumpTrigger");
            this.rigid2D.AddForce(transform.up * this.jumpForce);
        }

        //左右移動
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        //プレイヤーの速度
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        /***
        //スピード制限
        if (speedx < this.maxWalkForce)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }
        ***/

        this.rigid2D.AddForce(transform.right * key * this.walkForce);

        //動く向きに応じて反転
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

        //プレイヤの速度に応じてアニメーション速度を変える
        if(this.rigid2D.velocity.y == 0)
        {
            this.animator.speed = speedx / 2.0f;
        }
        else
        {
            this.animator.speed = 1.0f;
        }

        //画面外に出た場合は最初から
        if(transform.position.y < -10)
        {
            SceneManager.LoadScene("GameScene");
        }

        //着地する
        isGrounded = Physics2D.Linecast(
            transform.position + transform.up * 0.3f,
            transform.position - transform.up * 0.05f,
            layerMask);
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")
            && isGrounded)
        {
            this.animator.SetTrigger("LandTrigger");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Flag")
        {
            Debug.Log("ゴール");
            SceneManager.LoadScene("ClearScene");
        }
    }
}
