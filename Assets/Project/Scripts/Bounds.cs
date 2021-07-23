using System;
using System.Collections.Generic;
using Project.Classes;
using UnityEngine;

namespace Project.Scripts {
    public class Bounds : MonoBehaviour {
        [SerializeField] private Transform[] horizontalBounds = new Transform[2];
        [SerializeField] private Transform[] verticalBounds = new Transform[2];
        [SerializeField] private TransformsGrid grid;
        private SpriteRenderer[] _horSpriteRenderers = new SpriteRenderer[2];
        private SpriteRenderer[] _verSpriteRenderers = new SpriteRenderer[2];
        private Game _game;

        private void Start() {
            _game = GameObject.FindWithTag(Consts.GAME_TAG).GetComponent<Game>();
            for (var i = 0; i < 2; i++) {
                _horSpriteRenderers[i] = horizontalBounds[i].GetComponent<SpriteRenderer>();
                _verSpriteRenderers[i] = verticalBounds[i].GetComponent<SpriteRenderer>();
                Scaling(horizontalBounds[i], _horSpriteRenderers[i], _game.FieldXSize + 2);
                Scaling(verticalBounds[i], _verSpriteRenderers[i], _game.FieldYSize + 2);
            }

            SetPositions();
        }

        private void Scaling(Transform boundTransform, SpriteRenderer spriteRenderer, int multiplier) {
            var localScale = boundTransform.localScale;
            localScale *= _game.CellSize / spriteRenderer.bounds.size.x;
            localScale.y *= multiplier;
            boundTransform.localScale = localScale;
        }


        private void SetPositions() {
            horizontalBounds[0].position = grid.GetGlobalPositionByXAndY((_game.FieldXSize - 1) / 2f, -1);
            horizontalBounds[1].position = grid.GetGlobalPositionByXAndY((_game.FieldXSize - 1) / 2f, _game.FieldYSize);
            verticalBounds[0].position = grid.GetGlobalPositionByXAndY(-1, (_game.FieldYSize - 1) / 2f);
            verticalBounds[1].position = grid.GetGlobalPositionByXAndY(_game.FieldXSize, (_game.FieldYSize - 1) / 2f);
        }
    }
}