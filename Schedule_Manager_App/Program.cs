using System;
using System.Collections.Generic;
using System.Text;

public class Program
{
    private static UserManager userManager = UserManager.Instance;
    private static ScheduleManagerApp scheduleManager = ScheduleManagerApp.Instance;
    private static User currentUser = null;

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== HỆ THỐNG QUẢN LÝ CÔNG VIỆC =====");
            Console.WriteLine("1. Đăng nhập");
            Console.WriteLine("2. Đăng ký");
            Console.WriteLine("3. Thoát");
            Console.Write("Chọn: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Login();
            }
            else if (choice == "2")
            {
                Register();
            }
            else if (choice == "3")
            {
                Console.WriteLine("Thoát chương trình.");
                break;
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ!");
                Console.ReadKey();
            }
        }
    }

    private static void Register()
    {
        Console.Write("\nNhập tên đăng nhập: ");
        string username = Console.ReadLine();
        Console.Write("Nhập mật khẩu: ");
        string password = Console.ReadLine();

        bool success = userManager.Register(username, password);
        if (success)
        {
            Console.WriteLine("Bạn có thể đăng nhập ngay bây giờ.");
        }
        Console.ReadKey();
    }

    private static void Login()
    {
        Console.Write("\nNhập tên đăng nhập: ");
        string username = Console.ReadLine();
        Console.Write("Nhập mật khẩu: ");
        string password = Console.ReadLine();

        User user = userManager.Login(username, password);
        if (user != null)
        {
            currentUser = user;
            Console.WriteLine("Xin chào, " + user.Username + "!");
            Console.ReadKey();
            ShowMainMenu();
        }
        else
        {
            Console.WriteLine("Sai tên đăng nhập hoặc mật khẩu!");
            Console.ReadKey();
        }
    }

    private static void ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"===== CHÀO MỪNG {currentUser.Username} =====");
            Console.WriteLine("1. Xem lịch theo ngày");
            Console.WriteLine("2. Xem lịch theo tuần");
            Console.WriteLine("3. Thêm công việc");
            Console.WriteLine("4. Xóa công việc");
            Console.WriteLine("5. Đăng xuất");
            Console.Write("Chọn: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                ViewDailySchedule();
            }
            else if (choice == "2")
            {
                ViewWeeklySchedule();
            }
            else if (choice == "3")
            {
                AddTask();
            }
            else if (choice == "4")
            {
                RemoveTask();
            }
            else if (choice == "5")
            {
                currentUser = null;
                Console.WriteLine("Bạn đã đăng xuất.");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.WriteLine("Lựa chọn không hợp lệ!");
                Console.ReadKey();
            }
        }
    }

    private static void ViewDailySchedule()
    {
        Console.Write("\nNhập ngày cần xem (dd/MM/yyyy): ");
        DateTime date;
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
        {
            Console.Clear();
            Console.WriteLine("\n=== Lịch ngày " + date.ToString("dd/MM/yyyy") + " ===");
            scheduleManager.DisplayDailySchedule(date); // Chỉ hiển thị 1 ngày
        }
        else
        {
            Console.WriteLine("Định dạng ngày không hợp lệ! Vui lòng nhập lại.");
        }
        Console.ReadKey();
    }



    private static void ViewWeeklySchedule()
    {
        Console.Write("\nNhập ngày bắt đầu của tuần (dd/MM/yyyy): ");
        DateTime startDate;
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
        {
            Console.WriteLine("\n=== Lịch tuần từ " + startDate.ToString("dd/MM/yyyy") + " đến " + startDate.AddDays(6).ToString("dd/MM/yyyy") + " ===");
            scheduleManager.DisplayWeeklySchedule(startDate);
        }
        else
        {
            Console.WriteLine("Định dạng ngày không hợp lệ! Vui lòng nhập lại.");
        }
        Console.ReadKey();
    }



    private static void AddTask()
    {
        Console.Write("\nNhập tên công việc: ");
        string name = Console.ReadLine();
        Console.Write("Nhập ghi chú: ");
        string notes = Console.ReadLine();
        Console.Write("Nhập ngày bắt đầu (dd/MM/yyyy HH:mm): ");
        DateTime startTime;
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out startTime))
        {
            Console.WriteLine("Định dạng không hợp lệ!");
            return;
        }
        Console.Write("Nhập ngày kết thúc (dd/MM/yyyy HH:mm): ");
        DateTime endTime;
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out endTime))
        {
            Console.WriteLine("Định dạng không hợp lệ!");
            return;
        }

        Console.WriteLine("Chọn loại công việc:");
        Console.WriteLine("1. Công việc học tập");
        Console.WriteLine("2. Công việc làm việc");
        Console.WriteLine("3. Công việc cá nhân");
        Console.WriteLine("4. Sự kiện");
        Console.Write("Lựa chọn: ");
        string taskType = Console.ReadLine();
        Task newTask = null;

        switch (taskType)
        {
            case "1": // StudyTask
                Console.Write("Nhập môn học: ");
                string subject = Console.ReadLine();
                Console.Write("Nhập hạn chót (dd/MM/yyyy HH:mm): ");
                DateTime deadline;
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out deadline))
                {
                    Console.WriteLine("Định dạng không hợp lệ!");
                    return;
                }
                newTask = new StudyTask(name, notes, startTime, endTime, subject, deadline);
                break;

            case "2": // WorkTask
                Console.Write("Nhập tên dự án: ");
                string project = Console.ReadLine();
                newTask = new WorkTask(name, notes, startTime, endTime, project);
                break;

            case "3": // PersonalTask
                Console.Write("Nhập tần suất lặp lại: ");
                string frequency = Console.ReadLine();
                newTask = new PersonalTask(name, notes, startTime, endTime, frequency);
                break;

            case "4": // EventTask
                Console.Write("Nhập địa điểm sự kiện: ");
                string location = Console.ReadLine();
                newTask = new EventTask(name, notes, startTime, endTime, location);
                break;

            default:
                Console.WriteLine("Lựa chọn không hợp lệ!");
                return;
        }

        scheduleManager.AddTask(startTime.Date, newTask);
        Console.WriteLine("Công việc đã được thêm!");
        Console.ReadKey();
    }



    private static void RemoveTask()
    {
        Console.Write("\nNhập ngày cần xóa công việc (dd/MM/yyyy): ");
        DateTime date;
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
        {
            Console.WriteLine("Định dạng không hợp lệ!");
            return;
        }
        Console.Write("Nhập tên công việc cần xóa: ");
        string taskName = Console.ReadLine();

        scheduleManager.RemoveTask(date, taskName);
        Console.WriteLine("Công việc đã được xóa!");
        Console.ReadKey();
    }
}
