
// Set console colors
Console.BackgroundColor = ConsoleColor.DarkBlue;
Console.ForegroundColor = ConsoleColor.White;

string filePath = @"C:\Users\Maja\Desktop\Individual Project\TodoListApp\SavedTasks.txt";
List<TodoListApp.Task> tasksList = new List<TodoListApp.Task>();


ReadFromTextFile();
ShowMenu("main");

// Function to display different menus based on menuType
void ShowMenu(string menuType)
{
    // Count the number of tasks to do and tasks done
    int tasksToDo = tasksList.Count(task => !task.Status);
    int tasksDone = tasksList.Count(task => task.Status);

    switch (menuType)
    {

        case "main":
            Console.Clear();
            Console.WriteLine();
            // Display welcome message and task summary
            CreateMenuInfoText("Welcome to ToDoLy");
            CreateMenuInfoText($"You have {tasksToDo} tasks todo and {tasksDone} tasks are done", true);
           
            nextTaskToDo();// Display next task to do, if any

            Console.WriteLine();
            // Display menu options
            CreateMenuInfoText("Pick an option:");
            CreateMenuOptionsText("1", "Show Task List (by date or project)");
            CreateMenuOptionsText("2", "Add New Task");
            CreateMenuOptionsText("3", "Edit Task (Update, Change Status, Remove, Change Duedate)");
            CreateMenuOptionsText("0", "Save and Quit");

            // Process user input based on selected menu
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
            CreateMenuOptionsText("\nThank you for using this application.", "", true);
            //Console.WriteLine("Press any button to close window");

            break;
        default:
            ShowMenu("main");
            break;
    }
}

// Function to handle user input based on menu
void MenuChoice(string input, string menuType)
{
    switch (menuType)
    {
        case "main":
            switch (input)
            {
                case "1":
                    ShowMenu("showTask");   // Display menu to show tasks sorted by date or project
                    break;
                case "2":
                    ShowMenu("addTask");    // Display menu to add a new task
                    break;
                case "3":
                    ShowMenu("editTask");   // Display menu to edit tasks
                    break;
                case "0":
                    ShowMenu("saveAndQuit");// Display save and quit message
                    break;
                default:
                    ShowMenu("main");       // Show main menu again for invalid input
                    break;
            }
            break;

        case "showTask":
            switch (input)
            {
                case "1":
                    Console.Clear();
                    
                    ShowTasksBySorted(tasksList, true); // byDate = true == Sorted by Date.

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break;  //SortByDate
                case "2":
                    Console.Clear();

                    ShowTasksBySorted(tasksList, false); //byDate = false == Sorted by Project.

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
                    Console.WriteLine(" - - - Update Title - - - \n");

                    ShowTasks();
                    SelectAndEditTask("Title");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; // Update Title
                case "2":
                    Console.Clear();
                    Console.WriteLine(" - - - Change Status of Task - - - \n");

                    ShowTasks();
                    SelectAndEditTask("Status");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; // Check Uncheck
                case "3":

                    Console.Clear();
                    Console.WriteLine(" - - - Remove a task - - - \n");

                    ShowTasks();
                    SelectAndEditTask("Remove");

                    Console.WriteLine("\nPress any button to go back to Main Menu");
                    Console.ReadLine();
                    ShowMenu("main");
                    break; // Remove a task
                case "4":
                    Console.Clear();
                    Console.WriteLine(" - - - Change The Date of a Task - - - \n");

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

// Function to read task data from text file and add to tasklist
void ReadFromTextFile()
{
    tasksList.Clear();

    List<string> lines = File.ReadAllLines(filePath).ToList(); // Read from file

    foreach (var line in lines)
    {
        string[] entries = line.Split(','); // Split each line into task attributes
        
        int id = Convert.ToInt32(entries[0]);
        string title = entries[1];
        DateTime dueDate = DateTime.Parse(entries[2]);
        bool status = Convert.ToBoolean(entries[3]);
        string project = entries[4];

        TodoListApp.Task newTask = new TodoListApp.Task(id, title, dueDate, status, project);
        tasksList.Add(newTask); // adds the new task to the list of tasks. 
    }
}

// Function to write task data to text file
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
    File.WriteAllLines(filePath, output);
}

// Function to add a new task to the task list
void AddTask(List<TodoListApp.Task> taskslist, string filePath)
{
    // Variables to store task attributes
    int id;
    string title;
    DateTime dueDate;
    bool status = false;
    string project;
    id = taskslist.Count + 1; // Generate a unique task ID, +1 to remove 0 index. 

    while (true) // Loop to ensure valid title input
    {
        Console.Write("Enter a title: ");
        title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            UserFeedback("Please enter an Title"); 
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
    // Create a new task instance and add it to the task list
    TodoListApp.Task newTask = new TodoListApp.Task(id, title, dueDate, status, project);
    tasksList.Add(newTask);

    UserFeedback("\nYou have succesfullt added a new task", false);
    Console.WriteLine("\nPress any button to go back to Main Menu");
    Console.ReadLine();

}

// Function to select and edit a task based on user input
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
            EditTask(editType, userInput); // Calls EditTask function with selected task ID
            break;
        }
        else
        {
            UserFeedback($"Invalid task number. Please enter a valid task number between 1 and {tasksList.Count}");
        }
    }
}

// Function to display all tasks in the task list Unsorted. 
void ShowTasks()
{
    int padding = 20;
    Console.WriteLine("| Nr: ".PadRight(padding - 10) + "| Title ".PadRight(padding + 20) + "| Duedate".PadRight(padding) + "| Status".PadRight(padding) + "| Project");

    foreach (var task in tasksList)
    {

        // refactored if else statment using Ternary operator to change color and value in task.status (Complete/InComplete)
        Console.ForegroundColor = task.Status ? ConsoleColor.Green : ConsoleColor.White;
        Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding +20) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + $"| {(task.Status ? "Completed" : "Incomplete")}".PadRight(padding) + $"| {task.Project}");

    }
}

// Function to display tasks sorted by date or project
void ShowTasksBySorted(List<TodoListApp.Task> tasksList, bool byDate)
{
    int padding = 20;
    Console.WriteLine("| Nr: ".PadRight(padding - 10) + "| Title ".PadRight(padding + 20) + "| Duedate".PadRight(padding) + "| Status".PadRight(padding) + "| Project");

    var sortedTasks = byDate ? tasksList.OrderBy(task => task.DueDate) : tasksList.OrderBy(task => task.Project);

    foreach (var task in sortedTasks)
    {
        // refactored if else statment. Now using Ternary operator to change color and value in task.status (Complete/InComplete)
        Console.ForegroundColor = task.Status ? ConsoleColor.Green : ConsoleColor.White;
        Console.WriteLine($"| {task.Id} ".PadRight(padding - 10) + $"| {task.Title}".PadRight(padding + 20) + $"| {task.DueDate.ToShortDateString()}".PadRight(padding) + $"| {(task.Status ? "Completed" : "Incomplete")}".PadRight(padding) + $"| {task.Project}");

    }
}

// Function to edit task attributes based on editType
void EditTask(string EditType, string userInput)
{
    int userInputToInt = Convert.ToInt32(userInput) - 1; // -1 so the userInput matches the 0 index. 

    switch (EditType)
    {
        case "Title":
            // Edit task title
            string oldTitle = tasksList[userInputToInt].Title;
            Console.Clear();
            Console.Write($"Change {oldTitle} to: ");
            string newValue = Console.ReadLine();
            tasksList[userInputToInt].Title = newValue;
            WriteToTextFile();

            UserFeedback($"The Title have been changed to {newValue}", false);
            break;
        case "Status":
            // Toggle task status (complete/incomplete)
            bool oldValue = tasksList[userInputToInt].Status;
            bool newBoolValue = tasksList[userInputToInt].Status = !oldValue;

            WriteToTextFile();

            UserFeedback($"The Status have been changed to \"{(newBoolValue ? "Complete" : "incomplete")}\"", false);

            break;
        case "Remove":
            // Remove task from the list
            Console.Clear();
            string ChoosenItemToRemove = tasksList[userInputToInt].Title;
            Console.WriteLine($"Are You sure you want to Delete: {ChoosenItemToRemove}");
            CreateMenuOptionsText("1", "Yes");
            CreateMenuOptionsText("2", "Cancel");

            Console.Write("Enter your choice: ");

            // Loop to handle deletion confirmation
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
            // Edit task due date
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
                    Console.WriteLine($"The Title have been changed to {newDueDate.ToShortDateString()}", false);
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

// Function to display the next task to do
void nextTaskToDo()
{
    if (tasksList.Any(task => !task.Status))
    {
        var nextTask = tasksList
            .Where(task => !task.Status)
            .OrderBy(task => task.DueDate)
            .FirstOrDefault();

        if (nextTask != null)
        {
            Console.WriteLine();
            CreateMenuInfoText($"Next task to do: {nextTask.Title} (Due on {nextTask.DueDate.ToShortDateString()})", true);
        }
    }
}

// Function to display informational text in the menu
static void CreateMenuInfoText(string message, bool isExclamation = false)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(">> ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(message);
    if (isExclamation)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("!\n");
    }
    else
    {
        Console.WriteLine();
    }
}

// Function to display menu options in the console
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

// Function to provide user feedback
static void UserFeedback(string msg, bool errorMsg = true)
{

    Console.ForegroundColor = errorMsg ? ConsoleColor.Red : ConsoleColor.Green;
    Console.WriteLine(msg);
    Console.ForegroundColor = ConsoleColor.White;

}