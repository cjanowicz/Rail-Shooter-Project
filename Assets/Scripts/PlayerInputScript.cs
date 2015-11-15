using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
/// <summary>
/// /Not using Cross Platform Input Manager for now, will try to make it easier to add later.
/// </summary>

[RequireComponent(typeof(PlayerAimMovement))]
public class PlayerInputScript : MonoBehaviour {

    private PlayerAimMovement m_movementScript;
    private ShootingBehavior m_shootingScript;

    private void Awake() {
        m_movementScript = GetComponent<PlayerAimMovement>();
        m_shootingScript = GetComponent<ShootingBehavior>();
    }

    private void Update() {
        // Read the inputs.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //Vector2 newMouseInput = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        /*
        Vector2 newStickInput = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"))
            + (Vector2)transform.position;
            */
		//to use burst fire, use GetButtonDown
        if (Input.GetButton("Fire1")) {
			if(m_shootingScript.GetBufferedShot() == false)
            	m_shootingScript.Shoot();
        }
        // Pass all parameters to the character control script.
        m_movementScript.Move(h, v);


    }
    
        
}
