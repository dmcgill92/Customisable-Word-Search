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
	private float spacing = 0.4f;
	private float diagSpacing = 0.57f;

	// Start is called before the first frame update
	void Start()
	{
		
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
							Debug.Log("Hit tile");
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
					Debug.Log("Hit tile");
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
		startPos.z = -1;
		line.numPositions = 1;
		line.SetPosition(0, startPos);
	}

	void EndDraw(Vector2 pos)
	{
		Vector3 startPos = line.GetPosition(0);
		Vector3 endPos = (Vector3)pos;
		float distance = Vector2.Distance(startPos, endPos);
		float roundedDistance = 0.0f;
		int numTiles = 0;
		float angle = Quaternion.FromToRotation((Vector2)endPos - (Vector2)startPos, Vector2.up).eulerAngles.z;
		float roundedAngle = Mathf.Round(angle / 45) * 45;
		if(roundedAngle % 90 == 0)
		{
			numTiles = Mathf.RoundToInt(distance / spacing);
			Debug.Log("Number of Tiles: " + numTiles);
			roundedDistance = numTiles * spacing;
		}
		else
		{
			numTiles = Mathf.RoundToInt(distance / diagSpacing);
			Debug.Log("Number of Tiles: " + numTiles);
			roundedDistance = numTiles * diagSpacing;
		}
		Quaternion rotation = Quaternion.AngleAxis(roundedAngle, -Vector3.forward);
		Vector3 newLineVec = startPos + (rotation * Vector3.up * roundedDistance);
		endPos = newLineVec;
		endPos.z = -1;

		endPos = CheckForTiles(startPos, endPos, numTiles, roundedDistance);
		endPos.z = -1;
		line.SetPosition(1, endPos);
	}

	Vector3 CheckForTiles(Vector2 startPos, Vector2 endPos, int numTiles, float roundedDistance)
	{
		Vector2 line = endPos - startPos;
		Vector3 endPoint = Vector3.zero;
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
			Debug.Log("Step: " + step);
			Vector2 point = startPos + (line.normalized * step);
			Collider2D collider = Physics2D.OverlapPoint(point);
			if(collider)
			{
				Tile tile = collider.GetComponent<Tile>();
				tiles.Add(tile);
				endPoint = point;
			}
			else
			{
				grid.SelectTiles(tiles);
				return endPoint;
			}
			
		}
		grid.SelectTiles(tiles);
		return endPoint;
	}

	void UpdateDraw(Vector2 pos)
	{
		if(line.numPositions != 2)
		{
			line.numPositions = 2;
		}
		Vector3 endPos = pos;
		endPos.z = -1;
		line.SetPosition(1, endPos);
	}
}
