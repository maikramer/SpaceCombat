using Godot;
using MkGames;

public class PlayerShip : Area2D {
    [Export] private PackedScene projectile;
    private Vector2 shipSize;
    private float leftBound;
    private float rightBound;
    private GameParameters m_gameParameters;

    public override void _Ready () {
        GameManager gameManager = GetNode("/root").FindClass<GameManager>();
        if (gameManager == null) {
            GD.Print("Erro, gameManager nao encontrado");
        } else {
            m_gameParameters = gameManager.GameParameters;
        }

        var sprite = FindNode ("Sprite") as Sprite;
        if (sprite != null) {
            shipSize = sprite.GetTexture ().GetSize () * sprite.Scale;
        } else {
            GD.Print ($"O componente {nameof(sprite)} nao foi encontrado");
        }

        leftBound = shipSize.x / 2;
        rightBound = GetViewportRect ().Size.x - shipSize.x / 2;

        if (!(projectile.Instance () is Node2D)) {
            GD.Print ("Projetil invalido");
        }
    }

    private float nextFire = 0;
    private float elapsedTime = 0;
    public override void _Process (float delta) {

        elapsedTime += delta;

        float moveAmount = m_gameParameters.character.moveSpeed * delta;
        MoveShip (moveAmount);

        LimitToCorner ();
        FireWeapon ();
    }

    private void FireWeapon () {
        if (Input.IsActionPressed ("fire") && elapsedTime > nextFire) {
            var proj = projectile.Instance () as Node2D;
            GetParent ().AddChild (proj);
            var pos = proj.Position;
            pos.y = Position.y - shipSize.y / 2f;
            pos.x = Position.x;
            proj.Position = pos;
            nextFire = elapsedTime + 1f / m_gameParameters.character.shotsPerSecond;
        }
    }

    private void LimitToCorner () {
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

    private void MoveShip (float moveAmount) {
        if (Input.IsActionPressed ("left")) {
            Translate (new Vector2 (-moveAmount, 0));
        } else if (Input.IsActionPressed ("right")) {
            Translate (new Vector2 (moveAmount, 0));
        }
    }
}

