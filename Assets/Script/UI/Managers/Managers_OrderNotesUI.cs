using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_OrderNotesUI : MonoBehaviour
{
    public static Managers_OrderNotesUI Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<UI_OrderNotesController> orderNotesControllers = new List<UI_OrderNotesController>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateNotesList()
    {

    }
}
