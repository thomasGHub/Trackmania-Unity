using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType { TimeAttack, Rounds, Campaign }
public class GameMode
{
    public GameModeType type;

    // abstract methods



}

public class TimeAttack : GameMode
{

    public TimeAttack()
    {
        type = GameModeType.TimeAttack;
    }


}


public class Rounds : GameMode
{
    private int _numRounds;
    public int nbRounds { get { return _numRounds; } }
    public Rounds(int numRounds)
    {
        type = GameModeType.Rounds;

        this._numRounds = numRounds;
    }


}

public class Campaign : GameMode
{

    public Campaign()
    {
        type = GameModeType.Campaign;

    }

}

public static class GameModeFactory
{
    public static GameMode Create(GameModeType type, params object[] args)
    {
        switch (type)
        {
            case GameModeType.TimeAttack: return new TimeAttack();
            case GameModeType.Rounds: return new Rounds((int)args[0]);
            case GameModeType.Campaign: return new Campaign();

            default: throw new ArgumentException("Invalid game mode type.");
        }
    }
}