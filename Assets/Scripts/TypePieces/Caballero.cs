using System.Collections.Generic;
using UnityEngine;

public class Caballero : BasePiece
{
    public override List<Vector2Int> GetAttackMoves(BasePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // Movimiento en cruz (1 o 2 casillas)
        int[] crossX = { 1, -1, 0, 0 };  // Derecha, izquierda, arriba, abajo
        int[] crossY = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            // Movimiento de 1 casilla
            int newX1 = currentX + crossX[i];
            int newY1 = currentY + crossY[i];

            // Verifica límites del tablero 
            if (newX1 >= 0 && newX1 < tileCountX && newY1 >= 0 && newY1 < tileCountY)
            {
                //Puede moverse si está vacía o es una pieza enemiga
                if (board[newX1, newY1] == null || board[newX1, newY1].team != team)
                {
                    moves.Add(new Vector2Int(newX1, newY1));

                    if (board[newX1, newY1] == null)
                    {
                        // Movimiento de 2 casillas (solo si la primera está vacía)
                        int newX2 = currentX + crossX[i] * 2;
                        int newY2 = currentY + crossY[i] * 2;

                        // Verifica límites del tablero 
                        if (newX2 >= 0 && newX2 < tileCountX && newY2 >= 0 && newY2 < tileCountY)
                        {
                            //Puede moverse si está vacía o es una pieza enemiga
                            if (board[newX2, newY2] == null || board[newX2, newY2].team != team)
                            {
                                moves.Add(new Vector2Int(newX2, newY2));
                            }
                        }
                    }
                }
            }
        }

        // Movimiento diagonal (1 casilla)
        int[] diagonalX = { 1, 1, -1, -1 };
        int[] diagonalY = { 1, -1, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int newX = currentX + diagonalX[i];
            int newY = currentY + diagonalY[i];

            // Verifica límites del tablero 
            if (newX >= 0 && newX < tileCountX && newY >= 0 && newY < tileCountY)
            {
                // Puede moverse si está vacía o es una pieza enemiga
                if (board[newX, newY] == null || board[newX, newY].team != team)
                {
                    moves.Add(new Vector2Int(newX, newY));
                }
            }
        }

        return moves;
    }
}
