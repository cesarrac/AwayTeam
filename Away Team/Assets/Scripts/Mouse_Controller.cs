using UnityEngine;
using System.Collections;

public class Mouse_Controller : MonoBehaviour {

    public static Mouse_Controller Instance { get; protected set; }

    Vector3 curMousePos;
    Vector3 lastFramePosition;

    PathController selectedPathController;

    Vector3 curMouse_tileBased;
    Vector3 lastMouse_tileBased;

    public Tile TileUnderMouse { get; protected set; }

    bool mouseOverEnemy;
    public bool MouseOverEnemy {  get { return mouseOverEnemy; } set { mouseOverEnemy = value; } }

    Enemy_Handler enemyUnderMouse;
    public Enemy_Handler EnemyUnderMouse { get { return enemyUnderMouse; } set { enemyUnderMouse = value; } }

    Vector3 cursorPosition;

    void OnEnable()
    {
        Instance = this;
    }

    void Update()
    {
        TrackMousePosition();

        if (Input.GetMouseButtonDown(1))
        {
            Grid.Instance.OutputTileToDebug(Mathf.RoundToInt(curMousePos.x), Mathf.RoundToInt(curMousePos.y));
        }

        TrackDragMouse();



        if (Battle_StateManager.Instance._state == BattleState.PLAYER_TURN)
        {
            GetPathToPosition();

            TileUnderMouse = GetTileAtMouse();

       
        }

    }

    Tile GetTileAtMouse()
    {
        return Grid.Instance.GetTileAtCoord(Mathf.FloorToInt(curMousePos.x), Mathf.FloorToInt(curMousePos.y));
    }

    void TrackMousePosition()
    {
        Vector3 m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m.z = 0;
        curMousePos = m;

        Vector3 curMouse_tileBased = new Vector3(Mathf.Round(curMousePos.x), Mathf.Round(curMousePos.y), 0);
    }

    void TrackDragMouse()
    {
        if (Input.GetMouseButton(2) && lastFramePosition != Vector3.zero)
        {
            Vector3 diff = lastFramePosition - curMousePos;
            Camera_Controller.Instance.Drag(diff);
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    public void DisplayPathCursor()
    {
        if (TileUnderMouse != null)
        {
            cursorPosition = new Vector3(TileUnderMouse.posX, TileUnderMouse.posY, 0);
            Player_BattleController.Instance.DisplayCursor(cursorPosition);
        }
    }

    void GetPathToPosition()
    {
        // FIX THIS! ** Here we can add a check to make sure that the tile we just right clicked on is a WALKABLE tile
        if (Input.GetMouseButtonDown(1) && !MouseOverEnemy)
        {
            Player_BattleController.Instance.PlayerMove(curMousePos);
        }
    }

    public Vector3 GetTilePositionUnderMouse()
    {
        int x = Mathf.RoundToInt(curMousePos.x);
        int y = Mathf.RoundToInt(curMousePos.y);

        if (Grid.Instance.CheckMapBounds(x, y))
        {
            return new Vector3(x, y, 0);
        }
        else
        {
            return Vector3.zero;
        }

    }

    public bool CheckAttackRange(int range, Vector3 origin)
    {
        var diff = (curMousePos - origin).sqrMagnitude;

        if (diff <= range)
        {
           
            return true;
        }
        else
        {
            return false;
        }
    }
}
