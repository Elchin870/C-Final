using C__Final;
using System.Globalization;
using System.Text.Json;

namespace User
{
    internal class Program
    {
        public static class UserList
        {
            public static List<User> Users = new List<User>();

        }
        public static void LoadUsers()
        {
            if (File.Exists("users.json"))
            {
                var usersJson = File.ReadAllText("users.json");
                UserList.Users = JsonSerializer.Deserialize<List<User>>(usersJson) ?? new List<User>();
            }
        }

        public static void SaveUsers()
        {
            var usersJson = JsonSerializer.Serialize(UserList.Users);
            File.WriteAllText("users.json", usersJson);
        }

        public static User? userInDataBase = null;

        public static bool Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Username ve ya password bos ola bilmez");
                return false;
            }
            if (UserList.Users.Any(u => u.Username == username))
            {
                Console.WriteLine("Username already exists");
                return false;
            }

            var newUser = new User { Username = username, Password = password };
            UserList.Users.Add(newUser);
            SaveUsers();
            Console.WriteLine("User registered successfully");
            return true;
        }

        public static bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Username ve ya password bos ola bilmez");
                return false;
            }
            var user = UserList.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                userInDataBase = user;
                Console.WriteLine("Login successful");
                Thread.Sleep(1500);
                Console.Clear();
                return true;
            }

            Console.WriteLine("Invalid username or password");
            return false;
        }

        static void Main(string[] args)
        {
            LoadUsers();
            bool loggedUser = false;
            while (!loggedUser)
            {
                Console.WriteLine("1) Register");
                Console.WriteLine("2) Login");
                Console.WriteLine("3) Exit");
                Console.Write("Secim edin: ");
                var secim = Console.ReadLine();

                switch (secim)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Enter username: ");
                        var registerUsername = Console.ReadLine();
                        Console.Write("Enter password: ");
                        var registerPassword = Console.ReadLine();
                        Register(registerUsername, registerPassword);
                        break;
                    case "2":
                        Console.Write("Enter username: ");
                        var loginUsername = Console.ReadLine();
                        Console.Write("Enter password: ");
                        var loginPassword = Console.ReadLine();
                        bool log = Login(loginUsername, loginPassword);
                        if (log)
                        {
                            loggedUser = true;
                        }
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Wrong input!!!");
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;
                }
            }

            Basket basket = new Basket();
            var stockJson = File.ReadAllText("C:\\Users\\Elchin\\source\\repos\\C# Final\\C# Final\\bin\\Debug\\net8.0\\stock.json");
            Stock stockCategoryProduct = JsonSerializer.Deserialize<Stock>(stockJson);

            bool inMenuUser = true;
            while (inMenuUser)
            {
                Console.WriteLine($"Xos geldiniz {userInDataBase.Username}");
                Console.WriteLine("1)Katagoriyalar");
                Console.WriteLine("2)Sebete bax");
                Console.WriteLine("3)Profil");
                Console.WriteLine("4)Cixis");
                Console.Write("Secim edin: ");
                var secimUser = Console.ReadLine();

                switch (secimUser)
                {
                    case "1":
                        #region Kategoriya
                        for (int i = 0; i < stockCategoryProduct.Categories.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}) {stockCategoryProduct.Categories[i].Name}");
                        }
                        Console.Write("Secim edin: ");
                        var userCategory = Console.ReadLine();
                        if (userCategory != null && userCategory.Length > 0)
                        {
                            int categoryIndex = Convert.ToInt32(userCategory);
                            if (categoryIndex > 0 && categoryIndex <= stockCategoryProduct.Categories.Count)
                            {
                                var selectedCategory = stockCategoryProduct.Categories[categoryIndex - 1];
                                for (int i = 0; i < selectedCategory.Products.Count; i++)
                                {
                                    var product = selectedCategory.Products[i];
                                    Console.WriteLine($"{i + 1}){product.Name}  Price: {product.Price} azn  Count: {product.Count}");
                                }
                                Console.WriteLine();
                                Console.WriteLine("+)Sebete elave et");
                                Console.WriteLine("-)Menu");
                                Console.Write("Secim edin: ");
                                var sebetSecim = Console.ReadLine();
                                if (sebetSecim == "+")
                                {
                                    try
                                    {
                                        Console.Write("Almaq istediyiniz mehsulun adini girin: ");
                                        var productName = Console.ReadLine();
                                        if (!string.IsNullOrWhiteSpace(productName))
                                        {
                                            for (int i = 0; stockCategoryProduct.Products.Count > 0; i++)
                                            {
                                                if (productName == stockCategoryProduct.Products[i].Name)
                                                {
                                                    basket.AddProductToBasket(stockCategoryProduct.Products[i]);
                                                    Console.WriteLine("Mehsul sebete elave olundu");
                                                    Thread.Sleep(1250);
                                                    Console.Clear();
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Wrong input!!!");
                                            Thread.Sleep(1250);
                                            Console.Clear();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Duzgun ad daxil edin!!!");
                                        Thread.Sleep(1250);
                                        Console.Clear();
                                    }


                                }
                                else if (sebetSecim == "-")
                                {
                                    Console.Clear();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Wrong input!!!");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }

                            }

                            else
                            {
                                Console.WriteLine("Seçilmiş kateqoriya tapılmadı");
                                Thread.Sleep(1250);
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong input!!!");
                            Thread.Sleep(1250);
                            Console.Clear();
                        }

                        break;
                    #endregion
                    case "2":
                        #region Sebet
                        Console.Clear();
                        basket.ShowBasket();
                        Console.WriteLine("1)Sifarisi tesdiq etmek");
                        Console.WriteLine("2)Sebetden produkt silmek");
                        Console.WriteLine("3)Evvelki Menu");
                        Console.Write("Secim edin: ");
                        var secimBasket = Console.ReadLine();
                        if (secimBasket == "1")
                        {
                            Console.Clear();
                            double pul = basket.Hesab();
                            Console.WriteLine($"Odeyeceniz mebleg: {pul}");
                            Console.Write("Mebleg daxil edin: ");
                            var odenilenMebleg = Console.ReadLine();
                            if (!string.IsNullOrEmpty(odenilenMebleg))
                            {
                                double odenilenMeblegDouble = Convert.ToDouble(odenilenMebleg, CultureInfo.InvariantCulture);
                                if (odenilenMeblegDouble < pul)
                                {
                                    Console.WriteLine("Odenis yeterli deyil");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                                else
                                {
                                    double qaliq = odenilenMeblegDouble - pul;
                                    Console.WriteLine("Odenisiniz qebul edildi");
                                    Console.WriteLine($"Qaliq: {qaliq}");
                                    Console.WriteLine("Bizi secdiyiniz ucun tesekkur edirik");
                                    basket.ClearBasket();
                                    Thread.Sleep(2000);
                                    Console.Clear();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Duzgun mebleg daxil edin");
                                Thread.Sleep(1250);
                                Console.Clear();
                            }
                        }
                        else if (secimBasket == "2")
                        {

                            Console.Write("Silmek istediyiniz produktun adini yazin: ");
                            var silinenProdukt = Console.ReadLine();
                            if (!string.IsNullOrEmpty(silinenProdukt))
                            {
                                for (int i = 0; i < stockCategoryProduct.Products.Count; i++)
                                {
                                    if (silinenProdukt == stockCategoryProduct.Products[i].Name)
                                    {
                                        basket.Products.Remove(stockCategoryProduct.Products[i]);
                                        Console.WriteLine("Sebetden element silindi");
                                        Thread.Sleep(1250);
                                        Console.Clear();
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Produkt tapilmadi!!!");
                                        Thread.Sleep(1250);
                                        Console.Clear();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Wrong input!!!");
                                Thread.Sleep(1250);
                                Console.Clear();
                            }
                        }
                        else if (secimBasket == "3")
                        {
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input!!!");
                            Thread.Sleep(1250);
                            Console.Clear();
                        }

                        break;
                    #endregion
                    case "3":
                        #region Profil
                        try
                        {
                            Console.Clear();
                            Console.WriteLine($"Username: {userInDataBase.Username}");
                            Console.WriteLine($"Password: {userInDataBase.Password}");
                            Console.WriteLine("Username or password or both?");
                            Console.WriteLine("1)Username");
                            Console.WriteLine("2)Password");
                            Console.WriteLine("3)Both");
                            Console.WriteLine("4)Evvelki menu");
                            Console.Write("Secim edin: ");
                            var secimProfil = Console.ReadLine();
                            if (Convert.ToInt32(secimProfil) == 1)
                            {
                                Console.WriteLine("Enter username for searching: ");
                                var usernameForSearch = Console.ReadLine();
                                Console.WriteLine("Enter new username: ");
                                var newUsername = Console.ReadLine();
                                var usernameToUptade = UserList.Users.Find(p => p.Username == usernameForSearch);
                                if (usernameToUptade != null)
                                {
                                    usernameToUptade.Username = newUsername;
                                    SaveUsers();
                                    Console.WriteLine("Username succesfully changed");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Username not found!!!");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                            }
                            else if (Convert.ToInt32(secimProfil) == 2)
                            {
                                Console.WriteLine("Enter username for searching: ");
                                var usernameForSearch = Console.ReadLine();
                                Console.WriteLine("Enter new password: ");
                                var newPassword = Console.ReadLine();
                                var usernameToUptade = UserList.Users.Find(p => p.Username == usernameForSearch);
                                if (usernameToUptade != null)
                                {
                                    usernameToUptade.Password = newPassword;
                                    SaveUsers();
                                    Console.WriteLine("Password succesfully changed");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Username not found!!!");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                            }
                            else if (Convert.ToInt32(secimProfil) == 3)
                            {
                                Console.WriteLine("Enter username for searching: ");
                                var usernameForSearch = Console.ReadLine();
                                Console.WriteLine("Enter new username: ");
                                var newUsername = Console.ReadLine();
                                Console.WriteLine("Enter new password: ");
                                var newPassword = Console.ReadLine();
                                var usernameToUptade = UserList.Users.Find(p => p.Username == usernameForSearch);
                                if (usernameToUptade != null)
                                {
                                    usernameToUptade.Username = newUsername;
                                    usernameToUptade.Password = newPassword;
                                    SaveUsers();
                                    Console.WriteLine("Username and Password succesfully changed");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.WriteLine("Username not found!!!");
                                    Thread.Sleep(1250);
                                    Console.Clear();
                                }
                            }
                            else if (Convert.ToInt32(secimProfil) == 4)
                            {
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Wrong input!!!");
                                Thread.Sleep(1250);
                                Console.Clear();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Secim bos ola bilmez");
                            Thread.Sleep(1250);
                            Console.Clear();
                        }
                        #endregion
                        break;
                    case "4":
                        inMenuUser = false;
                        break;
                    default:
                        Console.WriteLine("Wrong input!!!");
                        Thread.Sleep(1250);
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
