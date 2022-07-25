using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Parsing
{
    public static class GollyParser
    {
        public static int[,] ParseState(string path)
        {
            int[,] cells = new int[1,1];
            int startX = 0;
            int startY = 0;
            int sizeX = 0;
            int sizeY = 0;
            int x = 0;
            int y = 0;
            int padding = 200;
            foreach (var line in File.ReadLines(path))
            {
                if (line[0] == 'x')
                {
                    var parms = line.Replace(" ", string.Empty).Split(',');
                    string xStr = parms[0].Substring(2);
                    string yStr = parms[1].Substring(2);
                    sizeX = int.Parse(xStr);
                    sizeY = int.Parse(yStr);
                    //x = startX + padding / 2;
                    //y = startY + padding / 2;

                    startX = padding / 2;
                    startY = padding / 2;
                    x = startX;
                    y = startY;

                    cells = new int[sizeX + padding, sizeY + padding];

                    continue;
                }
                else if (line[0] == '#')
                    continue;
                else
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        int n = 1;
                        string nStr = string.Empty;

                        if (char.IsDigit(line[i]))
                        {
                            while (char.IsDigit(line[i]))
                            {
                                nStr += line[i].ToString();
                                i++;
                            }

                            n = int.Parse(nStr);
                        }

                        switch (line[i])
                        {
                            case 'b':
                                for (int j = 0; j < n; j++)
                                    cells[x++, y] = 0;
                                break;

                            case 'o':
                                for (int j = 0; j < n; j++)
                                    cells[x++, y] = 1;
                                break;

                            case '$':
                                x = startX;
                                y += n;
                                break;

                            case '!':
                                return cells;
                        }
                    }
                }
            }

            return cells;
        }
    }
}
