using System;
using Godot;
using Newtonsoft.Json;

public class PlayerShip : Area2D {
    [Export] private float moveSpeed = 200f;
    [Export] private PackedScene projectile;
    [Export] private float shotsPerSecond = 2;

    private Vector2 shipSize;
    private float leftBound;
    private float rightBound;
    private PlayerSettings playerSettings;
    private MkGames.Timer verifyFileModTimer;

    public override void _Ready () {
        verifyFileModTimer = new MkGames.Timer (this, 1);
        InitSettings ();
        verifyFileModTimer.OnCounterFinished += TestSettingsChanges;
        verifyFileModTimer.Start ();

        var sprite = FindNode ("Sprite") as Sprite;
        if (sprite != null) {
            shipSize = sprite.GetTexture ().GetSize () * sprite.Scale;
        } else {
            GD.Print ($"O componente {nameof(sprite)} nao foi encontrado");
        }

        GD.Print ("Ship Size : " + shipSize);

        leftBound = shipSize.x / 2;
        rightBound = GetViewportRect ().Size.x - shipSize.x / 2;

        if (!(projectile.Instance () is Node2D)) {
            GD.Print ("Projetil invalido");
        }
    }
    private int psLastMod = 0;
    private void TestSettingsChanges () {
        using (File fl = new File ()) {
            string path = "user://playerSettings.json";
            int mod = fl.GetModifiedTime(path);
            if (mod > psLastMod) {
                InitSettings();
                psLastMod = mod;
            }
        }
    }
    
    private void InitSettings () {
        using (File fl = new File ()) {
            string path = "user://playerSettings.json";
            if (fl.FileExists (path)) {
                fl.Open (path, (int) File.ModeFlags.Read);
                playerSettings = JsonConvert.DeserializeObject<PlayerSettings> (fl.GetAsText ());
                moveSpeed = playerSettings.moveSpeed;
                shotsPerSecond = playerSettings.shotsPerSecond;
            } else {
                fl.Open (path, (int) File.ModeFlags.Write);
                playerSettings.moveSpeed = moveSpeed;
                playerSettings.shotsPerSecond = shotsPerSecond;
                fl.StoreString (JsonConvert.SerializeObject (playerSettings));
            }
        }
    }

    private float nextFire = 0;
    private float elapsedTime = 0;
    public override void _Process (float delta) {

        elapsedTime += delta;

        float moveAmount = moveSpeed * delta;
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
            nextFire = elapsedTime + 1f / shotsPerSecond;
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

class PlayerSettings {
    public float moveSpeed = 200f;
    public float shotsPerSecond = 2;
}