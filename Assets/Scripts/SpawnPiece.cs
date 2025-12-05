using UnityEngine;

public class SpawnPiece : MonoBehaviour
{
    [Header("Prefabs Pieces")]
    [SerializeField] private GameObject[] prefabs;

    [SerializeField] private GameObject pieceCanvasPrefab;


    private BasePiece[,] board; //Position

    public void Initialize(BasePiece[,] board)
    {
        this.board = board;
    }
    
    public BasePiece SpawnSinglePiece(ChessPieceType type, int team)
    {
        BasePiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<BasePiece>();
        cp.type = type;
        cp.team = team;

        // == Instanciar barra UI ==
        GameObject ui = Instantiate(pieceCanvasPrefab);

        // Posicionarlo encima de la pieza desde el inicio
        ui.transform.position = cp.transform.position;

        // Asignar target
        PieceUI uiScript = ui.GetComponent<PieceUI>();
        uiScript.target = cp;

        return cp;
    }

    public void SpawnFromLayout(PieceStart[] layout)
    {
        foreach (var p in layout)
        {
            board[p.x, p.y] = SpawnSinglePiece(p.type, p.team);
        }
    }
}
