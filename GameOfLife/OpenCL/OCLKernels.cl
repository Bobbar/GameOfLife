﻿
typedef struct
{
	int B;
	int S;
	int C;

} Rule;

constant int2 N_OFFSET_LUT[] = { { -1,-1 }, { 0,-1 }, { 1,-1 }, { -1,0 }, { 1,0 }, { -1,1 }, { 0,1 }, { 1,1 } };

int cIdx(int x, int y, int2 dims);
int NumLivingNeighbors(int cellX, int cellY, int2 dims, global int* cells);
int GetState(int current, int nAlive, Rule rules);


int cIdx(int x, int y, int2 dims)
{
	return y * dims.x + x;
}


int NumLivingNeighbors(int cellX, int cellY, int2 dims, global int* cells)
{
	int living = 0;

#pragma unroll(8)
	for (int i = 0; i < 8; i++)
	{
		int2 offset = N_OFFSET_LUT[i];
		int ox = cellX + offset.x;
		int oy = cellY + offset.y;
		int cellIdx = cIdx(ox, oy, dims);

		if (ox >= 0 && oy >= 0 && ox < dims.x && oy < dims.y)
			if (cells[cellIdx] == 1)
				living++;
	}

	return living;
}


int GetState(int current, int nAlive, Rule rules)
{
	int next = 0;

	// Select the appropriate rule based on the current state.
	int rule = select(rules.B, rules.S, current);

#pragma unroll(9)
	for (int i = 0; i < 9; i++)
	{
		int ruleVal = 1 << i;
		if ((ruleVal & rule) != 0)
		{
			if (nAlive == i)
				next = 1;
		}
	}

	// Handle generational (multi-state) rules.
	if (rules.C > 0)
	{
		if (next == 0 && current == 1)
			next = 2;
		else if (current >= 2)
			next = ((current + 1) % rules.C);
	}

	return next;
}


__kernel void ComputeNextState(global int* curState, global int* nextState, int2 dims, Rule rule)
{
	int x = get_global_id(0);
	int y = get_global_id(1);

	if (x >= dims.x || y >= dims.y)
		return;

	int cellIdx = cIdx(x, y, dims);
	int nAlive = NumLivingNeighbors(x, y, dims, curState);
	int curCell = curState[cellIdx];
	int next = GetState(curCell, nAlive, rule);

	nextState[cellIdx] = next;
}