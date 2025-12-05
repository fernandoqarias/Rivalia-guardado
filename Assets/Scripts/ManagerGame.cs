using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ManagerGame : MonoBehaviour
{
    private TableGenerator tabla;
    private SpawnPiece spawner;
    private PositionPiece positioner;
    private PieceLayout layout;
    private TileHighlighter highlighter;

    public bool gameStarted = false;
    public int currentTurnTeam = 0;
    public BasePiece selectedPiece = null;

    private bool refreshHighlights = false;
    private List<Vector2Int> cachedMoves;
    private List<Vector2Int> cachedAttacks;

    public BasePiece[,] Board { get; private set; }

    private void Awake()
    {
        tabla = GetComponent<TableGenerator>();
        spawner = GetComponent<SpawnPiece>();
        positioner = GetComponent<PositionPiece>();
        layout = GetComponent<PieceLayout>();
        highlighter = GetComponent<TileHighlighter>();

        /*
        layout.initialPieces = new PieceStart[]
        {
            new PieceStart { type = ChessPieceType.Rey, x = 0, y = 0, team = 0 },
            new PieceStart { type = ChessPieceType.Caballero, x = 1, y = 0, team = 0 },
            new PieceStart { type = ChessPieceType.Arquero, x = 2, y = 0, team = 1 }
        };*/
    }

    
    private void Start()
    {
        // Iniciamos el juego después de que todos los otros scripts hicieron Awake().
        SetupGame();
    }

    private void Update()
    {
        if (refreshHighlights)
        {
            highlighter.ClearHighlights();
            highlighter.HighlightTiles(cachedMoves, cachedAttacks);
            refreshHighlights = false;
        }
    }


    public void SetupGame()
    {
        // Crear matriz del tablero
        Board = new BasePiece[TableGenerator.TILE_COUNT_X, TableGenerator.TILE_COUNT_Y];

        // Pasar matriz a los sistemas
        spawner.Initialize(Board);
        positioner.Initialize(Board);

        // Secuencia del setup
        tabla.GenerateAllTiles();
       // spawner.SpawnAllPieces();
        spawner.SpawnFromLayout(layout.initialPieces);
        positioner.PositionAllPieces();

        Debug.Log("Juego inicializado correctamente.");
    }

    // Fácil de reiniciar el juego
    public void RestartGame()
    {
        // Aquí luego limpiarás piezas y volverás a SetupGame()
        SetupGame();
    }

    public BasePiece SpawnSingle(PieceStart ps)
    {
        return spawner.SpawnSinglePiece(ps.type, ps.team);
    }

    public void PositionPieceOn(int x, int y, BasePiece cp)
    {
        positioner.PositionSinglePiece(x, y, cp, true);
    }

    public void OnTileClicked(int x, int y)
    {       

        if (!gameStarted)
            return;

        BasePiece clickedPiece = Board[x, y];

        // 1) Si clicaste una pieza propia = seleccionar
        if (clickedPiece != null && clickedPiece.team == currentTurnTeam)
        {
            SelectPiece(clickedPiece);
            return;
        }

        // 2) Si no seleccionaste nada antes ? ignorar
        if (selectedPiece == null)
            return;

        // 3) Intentar mover a la casilla clicada
        TryMoveTo(x, y);
    }

    public void SelectPiece(BasePiece piece)
    {
        selectedPiece = piece;

        cachedMoves = piece.GetAvailableMoves(Board, TableGenerator.TILE_COUNT_X, TableGenerator.TILE_COUNT_Y);
        cachedAttacks = piece.GetAttackMoves(Board, TableGenerator.TILE_COUNT_X, TableGenerator.TILE_COUNT_Y);

        Debug.Log($"{piece.type} seleccionado. HP: {piece.currentHealth}/{piece.maxHealth} | EN: {piece.currentEnergy}/{piece.maxEnergy}");

        refreshHighlights = true;
    }

    public void TryMoveTo(int x, int y)
    {
        highlighter.ClearHighlights();

        var moves = selectedPiece.GetAvailableMoves(
            Board,
            TableGenerator.TILE_COUNT_X,
            TableGenerator.TILE_COUNT_Y
        );

        var attackList = selectedPiece.GetAttackMoves(
            Board,
            TableGenerator.TILE_COUNT_X,
            TableGenerator.TILE_COUNT_Y
        );

        Vector2Int target = new Vector2Int(x, y);

        BasePiece targetPiece = Board[x, y];

        //
        // PRIMERO: ATAQUE (prioridad sobre movimiento normal)
        //
        /*
        if (targetPiece != null && targetPiece.team != currentTurnTeam)
        {
            if (!attackList.Contains(target))
            {
                Debug.Log("No puedes atacar esa unidad.");
                return;
            }

           // Debug.Log($"{selectedPiece.type} mató a {targetPiece.type}");
            // Eliminar físicamente la pieza enemiga
            Destroy(targetPiece.gameObject);        

            // Vaciar tablero
            Board[x, y] = null;

            // Mover pieza atacante al lugar
            
            //Board[selectedPiece.currentX, selectedPiece.currentY] = null;
            //Board[x, y] = selectedPiece;
            //positioner.PositionSinglePiece(x, y, selectedPiece, false);

            // turn change
            selectedPiece = null;
            currentTurnTeam = (currentTurnTeam == 0) ? 1 : 0;

            // verificar victoria
            GetComponent<VictoryManager>().CheckVictory();          

            return;
        }*/
        // PRIMERO: ATAQUE (prioridad sobre movimiento normal)
        if (targetPiece != null && targetPiece.team != currentTurnTeam)
        {
            if (!attackList.Contains(target))
            {
                Debug.Log("No puedes atacar esa unidad.");
                return;
            }

            if (selectedPiece.currentEnergy < 2)
            {
                Debug.Log("No tienes energía suficiente para atacar (necesitas 2).");
                return;
            }

            // Aplicar daño y gastar energía del atacante
            targetPiece.TakeDamage(selectedPiece.attackDamage);
            selectedPiece.SpendEnergy(2);

            // Si el objetivo murió -> eliminar del tablero y mover atacante a la casilla
            if (targetPiece.currentHealth <= 0)
            {
                Debug.Log($"{selectedPiece.type} mató a {targetPiece.type} (HP llegó a 0).");

                Board[x, y] = null;

                // Eliminar físicamente la pieza enemiga
                Destroy(targetPiece.gameObject);

                // Actualizar matriz: quitar la pieza atacante de su antigua posición y ponerla en la nueva
                //Board[selectedPiece.currentX, selectedPiece.currentY] = null;
                //Board[x, y] = selectedPiece;
                //positioner.PositionSinglePiece(x, y, selectedPiece, false);

                // verificar victoria
                GetComponent<VictoryManager>().CheckVictory();
            }
            else
            {
                // Si NO murió, solo informar su nueva vida
                Debug.Log($"{targetPiece.type} recibió {selectedPiece.attackDamage} de daño. HP restante: {targetPiece.currentHealth}/{targetPiece.maxHealth}");
                // NO borrar Board[x,y], porque la pieza sigue ahí
            }

            // Limpiar selección y cambiar turno
            selectedPiece = null;
            currentTurnTeam = (currentTurnTeam == 0) ? 1 : 0;

            // Restaurar 1 energía al equipo que ahora recibe el turno
            RestoreEnergyToTeam(currentTurnTeam);

            // verificar victoria (si corresponde)
            //GetComponent<VictoryManager>().CheckVictory();

            // ver victoria
            // GetComponent<VictoryManager>().CheckVictory();
            StartCoroutine(DelayedVictoryCheck());

            return;
        }


        //
        // SEGUNDO: MOVIMIENTO NORMAL
        //

        if (!moves.Contains(target))
        {
            Debug.Log("Movimiento inválido");
            return;
        }

        int distance = Mathf.Abs(selectedPiece.currentX - x) + Mathf.Abs(selectedPiece.currentY - y);

        // Validar energía antes de mover
        int movementCost = (distance == 1 ? 1 : (distance == 2 ? 2 : 999));

        if (selectedPiece.currentEnergy < movementCost)
        {
            Debug.Log($" NO TIENES ENERGÍA SUFICIENTE ({selectedPiece.currentEnergy}) para moverte {distance} casillas. Costo: {movementCost}");
            return;
        }

        // Gastar energía si es válido
        selectedPiece.SpendEnergy(movementCost);


        // Movimiento válido = actualizar tablero
        Board[selectedPiece.currentX, selectedPiece.currentY] = null;
        Board[x, y] = selectedPiece;
        positioner.PositionSinglePiece(x, y, selectedPiece, false);

        // Limpiar selección
        selectedPiece = null;

        // Cambiar turno
        currentTurnTeam = (currentTurnTeam == 0) ? 1 : 0;
        RestoreEnergyToTeam(currentTurnTeam);

        // ver victoria
        // GetComponent<VictoryManager>().CheckVictory();
        StartCoroutine(DelayedVictoryCheck());

        Debug.Log("Turno del equipo " + currentTurnTeam);

       // highlighter.ClearHighlights();
    }

    private IEnumerator DelayedVictoryCheck()
    {
        yield return new WaitForSeconds(0.3f); // espera 200ms
        GetComponent<VictoryManager>().CheckVictory();
    }

    private void RestoreEnergyToTeam(int team)
    {
        foreach (var piece in Board)
        {
            if (piece != null && piece.team == team)
                piece.RestoreEnergy(1);
        }
    }

}
