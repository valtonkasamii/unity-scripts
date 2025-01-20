using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour
{
    public CharacterController CH;
    public Joystick joystick;

    public float MouseX;
    public float MouseY;
    public Transform Head;
    public Transform Body;
    float Angle;
    public Camera Cam;
    public GameObject GunFire_Prefab;
    private bool isShooting = false;
    private float nextShotTime = 0f;
    private float damageCooldown = 0.1f;
    private float muzzleFlashDuration = 0.1f;
    void Start()
    {
        GunFire_Prefab.SetActive(false);  
    }

    void LateUpdate()
    {
        if (Input.touchCount > 0) 
        {
          if (Input.touchCount == 1)
          {  
            if (Input.GetTouch(0).position.x > Screen.width/2) {
                ChangeView(0);
            }
          }

        if (Input.touchCount == 2) {
            if (Input.GetTouch(0).position.x > Screen.width/2) {
                ChangeView(0);
            }

            if (Input.GetTouch(1).position.x > Screen.width/2) {
                ChangeView(1);
            }
        }
        }
        
    }

    public void ChangeView(int number)
    {
        MouseX = Input.GetTouch(number).deltaPosition.x * 5 * Time.deltaTime;
        Body.Rotate(Vector3.up, MouseX);
        MouseY = Input.GetTouch(number).deltaPosition.y * 5 * Time.deltaTime;
        Angle -= MouseY;
        Angle = Mathf.Clamp(Angle , -30 , +45);
        Head.localRotation = Quaternion.Euler(Angle , 0 , 0);
    }

    void Update()
    {
        if (isShooting)
        {
            Flash();
           if (Time.time >= nextShotTime)
              {  
                Shooting();
              }
        }

        Vector3 Move = transform.right * joystick.Horizontal * 10 * Time.deltaTime + transform.forward * joystick.Vertical * 10 * Time.deltaTime;
        CH.Move(Move);
    }

    public void Down(BaseEventData eventData)
    {
        if (!isShooting)
        {
            isShooting = true;
            Shooting();
            Flash();
        }
    }

    public void Flash()
    {
        GunFire_Prefab.SetActive(true);

        StartCoroutine(DisableMuzzleFlash());
    }
    public void Shooting()
    {

            RaycastHit hit;
            
            
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 100f))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Damage();
                }
            }
            

            nextShotTime = Time.time + damageCooldown;
        }
    

    private IEnumerator DisableMuzzleFlash()
    {
        yield return new WaitForSeconds(muzzleFlashDuration);
        GunFire_Prefab.SetActive(false);
    }

    public void Up(BaseEventData eventData)
    {
        isShooting = false;
        GunFire_Prefab.SetActive(false);  
    }
}
