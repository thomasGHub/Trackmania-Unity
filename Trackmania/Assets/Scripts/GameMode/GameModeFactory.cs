using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameModeType { TimeAttack, Rounds }
public abstract class IGameMode
{
    public string mapID;
    public GameModeType type;

    // abstract methods



}

class TimeAttack : IGameMode
{
    private int _gameDuration;
    public int roundDuration { get { return _gameDuration; } }
    public TimeAttack(int duration)
    {
        this._gameDuration = duration;
    }


}


class Rounds : IGameMode
{
    private int _numLaps;
    public int numLaps { get { return _numLaps; } }
    public Rounds(int numLaps)
    {
        this._numLaps = numLaps;
    }


}

class GameModeFactory
{
    public static IGameMode Create(GameModeType type, params object[] args)
    {
        switch (type)
        {
            case GameModeType.TimeAttack: return new TimeAttack((int)args[0]);
            case GameModeType.Rounds: return new Rounds((int)args[0]);
            default: throw new ArgumentException("Invalid game mode type.");
        }
    }
}