using System.Collections.Generic;
using UnityEngine;

public class Arquero : BasePiece
{
    private int tileCountX, tileCountY;
    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < tileCountX && y >= 0 && y < tileCountY;
    }
    public override List<Vector2Int> GetAttackMoves(BasePiece[,] board, int tileCountX, int tileCountY)
    {
        this.tileCountX = tileCountX;
        this.tileCountY = tileCountY;

        List<Vector2Int> moves = new List<Vector2Int>();
        HashSet<Vector2Int> blocked = new HashSet<Vector2Int>();

        // --- MOVIMIENTO EN CRUZ (hasta 4 casillas) ---
        int[] crossX = { 1, -1, 0, 0 };
        int[] crossY = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            for (int step = 1; step <= 4; step++)
            {
                int newX = currentX + crossX[i] * step;
                int newY = currentY + crossY[i] * step;

                if (!IsInside(newX, newY)) break;

                if (blocked.Contains(new Vector2Int(newX, newY))) break;

                BasePiece p = board[newX, newY];

                if (p != null)
                {
                    if (p.team == team || step == 1)
                        break; // bloqueada por aliado o primera casilla
                    else
                    {
                        moves.Add(new Vector2Int(newX, newY)); // enemigo
                        break; // siguiente casilla bloqueada
                    }
                }
                else
                {
                    if (step > 1) moves.Add(new Vector2Int(newX, newY));
                }
            }
        }

        // --- MOVIMIENTO DIAGONAL (hasta 3 casillas) con bloqueo tipo L ---
        int[] diagX = { 1, 1, -1, -1 };
        int[] diagY = { 1, -1, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            // Recorremos la diagonal paso a paso
            for (int step = 1; step <= 3; step++)
            {
                int newX = currentX + diagX[i] * step;
                int newY = currentY + diagY[i] * step;

                if (!IsInside(newX, newY)) break;
                if (blocked.Contains(new Vector2Int(newX, newY))) break;

                BasePiece p = board[newX, newY];

                if (p != null)
                {
                    // Marcamos la L alrededor de esta casilla
                    blocked.Add(new Vector2Int(newX, newY));

                    int nextX = currentX + diagX[i] * (step + 1);
                    int nextY = currentY + diagY[i] * (step + 1);
                    if (IsInside(nextX, nextY)) blocked.Add(new Vector2Int(nextX, nextY));

                    int side1X = nextX;
                    int side1Y = newY;
                    int side2X = newX;
                    int side2Y = nextY;
                    if (IsInside(side1X, side1Y)) blocked.Add(new Vector2Int(side1X, side1Y));
                    if (IsInside(side2X, side2Y)) blocked.Add(new Vector2Int(side2X, side2Y));

                    if (p.team != team && step > 1)
                        moves.Add(new Vector2Int(newX, newY)); // enemigo en diagonal

                    break; // bloquea la diagonal y L
                }
                else
                {
                    moves.Add(new Vector2Int(newX, newY));
                }
            }
        }

        // --- CABALLO EXTENDIDO (2x2) ---
        int[,] offsets = {
            { 1, 2 }, { -1, 2 }, { 1, -2 }, { -1, -2 },
            { 2, 1 }, { -2, 1 }, { 2, -1 }, { -2, -1 }
        };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newX = currentX + offsets[i, 0];
            int newY = currentY + offsets[i, 1];

            if (!IsInside(newX, newY)) continue;
            if (blocked.Contains(new Vector2Int(newX, newY))) continue;

            int midX = currentX + (offsets[i, 0] / 2);
            int midY = currentY + (offsets[i, 1] / 2);

            if (board[midX, midY] != null) continue;

            BasePiece p = board[newX, newY];
            if (p == null || p.team != team)
                moves.Add(new Vector2Int(newX, newY));
        }

        return moves;
    }
}
