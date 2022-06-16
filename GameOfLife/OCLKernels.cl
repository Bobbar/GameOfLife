
int cIdx(int x, int y, int2 dims);
int cIdx(int x, int y, int2 dims) 
{
	return x * dims.x + y;
}

int NumLivingNeighbors(int cellX, int cellY, int2 dims, global int* cells);
int NumLivingNeighbors(int cellX, int cellY, int2 dims, global int* cells)
{
	int living = 0;

    for (int x = -1; x <= 1; x++)
    {
        for (int y = -1; y <= 1; y++)
        {
            if (x == 0 && y == 0)
                continue;

            int ox = cellX + x;
            int oy = cellY + y;
            int cellIdx = cIdx(ox, oy, dims.x);

            if (ox >= 0 && oy >= 0 && ox < dims.x && oy < dims.y)
                if (cells[cellIdx] == 1)
                    living++;

        }
    }

    return living;
}


__kernel void ComputeNextState(global int* curState, global int* nextState, int2 dims, int len) 
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int gid = get_global_linear_id();

	/*if (gid >= len)
		return;*/

    if (x >= dims.x || y >= dims.y)
        return;

    int cellIdx = cIdx(x, y, dims);
    int nAlive = NumLivingNeighbors(x, y, dims, curState);

    bool curCell = curState[cellIdx];
    bool nextCell = curCell;

    if (curCell == 1)
    {
        if (nAlive < 2)
            nextCell = 0;

        if (nAlive > 3)
            nextCell = 0;
    }
    else
    {
        if (nAlive == 3)
            nextCell = 1;
    }

    nextState[cellIdx] = nextCell;
}