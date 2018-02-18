using System;
using Godot;
using MkGames;

public class Projectile : Area2D {
    [Export] private float speed = 200f;
    private GameManager gameManager;

    public override void _Ready () {
        gameManager = GetNode ("/root").FindClass<GameManager> ();
        AddToGroup (gameManager.GameParameters.character.projectileGroup);
        Connect ("area_entered", this, nameof (OnAreaEntered));
    }

    public override void _Process (float delta) {
        Translate (new Vector2 (0, -speed * delta));

        if (GlobalPosition.y < 0) {
            this.QueueFree ();
        }
    }

    public void OnAreaEntered (Godot.Object obj) {
        if (obj is EnemyShip) {
            ((EnemyShip) obj).Destroy ();
            QueueFree();
        }

    }
}