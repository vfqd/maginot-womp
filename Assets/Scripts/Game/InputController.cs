using System;
using System.Collections.Generic;
using Library;
using Library.Grid;
using Map;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class InputController : SerializedMonoSingleton<InputController>
    {
        public MouseFollowInfoPanel mouseFollow;
        public SpriteRenderer roomArea;
        public Sprite defaultRoom;
        public Sprite archerRoom;
        public Sprite artilleryRoom;
        public Tool currentTool;

        public Dictionary<Tool, Sprite> toolSprites;

        public ToolButton currentButton;

        public BuildingType currentRoom;

        private void Update()
        {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var snappedPos = new Vector3(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y));

            var tileUnderCursor = MapController.Instance.Grid.GetTile(snappedPos.x, snappedPos.y);
            if (tileUnderCursor == null) return;
            
            if (currentTool == Tool.None)
            {
                if (tileUnderCursor.Type == TileType.Room)
                {
                    var building = MapController.Instance.GetBuildingAt(tileUnderCursor);
                    if (building && building.shouldShowPanel && building.buildingType != BuildingType.Bunks)
                    {
                        UiController.Instance.infoPanel.Show(building);

                        if (Input.GetMouseButtonDown(0) && GameController.Instance.CanAffordWomps())
                        {
                            building.AddWomp();
                            UiController.Instance.infoPanel.UpdateCount(building);
                        }
                    }
                    else
                    {
                        UiController.Instance.infoPanel.Hide();
                    }
                }
                else
                {
                    UiController.Instance.infoPanel.Hide();
                }

                mouseFollow.Toggle(false);
                return;
            }
            
            UiController.Instance.infoPanel.Hide();
            if (currentTool == Tool.Mine)
            {
                mouseFollow.Toggle(true);
                mouseFollow.icon.sprite = toolSprites[Tool.Mine];
                if (tileUnderCursor.Type is TileType.Ground or TileType.Sandcastle or TileType.Metal)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        if (Input.GetMouseButton(0))
                        {
                            MiningController.Instance.RemoveTileFromMiningList(tileUnderCursor);
                        }
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        MiningController.Instance.AddTileToMiningList(tileUnderCursor);
                    }
                    else if (Input.GetMouseButton(2))
                    {
                        MiningController.Instance.RemoveTileFromMiningList(tileUnderCursor);
                    }
                }
            }
            else if (currentTool == Tool.Ladder)
            {
                mouseFollow.Toggle(true);
                mouseFollow.icon.sprite = toolSprites[Tool.Ladder];
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (tileUnderCursor.Type == TileType.Ladder &&
                        Input.GetMouseButton(0))
                    {
                        tileUnderCursor.ChangeTileTo(TileType.Air);
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    if (tileUnderCursor.Type == TileType.Air)
                    {
                        ResourcesController.Instance.ChangeResourceValue(ResourceType.Sand,-GameController.Instance.ladderCost);
                        tileUnderCursor.ChangeTileTo(TileType.Ladder);
                    }
                }
                else if (Input.GetMouseButton(2))
                {
                    if (tileUnderCursor.Type == TileType.Ladder)
                    {
                        tileUnderCursor.ChangeTileTo(TileType.Air);
                    }
                }
            }
            else if (currentTool == Tool.Build)
            {
                mouseFollow.Toggle(true);
                mouseFollow.icon.sprite = toolSprites[Tool.Build];
                if (tileUnderCursor.Type == TileType.Air &&
                    tileUnderCursor.X > 46)
                {
                    if (Input.GetMouseButton(0))
                    {
                        ResourcesController.Instance.ChangeResourceValue(ResourceType.Sand,-GameController.Instance.castleCost);
                        tileUnderCursor.ChangeTileTo(TileType.Sandcastle);
                    }
                }
            }
            else if (currentTool == Tool.PlaceRoom)
            {
                mouseFollow.Toggle(false);
                
                roomArea.gameObject.SetActive(true);
                roomArea.transform.position = snappedPos;
                var valid = IsAreaValidForRoom(tileUnderCursor);
                roomArea.color = valid ? Color.green : Color.red;

                if (valid)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var tool = currentButton;
                        if (tool.HasCost) ResourcesController.Instance.ChangeResourceValue(tool.resourceCost, -tool.cost);
                        MapController.Instance.PlaceBuildingAt(currentRoom, tileUnderCursor.X, tileUnderCursor.Y);
                    }
                }
                /*if (tileUnderCursor.Type == TileType.Air &&
                    GameController.Instance.CanAffordCastle(tileUnderCursor) &&
                    tileUnderCursor.CanBeStoodOn())
                {
                    if (Input.GetMouseButton(0))
                    {
                        tileUnderCursor.ChangeTileTo(TileType.Sandcastle);
                    }
                }*/
            }

            if (currentButton && !currentButton.CheckCanAfford())
            {
                DeselectAll();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                DeselectAll();
            }

            if (currentTool != Tool.PlaceRoom)
            {
                roomArea.gameObject.SetActive(false);
            }
            
            mouseFollow.transform.position = snappedPos;
        }

        public void SelectTool(Button button)
        {
            DeselectAll();
            currentButton = button.GetComponent<ToolButton>();
            currentButton.GetComponent<Image>().color = Color.red;
            InfoArea.Instance.SHowText(currentButton.tooltip);
            SelectTool(currentButton.tool,currentButton.buildingType);
        }
        public void SelectTool(Tool tool, BuildingType buildingType)
        {
            currentTool = tool;
            currentRoom = buildingType;
        }

        public void DeselectAll()
        {
            if (currentButton)
            {
                currentButton.GetComponent<Image>().color = Color.white;
                currentButton = null;
            }
            InfoArea.Instance.HideText();
            SelectTool(Tool.None,BuildingType.None);
        }

        private bool IsAreaValidForRoom(Tile tile)
        {
            bool aboveGround = currentRoom is BuildingType.Archer or BuildingType.Artillery;
            if (!aboveGround && tile.Y >= 62) return false;
            if (aboveGround && tile.Y < 64) return false;

            roomArea.sprite = currentRoom switch
            {
                BuildingType.Archer => archerRoom,
                BuildingType.Artillery => artilleryRoom,
                _ => defaultRoom
            };

            var width = MapController.Instance.buildingLookup[currentRoom].width;
            var half = width / 2;
            var height = currentRoom == BuildingType.Archer ? 0 : 1;
            for (int i = -half; i <= half; i++)
            {
                for (int j = -height; j <= height; j++)
                {
                    var t = MapController.Instance.Grid.GetTile(tile.X + i, tile.Y + j);
                    if (t == null) return false;
                    if (t.Type != TileType.Air) return false;
                    if (j == -height && !t.CanBeStoodOn()) return false;
                }
            }
            
            return true;
        }
    }

    public enum Tool
    {
        None,
        Mine,
        Build,
        Ladder,
        PlaceRoom
    }
}