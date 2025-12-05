using UnityEngine;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    private Dictionary<GameObject, Material> originalMaterials = new Dictionary<GameObject, Material>();

    [Header("Materials")]
    [SerializeField] private Material moveMaterial;
    [SerializeField] private Material attackMaterial;

    private BasePiece[,] board;

    public void Initialize(BasePiece[,] board)
    {
        this.board = board;
    }

    public void HighlightTiles(List<Vector2Int> moveTiles, List<Vector2Int> attackTiles)
    {
        ClearHighlights();

        // Highlight ataques
        foreach (var pos in attackTiles)
        {
            HighlightSingleTile(pos.x, pos.y, attackMaterial);
        }
        // Highlight movimientos
        foreach (var pos in moveTiles)
        {
            HighlightSingleTile(pos.x, pos.y, moveMaterial);
        }

    }

    private void HighlightSingleTile(int x, int y, Material mat)
    {
        GameObject tile = TableGenerator.tiles[x, y];
        if (tile == null) return;

        MeshRenderer mr = tile.GetComponent<MeshRenderer>();
        if (mr == null) return;

        if (!originalMaterials.ContainsKey(tile))
            originalMaterials[tile] = mr.sharedMaterial;

        mr.material = mat;

    }

    public void ClearHighlights()
    {
        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
            {
                MeshRenderer mr = kvp.Key.GetComponent<MeshRenderer>();
                if (mr != null)
                    mr.material = kvp.Value;

            }
        }

        originalMaterials.Clear();
    }
}
