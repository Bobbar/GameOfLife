typedef struct 
{
    int B;
    int S;

} Rule;


int cIdx(int x, int y, int2 dims);
int cIdx(int x, int y, int2 dims) 
{
    return x * dims.y + y;
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
            int cellIdx = cIdx(ox, oy, dims);

            if (ox >= 0 && oy >= 0 && ox < dims.x && oy < dims.y)
                if (cells[cellIdx] == 1)
                    living++;
        }
    }

    return living;
}


int GetState(int current, int nAlive, Rule rules);
int GetState(int current, int nAlive, Rule rules) 
{
    if (current == 1) 
    {
        int next = 0;
        for (int i = 0; i < 9; i++) 
        {
            int srv = 1 << i;
            if ((srv & rules.S) != 0) 
            {
                if (nAlive == i)
                    next = 1;
            }
        }

        return next;
    }
    else 
    {
        int next = 0;
        for (int i = 0; i < 9; i++)
        {
            int brv = 1 << i;
            if ((brv & rules.B) != 0)
            {
                if (nAlive == i)
                    next = 1;
            }
        }

        return next;
    }
}

__kernel void ComputeNextState(global int* curState, global int* nextState, int2 dims, int len, Rule rule) 
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int gid = get_global_linear_id();

    if (x >= dims.x || y >= dims.y)
        return;

    int cellIdx = cIdx(x, y, dims);
    int nAlive = NumLivingNeighbors(x, y, dims, curState);

    int curCell = curState[cellIdx];
    int nextCell = curCell;

    nextCell = GetState(nextCell, nAlive, rule);

    nextState[cellIdx] = nextCell;
}