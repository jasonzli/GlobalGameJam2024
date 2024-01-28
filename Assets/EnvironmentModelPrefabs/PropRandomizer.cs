using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _props;

    void OnEnable()
    {
        int index = Random.Range(0,_props.Count);
        for(int i= 0; i < _props.Count; i++)
        {
            if(i == index)
            {
                _props[i].SetActive(true);
            }
            else
            {
                _props[i].SetActive(false);
            }
        }
    }
}
