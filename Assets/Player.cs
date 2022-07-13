using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    [SerializeField]
    public GameObject background;
    [SerializeField]
    public GameObject pipe;
    [SerializeField]
    public GameObject worldParent;
    [SerializeField]
    public TextMeshProUGUI scoreText;

    //game variables
    public float timeSurvived = 0;
    public float speed = 15;
    public float pipeSpread = 0.1f;
    public float backgroundSpread = 200;
    public List<GameObject> pipes;
    public float distance = 50;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Instantiate(background, worldParent.transform);

        timeSurvived = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeSurvived += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(0, 15, 0);
        }

        if (timeSurvived > 0)
        {
            foreach (var p in pipes.ToList())
            {
                p.transform.Translate(-Vector3.right * speed * Time.deltaTime);

                var pipeposx = p.transform.position.x;
                var playerposx = this.transform.position.x;
                var distance = pipeposx - playerposx;
                if (distance < -100)
                {
                    GameObject.Destroy(p);
                    pipes.Remove(p);
                }
            }          
        }

        if (timeSurvived > pipeSpread)
        {
            var p = Instantiate(pipe, worldParent.transform.position + new Vector3(distance, Random.Range(-20.0f, 5.0f), -10), worldParent.transform.rotation);
            pipes.Add(p);
            timeSurvived = 0;
        }

        if(this.transform.position.y < -30 || this.transform.position.y > 30)
        {
            EndGame();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.name == "Top" || collision.gameObject.name == "Bottom")
        {
            EndGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        AddScore(1);
    }

    public void EndGame()
    {
        speed = 0;

        //show game over restart

        //temp reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore(int amount)
    {
        score++;
        scoreText.text = score.ToString(); ;
    }
}
