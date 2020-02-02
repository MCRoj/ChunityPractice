using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;
    private float convertedX;
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        setCountText();
        winText.text = "";

        GetComponent<ChuckSubInstance>().RunCode(@"
            SinOsc foo => dac;
            repeat(12)
            {
            Math.random2f(440, 660) => foo.freq;
            25::ms => now; 
            440 => foo.freq;
            25::ms => now;
            }        
        ");
    }

    
    //called before rendering a frame.  where most game code goes.
    void Update()
    {
        convertedX = (int) transform.position.x + 9.5f;
        GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
            TriOsc foo => dac;
            440 * Math.pow(2.0, {0}/19.0) => foo.freq;
            samp => now; 
        ", count));
    }
    //called just before performing physics calculations.
    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        rb.AddForce(movement * speed);
    }

    // Destroy everything that enters the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            setCountText();
        }

        GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
            SinOsc foo => dac;
            repeat(12)
            {{
            Math.random2f(440 * Math.pow(2.0, {0}/12.0), 660 * Math.pow(2.0, {0}/12.0)) => foo.freq;
            25::ms => now; 
            440 * Math.pow(2.0, {0}/12.0) => foo.freq;
            25::ms => now;
            }}        
        ", count));
    }

    void setCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            winText.text = "You Win";
        }
    }
}

//Destroy(other.gameObject);
