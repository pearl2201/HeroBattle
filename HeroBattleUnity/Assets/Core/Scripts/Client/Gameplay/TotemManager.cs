using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class TotemManager : MonoBehaviour
{
    public List<Sprite> sprites;
    private TotemItem firstTotem;
    private TotemItem secondTotem;
    public List<TotemItem> items;
    public List<Totem> totems;
    public bool lockInput;
    public LudonPlayer player;

    public float inputXRange;
    public float inputYRange;

    public List<TotemItem> currentSelectTotemItems;

    public PendingHero pendingHero;

    public List<GameObject> planeInputs;
    public TotemManagerState status;
    public TextMeshProUGUI scoreLabel;

    public GameObject winnerPopup;
    public TextMeshProUGUI winnerLabel;
    public enum TotemManagerState
    {
        MatchTotem,
        PutHero,
        None
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void ShowPutPendingHero(PendingHero hero)
    {
        pendingHero = hero;
        status = TotemManagerState.PutHero;
        planeInputs[(int)player.team].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            scoreLabel.text = "Score: " + player.score.ToString("D3");
        }

        if (status == TotemManagerState.MatchTotem)
        {
            if (Input.GetMouseButtonUp(0))
            {
                var position = Input.mousePosition;
                var worldPosition = ScreenToRectPos(position);
                Debug.Log($"InputXRange: input {position}, local: {worldPosition}");
                if (Mathf.Abs(worldPosition.y) > inputYRange || Mathf.Abs(worldPosition.y) > inputXRange)
                {
                    foreach (var item in currentSelectTotemItems)
                    {
                        item.SetTotemUnselected();
                    }
                    currentSelectTotemItems.Clear();
                    firstTotem = null;
                    Debug.Log($"clear totem");
                }
                else
                {
                    Debug.Log($"trigger totem");
                    player.ConsumeTotems(currentSelectTotemItems.Select(x => x.data).ToList());
                    foreach (var item in currentSelectTotemItems)
                    {
                        item.SetTotemUnselected();
                    }
                    currentSelectTotemItems.Clear();
                    firstTotem = null;


                }
            }
        }
        else
        {
            int layerMask = 1 << 6;
            var position = Input.mousePosition;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSuccess = false;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                hitSuccess = true;
            }
            if (Input.GetMouseButtonUp(0) && hitSuccess)
            {
                status = TotemManagerState.None;
                player.DropHero(hit.point);

            }
        }


    }

    public void HideTotems(List<Totem> totems, List<int> newValues, int minTotemX)
    {
        for (int i = 0; i < totems.Count; i++)
        {
            var totemItem = items.FirstOrDefault(x => x.data.x == totems[i].x && x.data.y == totems[i].y);
            totemItem.SetTotem(new Totem()
            {
                x = totems[i].x + (14 - minTotemX),
                y = totems[i].y,
                value = newValues[i]
            });

        }
    }

    public void MoveTotems(int minTotemX, int deltaX, bool isTriggerTop, bool isTriggerBottom)
    {
        foreach (var item in items)
        {
            var totem = item.data;
            if (totem.x >= minTotemX)
            {
                if (totem.y == 0 && isTriggerTop)
                {

                    item.MoveTotemTo(totem.x - deltaX, totem.y, 0.75f);
                }
                else if (totem.y == 1 && isTriggerBottom)
                {
                    item.MoveTotemTo(totem.x - deltaX, totem.y, 0.75f);
                }
            }
        }
    }

    public Vector2 ScreenToRectPos(Vector2 screen_pos)
    {
        var rectTransform = GetComponent<RectTransform>();

        var canvas = rectTransform.GetComponentInParent<Canvas>().rootCanvas;
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null)
        {
            //Canvas is in Camera mode
            Vector2 anchorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screen_pos, canvas.worldCamera, out anchorPos);
            return anchorPos;
        }
        else
        {
            //Canvas is in Overlay mode
            Vector2 anchorPos = screen_pos - new Vector2(rectTransform.position.x, rectTransform.position.y);
            anchorPos = new Vector2(anchorPos.x / rectTransform.lossyScale.x, anchorPos.y / rectTransform.lossyScale.y);
            return anchorPos;
        }
    }

    public void OnPointerMove()
    {
        // option 1: mouse to center (1/3)

        // option 2: mouse to bottom/top center board

        // option 3: mouse to left/right center board
    }

    public void OnPostRender()
    {
        // option 1: outside manager box

        // option 2: inside manager box

        // option 2.1: trigger 1 only

        // option 2.2: trigger 2 ver only

        // option 2.3: trigger 2 hoz only

        // option 2.3: trigger 4 only.
    }
    public void SetupTotem(SyncList<Totem> totems)
    {

        this.totems = totems.ToList();
        for (int i = 0; i < totems.Count; i++)
        {
            items[i].SetTotem(new Totem()
            {
                x = totems[i].x,
                y = totems[i].y,
                value = totems[i].value,
            });
        }
        status = TotemManagerState.MatchTotem;
    }


    public void OnPointerDown(TotemItem totem)
    {
        if (status == TotemManagerState.MatchTotem)
        {
            Debug.Log($"on totem down: {totem.data.x}, {totem.data.y}");
            firstTotem = totem;
            totem.SetTotemSelected();

            var newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { });

            if (newInput != null)
            {
                foreach (var item in currentSelectTotemItems)
                {
                    item.SetTotemUnselected();
                }
                currentSelectTotemItems = newInput;
                foreach (var item in currentSelectTotemItems)
                {
                    item.SetTotemSelected();
                }
            }
        }

    }

    public void OnPointerEnter(TotemItem totem)
    {
        if (status == TotemManagerState.MatchTotem)
        {
            if (firstTotem == null)
            {
                return;
            }
            Debug.Log($"on totem enter: {totem.data.x}, {totem.data.y}");
            List<TotemItem> newInput = null;
            if (firstTotem.data.x == totem.data.x && firstTotem.data.y == totem.data.y + 1)
            {

                // up
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(0, -1) });
            }
            else if (firstTotem.data.x == totem.data.x && firstTotem.data.y == totem.data.y - 1)
            {

                // down
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(0, 1) });

            }
            else if (firstTotem.data.x == totem.data.x + 1 && firstTotem.data.y == totem.data.y)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(-1, 0) });

                // right

            }
            else if (firstTotem.data.x == totem.data.x - 1 && firstTotem.data.y == totem.data.y)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(1, 0) });
                // left
            }
            if (firstTotem.data.x == totem.data.x + 1 && firstTotem.data.y == totem.data.y + 1)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1) });
                // down left

            }
            else if (firstTotem.data.x == totem.data.x - 1 && firstTotem.data.y == totem.data.y + 1)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1) });
                // down right

            }
            else if (firstTotem.data.x == totem.data.x + 1 && firstTotem.data.y == totem.data.y - 1)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1) });

                // up left

            }
            else if (firstTotem.data.x == totem.data.x - 1 && firstTotem.data.y == totem.data.y - 1)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1) });
                // up right
            }
            else if (firstTotem.data.x == totem.data.x && firstTotem.data.y == totem.data.y)
            {
                newInput = DetectAndConstructInput(firstTotem, new List<Vector2Int>() { });
                // up

            }

            if (newInput != null)
            {
                foreach (var item in currentSelectTotemItems)
                {
                    item.SetTotemUnselected();
                }
                currentSelectTotemItems = newInput;
                foreach (var item in currentSelectTotemItems)
                {
                    item.SetTotemSelected();
                }
            }
        }
    }

    public List<TotemItem> DetectAndConstructInput(TotemItem root, List<Vector2Int> chains)
    {
        List<TotemItem> totemItemsInput = new List<TotemItem>() { root };

        bool valid = true;

        foreach (var vec2 in chains)
        {
            var totemItem = items.FirstOrDefault(x => x.data.x == root.data.x + vec2.x && x.data.y == root.data.y + vec2.y);
            if (totemItem == null || totemItem.data.value != root.data.value)
            {
                valid = false;
                break;
            }
            else
            {
                totemItemsInput.Add(totemItem);

            }
        }

        if (valid)
        {
            return totemItemsInput;
        }
        else
        {
            return null;
        }
    }

    public void ResetInput()
    {
        Debug.Log("Reset Input");
        status = TotemManagerState.MatchTotem;
        foreach (var plane in planeInputs)
        {
            plane.SetActive(false);
        }
        foreach (var item in currentSelectTotemItems)
        {
            item.SetTotemUnselected();
        }
        currentSelectTotemItems.Clear();
        firstTotem = null;
        pendingHero = null;
    }


    public void ShowWinnerInfo(LudonPlayer winner)
    {
        status = TotemManagerState.None;
        winnerPopup.gameObject.SetActive(true);
        winnerLabel.text = $"{winner.team} player win the game with {winner.score} point";
    }
}
