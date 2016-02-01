using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        public static void Start(List<User> usrs, List<Product> prods)
        {

     
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");

            Login:
          
            string name = PromptUserName();
            string pwd = PromptPassword();

            if (!string.IsNullOrEmpty(name))
            {
                if (ValidateUser(usrs, name) && ValidatePassword(usrs, name, pwd))
                {
              
                        // Show welcome message
                        DisplayWelcomeMessage(name);
                        
                        // Show remaining balance
                        double bal = 0;
                        bal = ShowRemainingBalance(usrs, name, pwd, bal);

                        // Show product list
                        while (true)
                        {
                            // Prompt for user input
                            ShowProductList(prods);

                            // Prompt for user input
                        //    string answer;
                            int num;
                            num = GetUserSelection();

                            // Check if user entered number that equals product count
                            if (num == 7)
                            {
                                // Update balance
                                foreach (var usr in usrs)
                                {
                                    // Check that name and password match
                                    if (usr.Name == name && usr.Pwd == pwd)
                                    {
                                        usr.Bal = bal;
                                    }
                                }
                                UpdateUserDetails(usrs, prods);


                                // Prevent console from closing
                                Console.WriteLine();
                                Console.WriteLine("Press Enter key to exit");
                                Console.ReadLine();
                                return;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("You want to buy: " + prods[num].Name);
                                Console.WriteLine("Your balance is " + bal.ToString("C"));

                                int qty;
                                qty = PromptUserPurchase();

                                bal = ProcessOrder(prods, bal, num, qty);
                            }
                        }
                    }
                   else
                    {
   

                        goto Login;
                    }
                }

            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static double ProcessOrder(List<Product> prods, double bal, int num, int qty)
        {


            // Check if quantity is greater than zero
            if (qty > 0)
            {
                // Check if balance - quantity * price is less than 0
                if (CheckBalance(prods, bal, num, qty) && ValidateQuantity(prods, num, qty)) 
                {
                    // Check if quantity is less than quantity
                    //             ValidateQuantity(prods, num, qty);

                    // Balance = Balance - Price * Quantity
                    bal = bal - prods[num].Price * qty;

                    // Quanity = Quantity - Quantity
                    prods[num].Qty = prods[num].Qty - qty;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You bought " + qty + " " + prods[num].Name);
                    Console.WriteLine("Your new balance is " + bal.ToString("C"));
                    Console.ResetColor();
                }
            }
            else
            {
                CancelOrder();
            }
            return bal;
        }

        private static void CancelOrder()
        {
           
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Purchase cancelled");
            Console.ResetColor();
        }

        private static int PromptUserPurchase()
        {
            string answer;
            int qty;
 
            Console.WriteLine("Enter amount to purchase:");
            answer = Console.ReadLine();
            qty = Convert.ToInt32(answer);

            return qty;
        }

        private static bool ValidateQuantity(List<Product> prods, int num, int qty)
        {
            if (prods[num].Qty <= qty)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Sorry, " + prods[num].Name + " is out of stock");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        private static bool CheckBalance(List<Product> prods, double bal, int num, int qty)
        {
            if (bal - prods[num].Price * qty < 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("You do not have enough money to buy that.");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        private static void UpdateUserDetails(List<User> usrs, List<Product> prods)
        {

            // Write out new balance
            string json = JsonConvert.SerializeObject(usrs, Formatting.Indented);
            File.WriteAllText(@"Data\Users.json", json);

            // Write out new quantities
            string json2 = JsonConvert.SerializeObject(prods, Formatting.Indented);
            File.WriteAllText(@"Data\Products.json", json2);
        }

        private static int GetUserSelection()
        {

            string answer;
            int num;

            Console.WriteLine("Enter a number:");
            answer = Console.ReadLine();
            num = Convert.ToInt32(answer);
            return num - 1; /* Subtract 1 from number
                            num = num + 1 // Add 1 to number */
        }

        private static void ShowProductList(List<Product> prods)
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");
            for (int i = 0; i < 7; i++)
            {
                Product prod = prods[i];
                Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
            }
            Console.WriteLine(prods.Count + 1 + ": Exit");
        }

        private static double ShowRemainingBalance(List<User> usrs, string name, string pwd, double bal)
        {
            for (int i = 0; i < 5; i++)
            {
                User usr = usrs[i];

                // Check that name and password match
                if (usr.Name == name && usr.Pwd == pwd)
                {
                    bal = usr.Bal;

                    // Show balance 
                    Console.WriteLine();
                    Console.WriteLine("Your balance is " + usr.Bal.ToString("C"));
                }
            }
            return bal;
        }

        private static void DisplayWelcomeMessage(string name)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + name + "!");
            Console.ResetColor();
        }

        private static bool ValidatePassword(List<User> usrs, string name, string pwd)
        {
            for (int i = 0; i < 5; i++)
            {
                User user = usrs[i];

                // Check that name and password match
                if (user.Name == name && user.Pwd == pwd)
                {
                    return true;
                    
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid password.");
            Console.ResetColor();
            return false;
        }

        private static string PromptPassword()
        {
        
            Console.WriteLine("Enter Password:");
            string pwd = Console.ReadLine();
            return pwd;
        }

        private static bool ValidateUser(List<User> usrs, string name)
        {
            for (int i = 0; i < 5; i++)
            {
                User user = usrs[i];
                if (user.Name == name)
                {
                    return true;
                }
            }

            // Invalid User
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid user.");
            Console.ResetColor();
            return false;
        }

        private static string PromptUserName()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            string name = Console.ReadLine();
            return name;
        }
    }
}
