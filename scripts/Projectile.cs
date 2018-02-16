using System;
using Godot;

public class Projectile : Area2D {
    [Export] private float speed = 200f;

    public override void _Ready () {

    }

    public override void _Process (float delta) {
        Translate (new Vector2 (0, -speed * delta));

        if (GlobalPosition.y < 0) {
            GetParent ().RemoveChild (this);
        }
    }
}