//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;
//using UnityEngine;
//using UnityEditor;
//using Object = UnityEngine.Object;

//public enum Category
//{
//    None,
//    Network,
//    Car,
//    Map,
//    Game
//}

//public static class GameLog
//{
//    static readonly Dictionary<Category, string> CatToColorDic = new Dictionary<Category, string>()
//    {
//        {Category.None, ""},
//        {Category.Network, "blue"},
//        {Category.Car, "green"},
//        {Category.Map, "red"},
//        {Category.Game, "black"}
//    };

//    public static void Log<T>(this Object myObj, T message, Category category = Category.None, string prefix = "")   //Classique
//    {

//        string categoryName = category.ToString().Color(CatToColorDic[category]);
//        string name = (myObj ? myObj.name : "NullObject").Color("lightblue");

//        categoryName = "[".Color(CatToColorDic[category]) + categoryName + "]".Color(CatToColorDic[category]);


//        if (prefix != "")
//        {
//            prefix = $"[{prefix}]".Color("grey");
//        }

//        string mess;
//        if (category == Category.None) { mess = $"[{name}] {prefix} {message}"; }
//        else { mess = $"{categoryName} [{name}] {prefix} {message}"; }

//        mess = $"<size=18>{mess}</size>\n";
//        Debug.Log($"{mess}");
//    }


//    public static void Logs<T>(this Object myObj, List<T> data, Category category = Category.None, string prefix = "") // List Colors
//    {
//        StringBuilder stb = ListToString(data);
//        GameLog.Log(myObj, stb, category, prefix);
//    }



//    private static StringBuilder ListToString<T>(List<T> data)
//    {
//        StringBuilder stb = new StringBuilder("{");

//        for (int i = 0; i < data.Count; i++)
//        {
//            stb.Append(data[i] + "; ");
//        }
//        stb.Remove(stb.Length - 2, 2);
//        stb.Append(" } ");
//        return stb;
//    }



//    #region Class

//    public static void Logs<T>(this Object myObj, T data)   // Class
//    {
//        StringBuilder stb = ClassToString(data);

//        GameLog.Log(myObj, stb);
//    }

//    public static void Logs<T>(this Object myObj, T data, Category category = Category.None, string prefix = "")   // Class
//    {
//        StringBuilder stb = ClassToString(data);
//        GameLog.Log(myObj, stb, category, prefix);
//    }

//    private static StringBuilder ClassToString<T>(T data)
//    {
//        StringBuilder stb = new StringBuilder(data.ToString() + " { ");


//        System.Reflection.FieldInfo[] fieldInfo = data.GetType().GetFields();
//        FieldInfoToString(data, stb, fieldInfo);

//        stb.Remove(stb.Length - 2, 2);
//        stb.Append("}");
//        return stb;
//    }

//    private static void FieldInfoToString<T>(T data, StringBuilder stb, FieldInfo[] fieldInfo)
//    {
//        foreach (System.Reflection.FieldInfo info in fieldInfo)
//        {
//            if (info.FieldType.IsGenericType)
//            {
//                if (info.GetValue(data).GetType() == typeof(List<int>))
//                {
//                    List<int> ts = (List<int>)info.GetValue(data);
//                    stb.Append(Logg(info.Name, ts));
//                }
//                else if (info.GetValue(data).GetType() == typeof(List<string>))
//                {
//                    List<string> ts = (List<string>)info.GetValue(data);
//                    stb.Append(Logg(info.Name, ts));
//                }
//                else if (info.GetValue(data).GetType() == typeof(List<bool>))
//                {
//                    List<bool> ts = (List<bool>)info.GetValue(data);
//                    stb.Append(Logg(info.Name, ts));
//                }

//            }
//            else if (info.FieldType.IsArray)
//            {
//                if (info.GetValue(data).GetType() == typeof(int[]))
//                {
//                    int[] ts = (int[])info.GetValue(data);
//                    stb.Append(Logg(info.Name, ts));
//                }
//                else if (info.GetValue(data).GetType() == typeof(string[]))
//                {
//                    string[] ts = (string[])info.GetValue(data);
//                    stb.Append(Logg(info.Name, ts));
//                }
//                else if (info.GetValue(data).GetType() == typeof(bool[]))
//                {
//                    bool[] ts = (bool[])info.GetValue(data);
//                    stb.Append(Logg(info.Name, ts));
//                }
//            }
//            else if (info.FieldType.IsClass)
//            {
//                if (info.GetValue(data).GetType() == typeof(string))
//                {
//                    stb.Append(info.Name + "=" + info.GetValue(data) + " | ");
//                }
//                else
//                {
//                    stb.Append(info.Name + "={");

//                    FieldInfoToString(info.GetValue(data), stb, info.GetValue(data).GetType().GetFields());

//                    stb.Remove(stb.Length - 2, 2);

//                    stb.Append("}");
//                    stb.Append(" | ");
//                    //foreach (var item in info.GetValue(data).GetType().GetFields())
//                    //{
//                    //    stb.Append(item.Name + "=" + item.GetValue(info.GetValue(data)) + "; ");
//                    //}
//                    //stb.Remove(stb.Length - 2, 2);
//                    //stb.Append(" | ");
//                }
//            }
//            else
//            {
//                stb.Append(info.Name + "=" + info.GetValue(data) + " | ");
//            }
//        }
//    }
//    #endregion


//    public static StringBuilder Logg<T>(string name, List<T> data)  // List Class
//    {
//        StringBuilder stb = new StringBuilder(name + "=");

//        for (int i = 0; i < data.Count; i++)
//        {
//            stb.Append(data[i] + "; ");
//        }
//        stb.Remove(stb.Length - 2, 2);
//        stb.Append(" | ");
//        return stb;
//    }

//    public static StringBuilder Logg<T>(string name, T[] data) // Array Class
//    {
//        StringBuilder stb = new StringBuilder(name + "=");

//        for (int i = 0; i < data.Length; i++)
//        {
//            stb.Append(data[i] + "; ");
//        }
//        stb.Remove(stb.Length - 2, 2);
//        stb.Append(" | ");
//        return stb;
//    }



//    public static void ClearConsole()
//    {
//        var assembly = Assembly.GetAssembly(typeof(SceneView));
//        var type = assembly.GetType("UnityEditor.LogEntries");
//        var method = type.GetMethod("Clear");
//        method.Invoke(new object(), null);
//    }

//    public static string Color(this string myStr, string color)
//    {
//        return $"<color={color}>{myStr}</color>";
//    }

//}
