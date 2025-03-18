using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Schedule_Manager_App.Excel_File;

class Program
{
    private static ScheduleManagerApp scheduleManager = new ScheduleManagerApp();
    private static UserManager userManager = UserManager.GetInstance();
    private static User? currentUser;

    static void Main()
    {
       
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=================================");
            Console.WriteLine("      QUẢN LÝ LỊCH TRÌNH         ");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Đăng ký");
            Console.WriteLine("2. Đăng nhập");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn một tùy chọn: ");

            string? option = Console.ReadLine();

            if (option == "1")
            {
                Register();
            }
            else if (option == "2")
            {
                Login();
                if (currentUser != null)
                {
                    MainMenu();
                }
            }
            else if (option == "0")
            {
                Console.WriteLine("👋 Tạm biệt! Hẹn gặp lại.");
                break;
            }
            else
            {
                Console.WriteLine("❌ Lựa chọn không hợp lệ. Nhấn Enter để thử lại...");
                Console.ReadLine();
            }
        }
    }

    private static void Register()
    {
        Console.Clear();
        Console.WriteLine("====== ĐĂNG KÝ NGƯỜI DÙNG ======");
        Console.Write("Nhập tên người dùng: ");
        string? userName = Console.ReadLine();
        Console.Write("Nhập mật khẩu: ");
        string? password = Console.ReadLine();

        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            userManager.Register(userName, password);
        }
        else
        {
            Console.WriteLine("❌ Lỗi: Không được để trống tên hoặc mật khẩu.");
        }
        Console.ReadLine();
    }

    private static void Login()
    {
        Console.Clear();
        Console.WriteLine("====== ĐĂNG NHẬP ======");
        Console.Write("Nhập tên người dùng: ");
        string? userName = Console.ReadLine();
        Console.Write("Nhập mật khẩu: ");
        string? password = Console.ReadLine();

        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            currentUser = userManager.Authenticate(userName, password);
        }

        if (currentUser is null)
        {
            Console.WriteLine("❌ Sai tên người dùng hoặc mật khẩu. Nhấn Enter để thử lại.");
            Console.ReadLine();
        }
    }

    private static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====== MENU CHÍNH ======");
            Console.WriteLine("1. Tạo nhiệm vụ");
            Console.WriteLine("2. Xóa nhiệm vụ");
            Console.WriteLine("0. Đăng xuất");
            Console.Write("Chọn một tùy chọn: ");

            string? option = Console.ReadLine();

            if (option == "1")
            {
                CreateTask();
            }
            else if (option == "2")
            {
                DeleteTask();
            }
            else if (option == "0")
            {
                currentUser = null;
                break;
            }
            else
            {
                Console.WriteLine("❌ Lựa chọn không hợp lệ. Nhấn Enter để thử lại...");
                Console.ReadLine();
            }
        }
    }

    private static void CreateTask()
    {
        Console.Write("Nhập tên nhiệm vụ: ");
        string? taskName = Console.ReadLine();

        DateTime startDate, endDate;
        TimeSpan startTime, endTime;

        // Nhập ngày bắt đầu
        while (true)
        {
            Console.Write("Nhập ngày bắt đầu (dd/MM/yyyy): ");
            string? startDateStr = Console.ReadLine();

            if (DateTime.TryParseExact(startDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                break;
            }
            Console.WriteLine("❌ Lỗi: Định dạng ngày không hợp lệ. Vui lòng nhập lại.");
        }

        // Nhập giờ bắt đầu
        while (true)
        {
            Console.Write("Nhập giờ bắt đầu (HH:mm): ");
            string? startTimeStr = Console.ReadLine();

            if (TimeSpan.TryParseExact(startTimeStr, @"hh\:mm", CultureInfo.InvariantCulture, out startTime))
            {
                break;
            }
            Console.WriteLine("❌ Lỗi: Định dạng giờ không hợp lệ. Vui lòng nhập lại.");
        }

        // Nhập ngày kết thúc
        while (true)
        {
            Console.Write("Nhập ngày kết thúc (dd/MM/yyyy): ");
            string? endDateStr = Console.ReadLine();

            if (DateTime.TryParseExact(endDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                if (endDate >= startDate)
                    break;
                Console.WriteLine("❌ Lỗi: Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
            }
            else
            {
                Console.WriteLine("❌ Lỗi: Định dạng ngày không hợp lệ. Vui lòng nhập lại.");
            }
        }

        // Nhập giờ kết thúc
        while (true)
        {
            Console.Write("Nhập giờ kết thúc (HH:mm): ");
            string? endTimeStr = Console.ReadLine();

            if (TimeSpan.TryParseExact(endTimeStr, @"hh\:mm", CultureInfo.InvariantCulture, out endTime))
            {
                DateTime fullStartTime = startDate.Add(startTime);
                DateTime fullEndTime = endDate.Add(endTime);

                if (fullEndTime > fullStartTime)
                    break;
                Console.WriteLine("❌ Lỗi: Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
            }
            else
            {
                Console.WriteLine("❌ Lỗi: Định dạng giờ không hợp lệ. Vui lòng nhập lại.");
            }
        }

        DateTime finalStartTime = startDate.Add(startTime);
        DateTime finalEndTime = endDate.Add(endTime);

        Console.WriteLine("Chọn loại nhiệm vụ:");
        Console.WriteLine("1. Công việc");
        Console.WriteLine("2. Cá nhân");
        Console.WriteLine("3. Sự kiện");
        Console.WriteLine("4. Học tập");
        Console.Write("Nhập số tương ứng: ");
        string? taskTypeChoice = Console.ReadLine();
        string? taskType = taskTypeChoice switch
        {
            "1" => "Work",
            "2" => "Personal",
            "3" => "Event",
            "4" => "Study",
            _ => null
        };

        if (taskType == null)
        {
            Console.WriteLine("❌ Lựa chọn không hợp lệ.");
            Console.ReadLine();
            return;
        }

        Console.Write("Nhập ghi chú: ");
        string? note = Console.ReadLine();

        scheduleManager.CreateTask(taskName, finalStartTime, finalEndTime, taskType, note);
        Console.WriteLine("✅ Nhiệm vụ đã được tạo thành công!");
    }



    private static void DeleteTask()
    {
        Console.Clear();
        Console.Write("Nhập ID nhiệm vụ cần xóa: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrEmpty(input) || !int.TryParse(input, out int taskId))
        {
            Console.WriteLine("❌ Lỗi: ID không hợp lệ.");
            Console.ReadLine();
            return;
        }

        bool isDeleted = scheduleManager.DeleteTask(taskId);
        Console.WriteLine(isDeleted ? "✅ Nhiệm vụ đã được xóa thành công!" : "❌ Không tìm thấy nhiệm vụ với ID đã nhập.");
        Console.ReadLine();
    }
}