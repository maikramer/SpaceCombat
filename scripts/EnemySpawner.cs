using System;
using Godot;

public class EnemySpawner : Node2D {
    [Export] PackedScene enemy1_PS;
    [Export] PackedScene enemy2_PS;
    [Export] PackedScene enemy3_PS;

    public override void _Ready () {
        PackedScene[] enemiesArray = { enemy1_PS, enemy2_PS, enemy3_PS };
        Random rng = new Random (DateTime.Now.Second);
        var placeHolders = GetChildren ();
        Color[] possibleColors = GetColorsArray (placeHolders.Length);
        for (var i = 0; i < placeHolders.Length; i++) {
            var enemy = enemiesArray[rng.Next (enemiesArray.Length)].Instance ();
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
            colors[i] = Color.Color8 ((byte) rng.Next (255), (byte) rng.Next (255), (byte) rng.Next (255), 255);
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