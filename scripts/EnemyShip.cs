using System;
using Godot;
using MkGames;

public class EnemyShip : Area2D {
    [Export (hint: PropertyHint.Range, hintString: "0,100")] public int enemyType = 0;
    private GameManager gameManager;
    private EnemyParameters m_EnemyParameters;
    public override void _Ready () {
        gameManager = GameManager.GetInstance (this);
        m_EnemyParameters = gameManager.EnemyParameters;

        var textures = m_EnemyParameters.textureResources;
        var sprite = GetNode (nameof (Sprite)) as Sprite;
        Texture image;
        if (enemyType >= textures.Length) {
            GD.Print ("Indice invalido");
            return;
        }

        if (textures[enemyType] != null && textures[enemyType].Contains ("res://")) {
            image = ResourceLoader.Load (textures[enemyType]) as Texture;
            if (image != null) {
                sprite.Texture = image;
            } else {
                GD.Print ("Textura invalida");
            }
        } else {
            GD.Print ("Caminho ou indice invalido");
        }
    }
}