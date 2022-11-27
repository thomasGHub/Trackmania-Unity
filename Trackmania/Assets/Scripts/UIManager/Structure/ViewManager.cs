using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager s_instance;

    [SerializeField] private View _startingView;

    [SerializeField] private View[] _views;

    private View _currentView;
    private View _currentViewAdditive;

    private readonly Stack<View> _history = new Stack<View>();


    private void Awake() => s_instance = this;

    private void Start()
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].Initialize();
            _views[i].Hide();


        }
        if (_startingView != null)
        {
            Show(_startingView, true);

        }
    }

    public static T GetView<T>() where T : View
    {
        for (int i = 0; i < s_instance._views.Length; i++)
        {
            if (s_instance._views[i] is T tView) 
            {
                return tView;
            }
        }
        return null;
    }

    public static void Show<T>(bool remember = true, bool additive = false) where T : View
    {
        if (s_instance._currentViewAdditive !=null)
        {
            s_instance._currentViewAdditive.Hide();
        }
        

        for (int i = 0; i < s_instance._views.Length; i++)
        {
            if (s_instance._views[i] is T)
            {
                if (s_instance._currentView != null)
                {
                    if (remember)
                    {
                        s_instance._history.Push(s_instance._currentView);
                    }
                    if (additive)
                    {
                        s_instance._currentViewAdditive = s_instance._views[i];
                    }
                    else
                    {
                        s_instance._currentView.Hide();

                    }


                }
                s_instance._views[i].Show();
                s_instance._currentView = s_instance._views[i];

            }
            
        }
    }

    public static void Show(View view, bool remember = true,  bool additive = false)
    {
        if (s_instance._currentView != null)
        {
            if (remember)
            {
                s_instance._history.Push(s_instance._currentView);
            }
            s_instance._currentView.Hide();

        }
        view.Show();

        s_instance._currentView = view;
    }

    public static void ShowLast()
    {
        if (s_instance._history.Count != 0)
        {
            Show(s_instance._history.Pop(), false);
        }

    }

    

}
