using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [Header("References")]
    public AudioSource generatorSound;
    public Slider progressBar;
    public Light lamp;

    [Header("Settings")]
    public float holdTime = 15f;
    public float soundDuration = 5f;

    private float holdTimer = 0f;
    private float soundTimer = 0f;
    private bool isActivated = false;
    private bool playerNearby = false;
    private bool soundPlaying = false;

    private void Start()
    {
        if (lamp != null)
            lamp.enabled = false;

        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
            progressBar.value = 0f;
            progressBar.maxValue = holdTime;
        }
    }

    private void Update()
    {
        if (playerNearby && !isActivated)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;
                if (progressBar != null)
                    progressBar.value = holdTimer;

                if (holdTimer >= holdTime)
                    ActivateGenerator();
            }
            else
            {
                holdTimer = 0f;
                if (progressBar != null)
                    progressBar.value = 0f;
            }
        }

        if (soundPlaying)
        {
            soundTimer += Time.deltaTime;
            if (soundTimer >= soundDuration)
            {
                generatorSound.Stop();
                soundPlaying = false;
            }
        }
    }

    private void ActivateGenerator()
    {
        isActivated = true;
        if (lamp != null)
            lamp.enabled = true;

        if (generatorSound != null)
        {
            generatorSound.Play();
            soundTimer = 0f;
            soundPlaying = true;
        }

        if (progressBar != null)
            progressBar.gameObject.SetActive(false);

        // Optional: notify your GameManager
        // GameManager.instance.GeneratorActivated();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            if (progressBar != null && !isActivated)
                progressBar.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            holdTimer = 0f;

            if (!isActivated && generatorSound.isPlaying)
            {
                generatorSound.Stop();
                soundPlaying = false;
                soundTimer = 0f;
            }

            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(false);
                progressBar.value = 0f;
            }
        }
    }
}
