//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//[System.Serializable]
//public class Testable
//{
//    public int number;
//    public string lettres;
//    public bool boolean;
//    public float floatet;
//    public List<int> list = new List<int>();
//    public List<string> list2 = new List<string>();

//    public int[] array1 = new int[] { 1, 2, 3 };

//    public Testable2 testable2 = new Testable2();
//    public string lettress;


//    public Testable()
//    {
//        number = 10;
//        lettres = "a";
//        boolean = true;
//        floatet = 0.1f;
//        list.Add(11);
//        list.Add(11);
//        list.Add(11);
//        list.Add(11);

//        list2.Add("a");
//        list2.Add("a");
//        list2.Add("a");

//        array1 = new int[] { 1, 2, 3 };
//        testable2 = new Testable2();
//        lettress = "a";

//    }
//}

//[System.Serializable]
//public class Testable2
//{
//    public int number;
//    public string cullotte;
//    public List<int> list = new List<int>();
//    public Testable3 testable3 = new Testable3();


//    public Testable2()
//    {
//        number = 10;
//        cullotte = "rallure";
//        list.Add(11);
//        list.Add(11);
//        list.Add(11);
//        testable3 = new Testable3();

//    }
//}

//[System.Serializable]
//public class Testable3
//{
//    public int number;
//    public string cullotte;
//    public List<int> list = new List<int>();

//    public Testable3()
//    {
//        number = 10;
//        cullotte = "rallure";
//        list.Add(11);
//        list.Add(11);
//        list.Add(11);

//    }
//}

//public class Test : MonoBehaviour
//{
//    public List<int> vs1 = new List<int>();
//    public List<string> vs2 = new List<string>();

//    public Testable detestable = new Testable();
//    public Testable2 detestable22 = new Testable2();


//    void Start()
//    {
//        ShowMessage();

//    }

//    [ContextMenu("Debug")]
//    private void ShowMessage()
//    {
//        detestable = new Testable();

//        detestable22 = new Testable2();

    

//        //GameLog.ClearConsole();

        
//        //GameLog.ClearConsole();




//        this.Logs(vs1, Category.None, "cc");

//        this.Log("vvv", Category.None, "ici");
//        this.Logs(detestable, Category.Map, "test11");


//    }

   
//}
