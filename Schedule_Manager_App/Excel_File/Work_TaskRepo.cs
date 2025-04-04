﻿using ClosedXML.Excel;
using System.Text.Json;


class Work_TaskRepo
{
    protected const string ExcelFilePath = "Work_TaskRepo.xlsx";
   
    private const string WorksheetName = "Task";

    // Serialize danh sách WorkTask thành JSON byte array
    private static byte[] SerializeListTasks(List<WorkTask> dailyTasks)
    {
        return JsonSerializer.SerializeToUtf8Bytes(dailyTasks);
    }

    // Serialize một WorkTasks thành JSON byte array
    private static byte[] SerializeTask(WorkTask dailyTask)
    {
        List<WorkTask> tempList = new List<WorkTask> { dailyTask };
        return JsonSerializer.SerializeToUtf8Bytes(tempList);
    }

    // Deserialize JSON byte array thành danh sách Task
    private static List<WorkTask> DeserializeTasks(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return new List<WorkTask>(); // Trả về danh sách rỗng nếu dữ liệu đầu vào không hợp lệ
        }

        List<WorkTask>? tasks = JsonSerializer.Deserialize<List<WorkTask>>(data);

        if (tasks == null)
        {
            return new List<WorkTask>(); // Trả về danh sách rỗng nếu quá trình giải tuần tự hóa thất bại
        }

        return tasks;
    }

    // Ghi danh sách Task vào Excel (Thêm mới, không ghi đè)
    public void SaveToExcel(List<WorkTask> newTasks)
    {
        if (newTasks == null || !newTasks.Any()) return;

        XLWorkbook workbook = LoadOrCreateWorkbook();
        IXLWorksheet worksheet = GetOrCreateWorksheet(workbook);

        try
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            byte[] serializedData = SerializeListTasks(newTasks);
            worksheet.Cell(lastRow + 1, 1).Value = Convert.ToBase64String(serializedData);
            workbook.SaveAs(ExcelFilePath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Lỗi khi lưu danh sách người dùng vào Excel: {ex.Message}", ex);
        }
    }

    // Ghi một Task vào Excel
    public void SaveTaskToExcel(WorkTask newTask)
    {
        if (newTask == null) throw new ArgumentNullException(nameof(newTask));

        XLWorkbook workbook = LoadOrCreateWorkbook();
        var worksheet = GetOrCreateWorksheet(workbook);

        try
        {
            int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
            byte[] serializedData = SerializeTask(newTask);
            worksheet.Cell(lastRow + 1, 1).Value = Convert.ToBase64String(serializedData);
            workbook.SaveAs(ExcelFilePath);
        }
        catch (Exception ex)
        {
            throw new IOException($"Lỗi khi lưu người dùng vào Excel: {ex.Message}", ex);
        }
    }

    // Đọc danh sách Task từ Excel
    public List<WorkTask> GetAllTasks()
    {
        if (!File.Exists(ExcelFilePath)) return new List<WorkTask>();

        XLWorkbook workbook = null;
        try
        {
            workbook = new XLWorkbook(ExcelFilePath);
            IXLWorksheet worksheet = workbook.Worksheet("Task");
            List<WorkTask> allTasks = new List<WorkTask>();

            foreach (IXLRow row in worksheet.RowsUsed())
            {
                //if (row.RowNumber() == 1) continue; // Bỏ qua dòng tiêu đề
                string base64Data = row.Cell(1).GetString();
                if (base64Data != "")
                {
                    byte[] serializedData = Convert.FromBase64String(base64Data);
                    List<WorkTask> deserializedTasks = DeserializeTasks(serializedData);
                    for (int i = 0; i < deserializedTasks.Count; i++)
                        allTasks.Add(deserializedTasks[i]);
                }
            }
            return allTasks;
        }
        finally
        {
            if (workbook != null) workbook.Dispose();  //đóng file
        }
    }

    // Xóa một Task theo Id
    public void DeleteTask(int id)
    {
        List<WorkTask> tasks = GetAllTasks();
        List<WorkTask> newList = new List<WorkTask>();

        // Duyệt qua danh sách tasks và thêm các Task không có ID khớp vào newList
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].TaskId != id)
            {
                newList.Add(tasks[i]);
            }
        }

        // Ghi lại danh sách đã lọc vào Excel
        RewriteExcel(newList);
    }

    // Cập nhật Task
    public void UpdateTask(WorkTask updateTask)
    {
        // Kiểm tra nếu updateTask là null
        if (updateTask == null)
        {
            throw new ArgumentNullException(nameof(updateTask));
        }

        // Lấy danh sách tất cả người dùng
        List<WorkTask> tasks = GetAllTasks();

        // Tìm chỉ số của task trong danh sách dựa trên ID
        int index = -1;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].TaskId == updateTask.TaskId)
            {
                index = i;
                break; // Thoát vòng lặp khi tìm thấy chỉ số đầu tiên
            }
        }

        // Kiểm tra nếu không tìm thấy task
        if (index == -1)
        {
            throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {updateTask.TaskId}");
        }

        // Cập nhật task tại chỉ số tìm được
        tasks[index] = updateTask;

        // Ghi lại danh sách đã cập nhật vào Excel
        RewriteExcel(tasks);
    }

    // Tìm task theo Id
    public WorkTask FindTaskById(int id)
    {
        List<WorkTask> tasks = GetAllTasks();

        List<WorkTask> temp = new List<WorkTask>();
        foreach (WorkTask task in tasks)
        {
            if (task.TaskId == id)
            {
                return task;
                
            }
        }

        //if (temp == null)
        //{
        //    //throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {id}");
        //    return null;
        //}
        return null;
    }

    // Tìm Task theo TaskName
    public WorkTask FindTaskByTaskname(string taskname)
    {
        if (string.IsNullOrEmpty(taskname))
        {
            throw new ArgumentException("Taskname không được để trống.", nameof(taskname));
        }

        // Lấy danh sách tất cả các WorkTask
        List<WorkTask> tasks = GetAllTasks();

        // Duyệt qua từng task trong danh sách
        foreach (WorkTask task in tasks)
        {
            // So sánh TaskName của task với taskname được truyền vào
            if (task.TaskName == taskname)
            {
                return task; // Trả về task nếu tìm thấy
            }
        }

        // Nếu không tìm thấy task nào khớp, ném ngoại lệ
        throw new KeyNotFoundException($"Không tìm thấy người dùng với taskname: {taskname}");
    }

    // In danh sách Task
    public void PrintTasks(List<WorkTask> tasks)
    {
        if (tasks == null || !tasks.Any())
        {
            Console.WriteLine("Danh sách người dùng trống.");
            return;
        }

        foreach (WorkTask task in tasks)
        {
            Console.WriteLine($"ID: {task.TaskId}, Taskname: {task.TaskName}");
        }
    }

    // Helper: Load hoặc tạo mới workbook
    private XLWorkbook LoadOrCreateWorkbook()
    {
        // Kiểm tra xem tệp Excel có tồn tại hay không
        if (File.Exists(ExcelFilePath))
        {
            // Nếu tệp tồn tại, tải workbook từ đường dẫn ExcelFilePath
            return new XLWorkbook(ExcelFilePath);
        }
        else
        {
            // Nếu tệp không tồn tại, tạo một workbook mới
            return new XLWorkbook();
        }
    }

    // Helper: Lấy hoặc tạo worksheet
    private IXLWorksheet GetOrCreateWorksheet(XLWorkbook workbook)
    {
        {
            // Duyệt qua tất cả các worksheet trong workbook
            foreach (IXLWorksheet ws in workbook.Worksheets)
            {
                // Kiểm tra xem tên của worksheet có khớp với WorksheetName không
                if (ws.Name == WorksheetName)
                {
                    // Nếu tìm thấy, trả về worksheet đó
                    return ws;
                }
            }

            // Nếu không tìm thấy worksheet nào có tên WorksheetName, tạo mới một worksheet
            return workbook.Worksheets.Add(WorksheetName);
        }
    }

    // Helper: Ghi lại toàn bộ dữ liệu vào Excel
    private void RewriteExcel(List<WorkTask> tasks)
    {
        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add(WorksheetName);

        try
        {
            if (tasks.Any())
            {
                byte[] serializedData = SerializeListTasks(tasks);
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

