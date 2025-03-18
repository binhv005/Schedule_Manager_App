using ClosedXML.Excel;
using System.Text.Json;


class User_excel
{
    private const string ExcelFilePath = "Users.xlsx";
    private const string WorksheetName = "Users";

    // Serialize danh sách User thành JSON byte array
    private static byte[] SerializeListUsers(List<User> users)
    {
        return JsonSerializer.SerializeToUtf8Bytes(users);
    }

    // Serialize một User thành JSON byte array
    private static byte[] SerializeUser(User user)
    {
        List<User> tempList = new List<User> { user };
        return JsonSerializer.SerializeToUtf8Bytes(tempList);
    }

    // Deserialize JSON byte array thành danh sách User
    private static List<User> DeserializeUsers(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return new List<User>(); // Trả về danh sách rỗng nếu dữ liệu đầu vào không hợp lệ
        }

        List<User>? tasks = JsonSerializer.Deserialize<List<User>>(data);

        if (tasks == null)
        {
            return new List<User>(); // Trả về danh sách rỗng nếu quá trình giải tuần tự hóa thất bại
        }

        return tasks;
    }

    // Ghi danh sách User vào Excel (Thêm mới, không ghi đè)
    public void SaveToExcel(List<User> newUsers)
    {
        if (newUsers == null || !newUsers.Any()) return;

        XLWorkbook workbook = LoadOrCreateWorkbook();
        IXLWorksheet worksheet = GetOrCreateWorksheet(workbook);

        try
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            byte[] serializedData = SerializeListUsers(newUsers);
            worksheet.Cell(lastRow + 1, 1).Value = Convert.ToBase64String(serializedData);
            workbook.SaveAs(ExcelFilePath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Lỗi khi lưu danh sách người dùng vào Excel: {ex.Message}", ex);
        }
    }

    // Ghi một User vào Excel
    public void SaveUserToExcel(User newUser)
    {
        if (newUser == null) throw new ArgumentNullException(nameof(newUser));

        XLWorkbook workbook = LoadOrCreateWorkbook();
        var worksheet = GetOrCreateWorksheet(workbook);

        try
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            byte[] serializedData = SerializeUser(newUser);
            worksheet.Cell(lastRow + 1, 1).Value = Convert.ToBase64String(serializedData);
            workbook.SaveAs(ExcelFilePath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Lỗi khi lưu người dùng vào Excel: {ex.Message}", ex);
        }
    }

    // Đọc danh sách User từ Excel
    public List<User> GetAllUsers()
    {
        if (!File.Exists(ExcelFilePath)) return new List<User>();

        XLWorkbook workbook = null;
        try
        {
            workbook = new XLWorkbook(ExcelFilePath);
            IXLWorksheet worksheet = workbook.Worksheet("Users");
            List<User> allUsers = new List<User>();

            foreach (IXLRow row in worksheet.RowsUsed())
            {
                //if (row.RowNumber() == 1) continue; // Bỏ qua dòng tiêu đề
                string base64Data = row.Cell(1).GetString();
                if (base64Data != "")
                {
                    byte[] serializedData = Convert.FromBase64String(base64Data);
                    List<User> deserializedUsers = DeserializeUsers(serializedData);
                    for (int i = 0; i < deserializedUsers.Count; i++)
                        allUsers.Add(deserializedUsers[i]);
                }
            }
            return allUsers;
        }
        finally
        {
            if (workbook != null) workbook.Dispose();  //đóng file
        }
    }

    // Xóa một User theo Id
    public void DeleteUser(int id)
    {
        List<User> users = GetAllUsers();
        List<User> newList = new List<User>();

        // Duyệt qua danh sách users và thêm các user không có ID khớp vào newList
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].GetUserId() != id)
            {
                newList.Add(users[i]);
            }
        }

        // Ghi lại danh sách đã lọc vào Excel
        RewriteExcel(newList);
    }

    // Cập nhật User
    public void UpdateUser(User updatedUser)
    {
        // Kiểm tra nếu updatedUser là null
        if (updatedUser == null)
        {
            throw new ArgumentNullException(nameof(updatedUser));
        }

        // Lấy danh sách tất cả người dùng
        List<User> users = GetAllUsers();

        // Tìm chỉ số của user trong danh sách dựa trên ID
        int index = -1;
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].GetUserId() == updatedUser.GetUserId())
            {
                index = i;
                break; // Thoát vòng lặp khi tìm thấy chỉ số đầu tiên
            }
        }

        // Kiểm tra nếu không tìm thấy user
        if (index == -1)
        {
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {updatedUser.GetUserId()}");
        }

        // Cập nhật user tại chỉ số tìm được
        users[index] = updatedUser;

        // Ghi lại danh sách đã cập nhật vào Excel
        RewriteExcel(users);
    }

    // Tìm User theo Id
    public User FindUserById(int id)
    {
        List<User> users = GetAllUsers();

        User temp = null;
        foreach (User user in users)
        {
            if (user.GetUserId() == id)
            {
                temp = user;
                break;
            }
        }

        if (temp == null)
        {
            //throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {id}");
            return null;
        }
        return temp;
    }

    // Tìm User theo Username
    public User FindUserByUsername(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("Username không được để trống.", nameof(username));

        var users = GetAllUsers();
        return users.FirstOrDefault(user => user.GetUserName() == username)
            ?? throw new KeyNotFoundException($"Không tìm thấy người dùng với username: {username}");
    }

    // In danh sách User
    public void PrintUsers(List<User> users)
    {
        if (users == null || !users.Any())
        {
            Console.WriteLine("Danh sách người dùng trống.");
            return;
        }

        foreach (User user in users)
        {
            Console.WriteLine($"ID: {user.GetUserId()}, Username: {user.GetUserId()}");
        }
    }

    // Helper: Load hoặc tạo mới workbook
    private XLWorkbook LoadOrCreateWorkbook()
    {
        return File.Exists(ExcelFilePath) ? new XLWorkbook(ExcelFilePath) : new XLWorkbook();
    }

    // Helper: Lấy hoặc tạo worksheet
    private IXLWorksheet GetOrCreateWorksheet(XLWorkbook workbook)
    {
        return workbook.Worksheets.FirstOrDefault(ws => ws.Name == WorksheetName)
            ?? workbook.Worksheets.Add(WorksheetName);
    }

    // Helper: Ghi lại toàn bộ dữ liệu vào Excel
    private void RewriteExcel(List<User> users)
    {
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add(WorksheetName);

        try
        {
            if (users.Any())
            {
                byte[] serializedData = SerializeListUsers(users);
                worksheet.Cell(1, 1).Value = Convert.ToBase64String(serializedData);
            }
            workbook.SaveAs(ExcelFilePath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Lỗi khi ghi lại dữ liệu vào Excel: {ex.Message}", ex);
        }
    }
}




