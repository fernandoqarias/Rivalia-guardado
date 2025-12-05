using UnityEngine;

public class TileClick : MonoBehaviour
{
    public int x;
    public int y;

    private PlacementPhase placement;

    private void Start()
    {
        placement = FindObjectOfType<PlacementPhase>();

        BoxCollider bc = GetComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {

        // Si NO está en fase de juego = fase de colocación
        if (!placement.IsPlacementFinished)
        {
            placement.TryPlacePiece(x, y);
            return;
        }

        // Si ya está en fase de juego = movimiento normal
        ManagerGame mg = FindObjectOfType<ManagerGame>();
        mg.OnTileClicked(x, y);
    }
}
