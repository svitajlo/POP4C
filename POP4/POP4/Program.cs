using System;
using System.Threading;

class Phylosopher
{
    private int Id;
    private int LeftFork;
    private int RightFork;
    private SemaphoreSlim[] Forks;
    private Random Random;

    public Phylosopher(int id, SemaphoreSlim[] forks)
    {
        Id = id;
        Forks = forks;
        Random = new Random();
        if (Id == 0) // Філософ 0 бере праву виделку спочатку
        {
            LeftFork = 1;
            RightFork = 0;
        }
        else // Інші філософи беруть ліву виделку спочатку
        {
            LeftFork = Id;
            RightFork = (Id + 1) % 5;
        }
    }

    public void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Phylosopher {Id} is thinking ({i + 1} time)");
            Thread.Sleep(Random.Next(1000, 2000)); // Мислення

            Forks[LeftFork].Wait(); // Взяти ліву виделку
            Console.WriteLine($"Phylosopher {Id} took left fork");

            Forks[RightFork].Wait(); // Взяти праву виделку
            Console.WriteLine($"Phylosopher {Id} took right fork");

            Console.WriteLine($"Phylosopher {Id} is eating ({i + 1} time)"); // Їсти
            Thread.Sleep(Random.Next(1000, 2000));

            Forks[RightFork].Release(); // Відпустити праву виделку
            Console.WriteLine($"Phylosopher {Id} put right fork");

            Forks[LeftFork].Release(); // Відпустити ліву виделку
            Console.WriteLine($"Phylosopher {Id} put left fork");
        }
    }
}

class DinnerPhilosophers
{
    public static void Main()
    {
        int numPhylosophers = 5;
        SemaphoreSlim[] forks = new SemaphoreSlim[numPhylosophers];
        for (int i = 0; i < numPhylosophers; i++)
        {
            forks[i] = new SemaphoreSlim(1, 1); // Вилки як семафори
        }

        Phylosopher[] phylosophers = new Phylosopher[numPhylosophers];
        Thread[] threads = new Thread[numPhylosophers];

        for (int i = 0; i < numPhylosophers; i++)
        {
            phylosophers[i] = new Phylosopher(i, forks);
            threads[i] = new Thread(phylosophers[i].Start); // Створення потоків
            threads[i].Start(); // Запуск потоків
        }

        for (int i = 0; i < numPhylosophers; i++)
        {
            threads[i].Join(); // Очікування завершення потоків
        }

        Console.WriteLine("Dinner is over.");
    }
}
