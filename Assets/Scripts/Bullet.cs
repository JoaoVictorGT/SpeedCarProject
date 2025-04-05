using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    //[SerializeField] private float speed = 100f;

    public int score = 0;
    public Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Alvo")){ 
            score++;
            scoreText.text = "Score: " + score;
            Debug.Log("Colidiu com o objeto" + score);
            Destroy(collision.gameObject); // destroi  o objeto que colidiu ou seja o carro
            Destroy(gameObject); //destroi ela msm
        }
        

    }
}
