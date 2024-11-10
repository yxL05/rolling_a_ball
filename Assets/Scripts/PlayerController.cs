using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Xml.Schema;
using System;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;

    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;

    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    public GameObject[] pickUpObjects;
    public int pickUpQty;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        count = 0;

        winTextObject.SetActive(false);

        pickUpObjects = GameObject.FindGameObjectsWithTag("PickUp");
        pickUpQty = pickUpObjects.Length;
        Debug.Log(pickUpQty);

        SetCountText();
    }

    // Called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        // Store the X and Y components of this movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= pickUpQty)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    // Called once per (fixed frame-rate) frame.
    void FixedUpdate() 
    {
        // 3D movement vector using movement inputs with up-down = 0.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force proportional to the movement vector to the player Rigidbody.
        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}

