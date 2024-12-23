using Assets._Project.Scripts.Enums;
using System;
using UnityEngine;

namespace Assets._Project.Scripts.MonoBehLogic
{
    public class Card : MonoBehaviour
    {
        public Vector2 atlasSize;
        [field: SerializeField] public PlayingCardSuit Suit { get; private set; }
        [field: SerializeField] public PlayingCardRank Rank { get; private set; }

        public void Start()
        {
            SetCardTexture(Suit, Rank);
        }

        public void SetCardTexture(PlayingCardSuit suit, PlayingCardRank rank)
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector2[] uvs = mesh.uv;

            var rowIndex = (int)rank;
            var columnIndex = (int)suit;

            var rankLength = Enum.GetValues(typeof(PlayingCardRank)).Length;
            var suitLength = Enum.GetValues(typeof(PlayingCardSuit)).Length;

            Vector2 uvStart = new Vector2(
                    (rowIndex * (atlasSize.x / rankLength)) / atlasSize.x,
                    (columnIndex * (atlasSize.y / suitLength)) / atlasSize.y
                );

            Vector2 uvEnd = new Vector2(
                ((rowIndex + 1) * (atlasSize.x / rankLength)) / atlasSize.x,
                ((columnIndex + 1) * (atlasSize.y / suitLength)) / atlasSize.y
            );

            uvs[0] = new Vector2(uvStart.x, uvStart.y);
            uvs[1] = new Vector2(uvEnd.x, uvStart.y);
            uvs[2] = new Vector2(uvStart.x, uvEnd.y);
            uvs[3] = new Vector2(uvEnd.x, uvEnd.y);

            mesh.uv = uvs;
        }
    }
}
