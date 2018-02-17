using System;
using Godot;
using MkGames;

public sealed class GameManager : Node {

    private GameParameters m_gameParameters = new GameParameters ();
    public GameParameters GameParameters { get => m_gameParameters; private set => m_gameParameters = value; }

    public override void _Ready () {
        var s = new SaveSystem<GameParameters> (nameof (GameParameters), ref m_gameParameters, this);
    }

    public static GameManager GetInstance (Node nodeInTree) {
        var gameManager = nodeInTree.GetNode ("/root").FindClass<GameManager> ();
        if (gameManager == null) {
            GD.Print ("Erro, gameManager nao encontrado");
        }
        return gameManager;
    }
}

public class GameParameters {
    public Character character = new Character ();
    public Enemy enemy = new Enemy();
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

    public class Enemy {
        public readonly string projectileGroup = nameof (projectileGroup);
        public string[] textureResources = new string[10];

        public int GetTypesCount () {
            int count = 0;
            foreach (string s in textureResources) {
                if (s != null && s.Contains ("res://")) {
                    count++;
                } else {
                    return count;
                }
            }
            return count;
        }
    }
}