using UnityEngine;

public class PositionPiece : MonoBehaviour
{
    private BasePiece[,] board;

    public void Initialize(BasePiece[,] board)
    {
        this.board = board;
    }

    public void PositionAllPieces()
    {
        for (int x = 0; x < TableGenerator.TILE_COUNT_X; x++)
            for (int y = 0; y < TableGenerator.TILE_COUNT_Y; y++)
                if (board[x, y] != null)
                    PositionSinglePiece(x, y, board[x, y], true);
    }

    public void PositionSinglePiece(int x, int y, BasePiece cp, bool force = false)
    {
        // Solo actualiza si la pieza no es null
        if (cp != null)
        {
            cp.currentX = x;
            cp.currentY = y;

            // Si 'force' es true, se teletransporta (para el setup inicial).
            // Si 'force' es false (movimiento normal), SetPosition solo actualiza desiredPosition.
            cp.SetPosition(GetTileCenter(x, y), force);
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        // El tileSize es 2 (visto en Awake), la mitad es 1.
        float tileSize = TableGenerator.tileSize;
        float xPos = x * tileSize + (tileSize / 2f); // x * 2 + 1
        float yPos = y * tileSize + (tileSize / 2f); // y * 2 + 1

        // Devolvemos la posición centrada (asumiendo Z=0 para 2D/2.5D)
        return new Vector3(xPos, yPos, 0f);
    }
}
