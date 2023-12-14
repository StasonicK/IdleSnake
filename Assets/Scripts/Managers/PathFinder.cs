using UnityEngine;
using Utilities;

namespace Managers
{
    public class PathFinder : Singleton<PathFinder>
    {
        private bool _isReverse;


        public static Vector2Int RandomDirection()
        {
            int nextDirRand = Random.Range(1, 5);
            //Debug.Log(nextDirRand);
            Vector2Int direction = nextDirRand switch
            {
                1 => Vector2Int.down,
                2 => Vector2Int.right,
                3 => Vector2Int.up,
                4 => Vector2Int.left,
                _ => Vector2Int.zero
            };
            return direction;
        }

        // public Vector2Int GetNextCell(Vector2Int position, Vector2Int direction)
        // {
        //     Vector2Int nextDirection;
        //     if (direction == Vector2Int.down || direction == Vector2Int.up)
        //     {
        //         Vector2Int nextCell =
        //             new Vector2Int(Mathf.Clamp(position.x + direction.x, 0, FieldManager.Instance.FieldSize - 1),
        //                 Mathf.Clamp(position.y + direction.y, 0, FieldManager.Instance.FieldSize - 1));
        //         if (nextCell != position) return direction;
        //         nextDirection = (_isReverse) ? Vector2Int.right : Vector2Int.left;
        //         nextCell = new Vector2Int(
        //             Mathf.Clamp(position.x + nextDirection.x, 0, FieldManager.Instance.FieldSize - 1),
        //             Mathf.Clamp(position.y + nextDirection.y, 0, FieldManager.Instance.FieldSize - 1));
        //         if (nextCell != position) return nextDirection;
        //         _isReverse = !_isReverse;
        //         nextDirection = (_isReverse) ? Vector2Int.right : Vector2Int.left;
        //         nextCell = new Vector2Int(
        //             Mathf.Clamp(position.x + nextDirection.x, 0, FieldManager.Instance.FieldSize - 1),
        //             Mathf.Clamp(position.y + nextDirection.y, 0, FieldManager.Instance.FieldSize - 1));
        //         return nextDirection;
        //     }
        //     else
        //     {
        //         nextDirection = Vector2Int.down;
        //         Vector2Int nextCell =
        //             new Vector2Int(Mathf.Clamp(position.x + nextDirection.x, 0, FieldManager.Instance.FieldSize - 1),
        //                 Mathf.Clamp(position.y + nextDirection.y, 0, FieldManager.Instance.FieldSize - 1));
        //         if (nextCell != position) return nextDirection;
        //         nextDirection = Vector2Int.up;
        //         nextCell = new Vector2Int(
        //             Mathf.Clamp(position.x + nextDirection.x, 0, FieldManager.Instance.FieldSize - 1),
        //             Mathf.Clamp(position.y + nextDirection.y, 0, FieldManager.Instance.FieldSize - 1));
        //         return nextDirection;
        //     }
        // }
    }
}