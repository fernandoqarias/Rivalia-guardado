using UnityEngine;

public class PlacementPhase : MonoBehaviour
{
    public int piecesPerTeam = 5;

    private int currentTeam = 0; // 0 = blanco, 1 = negro
    private int[] placedCount = new int[2];

    private bool[] hasKing = new bool[2];       // Un rey por equipo
    private int[] kingCount = new int[2];       // Para evitar más de 1 rey

    public bool IsPlacementFinished { get; private set; } = false;

    private ManagerGame manager;
    private ChessPieceType selectedType = ChessPieceType.Caballero;

    private void Awake()
    {
        manager = GetComponent<ManagerGame>();
    }

    private void Start()
    {
        manager.SetupGame();
        Debug.Log("Fase de colocación iniciada");
    }

    private void Update()
    {
        HandlePieceSelectionInput();
    }

    private void HandlePieceSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectPiece(ChessPieceType.Caballero);
            Debug.Log("Seleccionado: Caballero");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectPiece(ChessPieceType.Arquero);
            Debug.Log("Seleccionado: Arquero");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectPiece(ChessPieceType.Rey);
            Debug.Log("Seleccionado: Rey");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectPiece(ChessPieceType.Lanzero);
            Debug.Log("Seleccionado: Lanzero");
        }
    }


    public void SelectPiece(ChessPieceType type)
    {
        selectedType = type;
    }

    public void TryPlacePiece(int x, int y)
    {
        var board = manager.Board;

        // Si casilla ocupada ? no hacer nada
        if (board[x, y] != null)
        {
            Debug.Log("Casilla ocupada.");
            return;
        }

        // Si ya puso todas sus piezas ? no dejar
        if (placedCount[currentTeam] >= piecesPerTeam)
        {
            Debug.Log("El equipo ya colocó todas sus piezas.");
            return;
        }

        // --- REGLA 1: ZONA PERMITIDA ---
        if (currentTeam == 0 && x > 3)
        {
            Debug.Log("Blancas solo pueden colocar entre X=0 y X=4.");
            return;
        }

        if (currentTeam == 1 && x < 4)
        {
            Debug.Log("Negras solo pueden colocar entre X=5 y X=7.");
            return;
        }

        // --- REGLA 2: SOLO UN REY ---
        if (selectedType == ChessPieceType.Rey && kingCount[currentTeam] >= 1)
        {
            Debug.Log("Este equipo ya colocó su Rey.");
            return;
        }

        // --- REGLA 3: FORZAR QUE LA ÚLTIMA PIEZA SEA REY ---
        int remainingSpots = piecesPerTeam - placedCount[currentTeam];

        if (remainingSpots == 1 && !hasKing[currentTeam])
        {
            if (selectedType != ChessPieceType.Rey)
            {
                Debug.Log("Debes colocar un Rey como última pieza.");
                return;
            }
        }

        // Crear el pieceStart temporal
        PieceStart ps = new PieceStart
        {
            x = x,
            y = y,
            type = selectedType,
            team = currentTeam
        };

        // Spawnear usando tu sistema actual
        var spawned = manager.SpawnSingle(ps);
        board[x, y] = spawned;
        manager.PositionPieceOn(x, y, spawned);

        // Registrar rey
        if (selectedType == ChessPieceType.Rey)
        {
            hasKing[currentTeam] = true;
            kingCount[currentTeam]++;
        }

        placedCount[currentTeam]++;

        Debug.Log($"Equipo {currentTeam} colocó {selectedType} en {x},{y}");

        // Cambiar turno
        currentTeam = (currentTeam == 0) ? 1 : 0;

        // --- FIN DE FASE: ambos equipos deben haber colocado todo ---
        if (placedCount[0] >= piecesPerTeam && placedCount[1] >= piecesPerTeam)
        {
            if (!hasKing[0] || !hasKing[1])
            {
                // --- REGLA 4: AMBOS EQUIPOS DEBEN TENER AL MENOS UN REY ---
                Debug.Log("Ambos equipos deben colocar un Rey.");
                return;
            }

            Debug.Log("Fase de colocación completada.");
            IsPlacementFinished = true;
            manager.gameStarted = true;

            StartGame();
        }
    }

    private void StartGame()
    {
        Debug.Log("El juego comienza ahora.");
        // Aquí habilitas tu sistema de movimiento real
    }
}
