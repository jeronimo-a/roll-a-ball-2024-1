using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using TMPro;

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private float positionY;
    private float movementX;
    private float movementY;
    private float seconds;
    private float easterEggCount;
    private float gameOverCount;
    private int maxScore;
    private int count;

    public float speed;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI countText;
    public GameObject gameOverTextObject;
    public GameObject winTextObject;
    public GameObject easterEggText;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent <Rigidbody>();
        count = 0;
        seconds = 20;
        easterEggCount = 5;
        gameOverCount = 5;
        maxScore = 8;
        SetCountText();
        easterEggText.SetActive(false);
        winTextObject.SetActive(false);
        gameOverTextObject.SetActive(false);
    }

    private void FixedUpdate() {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        SetCountText();
        CheckPosition();
        SetTimeText();
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp") && seconds > 0) {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    void SetCountText() {
        if (count >= maxScore) {
            winTextObject.SetActive(true);
            gameOverCount -= Time.deltaTime;
        }
        countText.text = "Count: " + count.ToString();
        if (gameOverCount < 0) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void SetTimeText() {
        if (seconds > 0 && count < 8) {
            seconds -= Time.deltaTime;
        }
        else if (count < 8) {
            seconds = 0;
            gameOverTextObject.SetActive(true);
            gameOverCount -= Time.deltaTime;
        }
        
        float roundedSeconds = Mathf.Round(seconds * 100.0f) * 0.01f;
        timeText.text = roundedSeconds.ToString();
    }

    void CheckPosition() {
        Vector3 positionValue = rb.position;
        positionY = positionValue.y;
        if (positionY <= -50) {
            easterEggText.SetActive(true);
            easterEggCount -= Time.deltaTime;
        }
        if (easterEggCount < 0) {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
