using UnityEngine;

public class TableGenerator : MonoBehaviour
{
    [SerializeField] private Material lightTileMaterial; // Color para Tile casillas claras
    [SerializeField] private Material darkTileMaterial; // Color para Tile casillas Oscuras 

    public const int TILE_COUNT_X = 8; // Genera La Tabla: Cantidad de casillas direccion X
    public const int TILE_COUNT_Y = 8; // Genera La Tabla: Cantidad de casillas direccion Y
    public const float tileSize = 2.0f;

    public static GameObject[,] tiles;        // Genera La Tabla: Repobla la matriz de casillas

    private GameObject GenerateSingleTile(int x, int y) //Genera una sola casilla
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{1}", x, y)); //Nombre de cada Tile
        tileObject.transform.parent = transform;  //Hace que el gameObject sea parte de Chessboard Object 

        bool isLightSquare = (x + y) % 2 == 0;
        Material boardMaterial = isLightSquare ? lightTileMaterial : darkTileMaterial; //Asigna el color claro y oscuro

        //Para hacer render de un objeto en unity se necesita un MeshFilter y MeshRenderer 
        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().sharedMaterial = boardMaterial; //Le da 2 colores
    
        //Generar la Geometria
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, y * tileSize, 0);
        vertices[1] = new Vector3(x * tileSize, (y + 1) * tileSize, 0);
        vertices[2] = new Vector3((x + 1) * tileSize, y * tileSize, 0);
        vertices[3] = new Vector3((x + 1) * tileSize, (y + 1) * tileSize, 0);

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 }; //Orden en que se rendera el triangulo

        mesh.vertices = vertices;
        mesh.triangles = tris;

        tileObject.AddComponent<BoxCollider>(); // Permite que el mouse pueda detectar el tile

        TileClick t = tileObject.AddComponent<TileClick>();
        t.x = x;
        t.y = y;

        return tileObject;
    }

    public void GenerateAllTiles() //Genera todas las casillas del tablero
    {
        tiles = new GameObject[TILE_COUNT_X, TILE_COUNT_Y];

        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                tiles[x, y] = GenerateSingleTile(x, y);
    }
}
