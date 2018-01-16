using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;

    public GameObject PlayerBulletGO; //bullet prefab
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject ExplosionGO;

    public Text LivesUIText;

    const int MaxLives = 3;
    int lives;

    public float speed;

    public void Init()
    {
        //set lives
        lives = MaxLives;
        LivesUIText.text = lives.ToString();

        //reset player's position
        transform.position = new Vector2(0, 0);
        
        //activate player's ship
        gameObject.SetActive(true);
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //fire bullets when the spacebar is pressed
        if (Input.GetKeyDown("space"))
        {
            //sound laser
            GetComponent<AudioSource>().Play();

            GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGO);
            bullet01.transform.position = bulletPosition01.transform.position;
            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position;
        }

        //moving by arrows or wasd
        float x = Input.GetAxisRaw("Horizontal");//-1, 0, or 1 for left, no input, right
        float y = Input.GetAxisRaw("Vertical");//-1, 0, or 1 for down, no input, up

        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); //bottom-left point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)); //top-right point of the screen

        max.x = max.x - 0.225f;//sub half player width
        min.x = min.x + 0.225f;

        max.y = max.y - 0.285f;//sub half height
        min.y = min.y + 0.285f;

        Vector2 pos = transform.position;//player's ship position

        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Detect collision with playership
        if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag"))
        {
            PlayExplosion();

            lives--;
            LivesUIText.text = lives.ToString();

            if (lives == 0)
            {
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
                gameObject.SetActive(false);
            }
        }
    }

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);
        explosion.transform.position = transform.position;
    }
}
