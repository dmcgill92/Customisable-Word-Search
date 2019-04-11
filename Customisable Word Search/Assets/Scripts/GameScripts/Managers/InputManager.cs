using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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

	// Start is called before the first frame update
	void Start()
	{
		availableColors = new List<Color>(lineColors);
	}

	// Update is called once per frame
	void Update()
	{
		#if UNITY_EDITOR
			CheckClicks();
		#endif
		#if UNITY_ANDROID && !UNITY_EDITOR
			Debug.Log("CheckTouches");
			CheckTouches();
		#endif


	}

	void CheckTouches()
	{
		int nbTouches = Input.touchCount;

		if (nbTouches > 0)
		{
			for (int i = 0; i < nbTouches; i++)
			{
				Touch touch = Input.GetTouch(i);

				if (touch.phase == TouchPhase.Began)
				{
					Vector2 point = Camera.main.ScreenToWorldPoint(touch.position);

					Collider2D hit = Physics2D.OverlapPoint(point);
					if (hit)
					{
						if (hit.transform.CompareTag("Tile"))
						{
							isDrawing = true;
							StartDraw(hit.transform.GetComponent<Tile>());
						}
						else
						if (hit.transform.CompareTag("Button"))
						{

						}
					}
				}

				if(isDrawing)
				{
					if(touch.phase == TouchPhase.Moved)
					{
						Vector2 point = Camera.main.ScreenToWorldPoint(touch.position);
						UpdateDraw(point);
					}
				}

				if(touch.phase == TouchPhase.Ended)
				{

					Vector2 point = Camera.main.ScreenToWorldPoint(touch.position);

					if (isDrawing)
					{
						EndDraw(point);
						isDrawing = false;
					}

					Collider2D hit = Physics2D.OverlapPoint(point);
					if (hit)
					{
						if (hit.transform.CompareTag("Tile"))
						{

						}
					}
				}
			}
		}
	}

	void CheckClicks()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			Collider2D hit = Physics2D.OverlapPoint(point);
			if (hit)
			{
				if (hit.transform.CompareTag("Tile"))
				{
					isDrawing = true;
					StartDraw(hit.transform.GetComponent<Tile>());
				}
				else
				if (hit.transform.CompareTag("Button"))
				{

				}
			}
		}

		if(isDrawing)
		{
			if(Input.GetMouseButton(0))
			{
				Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				UpdateDraw(point);
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (isDrawing)
			{
				EndDraw(point);
				isDrawing = false;
			}

			Collider2D hit = Physics2D.OverlapPoint(point);
			if (hit)
			{
				if (hit.transform.CompareTag("Tile"))
				{
				}
			}
		}
	}

	void StartDraw(Tile tile)
	{
		Vector3 startPos = tile.transform.position;
		if(availableColors.Count == 0)
		{
			availableColors = new List<Color>(lineColors);
		}
		Color randCol = availableColors[Random.Range(0, availableColors.Count)];
		availableColors.Remove(randCol);
		randCol.a = 0.8f;
		line.SetColors(randCol, randCol);
		startPos.z = -1;
		line.positionCount = 1;
		line.SetPosition(0, startPos);
	}

	void EndDraw(Vector2 pos)
	{
		List<Tile> tiles = UpdateDraw(pos);
		grid.SelectTiles(tiles);
	}


	List<Tile> UpdateDraw(Vector2 pos)
	{
		if(line.positionCount < 2)
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
		endPos.z = -1;
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
			Collider2D collider = Physics2D.OverlapPoint(point);
			if(collider)
			{
				Tile tile = collider.GetComponent<Tile>();
				tiles.Add(tile);
				endPoint = point;
				endPoint.z = -1;
			}
			else
			{
				return tiles;
			}
			
		}
		return tiles;
	}
}
