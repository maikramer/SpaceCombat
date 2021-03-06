using System;
using Godot;

public class EnemySpawner : Node2D {
    [Export (hint: PropertyHint.Range, hintString: "0,255")] byte enemyBright;
    [Export] PackedScene enemyPS;

    public override void _Ready () {
        var gameManager = GameManager.GetInstance (this);
        var m_GameParameters = gameManager.GameParameters;
        Random rng = new Random (DateTime.Now.Second);
        var placeHolders = GetChildren ();
        Color[] possibleColors = GetColorsArray (placeHolders.Length);
        for (var i = 0; i < placeHolders.Length; i++) {
            var enemy = enemyPS.Instance () as EnemyShip;
            enemy.enemyType = rng.Next (m_GameParameters.enemy.GetTypesCount ());
            var enemySprite = enemy.FindNode ("Sprite") as Sprite;
            if (enemySprite == null) {
                GD.Print ("Sprite nao encontrado em inimigo " + enemy.Name);
            } else {
                enemySprite.SelfModulate = possibleColors[i];
            }
            var place = placeHolders[i] as Node;
            place.AddChild (enemy);
        }
    }

    private Color[] GetColorsArray (int nColors) {
        Random rng = new Random (DateTime.Now.Second);
        Color[] colors = new Color[nColors];
        for (int i = 0; i < colors.Length; i++) {
            colors[i] = Color.Color8 ((byte) rng.Next (enemyBright, 256), (byte) rng.Next (enemyBright, 256),
                (byte) rng.Next (enemyBright, 256), 255);
        }

        return colors;
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
}