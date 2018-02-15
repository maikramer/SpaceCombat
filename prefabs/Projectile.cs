using System;
using Godot;

public class Projectile : Area2D {
    [Export] private float speed = 200f;

    public override void _Ready () {
        // Called every time the node is added to the scene.
        // Initialization here

    }

    public override void _Process (float delta) {
        Translate (new Vector2 (0, -speed * delta));
    }
}