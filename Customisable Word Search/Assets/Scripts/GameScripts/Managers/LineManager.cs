using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
	private GameManager gameManager;
	[SerializeField]
	private LetterGrid grid;
	[SerializeField]
	private LineRenderer line;
	private bool isDrawing;
	[SerializeField]
	private List<Color> lineColors = new List<Color>();
	[SerializeField]
	private List<Color> availableColors = new List<Color>();

	[SerializeField]
	private FloatVariable spacing;
	[SerializeField]
	private FloatVariable diagSpacing;

	[SerializeField]
	private UIManager uiManager;
	// Start is called before the first frame update
	void Awake()
	{
		gameManager = gameObject.GetComponent<GameManager>();
		availableColors = new List<Color>(lineColors);
	}

	public void StartDraw(Tile tile)
	{
		Vector3 startPos = tile.GetComponent<RectTransform>().TransformPoint(Vector3.zero);

		if(availableColors.Count == 0)
		{
			availableColors = new List<Color>(lineColors);
		}
		Color randCol = availableColors[Random.Range(0, availableColors.Count)];
		availableColors.Remove(randCol);
		randCol.a = 0.8f;
		line.startColor = randCol;
		line.endColor = randCol;
		startPos.z = 90;
		line.positionCount = 1;
		line.SetPosition(0, startPos);
	}

	public void EndDraw(Vector2 pos)
	{
		List<Tile> tiles = UpdateDraw(pos);
		grid.SelectTiles(tiles);
	}


	public List<Tile> UpdateDraw(Vector2 point)
	{
		Vector2 pos = Camera.main.ScreenToWorldPoint(point);
		if (line.positionCount < 2)
		{
			line.positionCount = 2;
		}
		Vector3 startPos = line.GetPosition(0);
		Vector3 endPos = (Vector3)pos;
		float distance = Vector2.Distance(startPos, endPos);
		float roundedDistance = 0.0f;
		int numTiles = 0;
		float angle = Quaternion.FromToRotation((Vector2)endPos - (Vector2)startPos, Vector2.up).eulerAngles.z;
		float roundedAngle = Mathf.Round(angle / 45) * 45;
		if (roundedAngle % 90 == 0)
		{
			numTiles = Mathf.RoundToInt(distance / spacing.Number);
			roundedDistance = numTiles * spacing.Number;
		}
		else
		{
			numTiles = Mathf.RoundToInt(distance / diagSpacing.Number);
			roundedDistance = numTiles * diagSpacing.Number;
		}
		Quaternion rotation = Quaternion.AngleAxis(roundedAngle, -Vector3.forward);
		Vector3 newLineVec = startPos + (rotation * Vector3.up * roundedDistance);
		endPos = newLineVec;
		endPos.z = 90;

		List<Tile> tiles = CheckForTilesOnLine(startPos, endPos, out endPos, numTiles, roundedDistance);
		line.SetPosition(1, endPos);
		return tiles;
	}

	List<Tile> CheckForTilesOnLine(Vector2 startPos, Vector2 endPos, out Vector3 endPoint, int numTiles, float roundedDistance)
	{
		Vector2 line = endPos - startPos;
		endPoint = Vector3.zero;
		List<Tile> tiles = new List<Tile>();
		for(int i = 0; i <= numTiles; i++)
		{
			float step = 0.0f;
			if(numTiles > 0)
			{
				step = (float)i / numTiles * roundedDistance;
			}
			else
			{
				step = (float)i / 1 * roundedDistance;
			}
			Vector2 point = startPos + (line.normalized * step);
			Tile tile = uiManager.CheckForTile(point);
			if(tile)
			{
				tiles.Add(tile);
				endPoint = point;
				endPoint.z = 90;
			}
			else
			{
				return tiles;
			}
			
		}
		return tiles;
	}
}
