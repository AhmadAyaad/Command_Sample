using System;
using System.Collections.Generic;

namespace CommandDPSample
{
    /// <summary>
    /// reciver
    /// </summary>

    internal class ChiefResturant
    {
        public void MakeOrder(Order order)
        {
            order.OrderSatus = OrderSatus.Pedning;
            Console.WriteLine($"making order with status{ order.OrderSatus}");
        }

        public void MakeOrderQuickly(Order order)
        {
            order.OrderSatus = OrderSatus.Faster;
            Console.WriteLine($"making order with status {order.OrderSatus}");
        }

        public void CancelOrder(Order order)
        {
            order.OrderSatus = OrderSatus.Canceled;
            Console.WriteLine($"making order with status {order.OrderSatus}");
        }
    }

    /// <summary>
    /// Invoker
    /// </summary>
    internal class Waiter
    {
        public Stack<ICommand2> Commands { get; set; } = new Stack<ICommand2>();

        public void StoreAndExecute(ICommand2 command)
        {
            Commands.Push(command);
            command.Execute();
        }

        public void Undo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                ICommand2 command2 = Commands.Pop();
                command2.UnExecute();
            }
        }

        public void DisplayCommands()
        {
            foreach (var item in Commands)
            {
                Console.WriteLine(item);
            }
        }
    }

    internal interface ICommand2
    {
        public void Execute();

        public void UnExecute();
    }

    internal class AddOrderCommand : ICommand2
    {
        private readonly ChiefResturant _chiefResturant;
        private readonly Order _order;

        public AddOrderCommand(ChiefResturant chiefResturant, Order order)
        {
            _chiefResturant = chiefResturant;
            _order = order;
        }

        public void Execute()
        {
            _chiefResturant.MakeOrder(_order);
        }

        public void UnExecute()
        {
            Console.WriteLine("unexecute add command");
            _chiefResturant.CancelOrder(_order);
        }

        public override string ToString()
        {
            return $"Add Command";
        }
    }

    internal class FastOrderCommand : ICommand2
    {
        private readonly ChiefResturant _chiefResturant;
        private readonly Order _order;

        public FastOrderCommand(ChiefResturant chiefResturant, Order order)
        {
            _chiefResturant = chiefResturant;
            _order = order;
        }

        public void Execute()
        {
            _chiefResturant.MakeOrderQuickly(_order);
        }

        public override string ToString()
        {
            return $"Fast Command";
        }

        public void UnExecute()
        {
            Console.WriteLine("unexecute fast command");
            _chiefResturant.CancelOrder(_order);
        }
    }

    //internal class CancelOrderCommand : ICommand2
    //{
    //    private readonly ChiefResturant _chiefResturant;
    //    private readonly Order _order;

    //    public CancelOrderCommand(ChiefResturant chiefResturant, Order order)
    //    {
    //        _chiefResturant = chiefResturant;
    //        _order = order;
    //    }

    //    public void Execute()
    //    {
    //        _chiefResturant.CancelOrder(_order);
    //    }

    //    public override string ToString()
    //    {
    //        return $"Cancel Command";
    //    }
    //}

    /////////////////////////

    /// <summary>
    /// receiver
    /// </summary>
    internal class Document
    {
        public void Open()
        {
            Console.WriteLine("document opened");
        }

        public void Close()
        {
            Console.WriteLine("document closed");
        }

        public void Save()
        {
            Console.WriteLine("document saved");
        }
    }

    internal interface ICommand
    {
        void Execute();

        void UnExecute();
    }

    internal class OpenCommand : ICommand
    {
        private readonly Document _document;

        public OpenCommand(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            _document.Open();
        }

        public void UnExecute()
        {
            _document.Close();
        }
    }

    internal class CloseCommand : ICommand
    {
        private readonly Document _document;

        public CloseCommand(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            _document.Close();
        }

        public void UnExecute()
        {
            _document.Open();
        }
    }

    internal class SaveCommand : ICommand
    {
        private readonly Document _document;

        public SaveCommand(Document document)
        {
            _document = document;
        }

        public void Execute()
        {
            _document.Save();
        }

        public void UnExecute()
        {
            _document.Close();
        }
    }

    /// <summary>
    /// Invoker
    /// </summary>

    internal class MenuOptions
    {
        private ICommand _openCommand;
        private ICommand _closeCommand;
        private ICommand _saveCommand;
        private Stack<ICommand> _commands = new Stack<ICommand>();

        public MenuOptions(ICommand openCommand, ICommand closeCommand, ICommand saveCommand)
        {
            _openCommand = openCommand;
            _closeCommand = closeCommand;
            _saveCommand = saveCommand;
        }

        public void ClickOpen()
        {
            _commands.Push(_openCommand);
            _openCommand.Execute();
        }

        public void ClickClose()
        {
            _commands.Push(_closeCommand);
            _closeCommand.Execute();
        }

        public void ClickSave()
        {
            _commands.Push(_saveCommand);
            _saveCommand.Execute();
        }

        public void Undo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                ICommand command = _commands.Pop();
                command.UnExecute();
            }
        }

        public void DisplayCommands()
        {
            foreach (var item in _commands)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Document document = new Document();
            ICommand openCommand = new OpenCommand(document);
            ICommand saveCommand = new SaveCommand(document);
            ICommand closeCommand = new CloseCommand(document);

            MenuOptions menuOptions = new MenuOptions(openCommand, closeCommand, saveCommand);
            menuOptions.ClickOpen();
            menuOptions.ClickSave();
            menuOptions.ClickClose();

            menuOptions.Undo(2);

            menuOptions.DisplayCommands();
            ////////////////////////

            //Order order = new Order { Id = 1, Item = "Pizza" };
            //ChiefResturant chiefResturant = new ChiefResturant();
            //Waiter waiter = new Waiter();
            //ICommand2 makeOrderCommand = new AddOrderCommand(chiefResturant, order);
            //ICommand2 fastOrderCommand = new FastOrderCommand(chiefResturant, order);
            //waiter.StoreAndExecute(makeOrderCommand);
            //waiter.StoreAndExecute(fastOrderCommand);

            //waiter.Undo(2);
            //waiter.DisplayCommands();

            //////////////////////////////////////////////////////

            //Waiter waiter = new Waiter();
            //waiter.SetCommand(CommandType.NewOrder);
            //waiter.SetOrder(new Order { Id = 1, Item = "awl order" });
            //waiter.ExcuteCommand();
            //waiter.Display();

            //waiter.SetCommand(CommandType.CancelOrder);
            //waiter.ExcuteCommand();
            //waiter.Display();
        }
    }

    internal enum OrderSatus : byte
    {
        Pedning = 0,
        Faster = 1,
        Canceled = 2
    }

    internal class Order
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public OrderSatus OrderSatus { get; set; } = OrderSatus.Pedning;

        public override string ToString()
        {
            return $"Item {Item}";
        }
    }

    //internal enum CommandType : byte
    //{
    //    NewOrder = 1,
    //    CancelOrder = 0
    //}

    ///// <summary>
    ///// Abstract Command
    ///// </summary>
    //internal abstract class Command
    //{
    //    //public List<Order> Orders { get; set; }
    //    public abstract void Execute(List<Order> orders, Order order);
    //}

    ///// <summary>
    ///// Concrete Command
    ///// </summary>
    //internal class NewOrderCommand : Command
    //{
    //    public override void Execute(List<Order> orders, Order order)
    //    {
    //        orders.Add(order);
    //        Console.WriteLine($"Order with {order} is created");
    //    }
    //}

    ///// <summary>
    ///// Concrete Command
    ///// </summary>
    //internal class CanacelOrderCommand : Command
    //{
    //    public override void Execute(List<Order> orders, Order order)
    //    {
    //        orders.Remove(order);
    //        Console.WriteLine($"Order with {order} is canceled");
    //    }
    //}

    ///// <summary>
    ///// Reciever
    ///// </summary>
    //internal class Chief
    //{
    //    public List<Order> Orders { get; set; } = new List<Order>();

    //    public void ExecuteCommand(Command command, Order order)
    //    {
    //        command.Execute(Orders, order);
    //    }

    //    public void DisplayOrders()
    //    {
    //        foreach (var order in Orders)
    //        {
    //            Console.WriteLine($"Order id: {order.Id} , Order Item: {order.Item}");
    //        }
    //    }
    //}

    ///// <summary>
    ///// Invoker
    ///// </summary>
    //internal class Waiter
    //{
    //    public Chief Chief { get; set; } = new Chief();
    //    private Command Command;
    //    private Order Order;

    //    public void SetOrder(Order order)
    //    {
    //        Order = order;
    //    }

    //    public void SetCommand(CommandType commandType)
    //    {
    //        Command = GetCommandType(commandType);
    //    }

    //    public void ExcuteCommand()
    //    {
    //        Chief.ExecuteCommand(Command, Order);
    //    }

    //    public void Display()
    //    {
    //        Chief.DisplayOrders();
    //    }

    //    private Command GetCommandType(CommandType commandType)
    //    {
    //        switch (commandType)
    //        {
    //            case CommandType.NewOrder:
    //                return new NewOrderCommand();

    //            case CommandType.CancelOrder:
    //                return new CanacelOrderCommand();

    //            default:
    //                return new NewOrderCommand();
    //        }
    //    }
    //}
}