using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float sprintSpeed = 2f;
    public float stamina = 300f;

    public GameObject sprintingSprite;
    public GameObject walkingSprite;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        float sprintHorizontal = horizontal * sprintSpeed;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        if (isWalking && isSprinting && (stamina > 0))
        {
            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * sprintSpeed * m_Animator.deltaPosition.magnitude);
            stamina--;

            walkingSprite.SetActive(false);
            sprintingSprite.SetActive(true);

            Debug.Log(stamina);

            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else if (isWalking)
        {
            sprintingSprite.SetActive(false);
            walkingSprite.SetActive(true);

            m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
            if (stamina < 300)
            {
                stamina += 0.5f;
            }


            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else 
        {
            sprintingSprite.SetActive(false);
            walkingSprite.SetActive(true);

            if (stamina < 300) 
            {
                stamina++;
            }
            
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}
