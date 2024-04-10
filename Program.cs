Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.ForegroundColor = ConsoleColor.White;

string filePath = @"C:\Users\Maja\Desktop\Individual Project\TodoListApp\SavedTasks.txt";
List<Task> tasksList = new List<Task>();

ReadFromTextFile();
ShowMenu("main");


void AddTask(List<Task> taskslist, string filePath)
{

    int id;
    string title;
    DateTime dueDate;
    bool status = false;
    string project;

    id = taskslist.Count + 1;

    while (true)
    {
        Console.Write("Enter a title: ");
        title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            UserFeedback("Please enter an Office");
        }
        else
        {
            break;
        }
    }
    while (true)
    {
        Console.Write("Enter a Date (yyyy-MM-dd): ");
        string inputDate = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputDate))
        {
            UserFeedback("Please enter a dueDate, ex: 2020-01-01");
        }
        else if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dueDate))
        {
            break;
        }
        else
        {
            UserFeedback("Invalid date format. Please enter a valid date in the format yyyy-MM-dd");
        }
    }
    while (true)
    {
        Console.Write("Enter a Project name: ");
        project = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(project))
        {
            UserFeedback("Please enter a project name");
        }
        else
        {
            break;
        }
    }

    Task newTask = new Task(id, title, dueDate, status, project);
    tasksList.Add(newTask);
}
void ReadFromTextFile()
{
    tasksList.Clear();

    List<string> lines = File.ReadAllLines(filePath).ToList(); // Read from file

    foreach (var line in lines)
    {
        string[] entries = line.Split(',');

        int id = Convert.ToInt32(entries[0]);
        string title = entries[1];
        DateTime dueDate = DateTime.Parse(entries[2]);
        bool status = Convert.ToBoolean(entries[3]);
        string project = entries[4];

        Task newTask = new Task(id, title, dueDate, status, project); // create an new instance of the class Task.
        tasksList.Add(newTask); // adds the new task to the list of tasks. 
    }
}
void WriteToTextFile()
{
    List<string> output = new List<string>();
    int refreshID = 0;

    foreach (var task in tasksList)
    {
        refreshID++;
        task.Id = refreshID; 
        output.Add($"{task.Id},{task.Title},{task.DueDate},{task.Status},{task.Project}");
    }

    Console.WriteLine("Writing to text file");
    File.WriteAllLines(filePath, output);
    Console.WriteLine("All entries written\n");
}
void ShowMenu(string menuType)
{

    switch (menuType)
    {
        case "main":
            Console.Clear();
            CreateMenuInfoText("Welcome to ToDoLy");
            CreateMenuInfoText("You have X tasks todo and Y tasks are done", true);
            CreateMenuInfoText("Pick an option:");
            CreateMenuOptionsText("1", "Show Task List (by date or project)");
            CreateMenuOptionsText("2", "Add New Task");
            CreateMenuOptionsText("3", "Edit Task (Update, mark as done, remove)");
            CreateMenuOptionsText("0", "Save and Quit");

            string userInput = Console.ReadLine();
            MenuChoice(userInput, "main");

            break;
        case "showTask":
            Console.Clear();
            CreateMenuInfoText("Show Tasks:");
            CreateMenuOptionsText("1", "Sorted by Date");
            CreateMenuOptionsText("2", "Sorted by Project");
            CreateMenuOptionsText("0", "Go Back!");

            userInput = Console.ReadLine();
            MenuChoice(userInput, "showTask");
            break;
        case "addTask":
            Console.Clear();
            CreateMenuInfoText("Add task:");
            CreateMenuOptionsText("1", "Create new task");
            CreateMenuOptionsText("0", "Go Back!");

            userInput = Console.ReadLine();
            MenuChoice(userInput, "addTask");
            break;
        case "editTask":
            Console.Clear();
            CreateMenuInfoText("Edit task:");
            CreateMenuOptionsText("1", "Update Title");
            CreateMenuOptionsText("2", "Mark a task as done");
            CreateMenuOptionsText("3", "Remove a task");
            CreateMenuOptionsText("4", "Change a DueDate");
            CreateMenuOptionsText("0", "Go back to Main Menu");

            userInput = Console.ReadLine();
            MenuChoice(userInput, "editTask");
            break;
        case "saveAndQuit":
            Console.Clear();
            CreateMenuOptionsText("Thank you for using this application.", "", true);
            Console.WriteLine("Press any button to close window");

            break;
        default:
            ShowMenu("main");
            break;
    }
}
void MenuChoice(string input, string menuType)
{
    switch (menuType)
    {
        case "main":
            switch (input)
            {
                case "1":
                    ShowMenu("showTask");
                    break;
                case "2":
                    ShowMenu("addTask");
                    break;
                case "3":
                    ShowMenu("editTask");
                    break;
                case "0":
                    ShowMenu("saveAndQuit");
                    break;
                default:
                    ShowMenu("main");
                    break;
            }
            break;

        case "showTask":
            switch (input)
            {
                case "1":
                    Console.Clear();
                    //SortByDate
                    ShowTasksBySorted(tasksList, true);
                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;
                case "2":
                    Console.Clear();
                    //SortByProject
                    ShowTasksBySorted(tasksList, false);
                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;
                case "0":
                    ShowMenu("main");
                    break;
                default:
                    ShowMenu("showTask");
                    break;
            }
            break;

        case "addTask":
            switch (input)
            {
                case "1":
                    Console.Clear();
                    ReadFromTextFile();
                    AddTask(tasksList, filePath);
                    WriteToTextFile();
                    ShowMenu("main");
                    break;
                case "0":
                    ShowMenu("main");
                    break;
                default:
                    ShowMenu("addTask");
                    break;
            }
            break;

        case "editTask":
            switch (input)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine(" - - - Update Title - - - ");
                    Console.WriteLine($"Select a task number (1-{tasksList.Count}):\n");

                    ShowTasks();

                    while (true)
                    {
                        Console.WriteLine($"Select a task number (1-{tasksList.Count}) to change:\n");
                        string userInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(userInput))
                        {
                            UserFeedback($"Please select a task from above (1-{tasksList.Count})");
                        }
                        else
                        {
                            EditTask("Title", userInput);
                            break;
                        }
                    }
                    Console.WriteLine("Press any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine(" - - - Mark a task as done - - - ");
                    Console.WriteLine($"Select a task number (1-{tasksList.Count}):\n");

                    ShowTasks();

                    while (true)
                    {
                        Console.WriteLine($"Select a task number (1-{tasksList.Count}) to change:\n");
                        string userInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(userInput))
                        {
                            UserFeedback($"Please select a task from above (1-{tasksList.Count})");
                        }
                        else
                        {
                            EditTask("Status", userInput);
                            break;
                        }
                    }
                    //MarkAsDone method here

                    Console.WriteLine("Press any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;
                case "3":

                    Console.Clear();
                    Console.WriteLine(" - - - Remove a task - - - ");
                    ShowTasks();

                    while (true)
                    {
                        Console.WriteLine($"Select a task number (1-{tasksList.Count}) to Remove:\n");
                        string userInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(userInput))
                        {
                            UserFeedback($"Please select a task to Remove from above (1-{tasksList.Count})");
                        }
                        else
                        {
                            EditTask("Remove", userInput);
                            break;
                        }
                    }

                    Console.WriteLine("Press any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine(" - - - Change The Date of a Task - - - ");
                    Console.WriteLine($"Select a task number (1-{tasksList.Count}):\n");

                    ShowTasks();

                    while (true)
                    {
                        Console.WriteLine($"Select a task number (1-{tasksList.Count}) to change:\n");
                        string userInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(userInput))
                        {
                            UserFeedback($"Please select a task from above (1-{tasksList.Count})");
                        }
                        else
                        {
                            EditTask("DueDate", userInput);
                            break;
                        }
                    }
                    
                    Console.WriteLine("Press any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;
                case "0":
                    ShowMenu("main");
                    break;
                default:
                    ShowMenu("editTask");
                    break;
            }
            break;
    }
}
void ShowTasks()
{
    foreach (var task in tasksList)
    {

        if (task.Status == true)
        {
            Console.WriteLine($"{task.Id},{task.Title},{task.DueDate.ToShortDateString()},Finished,{task.Project}");
        }
        else
        {
            Console.WriteLine($"{task.Id},{task.Title},{task.DueDate.ToShortDateString()},Unfinished,{task.Project}");
        }
    }
}
void ShowTasksBySorted(List<Task> tasksList, bool byDate)
{
    if (byDate)
    {
        tasksList = tasksList.OrderBy(asset => asset.DueDate).ToList();
        foreach (var task in tasksList)
        {
            if (task.Status == true)
            {
                Console.WriteLine($"{task.Id},{task.Title},{task.DueDate.ToShortDateString()},Finished,{task.Project}");
            }
            else
            {
                Console.WriteLine($"{task.Id},{task.Title},{task.DueDate.ToShortDateString()},Unfinished,{task.Project}");
            }
        }

    }
    else
    {
        tasksList = tasksList.OrderBy(asset => asset.Project).ToList();
        foreach (var task in tasksList)
        {
            if (task.Status == true)
            {
                Console.WriteLine($"{task.Id},{task.Title},{task.DueDate.ToShortDateString()},Finished,{task.Project}");
            }
            else
            {
                Console.WriteLine($"{task.Id},{task.Title},{task.DueDate.ToShortDateString()},Unfinished,{task.Project}");
            }
        }
    }

}
void EditTask(string EditType, string userInput)
{
    int userInputToInt = Convert.ToInt32(userInput) - 1;

    switch (EditType)
    {
        case "Title":
            string oldTitle = tasksList[userInputToInt].Title;
            Console.Clear();
            Console.Write($"Change {oldTitle} to: ");
            string newValue = Console.ReadLine();
            tasksList[userInputToInt].Title = newValue;
            WriteToTextFile();
            Console.WriteLine($"The Title have been changed to {newValue}");
            break;
        case "Status":
            bool oldValue = tasksList[userInputToInt].Status;
            bool newBoolValue = tasksList[userInputToInt].Status = !oldValue;

                WriteToTextFile();

            if(newBoolValue == false)
            {
                Console.WriteLine($"The Status have been changed to \"Unfinished\"");
            }else
            {
                Console.WriteLine($"The Status have been changed to \"Finished\"");
            }
            break;
        case "Remove":
            Console.Clear();
            string ChoosenItemToRemove = tasksList[userInputToInt].Title;
            Console.WriteLine($"Are You sure you want to Delete: {ChoosenItemToRemove}");
            CreateMenuOptionsText("1", "Yes");
            CreateMenuOptionsText("2", "Cancel");

            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                if (value >= 1 && value <= 2)
                {
                    switch (value)
                    {
                        case 1:
                            // Delete the task
                            tasksList.RemoveAt(userInputToInt);
                            Console.WriteLine("Task deleted successfully!");
                            WriteToTextFile();
                            break;
                        case 2:
                            Console.WriteLine("Deletion cancelled.");
                            break;
                    }
                }
                else
                {
                    // User input is not within the valid range
                    UserFeedback("Please enter a number between 0 and 2");
                }
            }
            else
            {
                // User input is not a valid integer
                UserFeedback("Please enter a valid number");
            }
            break;
        case "DueDate":
            
            while (true)
            {
                Console.Write("Enter a Date (yyyy-MM-dd): ");
                DateTime oldDate = tasksList[userInputToInt].DueDate;
                Console.Clear();
                Console.Write($"Change {oldDate} to: ");
                string newInputDueDate = Console.ReadLine();
                DateTime newDueDate = default;
                if (string.IsNullOrWhiteSpace(newInputDueDate))
                {
                    UserFeedback("Please enter a dueDate, ex: 2020-01-01");
                }
                else if (DateTime.TryParseExact(newInputDueDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out newDueDate))
                {
                    tasksList[userInputToInt].DueDate = newDueDate;
                    WriteToTextFile();
                    Console.WriteLine($"The Title have been changed to {newDueDate}");
                    break;
                }
                else
                {
                    UserFeedback("Invalid date format. Please enter a valid date in the format yyyy-MM-dd");
                }
            }
            break;
        default:
            UserFeedback("something went wrong, please try again", true);
            break;
    }

}
static void CreateMenuInfoText(string message, bool isExclamation = false)
{
    if (isExclamation)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(">> ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(message);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("!\n");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(">> ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }
}
static void CreateMenuOptionsText(string menuNumber, string message, bool isQuit = false)
{
    if (isQuit)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{menuNumber}\n");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{message}");

    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(">> ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("(");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{menuNumber}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($") {message}\n");
    }
}
static void UserFeedback(string msg, bool errorMsg = true)
{
    if (errorMsg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ForegroundColor = ConsoleColor.White;
    }

}
//----------------------------

internal class Task
{
    public Task(int id, string title, DateTime dueDate, bool status, string project)
    {
        Id = id;
        Title = title;
        DueDate = dueDate;
        Status = status;
        Project = project;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
    public bool Status { get; set; }
    public string Project { get; set; }
}