using System;
using Godot;
using MkGames;

public class EnemyShip : Area2D {
    [Export (hint: PropertyHint.Range, hintString: "0,100")] public int enemyType = 0;
    [Export] PackedScene explosionPS;
    private GameManager gameManager;
    private GameParameters m_gameParameters;
    private Sprite m_sprite;
    public override void _Ready () {
        gameManager = GameManager.GetInstance (this);
        m_gameParameters = gameManager.GameParameters;
        AddToGroup (m_gameParameters.enemy.enemyGroup);

        var textures = m_gameParameters.enemy.textureResources;
        m_sprite = GetNode (nameof (Sprite)) as Sprite;
        Texture image;
        if (enemyType >= textures.Length) {
            GD.Print ("Indice invalido");
            return;
        }

        if (textures[enemyType] != null && textures[enemyType].Contains ("res://")) {
            image = ResourceLoader.Load (textures[enemyType]) as Texture;
            if (image != null) {
                m_sprite.Texture = image;
            } else {
                GD.Print ("Textura invalida");
            }
        } else {
            GD.Print ("Caminho ou indice invalido");
        }
    }

    public void Destroy () {
        var expl = explosionPS.Instance () as Particles2D;
        GetParent ().AddChild (expl);
        expl.GlobalPosition = GlobalPosition;
        expl.Emitting = true;
        QueueFree ();
    }
}