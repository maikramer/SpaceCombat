using System;
using Godot;

public sealed class GameManager : Node {
    public string projectileGroup = nameof(projectileGroup);

    private GameParameters m_gameParameters = new GameParameters ();
    public GameParameters GameParameters { get => m_gameParameters; private set => m_gameParameters = value; }

    public override void _Ready () {
        var s = new MkGames.SaveSystem<GameParameters> (nameof(GameParameters), ref m_gameParameters, this);
    }
}

public class GameParameters {
    public Character character = new Character ();
    public GameParameters () {
        character.moveSpeed = 200f;
        character.shotsPerSecond = 2;
        character.maxShots = 3;
    }
    public class Character {
        public float moveSpeed;
        public float shotsPerSecond;
        public int maxShots;
    }

}