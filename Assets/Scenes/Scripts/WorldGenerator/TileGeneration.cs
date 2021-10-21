using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour {
  [SerializeField]
  NoiseMapGeneration noiseMapGeneration;
  [SerializeField]
  private MeshRenderer tileRenderer;
  [SerializeField]
  private MeshFilter meshFilter;
        [SerializeField] 
        private MeshCollider meshCollider;
  [SerializeField]
  private float mapScale;
  [SerializeField]
  private TerrainType[] terrainTypes;
  [SerializeField]
  private float heightMultiplier;
  [SerializeField]
  private AnimationCurve heightCurve;
    [SerializeField]
  private Wave[] waves;
  void Start() {
    GenerateTile ();
  }

  void GenerateTile() {
    // calculate tile depth and width based on the mesh vertices
    Vector3[] meshVertices = this.meshFilter.mesh.vertices;
    int tileDepth = (int)Mathf.Sqrt (meshVertices.Length);
    int tileWidth = tileDepth;
    // calculate the offsets based on the tile position
    float offsetX = -this.gameObject.transform.position.x;
    float offsetZ = -this.gameObject.transform.position.z;
    // generate a heightMap using noise
    float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap (tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);
    // build a Texture2D from the height map
    Texture2D tileTexture = BuildTexture (heightMap);
    this.tileRenderer.material.mainTexture = tileTexture;
    // update the tile mesh vertices according to the height map
    UpdateMeshVertices (heightMap);
  }

  private Texture2D BuildTexture(float[,] heightMap) {
    int tileDepth = heightMap.GetLength (0);
    int tileWidth = heightMap.GetLength (1);
    Color[] colorMap = new Color[tileDepth * tileWidth];
    for (int zIndex = 0; zIndex < tileDepth; zIndex+=512) {
      for (int xIndex = 0; xIndex < tileWidth; xIndex+=512) {
        TerrainType terrainType = ChooseTerrainType (heightMap, zIndex, xIndex);
        //float height = heightMap [zIndex, xIndex];
        Texture2D texture = terrainType.texture;
            for(int z = 0;z < 512 && z + zIndex < tileDepth;z++){
              for(int x = 0;x < 512 && x + xIndex < tileWidth;x++){
                int colorIndex = (z + zIndex) * tileWidth + (x + zIndex);
                colorMap[colorIndex] = texture.GetPixel(x, z);
                //Debug.Log(texture.GetPixel(x, z));
          }
        }


        //colorMap[colorIndex] = terrainType.color;
      }
    }
    // create a new texture and set its pixel colors
    Texture2D tileTexture = new Texture2D (tileWidth, tileDepth);
    tileTexture.wrapMode = TextureWrapMode.Clamp;
    
    Debug.Log(string.Join(", ", colorMap));

    tileTexture.SetPixels (colorMap);
    tileTexture.Apply ();
    return tileTexture;
  }


  TerrainType ChooseTerrainType(float[,] heightMap, int z, int x) {
    float heightSum = 0.0f;
    for(int zIndex = z;zIndex < z + 512 && zIndex < heightMap.GetLength(0);zIndex++){
        for(int xIndex = x;xIndex < x + 512  && xIndex < heightMap.GetLength(1);xIndex++){
          heightSum += heightMap[zIndex, xIndex];
      }

    float heightAvg = heightSum / 250000.0f;

    foreach (TerrainType terrainType in terrainTypes) {
      if (heightAvg < terrainType.height) {
        return terrainType;
      }
    }
    }
    return terrainTypes [terrainTypes.Length - 1];
  }



  private void UpdateMeshVertices(float[,] heightMap) {
    int tileDepth = heightMap.GetLength (0);
    int tileWidth = heightMap.GetLength (1);
    Vector3[] meshVertices = this.meshFilter.mesh.vertices;
    // iterate through all the heightMap coordinates, updating the vertex index
    int vertexIndex = 0;
    for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
      for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
        float height = heightMap [zIndex, xIndex];
        Vector3 vertex = meshVertices [vertexIndex];
        // change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
        meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);
        vertexIndex++;
      }
    }
    // update the vertices in the mesh and update its properties
    this.meshFilter.mesh.vertices = meshVertices;
    this.meshFilter.mesh.RecalculateBounds ();
    this.meshFilter.mesh.RecalculateNormals ();
    // update the mesh collider
    this.meshCollider.sharedMesh = this.meshFilter.mesh;
  }
}
