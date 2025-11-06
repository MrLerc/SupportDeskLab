using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportDeskLab
{
    class Program
    {
        static int NextTicketId = 1;
        static Dictionary<int, string> customers = new Dictionary<int, string>();
        static Queue<int> tickets = new Queue<int>();
        static Stack<string> undoStack = new Stack<string>();

        static void Main()
        {
            initCustomer();

            while (true)
            {
                Console.WriteLine("\n=== Support Desk ===");
                Console.WriteLine("[1] Add customer");
                Console.WriteLine("[2] Find customer");
                Console.WriteLine("[3] Create ticket");
                Console.WriteLine("[4] Serve next ticket");
                Console.WriteLine("[5] List customers");
                Console.WriteLine("[6] List tickets");
                Console.WriteLine("[7] Undo last action");
                Console.WriteLine("[0] Exit");
                Console.Write("Choose: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddCustomer(); break;
                    case "2": FindCustomer(); break;
                    case "3": CreateTicket(); break;
                    case "4": ServeNext(); break;
                    case "5": ListCustomers(); break;
                    case "6": ListTickets(); break;
                    case "7": Undo(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void initCustomer()
        {
            customers[1] = "Ava Martin";
            customers[2] = "Ben Parker";
            customers[3] = "Chloe Diaz";
        }

        static void AddCustomer()
        {
            Console.Write("Enter CustomerId: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter customer name: ");
            string name = Console.ReadLine();

            customers[id] = name;
            undoStack.Push($"AddCustomer:{id}:{name}");
            Console.WriteLine("Customer added successfully.");
        }

        static void FindCustomer()
        {
            Console.Write("Enter CustomerId: ");
            int id = int.Parse(Console.ReadLine());

            if (customers.ContainsKey(id))
            {
                Console.WriteLine($"CustomerId: {id}, Name: {customers[id]}");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static void CreateTicket()
        {
            int ticketId = NextTicketId++;
            tickets.Enqueue(ticketId);
            undoStack.Push($"CreateTicket:{ticketId}");
            Console.WriteLine($"Ticket {ticketId} created.");
        }

        static void ServeNext()
        {
            if (tickets.Count > 0)
            {
                int servedTicket = tickets.Dequeue();
                Console.WriteLine($"Ticket {servedTicket} has been served.");
                undoStack.Push($"ServeTicket:{servedTicket}");
            }
            else
            {
                Console.WriteLine("No tickets to serve.");
            }
        }

        static void ListCustomers()
        {
            Console.WriteLine("-- Customers --");
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.Key}, Name: {customer.Value}");
                }
            }
        }

        static void ListTickets()
        {
            Console.WriteLine("-- Tickets (front to back) --");
            if (tickets.Count == 0)
            {
                Console.WriteLine("No tickets found.");
            }
            else
            {
                foreach (var ticket in tickets)
                {
                    Console.WriteLine($"Ticket ID: {ticket}");
                }
            }
        }

        static void Undo()
        {
            if (undoStack.Count == 0)
            {
                Console.WriteLine("No actions to undo.");
                return;
            }

            string lastAction = undoStack.Pop();
            string[] actionParts = lastAction.Split(':');
            string actionType = actionParts[0];

            switch (actionType)
            {
                case "AddCustomer":
                    int customerId = int.Parse(actionParts[1]);
                    customers.Remove(customerId);
                    Console.WriteLine($"Undone: Customer {customerId} removal.");
                    break;

                case "CreateTicket":
                    int ticketId = int.Parse(actionParts[1]);
                    Queue<int> tempQueue = new Queue<int>(tickets.Where(ticket => ticket != ticketId));
                    tickets = tempQueue;
                    Console.WriteLine($"Undone: Ticket {ticketId} removal.");
                    break;

                case "ServeTicket":
                    int servedTicket = int.Parse(actionParts[1]);
                    tickets.Enqueue(servedTicket);
                    Console.WriteLine($"Undone: Ticket {servedTicket} serving.");
                    break;

                default:
                    Console.WriteLine("Unknown action to undo.");
                    break;
            }
        }
    }
}
