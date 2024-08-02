using DredPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : SimpleSingleton<LineManager>
{
    public LineRenderer line;
    public bool isRaycastCell = false;
    public float positionZ;

    public void StartLine(Vector3 startPos, Vector3 endPos, List<Vector3> middlePos)
    {
        var posCount = 2 + middlePos.Count;
        line.positionCount = posCount;
        line.SetPosition(0, new Vector3(startPos.x, startPos.y, positionZ));

        for (int i = middlePos.Count - 1; i > -1; i--)
        {
            line.SetPosition(i + 1, new Vector3(middlePos[i].x, middlePos[i].y, positionZ));
        }

        line.SetPosition(posCount - 1, new Vector3(endPos.x, endPos.y, positionZ));
    }

    public void ClearLine()
    {
        if (line.positionCount > 0)
            line.positionCount = 0;
    }
}
