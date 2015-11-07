#include <iostream>
#include <cstdlib>
#include <cstdio>
#include <string>
#include <cstring>
#include <algorithm>

using namespace std;

char ss[100];
int x[4][1000], y[4][1000];
int s[4];

int main(int argc, char** argv) {
	freopen("a.txt", "r", stdin);
	freopen("z.txt", "w", stdout);
	s[0] = s[1] = s[2] = s[3] = 0;
	for (int i=0; i < 14; i++) {
		gets(ss);
		for (int j = 0; j < 19; j++) {
			int num = ss[j] - '0';
			if (num == 0) continue;
			if (num == 9) {
				printf("%d %d,;\n", j*32, (13 - i)*32);
				continue;
			}
			if (num == 'x' - '0') num = rand()%4;
			else num -= 1;
			y[num][s[num]] = (13 - i)*32;
			x[num][s[num]++] = j*32;
		}
	}
	for (int i=0; i < 4; i++) {
		for (int j=0; j < s[i]; j++) printf("%d %d,\n", x[i][j], y[i][j]);
		printf("\n");
	}
	return 0;
}

