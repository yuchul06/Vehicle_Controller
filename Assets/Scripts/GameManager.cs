using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   static public GameManager instance;

    public int FrameLimit = 60;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("�̹� ����");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Application.targetFrameRate = FrameLimit;
    }
}
