using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace BackendTesting
{
    internal class TaskTests
    {
        private UserService userService;
        private TaskService taskService;
        private BoardService boardService;
        public TaskTests(UserService userService, TaskService taskService, BoardService boardService)
        {
            this.userService = userService;
            this.taskService = taskService;
            this.boardService = boardService;
        }
        public void RunTests()
        {
            Console.WriteLine("~Test Add Task~");
            //Success Add Test

            userService.Register("amita@gmail.com", "ABac123");
            boardService.CreateBoard("amita@gmail.com", "amit");
            var r0 = taskService.AddTask("amita@gmail.com", "amit", "task1", "make the room", new DateTime(2024, 10, 13));
            var s0 = JsonSerializer.Deserialize<Response>(r0);
            if (s0.ErrorOccured)
            {
                Console.WriteLine("Register " + s0.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task add successfully");
            }

            //fail Add Test
            var r1 = taskService.AddTask("amita@gmail.com", "amit", "task2", "clean the kitchen", new DateTime(2022, 10, 13));
            var s1 = JsonSerializer.Deserialize<Response>(r1);
            if (s1.ErrorOccured)
            {
                Console.WriteLine("Register " + s1.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s1.ReturnValue);
            }

            //fail Add Test
            string t = "";
            for (int i = 0; i < 302; i++)
            {
                t = t + "+";
            }
            var r2 = taskService.AddTask("amita@gmail.com", "amit", t, "clean the kitchen", new DateTime(2025, 10, 13));
            var s2 = JsonSerializer.Deserialize<Response>(r2);
            if (s2.ErrorOccured)
            {
                Console.WriteLine("Register " + s2.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s2.ReturnValue);
            }


            //fail Add Test
            var r3 = taskService.AddTask("amita@gmail.com", "amit", "task2", t, new DateTime(2025, 10, 13));
            var s3 = JsonSerializer.Deserialize<Response>(r3);
            if (s3.ErrorOccured)
            {
                Console.WriteLine("Register " + s3.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s3.ReturnValue);
            }

            Console.WriteLine("~Test Update Task DueDate~");
            //Success Update Task DueDate
            var r4 = taskService.UpdateTaskDueDate("amita@gmail.com", "amit",0 ,0, new DateTime(2025, 10, 13));
            var s4 = JsonSerializer.Deserialize<Response>(r4);
            if (s4.ErrorOccured)
            {
                Console.WriteLine("Register " + s4.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("Task DueDate updated successfully");
            }

            //fail Update Task DueDate
            var r5 = taskService.UpdateTaskDueDate("amita@gmail.com", "amit", 0, 0, new DateTime(2021, 10, 13));
            var s5 = JsonSerializer.Deserialize<Response>(r5);
            if (s5.ErrorOccured)
            {
                Console.WriteLine("Register " + s5.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s5.ReturnValue);
            }


            Console.WriteLine("~Test Update Task Title~");
            //Success Update Task Title
            var r6 = taskService.UpdateTaskTitle("amita@gmail.com", "amit", 0, 0, "gil");
            var s6 = JsonSerializer.Deserialize<Response>(r6);
            if (s6.ErrorOccured)
            {
                Console.WriteLine("Register " + s6.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("Task title updated successfully");
            }

            //fail Update Task Title
            var r7 = taskService.UpdateTaskTitle("amita@gmail.com", "amit", 0, 0, t);
            var s7 = JsonSerializer.Deserialize<Response>(r7);
            if (s7.ErrorOccured)
            {
                Console.WriteLine("Register " + s7.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s7.ReturnValue);
            }

            Console.WriteLine("~Test Update Task Description~");
            //Success Update Task Description
            var r8 = taskService.UpdateTaskDescription("amita@gmail.com", "amit", 0, 0, "gil");
            var s8 = JsonSerializer.Deserialize<Response>(r8);
            if (s8.ErrorOccured)
            {
                Console.WriteLine("Register " + s8.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("Task Description updated successfully");
            }

            //fail Update Task Title
            var r9 = taskService.UpdateTaskDescription("amita@gmail.com", "amit", 0, 0, t);
            var s9 = JsonSerializer.Deserialize<Response>(r9);
            if (s9.ErrorOccured)
            {
                Console.WriteLine("Register " + s9.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s9.ReturnValue);
            }

         



        }
    }
}
