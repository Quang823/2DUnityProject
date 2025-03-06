//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MenuButtonController : MonoBehaviour {

//	// Use this for initialization
//	public int index;
//	[SerializeField] bool keyDown;
//	[SerializeField] int maxIndex;
//	public AudioSource audioSource;

//	void Start () {
//		audioSource = GetComponent<AudioSource>();
//	}

//	// Update is called once per frame
//	void Update () {
//		if(Input.GetAxis ("Vertical") != 0){
//			if(!keyDown){
//				if (Input.GetAxis ("Vertical") < 0) {
//					if(index < maxIndex){
//						index++;
//					}else{
//						index = 0;
//					}
//				} else if(Input.GetAxis ("Vertical") > 0){
//					if(index > 0){
//						index --; 
//					}else{
//						index = maxIndex;
//					}
//				}
//				keyDown = true;
//			}
//		}else{
//			keyDown = false;
//		}
//	}

//}
using UnityEngine;

public enum MenuState { MainMenu, MapSelection }

public class MenuButtonController : MonoBehaviour
{
    public MenuState currentState = MenuState.MainMenu;

    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxis("Vertical") < 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }
                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }

    public void ChangeState(MenuState newState)
    {
        currentState = newState;
        index = 0; // Reset lại index khi đổi menu
    }
}
