using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackendTesting
{
    internal class BoardTests
    {
        private UserService userService;
        private TaskService taskService;
        private BoardService boardService;
        public BoardTests(UserService userService, TaskService taskService, BoardService boardService)
        {
            this.userService = userService;
            this.taskService = taskService;
            this.boardService = boardService;

        }
        public void RunTests()
        {
            Console.WriteLine("~Test create board~");
            userService.Register("gileden@gmail.com", "Aa4145");
            ////Success create board
            string r0 = boardService.CreateBoard("gileden@gmail.com", "amit");
            var s0 = JsonSerializer.Deserialize<Response>(r0);
            if (s0.ErrorOccured)
            {
                Console.WriteLine("Register " + s0.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("board created successfully");
            }

            ////fail create board
            string r1 = boardService.CreateBoard("gileden@gmail.com", "amit");
            var s1 = JsonSerializer.Deserialize<Response>(r1);
            if (s1.ErrorOccured)
            {
                Console.WriteLine("Register " + s1.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s1.ReturnValue);
            }

            Console.WriteLine("~Test delete board~");
            //Success delete board
            string r2 = boardService.DeleteBoard("gileden@gmail.com", "amit");
            var s2 = JsonSerializer.Deserialize<Response>(r2);
            if (s2.ErrorOccured)
            {
                Console.WriteLine("Register " + s2.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("board deleted successfully");
            }

            //fail delete board
            string r3 = boardService.DeleteBoard("gileden@gmail.com", "amit");
            var s3 = JsonSerializer.Deserialize<Response>(r3);
            if (s3.ErrorOccured)
            {
                Console.WriteLine("Register " + s3.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s3.ReturnValue);
            }

            Console.WriteLine("~Test get board~");
            ////Success get board
            boardService.CreateBoard("gileden@gmail.com", "amit");
            string r4 = boardService.GetBoard("gileden@gmail.com", "amit");
            var s4 = JsonSerializer.Deserialize<Response>(r4);
            if (s4.ErrorOccured)
            {
                Console.WriteLine("Register " + s4.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("board returned successfully");
            }

            //fail get board
            boardService.DeleteBoard("gileden@gmail.com", "amit");
            string r5 = boardService.GetBoard("gileden@gmail.com", "amit");
            var s5 = JsonSerializer.Deserialize<Response>(r5);
            if (s5.ErrorOccured)
            {
                Console.WriteLine("Register " + s5.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("board returned successfully");
            }


            Console.WriteLine("~Test Limit Column~");
            //Success Limit Column 
            boardService.CreateBoard("gileden@gmail.com", "amit");
            string r6 = boardService.LimitColumn("gileden@gmail.com", "amit", 0, 5);
            var s6 = JsonSerializer.Deserialize<Response>(r6);
            if (s6.ErrorOccured)
            {
                Console.WriteLine("Register " + s6.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("column limited successfully");
            }

            //fail Limit Column
            string r7 = boardService.LimitColumn("gileden@gmail.com", "amit", 0, -3);
            var s7 = JsonSerializer.Deserialize<Response>(r7);
            if (s7.ErrorOccured)
            {
                Console.WriteLine("Register " + s7.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s7.ReturnValue);
            }


            //fail Limit Column
            string r8 = boardService.LimitColumn("gileden@gmail.com", "amit", 4, 3);
            var s8 = JsonSerializer.Deserialize<Response>(r8);
            if (s8.ErrorOccured)
            {
                Console.WriteLine("Register " + s8.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s8.ReturnValue);
            }

            Console.WriteLine("~Test get Column Limit~");
            //Success get Column Limit  
            string r9 = boardService.GetColumnLimit("gileden@gmail.com", "amit", 0);
            var s9 = JsonSerializer.Deserialize<Response>(r9);
            if (s9.ErrorOccured)
            {
                Console.WriteLine("Register " + s9.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("the limit of the columm is : " + s9.ReturnValue);
            }

            //fail get Column Limit
            string r10 = boardService.GetColumnLimit("gil.eden@co", "amit", 0);
            var s10 = JsonSerializer.Deserialize<Response>(r10);
            if (s10.ErrorOccured)
            {
                Console.WriteLine("Register " + s10.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("the limit of the columm is : " + s10.ReturnValue);
            }

            Console.WriteLine("~Test get Column name~");
            //Success get Column name 
            string r11 = boardService.GetColumnName("gileden@gmail.com", "amit", 1);
            var s11 = JsonSerializer.Deserialize<Response>(r11);
            if (s11.ErrorOccured)
            {
                Console.WriteLine("Register " + s11.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("the name of the columm is : " + s11.ReturnValue);
            }

            //fail get Column name  
            string r12 = boardService.GetColumnName("gileden@gmail.com", "amit", -1);
            var s12 = JsonSerializer.Deserialize<Response>(r12);
            if (s12.ErrorOccured)
            {
                Console.WriteLine("Register " + s12.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("the name of the columm is : " + s12.ReturnValue);
            }

            Console.WriteLine("~Test get Column~");
            //Success get Column   
            string r13 = boardService.GetColumn("gileden@gmail.com", "amit", 0);
            var s13 = JsonSerializer.Deserialize<Response>(r13);
            if (s13.ErrorOccured)
            {
                Console.WriteLine("Register " + s13.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("column returned successfully");
            }

            //fail get Column   
            string r14 = boardService.GetColumn("gileden@gmail.com", "amit", -1);
            var s14 = JsonSerializer.Deserialize<Response>(r14);
            if (s14.ErrorOccured)
            {
                Console.WriteLine("Register " + s14.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine("column returned successfully");
            }

            Console.WriteLine("~Test Advanced Task~");
            //Success Advanced Task   
            taskService.AddTask("gileden@gmail.com", "amit", "task1", "do the dishes", new DateTime(2025, 4, 01));
            string r15 = boardService.AdvanceTask("gileden@gmail.com", "amit", 0, 0);
            var s15 = JsonSerializer.Deserialize<Response>(r15);
            if (s15.ErrorOccured)
            {
                Console.WriteLine("Register " + s15.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s15.ReturnValue);
            }

            //Success Advanced Task
            string r16 = boardService.AdvanceTask("gileden@gmail.com", "amit", 1, 0);
            var s16 = JsonSerializer.Deserialize<Response>(r16);
            if (s16.ErrorOccured)
            {
                Console.WriteLine("Register " + s16.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s16.ReturnValue);
            }

            //fail Advanced Task   
            string r17 = boardService.AdvanceTask("gileden@gmail.com", "amit", 0, 1);
            var s17 = JsonSerializer.Deserialize<Response>(r17);
            if (s17.ErrorOccured)
            {
                Console.WriteLine("Register " + s17.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s17.ReturnValue);
            }
            //fail Advanced Task   
            string r18 = boardService.AdvanceTask("gileden@gmail.com", "amit", 2, 0);
            var s18 = JsonSerializer.Deserialize<Response>(r18);
            if (s18.ErrorOccured)
            {
                Console.WriteLine("Register " + s18.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s18.ReturnValue);
            }
            //final big test
            Console.WriteLine("---------------" + "\n" + "~Big Board Test~");
            //TEST
            userService.Register("giles@gmail.com", "Aa4145");
            //Success get Column   
            boardService.CreateBoard("giles@gmail.com", "Board2");
            taskService.AddTask("giles@gmail.com", "Board2", "task1", "go to the mall", new DateTime(2025, 1, 3));
            taskService.AddTask("giles@gmail.com", "Board2", "task2", "buy new car", new DateTime(2025, 1, 4));
            taskService.AddTask("giles@gmail.com", "Board2", "task3", "drive home", new DateTime(2025, 1, 4));
            boardService.AdvanceTask("giles@gmail.com", "Board2", 0, 0);
            boardService.AdvanceTask("giles@gmail.com", "Board2", 0, 1);
            boardService.AdvanceTask("giles@gmail.com", "Board2", 0, 2);
            boardService.AdvanceTask("giles@gmail.com", "Board2", 1, 0);
            boardService.AdvanceTask("giles@gmail.com", "Board2", 1, 1);
            boardService.AdvanceTask("giles@gmail.com", "Board2", 1, 2);
            string r19 = boardService.GetColumn("giles@gmail.com", "Board2", 2);
            var s19 = JsonSerializer.Deserialize<Response>(r19);
            if (s19.ErrorOccured)
            {
                Console.WriteLine("Register " + s19.ErrorMessage + "\n");
            }
            else
            {
                Console.WriteLine(s19.ReturnValue);
            }
            Console.WriteLine();

            Console.WriteLine("~Test Get Board Name~");

            string r20 = boardService.GetBoardName(12);   //GetBoardName should be successfull because the ID 12 exists . 
            var s20 = JsonSerializer.Deserialize<Response>(r20);
            if (s20.ErrorOccured)
            {
                Console.WriteLine("GetBoardName " + s20.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s20.ReturnValue);
            }

            string r20_ = boardService.GetBoardName(200);   //GetBoardName should be unsuccessfull because the ID 200 doesn't exist . 
            var s20_ = JsonSerializer.Deserialize<Response>(r20_);
            if (s20_.ErrorOccured)
            {
                Console.WriteLine("GetBoardName " + s20_.ErrorMessage);
            }
            else
            {
                Console.WriteLine(s20_.ReturnValue);
            }







        }

    }
}