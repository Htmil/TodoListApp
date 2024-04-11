Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.ForegroundColor = ConsoleColor.White;

string filePath = @"C:\Users\Maja\Desktop\Individual Project\TodoListApp\SavedTasks.txt";
List<TodoListApp.Task> tasksList = new List<TodoListApp.Task>();

ReadFromTextFile();
ShowMenu("main");

void AddTask(List<TodoListApp.Task> taskslist, string filePath)
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

    TodoListApp.Task newTask = new TodoListApp.Task(id, title, dueDate, status, project);
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

        TodoListApp.Task newTask = new TodoListApp.Task(id, title, dueDate, status, project); // create an new instance of the class Task.
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
    int tasksToDo = tasksList.Count(task => !task.Status);
    int tasksDone = tasksList.Count(task => task.Status);
  
    switch (menuType)
    {

        case "main":
            Console.Clear();
            Console.WriteLine();
            CreateMenuInfoText("Welcome to ToDoLy");
            CreateMenuInfoText($"You have {tasksToDo} tasks todo and {tasksDone} tasks are done", true);
            Console.WriteLine();
            CreateMenuInfoText("Pick an option:");
            CreateMenuOptionsText("1", "Show Task List (by date or project)");
            CreateMenuOptionsText("2", "Add New Task");
            CreateMenuOptionsText("3", "Edit Task (Update, Change Status, remove)");
            CreateMenuOptionsText("0", "Save and Quit");

            string userInput = Console.ReadLine();
            MenuChoice(userInput, "main");

            break;
        case "showTask":
            Console.Clear();
            CreateMenuInfoText("Show Tasks:");
            CreateMenuOptionsText("1", "Sorted by Date");
            CreateMenuOptionsText("2", "Sorted by Project");
            CreateMenuOptionsText("0", "Go back to Main Menu");

            userInput = Console.ReadLine();
            MenuChoice(userInput, "showTask");
            break;
        case "addTask":
            Console.Clear();
            CreateMenuInfoText("Add task:");
            CreateMenuOptionsText("1", "Create new task");
            CreateMenuOptionsText("0", "Go back to Main Menu");

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

                    ShowTasksBySorted(tasksList, true);
                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;  //SortByDate
                case "2":
                    Console.Clear();
                    ShowTasksBySorted(tasksList, false);
                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;  //SortByProject
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
                    break;  //AddTask
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
                    Console.WriteLine("\n - - - Update Title - - - ");

                    ShowTasks();
                    SelectAndEditTask("Title");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; // Update Title
                case "2":
                    Console.Clear();
                    Console.WriteLine("\n - - - Change Status of Task - - - ");

                    ShowTasks();
                    SelectAndEditTask("Status");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; // Check Uncheck
                case "3":

                    Console.Clear();
                    Console.WriteLine("\n - - - Remove a task - - - ");

                    ShowTasks();
                    SelectAndEditTask("Remove");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; // Remove a task
                case "4":
                    Console.Clear();
                    Console.WriteLine("\n - - - Change The Date of a Task - - - ");


                    ShowTasks();
                    SelectAndEditTask("DueDate");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; //Change Date
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
void SelectAndEditTask(string editType)
{
    while (true)
    {
        Console.WriteLine($"Select a task number (1-{tasksList.Count}) to change:\n");
        string userInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userInput))
        {
            UserFeedback($"Please select a task from above (1-{tasksList.Count})");
        }
        else if (int.TryParse(userInput, out int selectedTaskId) && selectedTaskId >= 1 && selectedTaskId <= tasksList.Count)
        {
            EditTask(editType, userInput);
            break;
        }
        else
        {
            UserFeedback($"Invalid task number. Please enter a valid task number between 1 and {tasksList.Count}");
        }
    }
}
void ShowTasks()
{
    int padding = 20;
    Console.WriteLine("| Nr: ".PadRight(padding - 10) + "| Title ".PadRight(padding) + "| Duedate".PadRight(padding) + "| Status".PadRight(padding) + "| Project");
    foreach (var task in tasksList)
    {

        if (task.Status == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + "| Finished".PadRight(padding) + $"| {task.Project}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + "| Unfinished".PadRight(padding) + $"| {task.Project}");
        }
    }
}
void ShowTasksBySorted(List<TodoListApp.Task> tasksList, bool byDate)
{
    int padding = 20;
    Console.WriteLine("| Nr: ".PadRight(padding - 10) + "| Title ".PadRight(padding) + "| Duedate".PadRight(padding) + "| Status".PadRight(padding) + "| Project");
    if (byDate)
    {
        tasksList = tasksList.OrderBy(asset => asset.DueDate).ToList();
        foreach (var task in tasksList)
        {
            if (task.Status == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + "| Finished".PadRight(padding) + $"| {task.Project}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + "| Unfinished".PadRight(padding) + $"| {task.Project}");
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + "| Finished".PadRight(padding) + $"| {task.Project}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + "| Unfinished".PadRight(padding) + $"| {task.Project}");
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

            UserFeedback($"The Title have been changed to {newValue}", false);
            break;
        case "Status":
            bool oldValue = tasksList[userInputToInt].Status;
            bool newBoolValue = tasksList[userInputToInt].Status = !oldValue;

            WriteToTextFile();

            if (newBoolValue == false)
            {
                UserFeedback($"The Status have been changed to \"Unfinished\"", false);
            }
            else
            {
                UserFeedback($"The Status have been changed to \"Finished\"", false);
            }
            break;
        case "Remove":
            Console.Clear();
            string ChoosenItemToRemove = tasksList[userInputToInt].Title;
            Console.WriteLine($"Are You sure you want to Delete: {ChoosenItemToRemove}");
            CreateMenuOptionsText("1", "Yes");
            CreateMenuOptionsText("2", "Cancel");

            Console.Write("Enter your choice: ");

            while (true)
            {
                string DeleteCheckInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(DeleteCheckInput))
                {
                    if (DeleteCheckInput == "1")
                    {
                        tasksList.RemoveAt(userInputToInt);
                        UserFeedback("Task deleted successfully!", false);
                        WriteToTextFile();
                        break; // Exit the loop after valid input
                    }
                    else if (DeleteCheckInput == "2")
                    {
                        UserFeedback("Deletion cancelled.", false);
                        break; // Exit the loop after valid input
                    }
                    else
                    {
                        // User input is not within the valid options
                        UserFeedback("Please enter either 1 or 2");
                        // Continue the loop to prompt the user for valid input
                    }
                }
                else
                {
                    // Handle empty input
                    UserFeedback("Please enter either 1 or 2");
                    // Continue the loop to prompt the user for valid input
                }
            }
            break;
        case "DueDate":

            while (true)
            {
                Console.Write("Enter a Date (yyyy-MM-dd): ");
                DateTime oldDate = tasksList[userInputToInt].DueDate;
                Console.Clear();
                Console.Write($"Change {oldDate.ToShortDateString()} to: ");
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
                    Console.WriteLine($"The Title have been changed to {newDueDate}", false);
                    break;
                }
                else
                {
                    UserFeedback("Invalid date format. Please enter a valid date in the format yyyy-MM-dd");
                }
            }
            break;
        default:
            UserFeedback("something went wrong, please try again");
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
