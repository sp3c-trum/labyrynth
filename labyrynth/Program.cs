using System.IO;
using System.Reflection.Metadata.Ecma335;

Console.OutputEncoding = System.Text.Encoding.UTF8;

int width;
int height;
string input;
bool running = true;


init();
char[,] mapa = new char[height, width];
for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        mapa[i, j] = ' ';
    }
}
while (running)
{
    menu();
}

void init()
{

    try
    {
        Console.WriteLine("Wybierz wysokość labiryntu: ");
        height = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Wybierz szerokość labiryntu: ");
        width = Convert.ToInt32(Console.ReadLine());
    }
    catch
    {
        init();
    }
    
}

void menu(string? error = null, int whichMenu = 0) //0 - mainMenu, 1 - editMenu, 2 - saveMenu, 3 - ściany menu
{
    Console.Clear();
    if (error != null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ForegroundColor = ConsoleColor.White;
    }
    Console.WriteLine("Aktualny labirynt:");
    showMaze();
    switch (whichMenu)
    {
        case 0:
            Console.WriteLine("Wybierz opcję:" +
                "\n1.Edytuj labirynt" +
                "\n2.Załaduj/Zapisz labirynt" +
                "\n3.Wyjdź z programu");
            break;
        case 1:
            Console.WriteLine("Wybierz opcję:" +
                "\n1.Usuń element" +
                "\n2.Połóż ścianę" +
                "\n3.Połóż ścieżkę" +
                "\n4.Połóż cały szereg elementu");
            break;
        case 2:
            Console.WriteLine("Wybierz opcję:" +
                "\n1.Załaduj labirynt z pliku" +
                "\n2.Zapisz labirynt do pliku" +
                "\n3.Pokaż kod aktualnego labiryntu");
            break;
        case 3:
            Console.WriteLine("Wybierz opcję:" +
                "\n1.Usuń szereg elementów" +
                "\n2.Połóż szereg ścian" +
                "\n3.Połóż szereg ścieżek");
            break;
    }

    input = Console.ReadLine();
    if (int.TryParse(input, out int result))
    {
        switch (whichMenu)
        {
            case 0:
                switch (result)
                {
                    case 1:
                        menu(null, 1);
                        break;
                    case 2:
                        menu(null, 2);
                        break;
                    case 3:
                        running = false;
                        Console.WriteLine("Dziękuję za korzystanie!");
                        break;
                    default:
                        menu("Zły wybór!", whichMenu);
                        break;
                }
                break;
            case 1:
                switch (result)
                {
                    case 1:
                        editTile(' ');
                        break;
                    case 2:
                        editTile('█');
                        break;
                    case 3:
                        editTile('.');
                        break;
                    case 4:
                        menu(null, 3);
                        break;
                    default: 
                        menu("Zły wybór!", whichMenu); 
                        break;
                }
                break;
            case 2:
                switch (result)
                {
                    case 1:
                        Console.WriteLine("Wybierz labirynt do otworzenia (bez końcówki .lab): ");
                        input = Console.ReadLine();
                        readFile(input);
                        break;
                    case 2:
                        Console.WriteLine("Nazwij swój labirynt: ");
                        input = Console.ReadLine();
                        writeToFile(input);
                        break;
                    case 3:
                        menu(encodeMazeToString(), 0);
                        break;
                    default:
                        menu("Zły wybór!", whichMenu);
                        break;
                }
                break;
            case 3:
                switch (result)
                {
                    case 1:
                        editMultipleTile(' ');
                        break;
                    case 2:
                        editMultipleTile('█');
                        break;
                    case 3:
                        editMultipleTile('.');
                        break;
                    case 4:
                        menu(null, 3);
                        break;
                    default:
                        menu("Zły wybór!", whichMenu);
                        break;
                }
                break;
        }
        
    } else
    {
        menu("Zły wybór!", whichMenu);
    }
    
}

void editTile(char tile) { 
    Console.Clear();
    showMaze();
    Console.WriteLine("Podaj wysokość pola: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out int result) && result > height)
    {
        menu("Zły wybór pola!", 1);
    }
    Console.WriteLine("Podaj szerokość pola: ");
    input = Console.ReadLine();
    if (!int.TryParse(input, out int result2) && result2 > width)
    {
        menu("Zły wybór pola!", 1);
    }
    mapa[result, result2] = tile;
    menu(null, 0);
}

void editMultipleTile(char tile)
{
    char whichAxis;
    int min = 1;
    int max = 1;

    Console.Clear();
    showMaze();
    Console.WriteLine("Chcesz postawić szereg na wysokości czy szerokości? (w lub s): ");
    input = Console.ReadLine();

    if(!char.TryParse(input, out whichAxis))
    {
        menu("Zły wybór osi!", 1);
    }

    Console.Write("Podaj");
    if (whichAxis == 'w')
    {
        Console.Write(" wysokość ");
    } else if (whichAxis == 's')
    {
        Console.Write(" szerokość ");
    } 
    Console.Write("na której chcesz zacząć szereg: ");
    min = Convert.ToInt32(Console.ReadLine());


    Console.Write("Podaj");
    if (whichAxis == 'w')
    {
        Console.Write(" wysokość ");
    }
    else if (whichAxis == 's')
    {
        Console.Write(" szerokość ");
    }
    Console.Write("na której chcesz skończyć szereg: ");
    max = Convert.ToInt32(Console.ReadLine());

    if(whichAxis == 'w')
    {
        Console.WriteLine("Na której szerokości ma się położyć szereg?");
    } else if(whichAxis == 's')
    {
        Console.WriteLine("Na której wysokości ma się położyć szereg?");
    }

    if(int.TryParse(Console.ReadLine(), out int linia))
    if (whichAxis == 'w')
    {
        for (int i = min; i <= max; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if (j == linia)
                {
                    mapa[i, j] = tile;       
                }
            }
            
        }
    } else if(whichAxis == 's')
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = min; j <= max; j++)
                {
                    if (i == linia)
                    {
                        mapa[i, j] = tile;
                    }
                }
            }
        }
}

void showMaze()
{
    bool flagHeight = true;
    bool flagWidth = true;
    Console.WriteLine("█ - Ściana, . - ścieżka");
    for (int i = 0; i < height; i++)
    {

        if (flagWidth)
        {
            Console.Write("   ");
            for (int j = 0; j < width; j++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (j < 10)
                {
                    Console.Write(j + "  ");
                }
                else
                {
                    Console.Write(j + " ");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
            flagWidth = false;
        }

        for (int j = 0; j < width; j++)
        {
            if (flagHeight)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (i > 9)
                {
                    Console.Write(i);
                }
                else
                {
                    Console.Write(i + " ");
                }
                Console.ForegroundColor = ConsoleColor.White;
                flagHeight = false;
            }
            Console.Write(" " + mapa[i, j] + " ");
        }
        flagHeight = true;
        Console.WriteLine();
    }
}

string encodeMazeToString()
{
    string finishedMaze = Convert.ToString(height);
    finishedMaze += "\n";
    finishedMaze += width;
    finishedMaze += "\n";
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
           finishedMaze += mapa[i, j] + "\n";
        }
    }
    return finishedMaze;
}

void writeToFile(string input)
{
    File.WriteAllText(input + ".lab", encodeMazeToString());
}

void readFile(string path)
{
    try
    {
        StreamReader read = new StreamReader(path + ".lab");
        height = Convert.ToInt32(read.ReadLine());
        width = Convert.ToInt32(read.ReadLine());
        mapa = changeArraySize(mapa, height, width);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                mapa[i, j] = Convert.ToChar(read.ReadLine());
            }
        }
    }
    catch
    {
        menu("Błąd odczytaia pliku!", 2);
    }
    
    
}

char[,] changeArraySize(char[,] original, int height, int width)
{
    char[,] newArray = new char[height, width];
    int minHeight = Math.Min(original.GetLength(0), newArray.GetLength(0));
    int minWidth = Math.Min(original.GetLength(1), newArray.GetLength(1));

    for (int i = 0; i < minWidth; ++i)
        Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minHeight);

    return newArray;
}