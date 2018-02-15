using System;
using Godot;

public class PlayerShip : Area2D {
    [Export] private float moveSpeed = 200f;

    private Vector2 shipSize;
    private float leftBound;
    private float rightBound;

    public override void _Ready () {
        var sprite = FindNode ("Sprite") as Sprite;
        if (sprite != null) {
            shipSize = sprite.GetTexture ().GetSize () * sprite.Scale;
        } else {
            GD.Print ($"O componente {nameof(sprite)} nao foi encontrado");
        }

        GD.Print ("Ship Size : " + shipSize);

        leftBound = shipSize.x / 2;
        rightBound = GetViewportRect ().Size.x - shipSize.x / 2;
    }

    public override void _Process (float delta) {
        float moveAmount = moveSpeed * delta;

        if (Input.IsActionPressed ("left")) {
            Translate (new Vector2 (-moveAmount, 0));
        } else if (Input.IsActionPressed ("right")) {
            Translate (new Vector2 (moveAmount, 0));
        }

        GD.Print ($"Left Bound : {leftBound}  Right Bound : {rightBound}");
        if (GlobalPosition.x > rightBound) {
            var position = GlobalPosition;
            position.x = rightBound;
            GlobalPosition = position;
        } else if (GlobalPosition.x < leftBound) {
            var position = GlobalPosition;
            position.x = leftBound;
            GlobalPosition = position;
        }

    }
}