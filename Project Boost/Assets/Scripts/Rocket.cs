using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidbody;
    private AudioSource audioSource;
    private State state;

    public float pushForce = 1;
    public float rotationSpeed = 1;

    enum State
    {
        Dying,
        Trancending,
        Alive
    };

    // Start is called before the first frame update
    void Start()
    {
        state = State.Alive;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space) && state == State.Alive)
        {
            rigidbody.AddRelativeForce(Vector3.up * pushForce);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true; // Take control of rotation
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        rigidbody.freezeRotation = false; //Resume physics
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("boop");
                    break;
                }
            case "Finish":
                {
                    
                    if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCount - 1)
                    {
                        state = State.Trancending;
                        Invoke("NextLevel", 1f);
                    }
                    print("boop");
                    break;
                }
            default:
                {
                    state = State.Dying;
                    Invoke("Die", 1f);
                    break;
                }
        }
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        state = State.Alive;
    }


    private void Die()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
    }


}
