using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzero : BasePiece
{
    public override List<Vector2Int> GetAttackMoves(BasePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // Movimiento en cruz (1, 2 o 3 casillas)
        int[] crossX = { 1, -1, 0, 0 };
        int[] crossY = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int newX1 = currentX + crossX[i];
            int newY1 = currentY + crossY[i];

            if (newX1 >= 0 && newX1 < tileCountX && newY1 >= 0 && newY1 < tileCountY)
            {
                int newX2 = currentX + crossX[i] * 2;
                int newY2 = currentY + crossY[i] * 2;

                if (newX2 >= 0 && newX2 < tileCountX && newY2 >= 0 && newY2 < tileCountY)
                {
                    moves.Add(new Vector2Int(newX2, newY2));

                    if (board[newX2, newY2] == null)
                    {
                        int newX3 = currentX + crossX[i] * 3;
                        int newY3 = currentY + crossY[i] * 3;

                        if (newX3 >= 0 && newX3 < tileCountX && newY3 >= 0 && newY3 < tileCountY)
                        {
                            moves.Add(new Vector2Int(newX3, newY3));
                        }
                    }
                }
            }
        }

        // Movimiento diagonal (X) — hasta 2 casillas
        int[] diagonalX = { 1, 1, -1, -1 };
        int[] diagonalY = { 1, -1, 1, -1 };

        List<Vector2Int> secondDiagonalTiles = new List<Vector2Int>();

        for (int i = 0; i < 4; i++)
        {
            int newX1 = currentX + diagonalX[i];
            int newY1 = currentY + diagonalY[i];

            if (newX1 >= 0 && newX1 < tileCountX && newY1 >= 0 && newY1 < tileCountY)
            {
                int newX2 = currentX + diagonalX[i] * 2;
                int newY2 = currentY + diagonalY[i] * 2;

                if (newX2 >= 0 && newX2 < tileCountX && newY2 >= 0 && newY2 < tileCountY)
                {
                    moves.Add(new Vector2Int(newX2, newY2));
                    secondDiagonalTiles.Add(new Vector2Int(newX2, newY2));
                }
            }
        }

        // Casillas centrales del cuadrado formado por las segundas diagonales
        int[,] offsets = {
            { 1, 2 }, { -1, 2 }, { 1, -2 }, { -1, -2 },
            { 2, 1 }, { -2, 1 }, { 2, -1 }, { -2, -1 }
        };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newX = currentX + offsets[i, 0];
            int newY = currentY + offsets[i, 1];

            if (newX >= 0 && newX < tileCountX && newY >= 0 && newY < tileCountY)
            {
                if (!moves.Contains(new Vector2Int(newX, newY)))
                    moves.Add(new Vector2Int(newX, newY));
            }
        }

        return moves;
    }
}
