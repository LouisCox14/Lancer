using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Teams/Player Team")]
public class PlayerTeam : Team
{
    private PlayerControls playerInputActions;
    private InputAction leftClickAction;
    private InputAction rightClickAction;
    [HideInInspector] public InputAction mousePos;

    private Vector2Int currentMouseOver;

    CancellationTokenSource actionPreviewCancellationToken = new CancellationTokenSource();
    private ActionBase selectedAction;

    public delegate void entitySelectedDelegateHandler(PlayerTeam team, ActingEntity actingEntity);
    public static entitySelectedDelegateHandler onEntitySelected;

    [SerializeField] private ObjectPool hexHighlightPool;
    private Dictionary<GridObject, HexHighlight> hexHighlights = new Dictionary<GridObject, HexHighlight>();

    void OnEnable()
    {
        playerInputActions = new PlayerControls();
        leftClickAction = playerInputActions.MouseInput.LeftClick;
        rightClickAction = playerInputActions.MouseInput.RightClick;
        mousePos = playerInputActions.MouseInput.MousePos;
    }

    void AddHighlights(List<GridObject> targetObjects)
    {
        foreach (GridObject targetObject in targetObjects)
        {
            HexHighlight hexHighlight = hexHighlightPool.RequestFromPool().GetComponent<HexHighlight>();
            hexHighlight.transform.position = targetObject.transform.position;
            hexHighlights.Add(targetObject, hexHighlight);
        }
    }

    void RemoveHighlights(List<GridObject> targetObjects)
    {
        foreach (GridObject targetObject in targetObjects)
        {
            if (hexHighlights.ContainsKey(targetObject))
            {
                hexHighlights[targetObject].gameObject.SetActive(false);
                hexHighlights.Remove(targetObject);
            }
        }
    }

    void SetHighlights(List<GridObject> targetObjects)
    {
        RemoveHighlights(hexHighlights.Keys.ToList());
        AddHighlights(targetObjects);
    }

    public override void StartTurn(CombatManager _combatManager)
    {
        base.StartTurn(_combatManager);

        leftClickAction.performed += OnLeftClick;
        leftClickAction.Enable();
        rightClickAction.performed += OnRightClick;
        rightClickAction.Enable();
        mousePos.performed += OnMouseMove;
        mousePos.Enable();

        selectedAction = null;
        actingEntity = null;

        AddHighlights(turnsAvaliable.Cast<GridObject>().ToList());
    }

    public override void EndTurn()
    {
        base.EndTurn();

        RemoveHighlights(hexHighlights.Keys.ToList());

        leftClickAction.performed -= OnLeftClick;
        leftClickAction.Disable();
        rightClickAction.performed -= OnRightClick;
        rightClickAction.Disable();
        mousePos.performed -= OnMouseMove;
        mousePos.Disable();
    }

    public void SetSelectedAction(ActionBase newAction)
    {
        if (selectedAction != newAction)
        {
            actionPreviewCancellationToken.Cancel();
            actionPreviewCancellationToken = new CancellationTokenSource();

            selectedAction = newAction;

            newAction.Preview(combatManager, actingEntity, this, actionPreviewCancellationToken.Token);
        }
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Grid grid = (Grid)combatManager.gridObject.value;
        Vector2Int cellClicked = (Vector2Int)grid.WorldToCell(Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>()));

        if ((!actingEntity || !actingEntity.hasActed) && teamEntities.GetObjectAtGridPos(cellClicked))
        {
            actingEntity = (ActingEntity)teamEntities.GetObjectAtGridPos(cellClicked);
            onEntitySelected?.Invoke(this, actingEntity);

            List<GridObject> highLightTargets = turnsAvaliable.Cast<GridObject>().ToList();
            highLightTargets.Remove(actingEntity);
            SetHighlights(highLightTargets);
            return;
        }

        if (!selectedAction)
        {
            return;
        }
        
        if (actingEntity != null && selectedAction.IsActionPossible(combatManager, actingEntity, cellClicked))
        {
            actionPreviewCancellationToken.Cancel();
            actionPreviewCancellationToken = new CancellationTokenSource();
            RemoveHighlights(hexHighlights.Keys.ToList());

            if (!actingEntity.hasActed)
            {
                actingEntity.StartTurn();
            }

            selectedAction.Execute(combatManager, actingEntity, cellClicked);
            selectedAction = null;
            return;
        }
    }

    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        Grid grid = (Grid)combatManager.gridObject.value;
        Vector2Int newMouseOver = (Vector2Int)grid.WorldToCell(Camera.main.ScreenToWorldPoint(mousePos.ReadValue<Vector2>()));

        if (newMouseOver == currentMouseOver)
        {
            return;
        }

        currentMouseOver = newMouseOver;
    }

    private void OnRightClick(InputAction.CallbackContext ctx)
    {
        actionPreviewCancellationToken.Cancel();
        actionPreviewCancellationToken = new CancellationTokenSource();
        selectedAction = null;
    }
}
