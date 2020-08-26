using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    void OnMouseDown()
    {
        //transform.parent.GetComponent<Panel>().OnClick(gameObject.name);
        transform.parent.GetComponent<Panel>().OnClick(gameObject);
    }
}
