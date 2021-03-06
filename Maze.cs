using System;
using System.Collections.Generic;
namespace MazeLib
{
    /// <summary>
    /// Генератор лабиринтов размерами X*Y
    /// Где 1 - это стена
    /// 2 - стрелка
    /// 3 - конечная точка
    /// 0 - пустое пространство
    /// </summary>
    public class Maze
    {
        Random rnd = new Random();
        /// <summary>
        /// Координаты конечной точки
        /// </summary>
        int EndPointX, EndPointY = 1;
        /// <summary>
        /// Самый длинный путь до цели
        /// </summary>
        public int MaxWay = 0;
        /// <summary>
        /// Массив матрицы лабиринта
        /// </summary>
        public int[,] Map { get; set; }
        /// <summary>
        /// Массив с размерами матрицы лабиринта
        /// </summary>
        private int[] MapSizeXY = new int[] {5,5};
        /// <summary>
        /// Вывод матрицы лабиринта на консоль
        /// </summary>
        public void Print()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Вывод матрицы лабиринта по заданным размерам на консоль
        /// При введении размеров < 5 переменная будет равна 5
        /// </summary>
        /// <param name="Xsize">Длинна по X</param>
        /// <param name="Ysize">Длинна по Y</param>

        public void Print(int Xsize, int Ysize)
        {
            GetNewLevel(Xsize, Ysize);
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Конструктор, принимающий размеры матрицы лабиринта
        /// При введении размеров < 5 переменная будет равна 5
        /// </summary>
        /// <param name="Xsize">Длинна по X</param>
        /// <param name="Ysize">Длинна по Y</param>
        
        public Maze(int Xsize, int Ysize)
        {
            if (Xsize < 5) { Xsize = 5; }
            if (Ysize < 5) { Ysize = 5; }
            MapSizeXY[0] = Xsize;
            MapSizeXY[1] = Ysize;
            Generation();
        }
        /// <summary>
        /// Метод создание матрицы лабиринта по заданным размерам
        /// При введении размеров < 5 переменная будет равна 5
        /// </summary>
        /// <param name="Xsize">Длинна по X</param>
        /// <param name="Ysize">Длинна по Y</param>
        
        /// <returns>Возвращает матрицу лабиринта по заданным размерам</returns>
        public int[,] GetNewLevel(int Xsize, int Ysize)
        {
            if (Xsize < 5) { Xsize = 5; }
            if (Ysize < 5) { Ysize = 5; }
            MapSizeXY[0] = Xsize;
            MapSizeXY[1] = Ysize;
            Generation();
            return (Map);
        }
        /// <summary>
        /// Метод создание матрицы лабиринта по уже заданным рамерам 
        /// </summary>
        /// <returns>Возвращает матрицу лабиринта по уже заданным размерам</returns>
        public int[,] GetNewLevel()
        {
            Generation();
            return (Map);
        }

        private void Generation()
        {
            Map = new int[MapSizeXY[1], MapSizeXY[0]];  
            for (int y = 0; y < Map.GetLength(0); y++)   //Map.GetLength(0) - Y 
            {
                for (int x = 0; x < Map.GetLength(1); x++)    //Map.GetLength(1) - X
                {
                    Map[y, x] = 1;
                }                     
            }
            int ArrowX = rnd.Next(1, Map.GetLength(1) - 1); // Случайный X
            int ArrowY = rnd.Next(1, Map.GetLength(0) - 1); // Случайный Y
             
            //MaxWay = 0;
            //Worm(ArrowX, ArrowY, 0, MaxWay);
            WormV2(ArrowX, ArrowY);
            Map[ArrowY, ArrowX] = 2;// Создание стрелки в случайном месте
            Map[EndPointY, EndPointX] = 3;

        }
        private void Worm(int x, int y, int BackDirection, int MaxWaylocal) //Метод с рекурсией
        {
            MaxWaylocal++; //Иттератор для максимального пути
            if(MaxWaylocal > MaxWay)
            {
                MaxWay = MaxWaylocal;
                EndPointX = x;
                EndPointY = y;
            }
            bool debug; //Результат проверки доступности клетки
            int SideCount = 10; //Максимальное количество сторон для проверки
            for(int i = 0; i < SideCount; i++) //Перебор каждой стороны
            {
                int DirectionSide = rnd.Next(1, 5);
                while(DirectionSide == BackDirection) //Выбирает не сторону, откуда пришел
                {
                    DirectionSide = rnd.Next(1, 5);
                }
                if (DirectionSide == 1) //Вправо
                {
                    debug = BoxCheck(x + 1, y, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                    if (debug)
                    {
                        Map[y, x + 1] = 0;
                        Worm(x + 1, y, 3, MaxWaylocal); // 3 - это сторона, с которой прийдет сигнал
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (DirectionSide == 2) //Вверх
                {
                    debug = BoxCheck(x, y - 1, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                    if (debug)
                    {
                        Map[y - 1, x] = 0;
                        Worm(x, y - 1, 4, MaxWaylocal); // 4 - это сторона, с которой прийдет сигнал
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (DirectionSide == 3) //Влево
                {
                    debug = BoxCheck(x - 1, y, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                    if (debug)
                    {
                        Map[y, x - 1] = 0;
                        Worm(x - 1, y, 1, MaxWaylocal); // 1 - это сторона, с которой прийдет сигнал
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (DirectionSide == 4) //Вниз
                {
                    debug = BoxCheck(x, y + 1, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                    if (debug)
                    {
                        Map[y + 1, x] = 0;
                        Worm(x, y + 1, 2, MaxWaylocal); // 2 - это сторона, с которой прийдет сигнал
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        private void WormV2(int x, int y, int BackDirection = 0, int MaxWaylocal = 0) // Новый метод, который использует рекурсию только для веток
        {
            List<TypeObject> NewWays = new List<TypeObject>();
            int[] Sides = { 0, 0, 0 };
            bool NewWay = false;
            //MaxWaylocal++; //Иттератор для максимального пути

            bool debug; //Результат проверки доступности клетки
                        //int SideCount = 10; //Максимальное количество сторон для проверки
            int Count;
            while (true)
            {
                if (MaxWaylocal > MaxWay)
                {
                    MaxWay = MaxWaylocal;
                    EndPointX = x;
                    EndPointY = y;
                }
                Count = 0;
                Sides = new int[] { 0, 0, 0 };
                NewWay = false;
                for (int i = 0; i < 3; i++) //Перебор каждой стороны
                {
                    int DirectionSide = rnd.Next(1, 5);
                    while (DirectionSide == BackDirection || DirectionSide == Sides[0] || DirectionSide == Sides[1] || DirectionSide == Sides[2]) //Выбирает не сторону, откуда пришел
                    {
                        DirectionSide = rnd.Next(1, 5);
                    }
                    Sides[i] = DirectionSide;
                    if (DirectionSide == 1) //Вправо
                    {
                        debug = BoxCheck(x + 1, y, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                        if (debug)
                        {
                            if (NewWay)
                            {
                                NewWays.Add(new TypeObject(x, y, 3, MaxWaylocal));
                            }
                            else
                            {
                                x++;
                                Map[y, x] = 0;
                                BackDirection = 3; // 3 - это сторона, с которой прийдет сигнал
                                MaxWaylocal++;
                                NewWay = true;
                            }
                        }
                        else
                        {
                            Count++;

                        }
                    }
                    else if (DirectionSide == 2) //Вверх
                    {
                        debug = BoxCheck(x, y - 1, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                        if (debug)
                        {
                            if (NewWay)
                            {
                                NewWays.Add(new TypeObject(x, y, 4, MaxWaylocal));
                            }
                            else
                            {
                                y--;
                                Map[y, x] = 0;
                                BackDirection = 4;// 4 - это сторона, с которой прийдет сигнал
                                MaxWaylocal++;
                                NewWay = true;
                            }

                        }
                        else
                        {
                            Count++;
                        }
                    }
                    else if (DirectionSide == 3) //Влево
                    {
                        debug = BoxCheck(x - 1, y, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                        if (debug)
                        {
                            if (NewWay)
                            {
                                NewWays.Add(new TypeObject(x, y, 1, MaxWaylocal));
                            }
                            else
                            {
                                x--;
                                Map[y, x] = 0;
                                BackDirection = 1;// 1 - это сторона, с которой прийдет сигнал
                                MaxWaylocal++;
                                NewWay = true;
                            }
                        }
                        else
                        {
                            Count++;

                        }
                    }
                    else if (DirectionSide == 4) //Вниз
                    {
                        debug = BoxCheck(x, y + 1, DirectionSide); // true Если клетки вокруг позволяют проделать разрез
                        if (debug)
                        {
                            if (NewWay)
                            {
                                NewWays.Add(new TypeObject(x, y, 2, MaxWaylocal));
                            }
                            else
                            {
                                y++;
                                Map[y, x] = 0;
                                BackDirection = 2;// 2 - это сторона, с которой прийдет сигнал
                                MaxWaylocal++;
                                NewWay = true;
                            }
                        }
                        else
                        {
                            Count++;

                        }
                    }


                }
                if (Count == 3)
                {
                    foreach (TypeObject Way in NewWays)
                    {
                        WormV2(Way.x, Way.y, Way.BackDirection);
                    }
                    break;
                }

            }
        }
        private bool BoxCheck(int x, int y, int DirectionSide)
        {
            if(DirectionSide == 1) //Вправо
            {
                try
                {
                    if(
                        Map[y - 1, x] == 1 && //Сверху в центре
                        Map[y - 1, x + 1] == 1 && //Сверху справа
                        Map[y, x + 1] == 1 && //Справа в центре
                        Map[y + 1, x] == 1 && //Снизу в центре
                        Map[y + 1, x + 1] == 1 //Справа снизу
                      )
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else if (DirectionSide == 2) //Вверх
            {
                try
                {
                    if (
                        Map[y - 1, x] == 1 && //Сверху в центре
                        Map[y - 1, x + 1] == 1 && //Сверху справа
                        Map[y - 1, x - 1] == 1 && //Сверху слева
                        Map[y, x + 1] == 1 && //Справа в центре
                        Map[y, x - 1] == 1  //Слева в центре
                      )
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else if (DirectionSide == 3) //Влево
            {
                try
                {
                    if (
                        Map[y - 1, x] == 1 && //Сверху в центре                       
                        Map[y - 1, x - 1] == 1 && //Сверху слева                        
                        Map[y, x - 1] == 1 && //Слева в центре
                        Map[y + 1, x - 1] == 1 && //Слева снизу
                        Map[y + 1, x] == 1  //Слева в центре
                      )
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else if (DirectionSide == 4) //Вниз
            {
                try
                {
                    if (                        
                        Map[y, x - 1] == 1 && //Слева в центре
                        Map[y + 1, x - 1] == 1 && //Слева снизу
                        Map[y + 1, x] == 1 && //Внизу в центре
                        Map[y + 1, x + 1] == 1 && //Справа снизу
                        Map[y, x + 1] == 1 //Справа в центре
                      )
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
    public class TypeObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public int BackDirection { get; set; }
        public int MaxWaylocal { get; set; }
        public TypeObject(int x, int y, int BackDirection, int maxWaylocal)
        {
            this.x = x;
            this.y = y;
            this.BackDirection = BackDirection;
            this.MaxWaylocal = maxWaylocal;
        }

    }
}













