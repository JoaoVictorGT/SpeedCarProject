using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{

    public int score = 0;
    public Text scoreText;

    void Start()
    {
        
    }

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
