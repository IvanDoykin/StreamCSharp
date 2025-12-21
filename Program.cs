using Spectre.Console;
using System.Runtime.InteropServices;

internal class Program
{
    private static async Task Main()
    {
        try
        {
            WindowSettings.Initialize();
            Logger.enabled = true;

            //var goldenSword = new Sword("Golden sword", 0, 10, 200);
            //var commonSword = new Sword("Common sword", 0, 5, 100);

            //var glove = new Glove("Glove", 10, 0);
            //var greave = new Greave("Greave", 20, 5);
            //var helmet = new Helmet("Helmet", 0, 10);
            //var vest = new Vest("Vest", 100, 0);

            //SaveLoad<IWeapon>.Save(goldenSword, goldenSword.Name);
            //SaveLoad<IWeapon>.Save(commonSword, commonSword.Name);

            //SaveLoad<IArmor>.Save(glove, glove.Name);
            //SaveLoad<IArmor>.Save(greave, greave.Name);
            //SaveLoad<IArmor>.Save(helmet, helmet.Name);
            //SaveLoad<IArmor>.Save(vest, vest.Name);

            //var unit1 = new Unit(SaveLoad<UnitModel>.Load("Timur"));
            //EquipUtility.EquipUnit(unit1, goldenSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit1, vest, BodyPartName.Body);
            //EquipUtility.EquipUnit(unit1, helmet, BodyPartName.Head);
            //EquipUtility.EquipUnit(unit1, glove, BodyPartName.RightArm);
            //var unitSave1 = new UnitSave(unit1);
            //SaveLoad<UnitSave>.Save(unitSave1, "Player");

            //var unit2 = new Unit(SaveLoad<UnitModel>.Load("Gregory"));
            //EquipUtility.EquipUnit(unit2, commonSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit2, commonSword, BodyPartName.LeftArm);
            //var unitSave2 = new UnitSave(unit2);
            //SaveLoad<UnitSave>.Save(unitSave2, "Gregory");

            //var unit3 = new Unit(SaveLoad<UnitModel>.Load("Michael"));
            //EquipUtility.EquipUnit(unit3, commonSword, BodyPartName.RightArm);
            //EquipUtility.EquipUnit(unit3, commonSword, BodyPartName.LeftArm);
            //var unitSave3 = new UnitSave(unit3);
            //SaveLoad<UnitSave>.Save(unitSave3, "Michael");

            //var playerUnits = new List<Unit>();
            //var enemyUnits = new List<Unit>();
            //playerUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Player")));
            //enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Gregory")));
            //enemyUnits.Add(UnitUtility.CreateUnit(SaveLoad<UnitSave>.Load("Michael")));

            //ArenaModel model = new ArenaModel(playerUnits.Select(x => x.Model.Name).ToList(), enemyUnits.Select(x => x.Model.Name).ToList());
            //SaveLoad<ArenaModel>.Save(model, "Title");

            //Arena arena = new Arena(SaveLoad<ArenaModel>.Load("Title"));
            //arena.Start();

            IConsoleRenderer renderer = new BufferedConsoleRenderer();
            int screenWidth = 56;
            int screenHeight = 18;
            int menuWidth = 15;
            int gameAreaWidth = screenWidth - menuWidth;

            renderer.SetSize(screenWidth, screenHeight + 1); // extra line to prevent scrollbar

            ScreenBuffer menuBuffer = new ScreenBuffer(menuWidth, screenHeight);
            
            DrawUtils draw = new DrawUtils(renderer.Buffer);
            DrawUtils drawMenu = new DrawUtils(menuBuffer);
            
            draw.ResetColor();

            int frame = 0;
            int rectWidth = 5;
            int rectHeight = 3;
            
            int rectX = 1;
            int rectY = 1;
            int speedX = 1;
            int speedY = 1;
            int score = 0;

            while (true)
            {
                frame++;
                
                // Нашел почему было моргание через кадр - это потому что в BufferedConsoleRenderer два
                // буфера - один активный, в котором идёт отрисовка в данный момент, а второй содержит прошлый кадр.
                // При отрисовке сравнивается какие символы в активном отличаются от прошлого кадра и отрисовываются
                // только измененные. После отрисовки буферы меняются местами (чтобы не делать лишние копирования) и
                // активный буфер полностью затирается. Но `draw` был привязан только к одному буферу из двух,
                // поэтому он рисовал только в четных кадрах - когда этот буфер становился активным.
                //
                // Обойти это можно двумя способами:
                // 1. Если очень хочется рисовать напрямую в экранном буфере, то каждый кадр нужно делать
                // 
                //      draw.SetTarget(renderer.Buffer); // Здесь отрисовка переключится на текущий активный буфер.
                //
                // 2. Второй способ - не рисовать напрямую в экранном буфере с помощью DrawUtils, а создать отдельный
                //     ScreenBuffer, рисовать в нем, а в конце скопировать нарисованное в экранный буфер - как это сделано
                //    с меню ниже.
                //  Для демонстрации здесь запилил способ 1 + способ 2 показан при отрисовке меню.
                draw.SetTarget(renderer.Buffer);
                
                // ---=== Render Menu ===---
                
                drawMenu
                    .DrawRect(0, 0, menuWidth, screenHeight, DrawUtils.HeavyLine, true)
                    .DrawTextCentered(menuWidth / 2, 2, $"Frame: {frame}")
                    .DrawTextCentered(menuWidth / 2, 4, $"Score: {score}");
                
                // Копируем то, что было нарисовано в menuBuffer в буфер экрана в нужную позицию.
                // Возможно, метод можно назвать CopyFrom.
                renderer.Buffer.DrawFrom(menuBuffer, gameAreaWidth, 0);
                
                // ---=== Render Game ===--
                
                // Здесь отрисовка идёт напрямую в главный буфер экрана, хотя игровую область можно рисовать
                // в буфере поменьше и потом копировать на экран. Так бывает проще управлять координатами
                // отрисовки, поскольку у каждого буфера своя точка отсчета. Но пример как их комбинировать
                // приведён с menu/drawMenu.
                draw
                    // Большой прямоугольник - не только рамку рисует, но и очищает область, поскольку
                    // отрисовка здесь крайне простая - если объект перемещается, то его прошлую позицию
                    // нужно затирать вручную. Впрочем, учитывая что это всё происходит в памяти (в буферах),
                    // о скорости можно не переживать. Можно буфер с динамическими объектами очищать каждый кадр
                    // и рисовать их заново.
                    // Пометка - если отрисовка идёт напрямую в буфер экрана, то, в отличие от обычных ScreenBuffer,
                    // он очищается каждый кадр, поэтому его можно дополнительно не чистить.
                    // Но в целом я бы рекомендовал рисолвать всё сначала в отдельных буферах, а потом их содержимое
                    // копировать в нужные позиции в экранный буфер с помощью `renderer.Buffer.DrawFrom(otherBuffer)`. 
                    .DrawRect(0, 0, gameAreaWidth, screenHeight, DrawUtils.DoubleLine, true)
                    .SetColor(AnsiColor.Rgb(frame % 6, (frame / 6) % 6, (frame / 36) % 6))
                    .DrawRect(rectX, rectY, rectWidth, rectHeight, DrawUtils.Rounded, true)
                    .ResetColor();
                
                renderer.Render();

                // ---=== State update ===---
                
                var prevSpeedX = speedX;
                var prevSpeedY = speedY;
                
                if (rectX >= gameAreaWidth - rectWidth - 1) {
                    speedX = -1;
                } else if (rectX <= 1) {
                    speedX = 1;
                }

                if (rectY >= screenHeight - rectHeight - 1) {
                    speedY = -1;
                } else if (rectY <= 1) {
                    speedY = 1;
                }

                if (prevSpeedX != speedX || prevSpeedY != speedY) {
                    score++;
                }
                
                rectX += speedX;
                rectY += speedY;
                
                await Task.Delay(500);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.ToString());
            return;
        }
    }

    //private const int NumberOfRows = 10;

    //private static readonly Random _random = new();
    //private static readonly string[] _exchanges = new string[]
    //{
    //        "SGD", "SEK", "PLN",
    //        "MYR", "EUR", "USD",
    //        "AUD", "JPY", "CNH",
    //        "HKD", "CAD", "INR",
    //        "DKK", "GBP", "RUB",
    //        "NZD", "MXN", "IDR",
    //        "TWD", "THB", "VND",
    //};

    //public static async Task Main(string[] args)
    //{
    //    DisableScrolling();

    //    // Write a markup line to the console
    //    AnsiConsole.MarkupLine("[yellow]Hello[/], [blue]World[/]!");

    //    // Write text to the console
    //    AnsiConsole.WriteLine("Hello, World!");

    //    // Write a table to the console
    //    AnsiConsole.Write(new Table()
    //        .RoundedBorder()
    //        .AddColumns("[red]Greeting[/]", "[red]Subject[/]")
    //        .AddRow("[yellow]Hello[/]", "World")
    //        .AddRow("[green]Oh hi[/]", "[blue u]Mark[/]"));
    //}

    //private static void AddExchangeRateRow(Table table)
    //{
    //    var (source, destination, rate) = GetExchangeRate();
    //    table.AddRow(
    //        source, destination,
    //        _random.NextDouble() > 0.35D ? $"[green]{rate}[/]" : $"[red]{rate}[/]");
    //}

    //private static (string Source, string Destination, double Rate) GetExchangeRate()
    //{
    //    var source = _exchanges[_random.Next(0, _exchanges.Length)];
    //    var dest = _exchanges[_random.Next(0, _exchanges.Length)];
    //    var rate = 200 / ((_random.NextDouble() * 320) + 1);

    //    while (source == dest)
    //    {
    //        dest = _exchanges[_random.Next(0, _exchanges.Length)];
    //    }

    //    return (source, dest, rate);
    //}

}