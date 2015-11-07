#include <iostream>
#include <cstdlib>
#include <cstdio>
#include <string>
#include <cstring>
#include <algorithm>
#include <time.h>

using namespace std;

const int h = 14; 
const int w = 19;
const int tw[4][2] = {{0, 1}, {1, 0}, {0, -1}, {-1, 0}};

int map[h][w];
int line[25];
int startx, starty;

void fillline(int st, int en) {
	for (int i = st; i <= en; i++) line[i] = 7;
	int ii = 1 + rand()%2;
	for (int i = 0; i < ii; i++) {
		int p = st + rand()%(en - st + 1);
		int l = 1 + rand()%2;
		for (int j = 0; j <= l; j++) line[p + j] = 0;
	}
	
	int c = 1 + rand()%4;
	for (int i = st; i <= en; i++)
		if (line[i] == 7) line[i] = c;
		else c = 1 + rand()%4;
}

void work(int hi, int ha, int wi, int wa) {
	if (rand() % 2 == 0 && ha - hi >= 2 && wa >= wi) {
		fillline(wi, wa);
		int i = hi + rand()%(ha - hi + 1);
		for (int j = wi; j <= wa; j++) map[i][j] = line[j];

		work(hi, i-1, wi, wa);
		work(i+1, ha, wi, wa);
	} else if (wa - wi >= 2 && ha >= hi ) {
		fillline(hi, ha);
		int i = wi + rand()%(wa - wi + 1);
		for (int j = hi; j <= ha; j++) map[j][i] = line[j];

		work(hi, ha, wi, i-1);
		work(hi, ha, i+1, wa);		
	}
}

void selectstart() {
	int xx = rand()%58;
	if (xx < 17) {startx = 0; starty = xx;}
	else if (xx < 34) {startx = h-1; starty = xx - 17;}
	else if (xx < 46) {startx = xx - 34; starty = 0;}
	else {startx = xx - 46; starty = w-1;}
}

bool checkmaze() {
	int m[25][25];
	int qx[400], qy[400];
	int st = 0, en = 0;
	for (int i = 0; i < h; i++)
		for (int j = 0; j < w; j++) m[i][j] = -1;
		
	qx[0] = startx; qy[0] = starty; m[startx][starty] = 0;
	for ( ; st <= en; st ++) {
		for (int i = 0; i < 4; i++) {
			int nx = qx[st] + tw[i][0], ny = qy[st] + tw[i][1];
			if (nx >= 0 && nx < h && ny >= 0 && ny < w && map[nx][ny] == 0 && m[nx][ny] == -1) {
				qx[++en] = nx;
				qy[en] = ny;
				m[nx][ny] = m[qx[st]][qy[st]] + 1;
			}
		}
	}
	if (en < 10) return false;
	else {
		map[qx[en]][qy[en]] = 8;
		map[startx][starty] = 0;
		return true;
	}
}

int main(int argc, char** argv) {
	srand((unsigned) time(NULL));
	
	for (int i = 0; i < h; i ++) {
		for (int j = 1; j < w-1; j++)
			if (i == 0 || i == h-1) map[i][j] = 1 + rand()%4;
			else map[i][j] = 0;
		map[i][0] =  1 + rand()%4;
		map[i][w-1] =  1 + rand()%4;
	}
	
	work(1, h-2, 1, w-2);
	selectstart();
	while (! checkmaze()) selectstart();
	
	freopen("a.txt", "w", stdout);
	for (int i=0; i<h; i++) {
		for (int j=0; j<w; j++) printf("%d", map[i][j]);
		printf("\n");
	}
	printf("%d, %d", startx, starty);
	return 0;
}

