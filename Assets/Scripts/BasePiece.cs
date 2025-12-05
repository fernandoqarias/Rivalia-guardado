using System.Collections.Generic;
using UnityEngine;
public enum ChessPieceType
{
    None = 0,
    Rey = 1,
    Caballero = 2,
    Lanzero = 3,
    Arquero = 4
}
public abstract class BasePiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;

    // Nuevo sistema de stats
    public int maxHealth = 10;
    public int currentHealth;

    public int maxEnergy = 10;
    public int currentEnergy;

    // Cada pieza podría tener daño base distinto
    public int attackDamage = 5;

    private Vector3 desiredPosition; // La posición objetivo a la que se debe mover la pieza
    private const float moveSpeed = 10f; //Velocidad de Movimiento

    private void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        transform.rotation = Quaternion.Euler((team == 0) ? Vector3.zero : new Vector3(0, 180, 0)); //rota las fichas rivales 180 grados

        //NOTA: Cambia el color de las fichas segun el equipo
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (team == 0)
            sr.color = Color.white;
        else
            sr.color = new Color(0.3f, 0.3f, 0.3f, 1f); // rojizo enemigo

    }

    //Hace que las fichas se muevan mas lento de un lugar a otro en lugar de teletransportarse 
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);     
    }

    public void SpendEnergy(int amount)
    {
        currentEnergy = Mathf.Max(0, currentEnergy - amount);
    }

    public void RestoreEnergy(int amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    //Tiene relacion con los movimientos validos
    public virtual List<Vector2Int> GetAvailableMoves(BasePiece[,] board, int tileCountX, int tileCountY)
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
                if (board[newX1, newY1] == null)
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
                            if (board[newX2, newY2] == null)
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
                if (board[newX, newY] == null)
                {
                    moves.Add(new Vector2Int(newX, newY));
                }
            }
        }

        return moves;
    }
    //Tiene relacion con los ataques validas
    public virtual List<Vector2Int> GetAttackMoves(BasePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> p = new List<Vector2Int>();
        return p;
    }
    public virtual List<Vector2Int> GetAttackMovesRange(BasePiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> p = new List<Vector2Int>();
        return p;
    }

    //Obliga a que las fichas comienzen desde la posicion designada
    public virtual void SetPosition(Vector3 position, bool force = false)
    {
        desiredPosition = position;
        if (force)
            transform.position = desiredPosition;
    }
}
