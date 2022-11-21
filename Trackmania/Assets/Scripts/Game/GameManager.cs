using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Singleton responsible of the game management
/// </summary>
public class GameManager : MonoBehaviour
{
    #region publicAttributes
    // CamelCase
    static public GameManager instance;

    
    #endregion

    #region privateAttributes
    // CamelCase + _ in front

    private GameMode _currentGameMode;
    private Map _currentMap;
    private List<Player> _players;
    private Player _currentPlayer;

    #endregion

    #region publicMethods
    // CamelCase + start with uppercase

    public void Start()
    {
        instance = this;
    }

    public void StartGame()
    {

    }

    public void RestartGame()
    {

    }

    #endregion

    #region privateMethods
    // CamelCase + start with uppercase



    #endregion

}
