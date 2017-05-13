#include <stdio.h>
#include <string.h>
#include <unistd.h>

#define FILE_PATH_LENGTH 1024

int main(void)
{
    char filePath[FILE_PATH_LENGTH];
    int i;

    while (1)
    {
        fgets(filePath, FILE_PATH_LENGTH, stdin);

        /* remove newline, if present */
        i = strlen(filePath) - 1;
        if (filePath[i] == '\n')
            filePath[i] = '\0';

        printf("asdasd\n");
        fflush(stdout);      

        int stdout_copy = dup(1);        
        close(1);        
        dup2(stdout_copy, 1);
        close(stdout_copy);
    }

    return 0;
}