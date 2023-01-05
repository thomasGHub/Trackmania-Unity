using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Testable
{
    public int number;
    public string lettres;
    public bool boolean;
    public float floatet;
    public List<int> list = new List<int>();
    public List<string> list2 = new List<string>();

    public int[] array1 = new int[] { 1, 2, 3 };

    public Testable2 testable2 = new Testable2();
    public string lettress;


    public Testable()
    {
        number = 10;
        lettres = "a";
        boolean = true;
        floatet = 0.1f;
        list.Add(11);
        list.Add(11);
        list.Add(11);
        list.Add(11);

        list2.Add("a");
        list2.Add("a");
        list2.Add("a");

        array1 = new int[] { 1, 2, 3 };
        testable2 = new Testable2();
        lettress = "a";

    }
}

[System.Serializable]
public class Testable2
{
    public int number;
    public string cullotte;
    public List<int> list = new List<int>();
    public Testable3 testable3 = new Testable3();


    public Testable2()
    {
        number = 10;
        cullotte = "rallure";
        list.Add(11);
        list.Add(11);
        list.Add(11);
        testable3 = new Testable3();

    }
}

[System.Serializable]
public class Testable3
{
    public int number;
    public string cullotte;
    public List<int> list = new List<int>();

    public Testable3()
    {
        number = 10;
        cullotte = "rallure";
        list.Add(11);
        list.Add(11);
        list.Add(11);

    }
}

public class Test : MonoBehaviour
{
    public List<int> vs1 = new List<int>();
    public List<string> vs2 = new List<string>();

    public Testable detestable = new Testable();
    public Testable2 detestable22 = new Testable2();

    public bool isNewDebugUpdate = false;
    public bool isOldDebugUpdate = false;


    void Start()
    {
        ShowMessage();

    }

    [ContextMenu("Debug")]
    private void ShowMessage()
    {
        detestable = new Testable();

        detestable22 = new Testable2();




        GameLog.ClearConsole();


        //GameLog.ClearConsole();




        //this.Log(vs1, Category.None, "cc");

        //this.Logger("vvv", Category.None, "ici");
        //this.Log(detestable, Category.Map, "test11");

        //Debug.Log(vs1[0] + " " + vs1[1] +true + transform.position+ detestable);
        //this.Log("cc");
        //this.Logger("cc");


        //ConsoleProDebug.Search("cc");
        //ConsoleProDebug.Watch("cc", "1");
        //ConsoleProDebug.LogToFilter("cc", "cc", this);

        //ConsoleProDebug.LogAsType("Hi", "Error");
        //ConsoleProDebug.LogToFilter("Hi", "Tested");

        //this.Log(vs1, Category.Car);
        //Debug.Log("Tested" +detestable);
        //Debug.LogState(detestable);
        //Debug.Log(()=>vs1);


    }

    private void Update()
    {
        DebugInUpdate();
    }

    private void DebugInUpdate()
    {
        if (isNewDebugUpdate)
        {
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
            this.Log(detestable, Category.Map, "test11");
        }
        if (isOldDebugUpdate)
        {
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
            Debug.Log(detestable);
        }
    }
}
