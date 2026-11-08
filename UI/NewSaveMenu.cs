using Spectre.Console;

public class NewSaveMenu : Menu
{
    private PlayerSave _newSave;
    public PlayerSave NewSave => _newSave;

    protected override void InitializeLayout()
    {
        _layout = new Layout("Root")
           .SplitColumns(
               new Layout("Empty")
           );

        _layout["Root"]["Empty"].Update(new Panel("").NoBorder());
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Console.Write("Введите название сохранения (или q, чтобы выйти): ");

        string name = Console.ReadLine();

        if (name == "q")
        {
            Discard();
        }
        
        if (name?.Length < 3)
        {
            Console.Write($"Введите название с 3 символами или больше (или q, чтобы выйти): ");
            return;
        }

        _newSave = new PlayerSave(name);
        if (JsonStorage<PlayerSave>.CheckOnExist(_newSave, name))
        {
            Console.Write($"Сохранение {name} уже существует. Введите другое название (или q, чтобы выйти): ");
        }

        if (!JsonStorage<PlayerSave>.CheckOnExist(_newSave, name) && name.Length >= 3)
        {
            Confirm();
        }
    }
}
