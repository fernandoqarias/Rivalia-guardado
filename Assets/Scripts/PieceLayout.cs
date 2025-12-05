using UnityEngine;

[System.Serializable]
public struct PieceStart
{
    public ChessPieceType type;
    public int x;
    public int y;
    public int team;
}

public class PieceLayout : MonoBehaviour
{
    public PieceStart[] initialPieces;
}
