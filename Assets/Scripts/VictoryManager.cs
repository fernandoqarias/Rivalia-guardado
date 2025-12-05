using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    private ManagerGame manager;
    public GameObject victoryPanelUI;

    private void Awake()
    {
        manager = GetComponent<ManagerGame>();
    }

    public void CheckVictory()
    {
        // Condición 1: ¿Un rey murió?
        if (IsKingDead(0))
        {
            TriggerVictory(1); // Gana negras
            return;
        }

        if (IsKingDead(1))
        {
            TriggerVictory(0); // Gana blancas
            return;
        }

        // Condición 2: ¿Solo queda el rey?
        if (OnlyKingLeft(0))
        {
            TriggerVictory(1);
            return;
        }

        if (OnlyKingLeft(1))
        {
            TriggerVictory(0);
            return;
        }

        // Condición 3: ¿Un rey llegó al lado enemigo?
        if (KingReachedEnemySide(0))
        {
            TriggerVictory(0);
            return;
        }

        if (KingReachedEnemySide(1))
        {
            TriggerVictory(1);
            return;
        }
    }

    private bool IsKingDead(int team)
    {
        foreach (var piece in manager.Board)
        {
            if (piece != null && piece.team == team && piece.type == ChessPieceType.Rey)
                return false;
        }

        return true;
    }

    private bool OnlyKingLeft(int team)
    {
        int count = 0;

        foreach (var piece in manager.Board)
        {
            if (piece != null && piece.team == team)
                count++;
        }

        return count == 1; // solo 1 pieza en ese equipo = el rey
    }

    private bool KingReachedEnemySide(int team)
    {
        // Si rey es blanco (team 0) debe llegar a x >= 4
        // Si rey es negro (team 1) debe llegar a x <= 3

        foreach (var piece in manager.Board)
        {
            if (piece != null && piece.team == team && piece.type == ChessPieceType.Rey)
            {
                if (team == 0 && piece.currentX >= TableGenerator.TILE_COUNT_X - 1) // ultima columna enemiga
                    return true;

                if (team == 1 && piece.currentX == 0) // primera columna enemiga
                    return true;
            }
        }

        return false;
    }

    private void TriggerVictory(int winnerTeam)
    {
        Debug.Log("El equipo " + winnerTeam + " ha ganado la partida!");

        manager.gameStarted = false;

        if (victoryPanelUI != null)
            victoryPanelUI.SetActive(true);
    }
}
