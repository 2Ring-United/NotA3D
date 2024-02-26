using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PickupContainer : MonoBehaviour, IInteractable
{
    public Pickable pickable;
    public float UpperFloatingLimit = 2f;
    public float FloatSpeed = 1f;

    Vector3 UpperLimit;
    Vector3 BottomLimit;
    private float t = 0f;

    private void Start()
    {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.sprite = pickable.sprite;
        UpperLimit = transform.position;
        UpperLimit += new Vector3(0, UpperFloatingLimit, 0);
        BottomLimit = transform.position;
    }

    private void Update()
    {
        FloatAboveGround();
    }

    private void FloatAboveGround()
    {

        t += Time.deltaTime * FloatSpeed;

        float pingPongT = Mathf.PingPong(t, 1f);       
        transform.position = Vector3.Lerp(BottomLimit, UpperLimit, pingPongT);
    }
    public bool Interact()
    {
        pickable.PickUp(GameManager.Instance.PlayerController.Inventory);
        Destroy(gameObject);
        return true;
    }

    public void ShowInteractionOnGUI()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void HideInteractionOnGUI()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
